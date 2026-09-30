package
{
    import flash.display.Sprite;
    import flash.events.Event;
    import flash.events.NetStatusEvent;
    import flash.net.SharedObject;
    import flash.net.SharedObjectFlushStatus;
    import flash.utils.ByteArray;
    import flash.utils.Dictionary;
    import flash.utils.getTimer;

    [SWF(width="1", height="1", frameRate="1")]
    public dynamic class ZCache extends Sprite
    {
        private static const STORAGE_REQUEST_BYTES:uint = 1024 * 1024 * 1024;
        private static const MAX_FLUSH_PER_FRAME:int = 1;
        private static const LEGACY_BUCKET_SIZE:int = 50;
        private static const BUCKET_MAX_BYTES:uint = 1024 * 1024;
        private static const LARGE_ASSET_BYTES:uint = 512 * 1024;
        private static const MAX_OPEN_BUCKETS:int = 16;
        private static const DEFAULT_FLUSH_DELAY_MS:int = 3000;
        private static const MAX_FLUSH_DELAY_MS:int = 20000;
        private static const TTL_MS:Number = 30 * 24 * 60 * 60 * 1000;

        private static const META_KEY:String = "__zcmeta";
        private static const LEGACY_COUNTER_KEY:String = "assetCounter";
        private static const LARGE_BUCKET_PREFIX:String = "zcl_";

        private var _ns:String;
        private var _indexSO:SharedObject;
        private var _meta:Object;
        private var _allowed:Boolean = false;
        private var _initError:Error;
        private var _lastFlushError:Error;
        private var _inactivityFlushTimeout:int = 0;

        private var _openBuckets:Dictionary = new Dictionary();
        private var _bucketLru:Vector.<String> = new Vector.<String>();

        private var _dirtyQueue:Vector.<String> = new Vector.<String>();
        private var _flushBatch:Vector.<String>;
        private var _dirtySet:Object = {};
        private var _indexDirty:Boolean = false;
        private var _lastWriteTime:int = 0;
        private var _firstPendingWriteTime:int = -1;

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

                if (!_indexSO.data[META_KEY])
                {
                    _indexSO.data[META_KEY] = {
                        bucketCounter: int(int(_indexSO.data[LEGACY_COUNTER_KEY]) / LEGACY_BUCKET_SIZE) + 1,
                        bucketBytes: 0,
                        largeCounter: 0
                    };
                    markIndexDirty();
                }
                _meta = _indexSO.data[META_KEY];

                _gets = 0;
                _hits = 0;
                _hitBytes = 0;
                _flops = 0;
                _puts = 0;
                _putBytes = 0;
                _generationMismatches = 0;
                _flushes = 0;

                _indexSO.flush();

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
                while (_flushBatch && _flushBatch.length > 0)
                {
                    flushBucket(_flushBatch.shift());
                }
                _flushBatch = null;
                while (_dirtyQueue.length > 0)
                {
                    flushBucket(_dirtyQueue.shift());
                }
                flushIndex();
                _firstPendingWriteTime = -1;
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
                if (_indexSO.flush(STORAGE_REQUEST_BYTES) != SharedObjectFlushStatus.PENDING)
                {
                    if (success != null) success();
                    return;
                }

                var onStatus:Function = function(event:NetStatusEvent):void
                {
                    _indexSO.removeEventListener(NetStatusEvent.NET_STATUS, onStatus);
                    if (event.info.code == "SharedObject.Flush.Success")
                    {
                        if (success != null) success();
                    }
                    else if (failure != null)
                    {
                        failure();
                    }
                };
                _indexSO.addEventListener(NetStatusEvent.NET_STATUS, onStatus);
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
                if (options && options.hasOwnProperty("generation"))
                    gen = options.generation;

                var previousEntry:Object = _indexSO.data[key];
                if (previousEntry)
                    removeFromBucket(key, previousEntry.bucket);

                var bytes:ByteArray = toByteArray(data);
                var bucketName:String = allocateBucket(bytes.length);

                _indexSO.data[key] = { bucket: bucketName, generation: gen, length: bytes.length, storedAt: new Date().getTime() };
                markIndexDirty();

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
                    removeEntry(key, indexEntry);
                    return null;
                }

                if (options && options.hasOwnProperty("generation"))
                {
                    if (indexEntry.hasOwnProperty("generation") &&
                        indexEntry.generation != null &&
                        indexEntry.generation != options.generation)
                    {
                        _generationMismatches++;
                        removeEntry(key, indexEntry);
                        return null;
                    }
                }

                var bucketName:String = indexEntry.bucket;
                var so:SharedObject = getOrOpenBucket(bucketName);
                _gets++;
                if (so && so.data.hasOwnProperty(key))
                {
                    var value:* = so.data[key];
                    _hits++;
                    _hitBytes += toByteArray(value).length;

                    if (isLargeBucket(bucketName) && !_dirtySet.hasOwnProperty(bucketName))
                        releaseBucket(bucketName);

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
            return !isMetaKey(key) && _indexSO.data.hasOwnProperty(key);
        }

        public function clear():Boolean
        {
            if (!_allowed) return false;
            try
            {
                var buckets:Object = {};
                for (var key:String in _indexSO.data)
                {
                    if (isMetaKey(key)) continue;
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
                    releaseBucket(bName);
                }

                _indexSO.clear();
                _indexSO.data[META_KEY] = { bucketCounter: 0, bucketBytes: 0, largeCounter: 0 };
                _meta = _indexSO.data[META_KEY];
                _indexSO.flush();
                _dirtyQueue.length = 0;
                _flushBatch = null;
                _dirtySet = {};
                _indexDirty = false;
                _firstPendingWriteTime = -1;
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

                return removeEntry(key, indexEntry);
            }
            catch (e:Error)
            {
                _lastFlushError = e;
            }
            return null;
        }

        public function get stats():Object
        {
            if (!_allowed) return { cardinality: 0, size: 0, gets: 0, hits: 0, hitBytes: 0, flops: 0, puts: 0, putBytes: 0, generationMismatches: 0, flushes: 0 };

            var count:int = 0;
            var totalSize:uint = 0;
            for (var key:String in _indexSO.data)
            {
                if (isMetaKey(key)) continue;
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

        private function isMetaKey(key:String):Boolean
        {
            return key == META_KEY || key == LEGACY_COUNTER_KEY;
        }

        private function isLargeBucket(bucketName:String):Boolean
        {
            return bucketName.indexOf(LARGE_BUCKET_PREFIX) == 0;
        }

        private function allocateBucket(size:uint):String
        {
            if (size >= LARGE_ASSET_BYTES)
            {
                return LARGE_BUCKET_PREFIX + _ns + "_" + (_meta.largeCounter++);
            }

            if (_meta.bucketBytes > 0 && _meta.bucketBytes + size > BUCKET_MAX_BYTES)
            {
                _meta.bucketCounter++;
                _meta.bucketBytes = 0;
            }
            _meta.bucketBytes += size;

            return "zcb_" + _ns + "_" + _meta.bucketCounter;
        }

        private function removeEntry(key:String, indexEntry:Object):*
        {
            var data:* = removeFromBucket(key, indexEntry.bucket);
            delete _indexSO.data[key];
            markIndexDirty();
            return data;
        }

        private function removeFromBucket(key:String, bucketName:String):*
        {
            var so:SharedObject = getOrOpenBucket(bucketName);
            if (!so || !so.data.hasOwnProperty(key)) return null;

            var data:* = so.data[key];
            delete so.data[key];

            if (isLargeBucket(bucketName))
            {
                so.clear();
                unmarkDirty(bucketName);
                releaseBucket(bucketName);
            }
            else
            {
                markDirty(bucketName);
            }

            return data;
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
            var so:SharedObject = _openBuckets[bucketName];
            if (!so)
            {
                try
                {
                    so = SharedObject.getLocal(bucketName, "/");
                    _openBuckets[bucketName] = so;
                }
                catch (e:Error)
                {
                    return null;
                }
            }

            touchBucket(bucketName);
            return so;
        }

        private function touchBucket(bucketName:String):void
        {
            var idx:int = _bucketLru.indexOf(bucketName);
            if (idx >= 0) _bucketLru.splice(idx, 1);
            _bucketLru.push(bucketName);

            evictBuckets();
        }

        private function evictBuckets():void
        {
            var i:int = 0;
            while (_bucketLru.length > MAX_OPEN_BUCKETS && i < _bucketLru.length - 1)
            {
                var bucketName:String = _bucketLru[i];
                if (_dirtySet.hasOwnProperty(bucketName))
                {
                    i++;
                    continue;
                }
                delete _openBuckets[bucketName];
                _bucketLru.splice(i, 1);
            }
        }

        private function releaseBucket(bucketName:String):void
        {
            delete _openBuckets[bucketName];
            var idx:int = _bucketLru.indexOf(bucketName);
            if (idx >= 0) _bucketLru.splice(idx, 1);
        }

        private function markIndexDirty():void
        {
            _indexDirty = true;
            notifyWrite();
        }

        private function markDirty(bucketName:String):void
        {
            if (!_dirtySet.hasOwnProperty(bucketName))
            {
                _dirtyQueue.push(bucketName);
                _dirtySet[bucketName] = true;
            }
            notifyWrite();
        }

        private function unmarkDirty(bucketName:String):void
        {
            if (!_dirtySet.hasOwnProperty(bucketName)) return;

            delete _dirtySet[bucketName];
            var idx:int = _dirtyQueue.indexOf(bucketName);
            if (idx >= 0) _dirtyQueue.splice(idx, 1);
            if (_flushBatch)
            {
                idx = _flushBatch.indexOf(bucketName);
                if (idx >= 0) _flushBatch.splice(idx, 1);
            }
        }

        private function notifyWrite():void
        {
            _lastWriteTime = getTimer();
            if (_firstPendingWriteTime < 0) _firstPendingWriteTime = _lastWriteTime;
        }

        private function flushIndex():void
        {
            if (_indexDirty)
            {
                try { _indexSO.flush(); } catch (e:Error) { _lastFlushError = e; }
                _indexDirty = false;
            }
        }

        private function flushBucket(bucketName:String):void
        {
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

            if (isLargeBucket(bucketName)) releaseBucket(bucketName);
        }

        private function onEnterFrame(e:Event):void
        {
            if (!_flushBatch)
            {
                if (!_indexDirty && _dirtyQueue.length == 0) return;

                var now:int = getTimer();
                var delay:int = _inactivityFlushTimeout > 0 ? _inactivityFlushTimeout : DEFAULT_FLUSH_DELAY_MS;
                if (now - _lastWriteTime < delay && now - _firstPendingWriteTime < MAX_FLUSH_DELAY_MS) return;

                _flushBatch = _dirtyQueue;
                _dirtyQueue = new Vector.<String>();
            }

            var flushed:int = 0;
            while (_flushBatch.length > 0 && flushed < MAX_FLUSH_PER_FRAME)
            {
                flushBucket(_flushBatch.shift());
                flushed++;
            }

            if (_flushBatch.length > 0 || flushed >= MAX_FLUSH_PER_FRAME) return;

            flushIndex();
            _flushBatch = null;
            _firstPendingWriteTime = (_indexDirty || _dirtyQueue.length > 0) ? getTimer() : -1;
            evictBuckets();
        }
    }
}
