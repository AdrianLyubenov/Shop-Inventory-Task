using System.Collections.Generic;
using System;
using UnityEngine;

public class WalletService : IService, IWallet
{
    private CurrencyCatalogSO _catalog;
    private SerializableDictionary<CurrencyType, int> _amounts = new SerializableDictionary<CurrencyType, int>();
    public event Action<CurrencyType, int> onChanged;
    
    public void Initialize(ServiceLocator serviceLocator)
    {
        _catalog = ResourceProvider.Instance.GetCurrencyCatalogSO();
    }
    
    public int GetCurrencyAmount(CurrencyType type)
    {
        if (_amounts.TryGetValue(type, out var v)) return v;
        return 0;
    }
    public int GetCurrencyAmount(CurrencyDefinitionSO def)
    {
        if (def == null) return 0;
        return GetCurrencyAmount(def.type);
    }
    public bool HasEnough(CurrencyType type, int amount)
    {
        if (amount <= 0) return true;
        return GetCurrencyAmount(type) >= amount;
    }
    public bool AddCurrency(CurrencyType type, int amount)
    {
        if (amount == 0) return true;
        if (!_amounts.TryGetValue(type, out var v)) v = 0;
        int target = v + amount;
        _amounts[type] = target;
        onChanged?.Invoke(type, target);
        return true;
    }
    public bool SubtractCurrency(CurrencyType type, int amount)
    {
        if (amount <= 0) return true;
        if (!HasEnough(type, amount)) return false;
        int v = GetCurrencyAmount(type) - amount;
        _amounts[type] = v;
        onChanged?.Invoke(type, v);
        return true;
    }
    public bool HasEnough(CurrencyDefinitionSO def, int amount)
    {
        if (def == null) return false;
        return HasEnough(def.type, amount);
    }
    public bool AddCurrency(CurrencyDefinitionSO def, int amount)
    {
        if (def == null) return false;
        return AddCurrency(def.type, amount);
    }
    public bool SubtractCurrency(CurrencyDefinitionSO def, int amount)
    {
        if (def == null) return false;
        return SubtractCurrency(def.type, amount);
    }

    public bool HasEnoughAll(IReadOnlyList<PriceRequirement> reqs, int multiplier)
    {
        int n = reqs.Count;
        for (int i = 0; i < n; i++)
        {
            var r = reqs[i];
            if (!HasEnough(r.currency, r.amount * multiplier)) return false;
        }
        return true;
    }
    public bool SubtractAll(IReadOnlyList<PriceRequirement> reqs, int multiplier)
    {
        if (!HasEnoughAll(reqs, multiplier)) return false;
        int n = reqs.Count;
        for (int i = 0; i < n; i++)
        {
            var r = reqs[i];
            if (!SubtractCurrency(r.currency, r.amount * multiplier)) return false;
        }
        return true;
    }
}