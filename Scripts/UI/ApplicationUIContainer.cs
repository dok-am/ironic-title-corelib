using UnityEngine;
using System;
using IT.CoreLib.Application;
using IT.CoreLib.Interfaces;
using System.Collections.Generic;

namespace IT.CoreLib.UI
{
    public class ApplicationUIContainer : MonoBehaviour
    {
        public UITransitionBase UITransition => _sceneUITransition;


        [SerializeField] protected Transform _sceneUiContainer;
        [SerializeField] protected UITransitionBase _sceneUITransition;

        protected Dictionary<SceneContext, SceneUIBase> _sceneUIs = new();


        public virtual void Initialize(ApplicationContext appBoostrap)
        {
            _sceneUITransition.Initialize();
        }

        public SceneUIBase AddSceneUI(SceneUIBase prefab, SceneContext scene)
        {
            if (prefab == null)
                throw new Exception("[UI] ERROR: Can't create Scene UI: prefab is null");

            if (_sceneUIs.ContainsKey(scene))
                throw new Exception("[UI] ERROR: Previous scene UI wasn't removed");

            var currentSceneUI = Instantiate(prefab, _sceneUiContainer);
            currentSceneUI.Initialize(scene, this);
            _sceneUIs.Add(scene, currentSceneUI);

            return currentSceneUI;
        }

        public void RemoveSceneUI(SceneContext scene)
        {
            if (!_sceneUIs.ContainsKey(scene))
                return;

            var sceneUI = _sceneUIs[scene];
            sceneUI.Deinitialize();

            _sceneUIs.Remove(scene);

            Destroy(sceneUI.gameObject);
        }
    }
}
