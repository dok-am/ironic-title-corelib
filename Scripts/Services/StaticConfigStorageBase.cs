using System;
using UnityEngine;
using IT.CoreLib.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IT.CoreLib.Services
{
    /// <summary>
    /// Base class for getting ScriptableObjects with IStaticConfig
    /// Technically, it's a provider, not a service, but for now 
    /// let it be in the same basket
    /// 
    /// HOW TO USE:
    /// 1) Create child class with necessary T type
    /// 2) In class constructor set the _pathToStorage to a folder
    ///    with the ScriptableObjects inside Resources folder
    ///    Example: if your full path is {ProjecFolder}/Assets/Resources/Items
    ///    you should set _pathToStorage = "Items"
    /// 3) Add as a service in a needed IContext 
    /// </summary>
    /// <typeparam name="T">IStaticConfig storing type</typeparam>
    public abstract class StaticConfigStorageBase<T> : IService where T : IStaticConfig
    {
        /// <summary>
        /// Path to a storage, must be set in constructor
        /// </summary>
        protected string _pathToStorage;

        protected Dictionary<string, T> _cachedStorage = new();


        public virtual void Initialize()
        {
            CacheAllConfigs();
        }

        public virtual T GetConfig(string id)
        {
            if (_cachedStorage.TryGetValue(id, out T config))
                return config;

            throw new Exception($"[STORAGE] There are no config with id {id} in storage for type {typeof(T).Name}");
        }

        public virtual T[] GetAllConfigs()
        {
            return _cachedStorage.Values.ToArray();
        }


        protected virtual void CacheAllConfigs()
        {
            ScriptableObject[] allObjects = Resources.LoadAll<ScriptableObject>(_pathToStorage);
            foreach (ScriptableObject obj in allObjects)
            {
                if (obj is T config)
                {

                    if (_cachedStorage.ContainsKey(config.Id))
                        throw new Exception($"[STORAGE] Duplicating ids {config.Id} in storage type {typeof(T).Name}");

                    _cachedStorage.Add(config.Id, config);
                }
            }
        }
    }
}
