using System.Collections.Generic;
using System.Threading.Tasks;
using Bargreen.Services.Interfaces;
using Bargreen.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bargreen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }
        
        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            var result = await _inventoryService.GetInventoryBalances();
            
            if(result == null)
            {
                _logger.LogError("Inventory balances were null");
            }

            return result;
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            var result = await _inventoryService.GetAccountingBalances();
            
            if (result == null)
            {
                _logger.LogError("Accounting balances were null");
            }
            return result;
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            var inventoryBalances = _inventoryService.GetInventoryBalances();
            var accountingBalances = _inventoryService.GetAccountingBalances();
            await Task.WhenAll(inventoryBalances, accountingBalances);
            
            if (inventoryBalances.Result == null)
            {
                _logger.LogError("Inventory balances were null");
            }
            if (accountingBalances.Result == null)
            {
                _logger.LogError("Accounting balances were null");
            }
            
            if(inventoryBalances.Result == null || accountingBalances.Result == null)
            {
                _logger.LogError("Unable to reconcile inventory to accounting");
                return null;
            }
            
            var reconciledResult = await _inventoryService.ReconcileInventoryToAccounting(inventoryBalances.Result, accountingBalances.Result);
            return reconciledResult;
        }
    }
}