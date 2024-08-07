using IT.CoreLib.Interfaces;
using IT.CoreLib.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IT.CoreLib.Application
{
    public abstract class AbstractBootstrap : MonoBehaviour, IBootstrap
    {
        public event Action<bool> OnPaused;

        public IBootstrap Parent {  get; protected set; }
        public bool IsPaused => _isPaused;


        private bool _isPaused;
        private Dictionary<Type, IService> _services;
        private List<IUpdatable> _updatables;
        private List<IFixedUpdatable> _fixedUpdatables;


        /// <summary>
        /// You must set this in children
        /// </summary>
        public bool Initialized { get; protected set; }

        public T GetService<T>() where T : IService
        {
            if (_services.TryGetValue(typeof(T), out var service)) 
                return (T)service;

            throw new Exception($"[BOOTSTRAP] There is no service of type {typeof(T).Name} in Bootstrap {gameObject.name} ");
        }

        public virtual void SetPaused(bool paused)
        {
            _isPaused = paused;
            foreach (IService service in _services.Values)
            {
                service.OnPaused(paused);
            }

            OnPaused?.Invoke(paused);
        }


        protected T AddService<T>(GameObject servicePrefab = null) where T: IService, new()
        {
            if (_services.ContainsKey(typeof(T)))
                throw new Exception($"[BOOTSTRAP] Can't add service {typeof(T).Name}, bootstrap {gameObject.name} already has it");

            T service;

            if (servicePrefab == null)
            {
                service = new T();
            } 
            else
            {
                service = Instantiate(servicePrefab, transform).GetComponent<T>();

                if (service == null)
                    throw new Exception($"[BOOTSTRAP] Can't add service {typeof(T).Name}, prefab is incorrect!");
            }

            
            service.Initialize();

            _services.Add(typeof(T), service);

            if (service is IUpdatable)
                _updatables.Add(service as IUpdatable);

            if (service is IFixedUpdatable)
                _fixedUpdatables.Add(service as IFixedUpdatable);

            return service;
        }

        protected abstract void InitializeServices();

        protected abstract void InitializeUI(ApplicationUIContainer uiContainer);

        protected virtual void InitializeInternal()
        {
            Parent = null;
            _services = new Dictionary<Type, IService>();
            _updatables = new List<IUpdatable>();
            _fixedUpdatables = new List<IFixedUpdatable>();
        }       

        protected virtual void OnServicesInitialized()
        {
            foreach (IService service in _services.Values)
            {
                service.OnInitialized(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (!Initialized) 
                return;

            foreach (IService service in _services.Values)
            {
                service.Destroy();
                if (service is MonoBehaviour monoService)
                {
                    Destroy(monoService.gameObject);
                }
            }
        }


        private void Update()
        {
            if (!Initialized || _isPaused) 
                return;

            foreach (IUpdatable service in _updatables)
            {
                service.Update(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (!Initialized || _isPaused) 
                return;

            foreach (IFixedUpdatable service in _fixedUpdatables)
            {
                service.FixedUpdate(Time.fixedDeltaTime);
            }
        }
    }
}
