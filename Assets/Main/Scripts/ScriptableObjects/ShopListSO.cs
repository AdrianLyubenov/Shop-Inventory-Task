using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store/ShopList")]
public class ShopListSO : ScriptableObject
{
    [SerializeField] private List<ShopOfferSO> _shopItems = new List<ShopOfferSO>();
    public List<ShopOfferSO> shopItems => _shopItems;
}