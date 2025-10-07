using System.Transactions;
using UnityEngine;

public class ShopController : ControllerBase<ShopModel, ShopView>
{
    private StateMachine _stateMachine;

    public ShopController(ShopModel model, ShopView view, StateMachine sm) : base(model, view)
    {
        _stateMachine = sm;
    }
    
    public override void Initialize()
    {
        Model.ShopItems = ResourceProvider.Instance.GetShopItems();
        View.OnItemClicked += OnItemClicked;
        View.ResetSlots();
        View.InitializeShopItems(Model.ShopItems);
        View.OnInventoryButtonClick += OnInventoryButtonClick;
        View.Show();
    }

    private void OnInventoryButtonClick()
    {
        _stateMachine.ChangeState(StateId.Inventory);
    }

    private void OnItemClicked(ShopOfferSO shopOffer, int quantity)
    {
        var transactionService = ServiceLocator.Instance.Get<TransactionService>();
        var response = transactionService.TryPurchase(shopOffer);
        View.ShowMessage(response.message);
    }

    public override void Cleanup()
    {
        View.OnInventoryButtonClick -= OnInventoryButtonClick;
        View.OnItemClicked -= OnItemClicked;
        View.Hide();
    }
}
