package
{
    import flash.display.Sprite;
    import flash.events.Event;
    import flash.net.SharedObject;
    import flash.net.SharedObjectFlushStatus;
    import flash.utils.ByteArray;
    import flash.utils.Dictionary;

    [SWF(width="1", height="1", frameRate="1")]
    public dynamic class ZCache extends Sprite
    {
        private static const STORAGE_REQUEST_BYTES:uint = 4096 * 1024 * 1024;
        private static const MAX_FLUSH_PER_FRAME:int = 1;
        private static const BUCKET_SIZE:int = 50;
        private static const TTL_MS:Number = 30 * 24 * 60 * 60 * 1000;

        private var _ns:String;
        private var _indexSO:SharedObject;
        private var _allowed:Boolean = false;
        private var _initError:Error;
        private var _lastFlushError:Error;
        private var _inactivityFlushTimeout:int = 0;

        private var _openBuckets:Dictionary = new Dictionary();

        private var _dirtyQueue:Vector.<String> = new Vector.<String>();
        private var _dirtySet:Object = {};
        private var _indexDirty:Boolean = false;

        private var _assetCounter:int = 0;

        private var _gets:int = 0;
        private var _hits:int = 0;
        private var _hitBytes:uint = 0;
        private var _flops:int = 0;
        private var _puts:int = 0;
        private var _putBytes:uint = 0;
        private var _generationMismatches:int = 0;
        private var _flushes:int = 0;

        public function ZCache()
        {
            super();
        }

        public function get initError():Error
        {
            return _initError;
        }

        public function get lastFlushError():Error
        {
            return _lastFlushError;
        }

        public function get inactivityFlushTimeout():int
        {
            return _inactivityFlushTimeout;
        }

        public function set inactivityFlushTimeout(value:int):void
        {
            _inactivityFlushTimeout = value;
        }

        public function init(ns:String):Boolean
        {
            _ns = ns;
            this["namespace"] = ns;
            try
            {
                _indexSO = SharedObject.getLocal("zci_" + ns, "/");

                if (_indexSO.data.assetCounter == undefined)
                    _indexSO.data.assetCounter = 0;

                _assetCounter = _indexSO.data.assetCounter;

                _gets = 0;
                _hits = 0;
                _hitBytes = 0;
                _flops = 0;
                _puts = 0;
                _putBytes = 0;
                _generationMismatches = 0;
                _flushes = 0;

                _indexSO.flush(STORAGE_REQUEST_BYTES);

                addEventListener(Event.ENTER_FRAME, onEnterFrame);

                _allowed = true;
                return true;
            }
            catch (e:Error)
            {
                _initError = e;
                _allowed = false;
            }
            return false;
        }

        public function flush():Boolean
        {
            if (!_allowed) return false;
            try
            {
                flushIndex();
                for each (var bucketName:String in _dirtyQueue)
                {
                    var so:SharedObject = getOrOpenBucket(bucketName);
                    if (so) so.flush();
                }
                _dirtyQueue.length = 0;
                _dirtySet = {};
                _flushes++;
                return true;
            }
            catch (e:Error)
            {
                _lastFlushError = e;
            }
            return false;
        }

        public function get allowed():Boolean
        {
            return _allowed;
        }

        public function promptForStorage(success:Function, failure:Function):void
        {
            try
            {
                var result:String = _indexSO.flush(STORAGE_REQUEST_BYTES);
                if (success != null) success();
            }
            catch (e:Error)
            {
                if (failure != null) failure();
            }
        }

        public function put(key:String, data:*, options:Object = null):Boolean
        {
            if (!_allowed) return false;
            try
            {
                var gen:* = null;
                var len:* = null;
                if (options)
                {
                    if (options.hasOwnProperty("generation"))
                        gen = options.generation;
                    if (options.hasOwnProperty("length"))
                        len = options.length;
                }

                var bucketName:String;
                var indexEntry:Object = _indexSO.data[key];
                if (indexEntry)
                {
                    bucketName = indexEntry.bucket;
                }
                else
                {
                    var bucketId:int = int(_assetCounter / BUCKET_SIZE);
                    bucketName = "zcb_" + _ns + "_" + bucketId;
                    _assetCounter++;
                    _indexSO.data.assetCounter = _assetCounter;
                }

                var bytes:ByteArray = toByteArray(data);

                _indexSO.data[key] = { bucket: bucketName, generation: gen, length: bytes.length, storedAt: new Date().getTime() };
                _indexDirty = true;

                var so:SharedObject = getOrOpenBucket(bucketName);
                so.data[key] = data;

                markDirty(bucketName);

                _puts++;
                _putBytes += bytes.length;

                return true;
            }
            catch (e:Error)
            {
                _lastFlushError = e;
            }
            return false;
        }

        public function get(key:String, options:Object = null):*
        {
            if (!_allowed) return null;
            try
            {
                var indexEntry:Object = _indexSO.data[key];
                if (!indexEntry) return null;

                if (!indexEntry.hasOwnProperty("storedAt") || (new Date().getTime() - Number(indexEntry.storedAt)) > TTL_MS)
                {
                    var expiredSO:SharedObject = getOrOpenBucket(indexEntry.bucket);
                    if (expiredSO) delete expiredSO.data[key];
                    delete _indexSO.data[key];
                    _indexDirty = true;
                    markDirty(indexEntry.bucket);
                    return null;
                }

                if (options && options.hasOwnProperty("generation"))
                {
                    if (indexEntry.hasOwnProperty("generation") &&
                        indexEntry.generation != null &&
                        indexEntry.generation != options.generation)
                    {
                        _generationMismatches++;
                        var staleSO:SharedObject = getOrOpenBucket(indexEntry.bucket);
                        if (staleSO) delete staleSO.data[key];
                        delete _indexSO.data[key];
                        _indexDirty = true;
                        markDirty(indexEntry.bucket);
                        return null;
                    }
                }

                var so:SharedObject = getOrOpenBucket(indexEntry.bucket);
                _gets++;
                if (so && so.data.hasOwnProperty(key))
                {
                    var value:* = so.data[key];
                    var valueBytes:ByteArray = toByteArray(value);
                    _hits++;
                    _hitBytes += valueBytes.length;
                    return value;
                }
                _flops++;
                return null;
            }
            catch (e:Error)
            {
                return null;
            }
        }

        public function containsKey(key:String):Boolean
        {
            if (!_allowed) return false;
            return _indexSO.data.hasOwnProperty(key);
        }

        public function clear():Boolean
        {
            if (!_allowed) return false;
            try
            {
                var buckets:Object = {};
                for (var key:String in _indexSO.data)
                {
                    if (key == "assetCounter") continue;
                    var entry:Object = _indexSO.data[key];
                    if (entry && entry.hasOwnProperty("bucket"))
                    {
                        buckets[entry.bucket] = true;
                    }
                }
                for (var bName:String in buckets)
                {
                    var so:SharedObject = getOrOpenBucket(bName);
                    if (so) so.clear();
                    delete _openBuckets[bName];
                }

                _indexSO.clear();
                _indexSO.data.assetCounter = 0;
                _assetCounter = 0;
                _indexSO.flush();
                _dirtyQueue.length = 0;
                _dirtySet = {};
                _indexDirty = false;
                return true;
            }
            catch (e:Error)
            {
                _lastFlushError = e;
            }
            return false;
        }

        public function remove(key:String):*
        {
            if (!_allowed) return null;
            try
            {
                var indexEntry:Object = _indexSO.data[key];
                if (!indexEntry) return null;

                var data:* = null;
                var so:SharedObject = getOrOpenBucket(indexEntry.bucket);
                if (so && so.data.hasOwnProperty(key))
                {
                    data = so.data[key];
                    delete so.data[key];
                    markDirty(indexEntry.bucket);
                }

                delete _indexSO.data[key];
                _indexDirty = true;

                return data;
            }
            catch (e:Error)
            {
                _lastFlushError = e;
                return null;
            }
        }

        public function get stats():Object
        {
            if (!_allowed) return { cardinality: 0, size: 0, gets: 0, hits: 0, hitBytes: 0, flops: 0, puts: 0, putBytes: 0, generationMismatches: 0, flushes: 0 };

            var count:int = 0;
            var totalSize:uint = 0;
            for (var key:String in _indexSO.data)
            {
                if (key == "assetCounter") continue;
                var entry:Object = _indexSO.data[key];
                if (entry && entry.hasOwnProperty("length"))
                    totalSize += uint(entry.length);
                count++;
            }

            return {
                cardinality: count,
                size: totalSize,
                gets: _gets,
                hits: _hits,
                hitBytes: _hitBytes,
                flops: _flops,
                puts: _puts,
                putBytes: _putBytes,
                generationMismatches: _generationMismatches,
                flushes: _flushes
            };
        }
        
        private function toByteArray(data:*):ByteArray
        {
            if (data is ByteArray)
            {
                return data as ByteArray;
            }
            var ba:ByteArray = new ByteArray();
            ba.writeObject(data);
            ba.position = 0;
            return ba;
        }

        private function getOrOpenBucket(bucketName:String):SharedObject
        {
            if (_openBuckets[bucketName])
                return _openBuckets[bucketName];

            try
            {
                var so:SharedObject = SharedObject.getLocal(bucketName, "/");
                _openBuckets[bucketName] = so;
                return so;
            }
            catch (e:Error)
            {
            }
            return null;
        }

        private function markDirty(bucketName:String):void
        {
            if (!_dirtySet.hasOwnProperty(bucketName))
            {
                _dirtyQueue.push(bucketName);
                _dirtySet[bucketName] = true;
            }
        }

        private function flushIndex():void
        {
            if (_indexDirty)
            {
                try { _indexSO.flush(); } catch (e:Error) { _lastFlushError = e; }
                _indexDirty = false;
            }
        }

        private function onEnterFrame(e:Event):void
        {
            var flushed:int = 0;

            if (_indexDirty && flushed < MAX_FLUSH_PER_FRAME)
            {
                flushIndex();
                flushed++;
            }

            while (_dirtyQueue.length > 0 && flushed < MAX_FLUSH_PER_FRAME)
            {
                var bucketName:String = _dirtyQueue.shift();
                delete _dirtySet[bucketName];
                try
                {
                    var so:SharedObject = getOrOpenBucket(bucketName);
                    if (so) so.flush();
                }
                catch (err:Error)
                {
                    _lastFlushError = err;
                }
                flushed++;
            }
        }
    }
}
