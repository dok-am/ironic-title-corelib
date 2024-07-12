using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IT.CoreLib.Application
{

    public class ApplicationEntryPoint  
    {

        private static ApplicationEntryPoint _appEntryPoint;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ApplicationInit()
        {
            _appEntryPoint = new ApplicationEntryPoint();
            _appEntryPoint.RunApplication();
        }

        ApplicationEntryPoint()
        {

        }

        private void RunApplication()
        {
            int redirectSceneIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(0);

            InitializeBootstrap(redirectSceneIndex);
        }

        private async void InitializeBootstrap(int redirectSceneIndex)
        {
            await Task.Yield();

            ApplicationBootstrap sceneEP = UnityEngine.Object.FindFirstObjectByType<ApplicationBootstrap>();
            if (sceneEP == null)
                throw new Exception("0 scene is not an application bootstrap scene!");

            sceneEP.InitializeApplication(this, redirectSceneIndex);
        }

    }

}
