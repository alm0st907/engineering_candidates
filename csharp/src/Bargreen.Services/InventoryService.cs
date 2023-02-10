using System.Collections.Generic;
using System.Threading.Tasks;
using Bargreen.Services.Extensions;
using Bargreen.Services.Interfaces;
using Bargreen.Services.Models;

namespace Bargreen.Services
{
    public class InventoryService : IInventoryService
    {
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            //some sort of async db call would be here and awaited, then returned
            return new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 312,
                     WarehouseLocation = "WLA1"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 146,
                     WarehouseLocation = "WLA2"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ZZZ99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 47,
                     WarehouseLocation = "WLA3"
                },
                new InventoryBalance()
                {
                     ItemNumber = "zzz99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 91,
                     WarehouseLocation = "WLA4"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxccM",
                     PricePerItem = 245.25M,
                     QuantityOnHand = 32,
                     WarehouseLocation = "WLA5"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxddM",
                     PricePerItem = 747.47M,
                     QuantityOnHand = 15,
                     WarehouseLocation = "WLA6"
                }
            };
        }

        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            
            //some sort of async db call would be here and awaited, then returned
            return new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                     ItemNumber = "ABC123",
                     TotalInventoryValue = 3435M
                },
                new AccountingBalance()
                {
                     ItemNumber = "ZZZ99",
                     TotalInventoryValue = 1930.62M
                },
                new AccountingBalance()
                {
                     ItemNumber = "xxccM",
                     TotalInventoryValue = 7602.75M
                },
                new AccountingBalance()
                {
                     ItemNumber = "fbr77",
                     TotalInventoryValue = 17.99M
                }
            };
        }

        public async Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //convert inventory balances to accounting balances as a dictionary for quick checks
            var convertedInventoryBalance = new Dictionary<string, decimal>();
            
            foreach (var balance in inventoryBalances)
            {
                var convertedBalance = balance.ConvertToAccountingBalance();
                
                //account for casing differences and normalize to uppercase
                if (!convertedInventoryBalance.ContainsKey(balance.ItemNumber.ToUpperInvariant()))
                {
                    convertedInventoryBalance.Add(convertedBalance.ItemNumber.ToUpperInvariant(), convertedBalance.TotalInventoryValue);
                }
                else
                {
                    convertedInventoryBalance[convertedBalance.ItemNumber.ToUpperInvariant()] += convertedBalance.TotalInventoryValue;
                }
            }
            
            var results = new List<InventoryReconciliationResult>();
            foreach(var balance in accountingBalances)
            {
                convertedInventoryBalance.TryGetValue(balance.ItemNumber.ToUpperInvariant(), out var inventoryValue);
                if (inventoryValue != balance.TotalInventoryValue)
                {
                    results.Add(new InventoryReconciliationResult()
                    {
                        ItemNumber = balance.ItemNumber,
                        TotalValueInAccountingBalance = balance.TotalInventoryValue,
                        TotalValueOnHandInInventory = inventoryValue
                    });
                }
            }
            
            return results;
        }
    }
}