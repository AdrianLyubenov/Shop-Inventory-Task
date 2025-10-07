using System;
using System.Collections.Generic;
using UnityEngine;

    public class ShopServicesProvider : ServicesProvider
    {
        protected override void PrepareServicesToAdd()
        {
            _services = new Dictionary<Type, object>
            {
                { typeof(WalletService), new WalletService() },
                { typeof(InventoryService), new InventoryService() },
                { typeof(TransactionService), new TransactionService() },
            };

            base.PrepareServicesToAdd();
        }
    }
