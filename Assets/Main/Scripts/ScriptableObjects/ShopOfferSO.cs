using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Store/ShopOffer")]
public class ShopOfferSO : ScriptableObject
{
    [SerializeField] private BaseInventoryItemSO _item;
    [SerializeField] private int _quantityPerPurchase = 1;
    [SerializeField] private List<PriceRequirement> _price = new List<PriceRequirement>();
    public BaseInventoryItemSO item => _item;
    public int quantityPerPurchase => _quantityPerPurchase;
    public IReadOnlyList<PriceRequirement> price => _price;

    public bool HasCurrency(CurrencyType type, out int amount)
    {
        // Potentially could be for loop for less garbage if using it often.
        foreach (var t in _price.Where(t => t.currency == type))
        {
            amount = t.amount;
            return true;
        }

        amount = 0;
        return false;
    }
}