using System.Threading.Tasks;
using Bargreen.Services;
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
        
    }
}
