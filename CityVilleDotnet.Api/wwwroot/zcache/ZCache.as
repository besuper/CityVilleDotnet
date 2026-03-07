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

                _indexSO.data[key] = { bucket: bucketName, generation: gen, length: len };
                _indexDirty = true;

                var so:SharedObject = getOrOpenBucket(bucketName);
                so.data[key] = data;

                markDirty(bucketName);

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

                if (options && options.hasOwnProperty("generation"))
                {
                    if (indexEntry.hasOwnProperty("generation") &&
                        indexEntry.generation != null &&
                        indexEntry.generation != options.generation)
                    {
                        var staleSO:SharedObject = getOrOpenBucket(indexEntry.bucket);
                        if (staleSO) delete staleSO.data[key];
                        delete _indexSO.data[key];
                        _indexDirty = true;
                        markDirty(indexEntry.bucket);
                        return null;
                    }
                }

                var so:SharedObject = getOrOpenBucket(indexEntry.bucket);
                if (so && so.data.hasOwnProperty(key))
                {
                    return so.data[key];
                }
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
            if (!_allowed) return { cardinality: 0, size: 0 };

            var count:int = 0;
            var totalSize:uint = _indexSO.size;
            for (var key:String in _indexSO.data)
            {
                if (key == "assetCounter") continue;
                count++;
            }

            return {
                cardinality: count,
                size: totalSize
            };
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
