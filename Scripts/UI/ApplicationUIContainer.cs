using UnityEngine;
using System;
using IT.CoreLib.Application;

namespace IT.CoreLib.UI
{
    public class ApplicationUIContainer : MonoBehaviour
    {
        public UITransitionBase UITransition => _sceneUITransition;


        [SerializeField] protected Transform _sceneUiContainer;
        [SerializeField] protected UITransitionBase _sceneUITransition;

        protected SceneUIBase _currentSceneUI;


        public virtual void Initialize(ApplicationContext appBoostrap)
        {
            _sceneUITransition.Initialize();
        }

        public virtual SceneUIBase AddSceneUI(SceneUIBase prefab, SceneContext scene)
        {
            if (_currentSceneUI != null)
                throw new Exception("[UI] ERROR: Previous scene UI wasn't removed");

            _currentSceneUI = Instantiate(prefab, _sceneUiContainer);
            _currentSceneUI.Initialize(scene, this);
            return _currentSceneUI;
        }

        public virtual void RemoveCurrentSceneUI()
        {
            if (_currentSceneUI == null)
                return;

            _currentSceneUI.Deinitialize();
            Destroy(_currentSceneUI.gameObject);
            _currentSceneUI = null;
        }
    }
}
