using IT.CoreLib.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IT.CoreLib.Application
{

    public abstract class ApplicationBootstrap : AbstractBootstrap
    {
        [SerializeField] ApplicationUIContainer UIContainerPrefab;

        public static ApplicationBootstrap Instance { 
            get {

                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ApplicationBootstrap>();
                    if (_instance == null)
                        throw new Exception("[BOOTSTRAP] There are no ApplicationBootstrap!");
                }

                return _instance;
            } 
        }
        private static ApplicationBootstrap _instance;

        public SceneBootstrap CurrentScene { get; protected set; }


        protected ApplicationEntryPoint _appEntryPoint;
        protected ApplicationUIContainer _appUIContainer;
        

        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            if (_instance != this)
            {
                Destroy(gameObject);
                throw new Exception("There are more than one application bootstrap!");
            }
        }

        public virtual void InitializeApplication(ApplicationEntryPoint applicationEP, int redirectSceneIndex)
        {
            _appEntryPoint = applicationEP;
            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeInternal();
            InitializeServices();
            OnServicesInitialized();
            InitializeUI(Instantiate(UIContainerPrefab));

            LoadScene(redirectSceneIndex != 0 ? redirectSceneIndex : 1);
        }

        //TODO: Possible problems here, should be changed
        protected override void InitializeUI(ApplicationUIContainer uiContainer)
        {
            _appUIContainer = uiContainer;
            DontDestroyOnLoad(_appUIContainer);
            _appUIContainer.Initialize(this);
        }

        public virtual async void LoadScene(int index)
        {
            await _appUIContainer.UITransition.StartTransitionAsync(false);
            await SceneManager.LoadSceneAsync(index);
            await Task.Yield();

            CurrentScene = FindFirstObjectByType<SceneBootstrap>();
            if (CurrentScene == null)
                throw new Exception($"There are no SceneBootstrap on scene {index}");

            CurrentScene.InitializeBootstrap(this, _appUIContainer);

            Initialized = true;
            _appUIContainer.UITransition.FinishTransition();
        }

    }

}