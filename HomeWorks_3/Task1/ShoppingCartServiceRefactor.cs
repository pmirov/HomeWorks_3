using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class ShoppingCartServiceRefactor
    {
           
        public decimal CalculateTotalPriceWithQuantities(string customerType, Dictionary<decimal, int> itemsWithQuantities)
        {

            decimal baseTotal = itemsWithQuantities.Sum(p => p.Value * p.Key);
            decimal discount = CalculateDiscount(customerType, baseTotal);
            decimal finalPrice = CalculateFinalPrice(baseTotal, discount);
            PrintFinalPrice(baseTotal, discount, finalPrice);
            return finalPrice;

        }

        private static void PrintFinalPrice(decimal baseTotal, decimal discount, decimal finalPrice)
        {
            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");
        }

        private static decimal CalculateFinalPrice(decimal baseTotal, decimal discount)
        {
            return baseTotal - discount;
        }

        private decimal CalculateDiscount(string customerType, decimal baseTotal)
        {
            decimal discount = 0;
            if (customerType == "Regular")
            {
                discount = baseTotal * 0.05m; // 5%
            }
            return discount;
        }
          
    }
}
