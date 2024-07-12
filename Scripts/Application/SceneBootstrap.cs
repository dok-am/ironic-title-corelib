using IT.CoreLib.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IT.CoreLib.Application
{

    public abstract class SceneBootstrap : AbstractBootstrap
    {
        public event Action<SceneBootstrap> OnSceneServicesLoaded;
        public event Action<SceneBootstrap> OnSceneInitialied;

        public SceneUIBase SceneUI => _sceneUI;

        [SerializeField] private SceneUIBase _sceneUIPrefab;

        private SceneUIBase _sceneUI;

        public void InitializeBootstrap(AbstractBootstrap parentBootstrap, ApplicationUIContainer uiContainer)
        {
            Parent = parentBootstrap;
            InitializeInternal();

            InitializeServices();
            OnServicesInitialized();
            OnSceneServicesLoaded?.Invoke(this);

            InitializeUI(uiContainer);
            InitializeScene();
            
            Initialized = true;
            OnSceneInitialied?.Invoke(this);
        }

        protected abstract void InitializeScene();

        protected override void InitializeUI(ApplicationUIContainer uiContainer)
        {
            _sceneUI = uiContainer.AddSceneUI(_sceneUIPrefab, this);
        }

    }

}
