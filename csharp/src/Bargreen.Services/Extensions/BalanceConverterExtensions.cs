using Bargreen.Services.Models;

namespace Bargreen.Services.Extensions
{
    public static class BalanceConverterExtensions
    {
        public static AccountingBalance ConvertToAccountingBalance(this InventoryBalance inventoryBalance)
        {
            return new AccountingBalance()
            {
                ItemNumber = inventoryBalance.ItemNumber,
                TotalInventoryValue = inventoryBalance.QuantityOnHand * inventoryBalance.PricePerItem
            };
        } 
    }
}