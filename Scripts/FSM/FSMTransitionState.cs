namespace IT.CoreLib.FSM
{
    public abstract class FSMTransitionState : IFSMState
    {
        protected FSMState _fromState;
        protected FSMState _toState;

        protected bool _transitionFinished;
        protected bool _statesSwitched;


        public FSMTransitionState()
        {

        }

        public void SetStates(FSMState fromState, FSMState toState)
        {
            _fromState = fromState;
            _toState = toState;
        }


        public abstract string Name { get; }

        public virtual IFSMState NextAvaliableState()
        {
            if (_transitionFinished)
            {
                return _toState;
            }

            return null;
        }

        public virtual void OnEnter()
        {
            _transitionFinished = false;
            if (_fromState == null || _toState == null)
            {
                throw new System.NullReferenceException("Can't enter transition state without states setup");
            }
        }

        public virtual void OnExit()
        {

        }

        public virtual void OnUpdate(float deltaTime)
        {
            if (!_statesSwitched)
                _fromState.OnUpdate(deltaTime);
            else 
                _toState.OnUpdate(deltaTime);
        }

        protected virtual void SwitchStates()
        {
            _statesSwitched = true;
            _fromState.OnExit();
            _toState.OnEnter();
        }
    }
}
