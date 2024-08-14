
using IT.CoreLib.Interfaces;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Pool;
using IT.CoreLib.Managers;

namespace IT.CoreLib.Scripts
{
    public abstract class TypedPoolSpawnManager<T, P> : IManager 
        where T: UnityEngine.Object, IPoolableObject 
        where P : class, IIdentifiable
    {
        private Dictionary<string, ObjectPool<T>> _pools = new();
        private Transform _container;

        /// <summary>
        /// Changed this is children's constructor before Initialize()
        /// </summary>
        protected int _maxCount = 20;
        protected string _containerName = "POOL";

        private int _effectsCount = 0;

        /// <summary>
        /// Call this in children constructor
        /// </summary>
        protected void Initialize()
        {
            _container = new GameObject(_containerName).transform;
        }

        protected T Spawn(T prefab, P config = null)
        {
            if (_effectsCount >= _maxCount)
                return null;

            string id = config != null ? config.Id : prefab.Id;

            ObjectPool<T> pool;
            _pools.TryGetValue(id, out pool);
            if (pool == null)
                pool = CreatePool(prefab, config);

            _effectsCount++;

            return pool.Get();
        }


        private ObjectPool<T> CreatePool(T effectPrefab, P config)
        {
            ObjectPool<T> pool = new ObjectPool<T>(
                () =>
                {
                    return CreateObject(effectPrefab, config);
                },
                null,
                null,
                OnDestroyObject,
                false, _maxCount, _maxCount);

            _pools.Add(config.Id, pool);

            return pool;
        }

        private T CreateObject(T prefab, P config)
        {
            T instance = GameObject.Instantiate(prefab, _container);

            instance.ReleaseFromPool += ReleaseObject;

            OnObjectCreated(instance, config);

            return instance;
        }

        private void ReleaseObject(IPoolableObject obj)
        {
            if (_pools.TryGetValue(obj.Id, out ObjectPool<T> pool))
                pool.Release((T)obj);

            _effectsCount--;
        }

        private void OnDestroyObject(T obj)
        {
            obj.ReleaseFromPool -= ReleaseObject;
        }

        protected abstract void OnObjectCreated(T obj, P config);
    }
}