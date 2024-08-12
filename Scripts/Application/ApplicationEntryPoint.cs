using IT.CoreLib.Tools;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IT.CoreLib.Application
{
    public class ApplicationEntryPoint  
    {
        private static ApplicationEntryPoint _appEntryPoint;


        ApplicationEntryPoint()
        {

        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ApplicationInit()
        {
            _appEntryPoint = new ApplicationEntryPoint();
            _appEntryPoint.RunApplication();
        }
        

        private void RunApplication()
        {
            string redirectSceneName = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(0);

            InitializeContext(redirectSceneName);
        }

        private async void InitializeContext(string redirectSceneName)
        {
            await Task.Yield();

            CLDebug.BootLog("Bootstrap scene loaded");

            ApplicationContext sceneEP = UnityEngine.Object.FindFirstObjectByType<ApplicationContext>();
            if (sceneEP == null)
                throw new Exception("0 scene is not an application bootstrap scene!");

            await sceneEP.InitializeApplication(this, redirectSceneName);

            CLDebug.BootLog("Application initialization finished");
        }
    }
}
