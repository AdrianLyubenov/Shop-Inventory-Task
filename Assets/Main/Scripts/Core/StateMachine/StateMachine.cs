public class StateMachine
{
    private readonly System.Collections.Generic.Dictionary<StateId, IState> _map = new();
    private IState _currentState;

    public StateMachine Add(StateId id, IState state)
    {
        _map[id] = state;
        return this;
    }

    public void ChangeState(StateId id)
    {
        _currentState?.Exit();
        _currentState = _map[id];
        _currentState.Enter();
    }

    public IState GetCurrentState() => _currentState;
}

public enum StateId
{
    Shop = 0,
    Inventory = 1
}