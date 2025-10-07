using UnityEngine;

public class InventoryItem 
{
    public BaseInventoryItemSO Item { get; set; }
    public int Quantity { get; set; }
    
    public InventoryItem(BaseInventoryItemSO item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}
