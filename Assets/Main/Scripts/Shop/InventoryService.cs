using System;
using System.Collections.Generic;

public class InventoryService : IService
{
    private int _inventoryCapacity;
    private SerializableDictionary<string, int> _items = new();
    public event Action<BaseInventoryItemSO, int> onChanged;

    public void Initialize(ServiceLocator serviceLocator)
    {
        var inventoryConfiguration = ResourceProvider.Instance.GetInventoryConfiguration();
        _inventoryCapacity = inventoryConfiguration.inventoryCapacity;
    }
    
    public int GetQuantity(BaseInventoryItemSO item)
    {
        if (item == null) return 0;
        if (_items.TryGetValue(item.id, out var v)) return v;
        return 0;
    }

    public bool CanAdd(BaseInventoryItemSO item, int qty)
    {
        if (item == null || qty <= 0) return false;
        var occupied = 0;
        // using (var e = _items.GetEnumerator())
        // {
        //     while (e.MoveNext()) occupied++;
        // }

        var hasSlot = _items.ContainsKey(item.id) || occupied < _inventoryCapacity;
        if (!hasSlot) return false;
        var current = GetQuantity(item);
        var target = (long)current + qty;
        if (item.stackable) return target <= item.maxStack || _items.ContainsKey(item.id) == false;
        return qty == 1 && !_items.ContainsKey(item.id);
    }

    public bool Add(BaseInventoryItemSO item, int qty)
    {
        if (!CanAdd(item, qty)) return false;
        var v = GetQuantity(item) + qty;
        _items[item.id] = v;
        onChanged?.Invoke(item, v);
        return true;
    }

    public bool Remove(BaseInventoryItemSO item, int qty)
    {
        if (item == null || qty <= 0) return false;
        var current = GetQuantity(item);
        if (current < qty) return false;
        var v = current - qty;
        if (v == 0) _items.Remove(item.id);
        else _items[item.id] = v;
        // onChanged?.Invoke(item, v);
        return true;
    }

    public IReadOnlyDictionary<string, int> Snapshot()
    {
        return _items;
    }
}