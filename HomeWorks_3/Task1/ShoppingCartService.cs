using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class ShoppingCartService
    {
        //избыточный метод, можно остать один метод CalculateTotalPriceWithQuantities (принцип KISS)
        public decimal CalculateTotalPrice(string customerType, List<decimal> itemPrices)
        {
            //В 14-18 строках нарушен принцип KISS, излишне усложнено решение задачи
            decimal baseTotal = 0;
            for (int i = 0; i < itemPrices.Count; i++)
            {
                baseTotal += itemPrices[i];
            }

            decimal discount = 0;


            if (customerType == "Regular")
            {
                discount = baseTotal * 0.05m; // 5%
            }

            //В строках 27 - 38 нарушен принцип YAGNI, выполнена реализация которая не тробовалась по заданию
            else if (customerType == "Premium")
            {
                //В строках 23,30,38 нарушен принцип DRY, дублирование кода
                discount = baseTotal * 0.15m; // 15%
                if (discount > 1000)
                {
                    discount = 1000 + (discount - 1000) * 0.1m;
                }
            }
            else if (customerType == "VIP")
            {
                discount = baseTotal * 0.20m; // 20%
            }

            decimal finalPrice = baseTotal - discount;

            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");
            return finalPrice;
        }


        public decimal CalculateTotalPriceWithQuantities(string customerType, Dictionary<decimal, int> itemsWithQuantities)
        {

            // в строках 53-60 нарушен принцип KISS, излишне усложнено решение задачи
            List<decimal> allPrices = new List<decimal>();
            foreach (var item in itemsWithQuantities)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    allPrices.Add(item.Key);
                }
            }
            return CalculateTotalPrice(customerType, allPrices);
        }

    }
}
