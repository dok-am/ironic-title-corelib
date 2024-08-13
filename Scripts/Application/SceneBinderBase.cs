using IT.CoreLib.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IT.CoreLib.Application
{
    public abstract class SceneBinderBase : MonoBehaviour, ISceneBinder
    {
        private Dictionary<Type, IManager> _managers = new();
        private List<IUpdatable> _updatableManagers = new();
        private List<IFixedUpdatable> _fixedUpdatableManagers = new();


        public abstract void Bind(IContext context);

        public virtual void Unbind(IContext context)
        {
            foreach (var manager in _managers.Values)
            {
                manager.Unbind();
            }
        }

        public T GetManager<T>() where T : IManager
        {
            if (_managers.TryGetValue(typeof(T), out var manager))
                return (T)manager;

            throw new Exception($"[BINDER] Scene binder {gameObject.name} doesn't have manager {typeof(T).Name}");
        }


        protected void AddManager(IManager manager)
        {
            _managers.Add(manager.GetType(), manager);

            if (manager is IUpdatable updatableManager)
                _updatableManagers.Add(updatableManager);

            if (manager is IFixedUpdatable fixedUpdatableManager)
                _fixedUpdatableManagers.Add(fixedUpdatableManager);
        }


        private void Update()
        {
            foreach (var manager in _updatableManagers)
            {
                manager.Update(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach(var manager in _fixedUpdatableManagers)
            {
                manager.FixedUpdate(Time.fixedDeltaTime);
            }
        }
        
    }
}