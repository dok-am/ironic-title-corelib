using IT.CoreLib.Interfaces;
using IT.CoreLib.UI;
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using VContainer.Unity;

namespace IT.CoreLib.Application
{
    public abstract class SceneContext : AbstractContext
    {
        public event Action<SceneContext> OnSceneServicesLoaded;
        public event Action<SceneContext> OnBindersInitialized;
        public event Action<SceneContext> OnSceneInitialized;

        public SceneUIBase SceneUI => _sceneUI;
        public LifetimeScope SceneScope => _sceneScope;
        public string[] DefaultSubscenes => _defaultSubscenes;


        protected ISceneContextInputParameters _inputParameters;

        [Header("DI Container")]
        [SerializeField, Required] private LifetimeScope _sceneScope;
        [Header("UI")]
        [SerializeField] private SceneUIBase _sceneUIPrefab;
        [Header("Structure")]
        [SerializeField] private string[] _defaultSubscenes;
        [Header("Binders")]
        [SerializeField] private SceneBinderBase[] _sceneBinders;

        private SceneUIBase _sceneUI;
        private Dictionary<Type, ISceneBinder> _sceneBindersDict = new();


        public void InitializeContext(AbstractContext parentContext, 
            ApplicationUIContainer uiContainer,
            ISceneContextInputParameters inputParameters)
        {
            Parent = parentContext;
            _inputParameters = inputParameters;

            InitializeServices();
            OnServicesInitialized();
            OnSceneServicesLoaded?.Invoke(this);

            InitializeUI(uiContainer);
            InitializeBinders();
            OnBindersInitialized?.Invoke(this);

            InitializeScene();
            Initialized = true;
            OnSceneInitialized?.Invoke(this);
        }

        public T GetSceneBinder<T>() where T : ISceneBinder
        {
            if (_sceneBindersDict.TryGetValue(typeof(T), out var binder))
                return (T)binder;

            throw new Exception($"[CONTEXT] There is no scene binder of type {typeof(T).Name} in Context {gameObject.name} ");
        }


        protected abstract void InitializeScene();

        protected override void InitializeUI(ApplicationUIContainer uiContainer)
        {
            if (_sceneUIPrefab != null)
                _sceneUI = uiContainer.AddSceneUI(_sceneUIPrefab, this);
        }

        protected virtual void InitializeBinders()
        {
            if (_sceneBinders != null)
            {
                foreach (SceneBinderBase sceneBinder in _sceneBinders)
                {
                    sceneBinder.Bind(this);
                    _sceneBindersDict.Add(sceneBinder.GetType(), sceneBinder);
                }
            }
        }

        protected override void OnDestroy()
        {
            if (_sceneUI != null)
                _sceneUI.Deinitialize();

            if (_sceneBinders != null)
            {
                foreach (SceneBinderBase sceneBinder in _sceneBinders)
                {
                    sceneBinder.Unbind(this);
                }
            }

            base.OnDestroy();
        }
    }
}
