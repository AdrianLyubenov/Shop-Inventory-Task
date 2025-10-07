using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryView : SimpleView
{
    [SerializeField]
    private List<ItemSlot> _itemSlots = new List<ItemSlot>();
    [SerializeField]
    private TextMeshProUGUI _responseMessage;
    [SerializeField]
    private Button _shopButton;
    
    private List<InventoryItem> _modelInventoryItems = new List<InventoryItem>();
    public UnityAction<ShopOfferSO, int> OnItemClicked { get; set; }
    public UnityAction OnShopButtonClick { get; set; }


    public override void Show()
    {
        _responseMessage.text = "";
        _shopButton.onClick.AddListener(OnShopButtonClick);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void InitializeInventoryItems(List<InventoryItem> modelInventoryItems)
    {
        _modelInventoryItems = modelInventoryItems;

        //For less garbage for loop could be used 
        foreach (var pair in _itemSlots.Zip(_modelInventoryItems, (slot, item) => (slot, item)))
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

    public void RemoveItem(ShopOfferSO shopOffer)
    {
        var slot = _itemSlots.FirstOrDefault(x => x.itemId == shopOffer.item.id);
        slot.ResetSlot();
    }

    public void ResetSlots()
    {
        _itemSlots.ForEach(x => x.ResetSlot());
    }
}
