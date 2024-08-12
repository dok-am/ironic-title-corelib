using IT.CoreLib.Tools;
using IT.CoreLib.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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


        public virtual async Task InitializeApplication(ApplicationEntryPoint applicationEP, string redirectSceneName)
        {
            CLDebug.BootLog("Initialization begun");

            _appEntryPoint = applicationEP;
            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeInternal();
            InitializeServices();
            OnServicesInitialized();
            InitializeUI(Instantiate(_UIContainerPrefab));

            await LoadScene(redirectSceneName == null ? redirectSceneName : _defaultSceneName);
        }

        public virtual async Task LoadScene(string name)
        {
            CLDebug.BootLog("Begin transition");

            await _appUIContainer.UITransition.StartTransitionAsync(false);

            //TODO: Check if it is a correct point to do this
            _appUIContainer.RemoveCurrentSceneUI();

            await SceneManager.LoadSceneAsync(0);

            CLDebug.BootLog("Begin loading scene");
            await SceneManager.LoadSceneAsync(name);
            await Task.Yield();

            CLDebug.BootLog("Scene loaded");
            CurrentScene = FindFirstObjectByType<SceneContext>();
            if (CurrentScene == null)
                throw new Exception($"There are no SceneContext on scene {name}");
                        
            CurrentScene.InitializeContext(this, _appUIContainer);

            CLDebug.BootLog("Scene initialized");

            Initialized = true;
            _appUIContainer.UITransition.FinishTransition();

            CLDebug.BootLog("Transition completed");
        }


        //TODO: Possible problems here?
        protected override void InitializeUI(ApplicationUIContainer uiContainer)
        {
            _appUIContainer = uiContainer;
            DontDestroyOnLoad(_appUIContainer);
            _appUIContainer.Initialize(this);
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
