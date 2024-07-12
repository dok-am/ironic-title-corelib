using UnityEngine;

namespace IT.CoreLib.FSM
{

    public interface IFSMState
    {
        public string Name { get; }
        public void OnEnter();
        public void OnExit();
        public IFSMState NextAvaliableState();
        public void OnUpdate(float deltaTime);
        

        /// <summary>
        /// Optional fixed update
        /// </summary>
        /// <param name="deltaTime">Fixed delta time</param>
        public void OnFixedUpdate(float deltaTime)
        {

        }
    }

}
