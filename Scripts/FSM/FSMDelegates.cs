namespace IT.CoreLib.FSM
{
    public delegate bool FSMCondition(FSMState thisState, FSMState otherState);
}