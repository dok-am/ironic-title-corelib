using IT.CoreLib.Tools;
using System.Collections.Generic;

namespace IT.CoreLib.FSM
{
    public class FSMBase
    {
        private List<IFSMState> _statesList;
        private IFSMState _currentState;

        private string _name;


        public FSMBase(string name, IFSMState defaultState)
        {
            _name = name;
            _statesList = new List<IFSMState>() { defaultState };
            ChangeState(defaultState);
        }

        public void AddState(IFSMState state)
        {
            _statesList.Add(state);
        }

        public void ChangeState(IFSMState state)
        {
            bool isTransition = _currentState is FSMTransitionState;

            if (_currentState != null && state is not FSMTransitionState)
                _currentState.OnExit();

            _currentState = state;
            CLDebug.Log("FMS: " + _name + " is Entering state " + _currentState.Name);

            if (!isTransition)
                _currentState.OnEnter();
        }

        public void Update(float deltaTime)
        {
            IFSMState nextState = _currentState.NextAvaliableState();
            if (nextState != null)
            {
                ChangeState(nextState);
            }

            _currentState.OnUpdate(deltaTime);
        }
    }
}
