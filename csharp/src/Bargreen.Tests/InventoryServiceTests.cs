using System.Collections.Generic;
using System.Threading.Tasks;
using Bargreen.Services;
using Bargreen.Services.Models;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public async Task Inventory_Reconciliation_Performs_As_Expected()
        {
            //use default data as a test case for mismatched inven/accounting   
            var inventoryService = new InventoryService();
            var inventoryBalances = await inventoryService.GetInventoryBalances();
            var accountingBalances = await inventoryService.GetAccountingBalances();
            var reconciliationResults = await inventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);
            Assert.NotEmpty(reconciliationResults);
        }
        
        [Fact]
        public async Task Inventory_Reconciliation_Works_MatchedData()
        {
            var inventoryService = new InventoryService();
            var inventoryBalances = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                    ItemNumber = "A",
                    WarehouseLocation = "A",
                    PricePerItem = 1M,
                    QuantityOnHand = 1
                },
            };
            var accountingBalances = new List<AccountingBalance>()
            {
              new AccountingBalance()
              {
                  ItemNumber = "A",
                  TotalInventoryValue = 1M
              }
            };
            var reconciliationResults = await inventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);
            Assert.Empty(reconciliationResults);
        }
        
        [Fact]
        public async Task Inventory_Reconciliation_Works_MatchedData_MultiWarehouse()
        {
            var inventoryService = new InventoryService();
            var inventoryBalances = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                    ItemNumber = "A",
                    WarehouseLocation = "A",
                    PricePerItem = 1M,
                    QuantityOnHand = 1
                },
                new InventoryBalance()
                {
                    ItemNumber = "A",
                    WarehouseLocation = "B",
                    PricePerItem = 1M,
                    QuantityOnHand = 1
                },
            };
            var accountingBalances = new List<AccountingBalance>()
            {
              new AccountingBalance()
              {
                  ItemNumber = "A",
                  TotalInventoryValue = 2M
              }
            };
            var reconciliationResults = await inventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);
            Assert.Empty(reconciliationResults);
        }
        
        
        
        [Fact]
        public async Task Inventory_Reconciliation_Works_MismatchedCasingInven()
        {
            var inventoryService = new InventoryService();
            var inventoryBalances = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                    ItemNumber = "a",
                    WarehouseLocation = "A",
                    PricePerItem = 1M,
                    QuantityOnHand = 1
                },
            };
            var accountingBalances = new List<AccountingBalance>()
            {
              new AccountingBalance()
              {
                  ItemNumber = "A",
                  TotalInventoryValue = 1M
              }
            };
            var reconciliationResults = await inventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);
            Assert.Empty(reconciliationResults);
        }
        
        
        [Fact]
        public async Task Inventory_Reconciliation_Works_MismatchedCasingAccounting()
        {
            var inventoryService = new InventoryService();
            var inventoryBalances = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                    ItemNumber = "A",
                    WarehouseLocation = "A",
                    PricePerItem = 1M,
                    QuantityOnHand = 1
                },
            };
            var accountingBalances = new List<AccountingBalance>()
            {
              new AccountingBalance()
              {
                  ItemNumber = "a",
                  TotalInventoryValue = 1M
              }
            };
            var reconciliationResults = await inventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);
            Assert.Empty(reconciliationResults);
        }
    }
}
