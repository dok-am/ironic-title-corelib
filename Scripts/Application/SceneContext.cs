using IT.CoreLib.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IT.CoreLib.Application
{
    public abstract class SceneContext : AbstractContext
    {
        public event Action<SceneContext> OnSceneServicesLoaded;
        public event Action<SceneContext> OnSceneInitialied;

        public SceneUIBase SceneUI => _sceneUI;


        private SceneUIBase _sceneUI;

        [Header("UI")]
        [SerializeField] private SceneUIBase _sceneUIPrefab;


        public void InitializeContext(AbstractContext parentContext, ApplicationUIContainer uiContainer)
        {
            Parent = parentContext;
            InitializeInternal();

            InitializeServices();
            OnServicesInitialized();
            OnSceneServicesLoaded?.Invoke(this);

            InitializeUI(uiContainer);
            InitializeScene();
            
            Initialized = true;
            OnSceneInitialied?.Invoke(this);
        }

        public void ReloadScene()
        {
            //TODO: Should be more abstract?
            _ = ApplicationContext.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
