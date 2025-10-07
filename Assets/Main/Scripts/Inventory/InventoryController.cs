using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : ControllerBase<InventoryModel, InventoryView>
{
    private StateMachine _stateMachine;

    public InventoryController(InventoryModel model, InventoryView view, StateMachine sm) : base(model, view)
    {
        _stateMachine = sm;
    }
    
    public override void Initialize()
    {
        var inventoryService = ServiceLocator.Instance.Get<InventoryService>();
        var inventoryItems = inventoryService.Snapshot();
        var allItems = ResourceProvider.Instance.GetAllItems();
        Model.InventoryItems = BuildInventory(inventoryItems, allItems);
        
        View.OnItemClicked += OnItemClicked;
        View.OnShopButtonClick += OnShopButtonClick;
        View.ResetSlots();
        View.InitializeInventoryItems(Model.InventoryItems);
        View.Show();
    }

    private void OnShopButtonClick()
    {
        _stateMachine.ChangeState(StateId.Shop);
    }

    private Dictionary<BaseInventoryItemSO, int> GetInventoryItemSOs(IReadOnlyDictionary<string, int> inventoryItems, List<BaseInventoryItemSO> allItems)
    {
        var iInventoryItemSOs = new Dictionary<BaseInventoryItemSO, int>(inventoryItems.Count);
        
        foreach (var inventoryItem in inventoryItems)
        {
            var item = allItems.FirstOrDefault(x => x.id == inventoryItem.Key);
            if (item == null)
            {
                Debug.LogError($"Item {inventoryItem.Key} not found");
                break;
            }
            iInventoryItemSOs.Add(item, inventoryItem.Value);
        }
        
        return iInventoryItemSOs;
    }
    
    private static List<InventoryItem> BuildInventory(
        IReadOnlyDictionary<string, int> inventoryItems,
        IReadOnlyList<BaseInventoryItemSO> allItems,
        List<string>? missingIds = null) 
    {
        // Index catalog by id
        var idToItem = new Dictionary<string, BaseInventoryItemSO>(allItems.Count);
        for (int i = 0; i < allItems.Count; i++)
        {
            var it = allItems[i];
            idToItem[it.id] = it; 
        }

        // Translate id->count to InventoryItem list
        var result = new List<InventoryItem>(inventoryItems.Count);

        foreach (var kvp in inventoryItems)
        {
            if (idToItem.TryGetValue(kvp.Key, out var so))
            {
                result.Add(new InventoryItem(so, kvp.Value));
            }
            else
            {
                missingIds?.Add(kvp.Key);
            }
        }

        return result;
    }

    private void OnItemClicked(ShopOfferSO shopOffer, int quantity)
    {
        var transactionService = ServiceLocator.Instance.Get<TransactionService>();
        var response = transactionService.TrySell(shopOffer, quantity);
        View.ShowMessage(response.message);

        if (response.success)
            View.RemoveItem(shopOffer);
    }

    public override void Cleanup()
    {
        View.OnItemClicked -= OnItemClicked;
        View.OnShopButtonClick -= OnShopButtonClick;
        View.Hide();
    }
}
