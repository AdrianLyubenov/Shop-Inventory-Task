using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/InventoryConfiguration")]
public class InventoryConfigurationSO : ScriptableObject
{
    [SerializeField] private int _inventoryCapacity = 9;
    public int inventoryCapacity => _inventoryCapacity;
}