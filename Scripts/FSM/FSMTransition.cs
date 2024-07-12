
namespace IT.CoreLib.FSM
{

    public delegate bool FSMCondition(FSMState thisState, FSMState otherState);

    public class FSMTransition
    {
        private FSMState _fromState;
        private FSMState _toState;
        private FSMCondition _condition;
        private FSMTransitionState _transitionState;

        public FSMTransition(FSMState fromState, FSMState toState, FSMCondition condition = null, FSMTransitionState transitionState = null)
        {
            _fromState = fromState;
            _toState = toState;
            _condition = condition;
            _transitionState = transitionState;

            if (transitionState != null)
            {
                _transitionState.SetStates(fromState, toState);
            }
        }

        public bool CheckTransition()
        {
            if (_condition != null)
            {
                return _condition.Invoke(_fromState, _toState);
            }

            return true;
        }

        public IFSMState GetNextState()
        {
            return _transitionState != null ? _transitionState : _toState;
        }
    }

}
