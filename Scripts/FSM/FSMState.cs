using System.Collections.Generic;

namespace IT.CoreLib.FSM
{
    public abstract class FSMState : IFSMState
    {
        public string Name => _name;

        protected List<FSMTransition> _transitions;

        private readonly string _name;


        // TODO: Add contexts?
        protected FSMState(string name)
        {
            _name = name;
            _transitions = new List<FSMTransition>();
        }


        /// <summary>
        /// Adding transition to other state
        /// </summary>
        /// <param name="otherState">Other FSM state</param>
        /// <param name="condition">Condition for transition</param>
        /// <returns>Current state to concatenate</returns>
        public FSMState GoTo(FSMState otherState, FSMCondition condition = null, FSMTransitionState transitionState = null)
        {
            _transitions.Add(new FSMTransition(this, otherState, condition, transitionState));
            return this;
        }

        /// <summary>
        /// Check if transition avaliable
        /// </summary>
        /// <returns>Next state OR null if not avaliable</returns>
        public IFSMState NextAvaliableState()
        {
            for (int i = 0; i < _transitions.Count; i++)
            {
                if (_transitions[i].CheckTransition())
                {
                    return _transitions[i].GetNextState();
                }
            }

            return null;
        }

        /// 
        /// State life cycle
        /// 
        public abstract void OnEnter();
        public abstract void OnUpdate(float deltaTime);
        public abstract void OnExit();
    }
}
