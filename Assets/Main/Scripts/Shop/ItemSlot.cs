using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemNameText;
    [SerializeField]
    private GameObject _descriptionBox;
    [SerializeField]
    private TextMeshProUGUI _descriptionText;
    
    [Header("Currencies")]
    [SerializeField]
    private TextMeshProUGUI _goldText;
    [SerializeField]
    private TextMeshProUGUI _gemText;

    [SerializeField] 
    private GameObject _goldObject;
    [SerializeField] 
    private GameObject _gemObject;

    private BaseInventoryItemSO _itemSo;
    private ShopOfferSO _shopOfferSo;
    private int _quantity;

    public string itemId => _itemSo?.id;

    private Action<ShopOfferSO, int> OnItemClickedCallback;
    
    public void Initialize(ShopOfferSO shopOfferSomSo, Action<ShopOfferSO, int> onItemClicked)
    {
        _shopOfferSo = shopOfferSomSo;
        _itemSo = shopOfferSomSo.item;
        OnItemClickedCallback += onItemClicked;
        _quantity = 1;
        FillData(_quantity);
    }
    
    public void Initialize(InventoryItem inventoryItem, Action<ShopOfferSO, int> onItemClicked)
    {
        _shopOfferSo = inventoryItem.Item.sellOfferSo;
        _itemSo = inventoryItem.Item;
        OnItemClickedCallback = onItemClicked;
        _quantity = inventoryItem.Quantity;
        FillData(_quantity);
    }

    private void FillData(int quantity)
    {
        _itemImage.sprite = _itemSo.icon;
        _itemNameText.text = $"{_itemSo.name} x {quantity}";  
        _descriptionText.text = _itemSo.description;

        if (_shopOfferSo.HasCurrency(CurrencyType.Gold, out var goldAmount))
        {
            _goldText.text = goldAmount.ToString();
            _goldObject.SetActive(true);
        }
        
        if (_shopOfferSo.HasCurrency(CurrencyType.Gems, out var gemAmount))
        {
            _gemText.text = gemAmount.ToString();
            _gemObject.SetActive(true);
        }
    }
    
    public void OnMouseEnter()
    {
        if (_itemSo)
            _descriptionBox.SetActive(true);
    }

    public void OnMouseExit()
    {
        _descriptionBox.SetActive(false);
    }

    public void OnItemClicked()
    {
        OnItemClickedCallback?.Invoke(_shopOfferSo, _quantity);
    }

    public void ResetSlot()
    {
        _itemSo = null;
        _shopOfferSo = null;
        _itemImage.sprite = null;
        _itemNameText.text = string.Empty;
        _gemObject.SetActive(false);
        _goldObject.SetActive(false);
        OnItemClickedCallback = null;
    }
}
