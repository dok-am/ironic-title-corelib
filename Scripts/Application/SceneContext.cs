using IT.CoreLib.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IT.CoreLib.Application
{
    public abstract class SceneContext : AbstractContext
    {
        public event Action<SceneContext> OnSceneServicesLoaded;
        public event Action<SceneContext> OnSceneInitialized;

        public SceneUIBase SceneUI => _sceneUI;


        private SceneUIBase _sceneUI;

        [Header("UI")]
        [SerializeField] private SceneUIBase _sceneUIPrefab;


        public void InitializeContext(AbstractContext parentContext, ApplicationUIContainer uiContainer)
        {
            Parent = parentContext;

            InitializeServices();
            OnServicesInitialized();
            OnSceneServicesLoaded?.Invoke(this);

            InitializeUI(uiContainer);
            InitializeScene();
            
            Initialized = true;
            OnSceneInitialized?.Invoke(this);
        }

        protected abstract void InitializeScene();

        protected override void InitializeUI(ApplicationUIContainer uiContainer)
        {
            _sceneUI = uiContainer.AddSceneUI(_sceneUIPrefab, this);
        }

        protected override void OnDestroy()
        {
            if (_sceneUI != null)
                _sceneUI.Deinitialize();

            base.OnDestroy();
        }
    }
}
