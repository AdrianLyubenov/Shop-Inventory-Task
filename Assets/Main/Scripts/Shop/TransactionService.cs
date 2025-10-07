using UnityEngine;

public class TransactionService : IService
{
    public WalletService _wallet;
    public InventoryService _inventory;

    public void Initialize(ServiceLocator serviceLocator)
    {
        _wallet = serviceLocator.Get<WalletService>();
        _inventory = serviceLocator.Get<InventoryService>();
    }

    public TransactionResponse TryPurchase(ShopOfferSO offer, int purchaseQuantity = 1)
    {
        if (offer == null) return TransactionResponse.Fail("Offer is null.");
        if (purchaseQuantity <= 0) return TransactionResponse.Fail("Purchase quantity must be greater than zero.");

        var qty = offer.quantityPerPurchase * purchaseQuantity;

        if (!_inventory.CanAdd(offer.item, qty))
            return TransactionResponse.Fail("Not enough inventory space or maximum quantity per transaction.");

        // Charge player
        if (!_wallet.SubtractAll(offer.price, purchaseQuantity))
            return TransactionResponse.Fail("Not enough currency.");

        // Try to deliver items
        if (!_inventory.Add(offer.item, qty))
        {
            // Refund if something went wrong despite CanAdd earlier, usually if we need to sync with Server
            RefundPrice(offer, purchaseQuantity);
            return TransactionResponse.Fail("Could not add items to inventory. Payment has been refunded.");
        }

        return TransactionResponse.Ok($"Purchased x{qty} {offer.item.name}.");
    }

    public TransactionResponse TrySell(ShopOfferSO offer, int quantity)
    {
        if (offer == null) return TransactionResponse.Fail("Offer is null.");
        if (quantity <= 0) return TransactionResponse.Fail("Sell quantity must be greater than zero.");

        var qty = offer.quantityPerPurchase * quantity;

        if (_inventory.GetQuantity(offer.item) < qty)
            return TransactionResponse.Fail("Not enough items to sell.");

        if (!_inventory.Remove(offer.item, qty))
            return TransactionResponse.Fail("Failed to remove items from inventory.");

        // Pay player
        var n = offer.price.Count;
        for (var i = 0; i < n; i++)
        {
            var r = offer.price[i];
            var reward = r.amount * quantity;
            _wallet.AddCurrency(r.currency, reward);
        }

        return TransactionResponse.Ok($"Sold x{qty} {offer.item.name}.");
    }

    private void RefundPrice(ShopOfferSO offer, int purchaseQuantity)
    {
        var n = offer.price.Count;
        for (var i = 0; i < n; i++)
        {
            var r = offer.price[i];
            var refund = r.amount * purchaseQuantity;
            _wallet.AddCurrency(r.currency, refund);
        }
    }
}

public class TransactionResponse
{
    public bool success;
    public string message;

    public static TransactionResponse Ok(string msg = "OK")
    {
        return new TransactionResponse { success = true, message = msg };
    }

    public static TransactionResponse Fail(string msg)
    {
        return new TransactionResponse { success = false, message = msg };
    }
}