using Bargreen.API.Controllers;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bargreen.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryControllerTests
    {
        [Fact]
        public async Task InventoryController_Can_Return_Inventory_Balances()
        {
            var inventoryService = new InventoryService();
            var controller = new InventoryController(inventoryService, new NullLogger<InventoryController>());
            var result = await controller.GetInventoryBalances();
            Assert.NotEmpty(result);
        }
        
        [Fact]
        public async Task InventoryController_Can_Return_Accounting_Balances()
        {
            var inventoryService = new InventoryService();
            var controller = new InventoryController(inventoryService, new NullLogger<InventoryController>());
            var result = await controller.GetAccountingBalances();
            Assert.NotEmpty(result);
        }
        
        //test for reconciliation
        [Fact]
        public async Task InventoryController_Can_Return_Reconciliation_Results()
        {
            var inventoryService = new InventoryService();
            var controller = new InventoryController(inventoryService, new NullLogger<InventoryController>());
            var result = await controller.GetReconciliation();
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Controller_Methods_Are_Async()
        {
            var methods = typeof(InventoryController)
                .GetMethods()
                .Where(m=>m.DeclaringType==typeof(InventoryController));

            Assert.All(methods, m =>
            {
                Type attType = typeof(AsyncStateMachineAttribute); 
                var attrib = (AsyncStateMachineAttribute)m.GetCustomAttribute(attType);
                Assert.NotNull(attrib);
                Assert.Equal(typeof(Task), m.ReturnType.BaseType);
            });
        }
    }
}
