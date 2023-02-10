﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Bargreen.Services.Interfaces;
using Bargreen.Services.Models;
using Microsoft.AspNetCore.Http;
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
            var inventoryService = new InventoryService();
            return await inventoryService.GetInventoryBalances();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            var inventoryService = new InventoryService();
            return await inventoryService.GetAccountingBalances();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            var inventoryService = new InventoryService();
            var inventoryBalances = inventoryService.GetInventoryBalances();
            var accountingBalances = inventoryService.GetAccountingBalances();
            Task.WaitAll(inventoryBalances, accountingBalances);
            
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