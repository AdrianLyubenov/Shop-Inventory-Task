using System.Collections.Generic;
using UnityEngine;

public class ResourceProvider : Singleton<ResourceProvider>
{
    [SerializeField]
    private ShopListSO _shopListSO;
    [SerializeField]
    private CurrencyCatalogSO _currencyCatalogSo;
    [SerializeField]
    private InventoryConfigurationSO _inventoryConfigurationSO;
    [SerializeField]
    private List<BaseInventoryItemSO> _allItems;
    
    public List<ShopOfferSO> GetShopItems()
    {
        return _shopListSO.shopItems;
    }

    public InventoryConfigurationSO GetInventoryConfiguration()
    {
        return _inventoryConfigurationSO;
    }

    public CurrencyCatalogSO GetCurrencyCatalogSO()
    {
        return _currencyCatalogSo;
    }

    public List<BaseInventoryItemSO> GetAllItems()
    {
        return _allItems;
    }
}
