using Cysharp.Threading.Tasks;
using IT.CoreLib.Interfaces;
using IT.CoreLib.Tools;
using IT.CoreLib.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace IT.CoreLib.Application
{
    public abstract class ApplicationContext : AbstractContext
    {
        public SceneContext CurrentScene { get; protected set; }
        public static ApplicationContext Instance { 
            get {

                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ApplicationContext>();
                    if (_instance == null)
                        throw new Exception("[CONTEXT] There are no ApplicationContext!");
                }

                return _instance;
            } 
        }
        
        
        protected ApplicationEntryPoint _appEntryPoint;
        protected ApplicationUIContainer _appUIContainer;

        [SerializeField] private ApplicationUIContainer _UIContainerPrefab;
        [SerializeField] private string _defaultSceneName;

        private static ApplicationContext _instance;


        public virtual async UniTask InitializeApplication(ApplicationEntryPoint applicationEP, string redirectSceneName)
        {
            CLDebug.BootLog("Initialization begun");

            _appEntryPoint = applicationEP;
            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeServices();
            OnServicesInitialized();
            InitializeUI(Instantiate(_UIContainerPrefab));

            await LoadScene(redirectSceneName != null ? redirectSceneName : _defaultSceneName, null);
        }

        public async UniTask LoadScene(string name, ISceneContextInputParameters inputParameters)
        {
            CLDebug.BootLog($"Begin loading scene: {name}");

            await _appUIContainer.UITransition.StartTransitionAsync(false);

            if (CurrentScene != null)
                _appUIContainer.RemoveSceneUI(CurrentScene);

            await SceneManager.LoadSceneAsync(0);
            await SceneManager.LoadSceneAsync(name);
            await UniTask.Yield();

            CLDebug.BootLog($"Scene loaded: {name}");

            CurrentScene = await InitializeLoadedSceneContext(name, this, inputParameters);

            Initialized = true;
            _appUIContainer.UITransition.FinishTransition();

            CLDebug.BootLog($"Scene initialized: {name}");
        }

        public async UniTask LoadChildScene(string name, SceneContext parent, ISceneContextInputParameters inputParameters)
        {
            CLDebug.BootLog($"Begin adding scene: {name}");

            using (LifetimeScope.EnqueueParent(parent.SceneScope))
            {
                await SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
                await UniTask.Yield();

                CLDebug.BootLog($"Scene added: {name}");

                await InitializeLoadedSceneContext(name, parent, inputParameters);
            }

            CLDebug.BootLog($"Scene initialized: {name}");
        }

        public async UniTask UnloadChildScene(string name)
        {
            await SceneManager.UnloadSceneAsync(name);
            CLDebug.BootLog($"Scene unloaded: {name}");
        }

        

        protected override void InitializeUI(ApplicationUIContainer uiContainer)
        {
            _appUIContainer = uiContainer;
            DontDestroyOnLoad(_appUIContainer);
            _appUIContainer.Initialize(this);
        }


        private async UniTask<SceneContext> InitializeLoadedSceneContext(string name,
            AbstractContext parent,
            ISceneContextInputParameters inputParameters)
        {
            Scene scene = SceneManager.GetSceneByName(name);
            GameObject[] rootObjects = scene.GetRootGameObjects();
            SceneContext sceneContext = null;
            foreach (var rootObject in rootObjects)
            {
                sceneContext = rootObject.GetComponent<SceneContext>();
                if (sceneContext != null)
                    break;
            }

            if (sceneContext == null)
                throw new Exception($"[BOOT] Can't find SceneContext on scene {name}");

            if (sceneContext.DefaultSubscenes != null)
            {
                foreach (string subscene in sceneContext.DefaultSubscenes)
                {
                    await LoadChildScene(subscene, sceneContext, inputParameters);
                }
            }

            sceneContext.InitializeContext(parent, _appUIContainer, inputParameters);

            return sceneContext;
        }


        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
