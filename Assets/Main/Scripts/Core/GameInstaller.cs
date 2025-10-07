using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [Header("Views in the scene")]
    [SerializeField] private ShopView  shopView;
    [SerializeField] private InventoryView  inventoryView;

    private StateMachine _sm;

    private void Start()
    {
        _sm = new StateMachine();

        var shop  = new ShopState(new ShopController(new ShopModel(), shopView, _sm));
        var inventory  = new InventoryState(new InventoryController(new InventoryModel(), inventoryView, _sm));

        _sm.Add(StateId.Shop, shop)
            .Add(StateId.Inventory, inventory);

        _sm.ChangeState(StateId.Shop);

        var wallet = ServiceLocator.Instance.Get<WalletService>();
        wallet.AddCurrency(CurrencyType.Gems, 10000);
        wallet.AddCurrency(CurrencyType.Gold, 10000);
    }
}
