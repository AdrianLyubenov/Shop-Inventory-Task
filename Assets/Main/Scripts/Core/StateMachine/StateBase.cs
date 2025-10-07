public abstract class StateBase<TModel, TView, TController> : IState
    where TModel : IModel
    where TView : IView
    where TController : IController
{
    protected readonly TModel Model;
    protected readonly TView View;
    protected readonly TController Controller;

    protected StateBase(TModel m, TView v, TController c)
    {
        Model = m;
        View = v;
        Controller = c;
    }

    public virtual void Enter()
    {
        View.Show();
        Controller.Initialize();
    }

    public virtual void Exit()
    {
        Controller.Cleanup();
        View.Hide();
    }
}