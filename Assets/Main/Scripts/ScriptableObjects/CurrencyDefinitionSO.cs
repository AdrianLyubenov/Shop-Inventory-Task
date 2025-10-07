

using UnityEngine;

[CreateAssetMenu(menuName = "Store/Currency Definition", fileName = "NewCurrency")]
public class CurrencyDefinitionSO :ScriptableObject
{
    [SerializeField] private CurrencyType _type;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _value;
    public CurrencyType type => _type;
    public string displayName => _displayName;
    public Sprite icon => _icon;
    public int value => _value;
}
public enum CurrencyType
{
    None,
    Gold,
    Gems
}