using UnityEngine;

public class SimpleState : IState
{
    private readonly IController _controller;
    
    protected SimpleState(IController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Initialize();
    }

    public void Exit()
    {
        _controller.Cleanup();
    }
}