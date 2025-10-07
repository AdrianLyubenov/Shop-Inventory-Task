using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopView : SimpleView
{
    [SerializeField]
    private List<ItemSlot> _itemSlots = new List<ItemSlot>();
    [SerializeField]
    private TextMeshProUGUI _responseMessage;
    [SerializeField]
    private Button _inventoryButton;
    
    private List<ShopOfferSO> _shopItems = new List<ShopOfferSO>();
    public UnityAction<ShopOfferSO, int> OnItemClicked { get; set; }
    public UnityAction OnInventoryButtonClick { get; set; }

    public override void Show()
    {
        _responseMessage.text = "";
        _inventoryButton.onClick.AddListener(OnInventoryButtonClick);
        base.Show();
    }

    public override void Hide()
    {
        _inventoryButton.onClick.RemoveAllListeners();
        base.Hide();
    }

    public void InitializeShopItems(List<ShopOfferSO> modelShopItems)
    {
        _shopItems = modelShopItems;

        //For less garbage for loop could be used 
        foreach (var pair in _itemSlots.Zip(_shopItems, (slot, item) => (slot, item)))
        {
            if (!pair.slot)
            {
                Debug.LogError("A slot is null");
                continue;
            }
            pair.slot.Initialize(pair.item, OnItemClick);
        }
    }

    private void OnItemClick(ShopOfferSO shopItem, int quantity)
    {
        OnItemClicked?.Invoke(shopItem, quantity);
    }

    public void ShowMessage(string responseMessage)
    {
        _responseMessage.text = responseMessage;
    }
    
    public void ResetSlots()
    {
        _itemSlots.ForEach(x => x.ResetSlot());
    }
}
