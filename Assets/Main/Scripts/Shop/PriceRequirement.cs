using System;
using UnityEngine;

[Serializable]
public struct PriceRequirement
{
    [SerializeField] private CurrencyType _currency;
    [SerializeField] private int _amount;
    public CurrencyType currency => _currency;
    public int amount => _amount;
    public PriceRequirement(CurrencyType currency, int amount)
    {
        _currency = currency;
        _amount = amount;
    }
}
