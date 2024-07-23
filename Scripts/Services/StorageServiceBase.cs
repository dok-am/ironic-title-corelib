using System;
using UnityEngine;
using IT.CoreLib.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IT.CoreLib.Services
{

    public abstract class StorageServiceBase<T> : IService where T : IStaticModel
    {
        protected string _pathToStorage;

        protected Dictionary<string, T> _storage;

        /// <summary>
        /// WARNING: Set _pathToStorage in child constructor
        /// </summary>
        public virtual void Initialize()
        {
            _storage = new Dictionary<string, T>();

            ScriptableObject[] allObjects = Resources.LoadAll<ScriptableObject>(_pathToStorage);
            foreach (ScriptableObject obj in allObjects)
            {
                if (obj is T model) {

                    if (_storage.ContainsKey(model.Id))
                        throw new Exception($"[STORAGE] Duplicating ids {model.Id} in storage type {typeof(T).Name}");
                    
                    _storage.Add(model.Id, model);
                }
            }
        }

        public void OnInitialized(IBootstrap bootstrap)
        {
            //cool
        }

        public virtual T GetModel(string id)
        {
            if (_storage.TryGetValue(id, out T model))
                return model;

            throw new Exception($"[STORAGE] There are no model with id {id} in storage for type {typeof(T).Name}");
        }

        public virtual T[] GetAllModels()
        {
            return _storage.Values.ToArray();
        }

        public void Destroy()
        {
            //cool
        }
    }

}
