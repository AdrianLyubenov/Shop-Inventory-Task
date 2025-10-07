using System;

public interface IWallet
{
    event Action<CurrencyType, int> onChanged;
    int GetCurrencyAmount(CurrencyType type);
    bool HasEnough(CurrencyType type, int amount);
    bool AddCurrency(CurrencyType type, int amount);
    bool SubtractCurrency(CurrencyType type, int amount);
    int GetCurrencyAmount(CurrencyDefinitionSO def);
    bool HasEnough(CurrencyDefinitionSO def, int amount);
    bool AddCurrency(CurrencyDefinitionSO def, int amount);
    bool SubtractCurrency(CurrencyDefinitionSO def, int amount);
}