using TMPro;
using UnityEngine;

public class CurrencyHeader : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI goldCurrencyText;
    [SerializeField] 
    private TextMeshProUGUI gemCurrencyText;

    private void Start()
    {
        var walletService = ServiceLocator.Instance.Get<WalletService>();
        walletService.onChanged += WalletServiceOnChanged;
    }

    private void WalletServiceOnChanged(CurrencyType currencyType, int newAmonunt)
    {
        if(currencyType == CurrencyType.Gold)
            goldCurrencyText.text = newAmonunt.ToString();
        
        if(currencyType == CurrencyType.Gems)
            gemCurrencyText.text = newAmonunt.ToString();
    }

    private void OnDestroy()
    {
        var walletService = ServiceLocator.Instance.Get<WalletService>();
        walletService.onChanged -= WalletServiceOnChanged;
    }
}
