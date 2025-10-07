
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Store/Base InventoryItem", fileName = "BaseInventoryItem")]
public class BaseInventoryItemSO : ScriptableObject,ISerializationCallbackReceiver
{
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ItemTypeEnum _itemType;
    [SerializeField] private bool _stackable = true;
    [SerializeField] private int _maxStack = 99;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private ShopOfferSO _sellOfferSO;

    public string id => _id;
    public string displayName => _displayName;
    public Sprite icon => _icon;
    public ItemTypeEnum ItemTypeEnum => _itemType;
    public bool stackable => _stackable;
    public int maxStack => _maxStack;
    public string description => _description;
    public ShopOfferSO sellOfferSo => _sellOfferSO;

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(_id))
            _id = Guid.NewGuid().ToString();
    }

}