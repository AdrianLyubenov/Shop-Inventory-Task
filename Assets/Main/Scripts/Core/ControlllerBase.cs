public abstract class ControllerBase<TModel, TView> : IController
    where TModel : IModel
    where TView  : IView
{
    protected readonly TModel Model;
    protected readonly TView  View;

    protected ControllerBase(TModel model, TView view)
    {
        Model = model;
        View  = view;
    }

    public virtual void Initialize() {}
    public virtual void Cleanup() {}
}