using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store/Currency Catalog", fileName = "CurrencyCatalog")]
public class CurrencyCatalogSO : ScriptableObject
{
    [SerializeField] private List<CurrencyDefinitionSO> _definitions = new List<CurrencyDefinitionSO>();
    private Dictionary<CurrencyType, CurrencyDefinitionSO> _map = new Dictionary<CurrencyType, CurrencyDefinitionSO>();

    void OnEnable()
    {
        _map.Clear();
        for (int i = 0; i < _definitions.Count; i++)
        {
            var def = _definitions[i];
            if (def != null && !_map.ContainsKey(def.type))
                _map.Add(def.type, def);
        }
    }

    public bool TryGet(CurrencyType type, out CurrencyDefinitionSO def)
    {
        if (_map.Count == 0) OnEnable();
        return _map.TryGetValue(type, out def);
    }

    public CurrencyDefinitionSO Get(CurrencyType type)
    {
        if (_map.Count == 0) OnEnable();
        _map.TryGetValue(type, out var def);
        return def;
    }

    public IEnumerable<CurrencyDefinitionSO> All()
    {
        return _definitions;
    }
}