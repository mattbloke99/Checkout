using System.Collections.Generic;
using System.Linq;

namespace Checkout.Core
{
    public class Checkout : ICheckout
    {
        private readonly Dictionary<string, Sku> priceList = new Dictionary<string, Sku>();

        public Checkout()
        {
            //this would normally stored elsewhere but for the purposes of this exercise it is initialised here
            priceList.Add("A", new Sku() { Name = "A", UnitPrice = 50, SpecialPrice = new SpecialPrice() { Quantity = 3, OfferPrice = 130 } });
            priceList.Add("B", new Sku() { Name = "B", UnitPrice = 30, SpecialPrice = new SpecialPrice() { Quantity = 2, OfferPrice = 45 } });
            priceList.Add("C", new Sku() { Name = "C", UnitPrice = 20 });
            priceList.Add("D", new Sku() { Name = "D", UnitPrice = 15 });
        }

        public Dictionary<Sku, int> Basket { get; set; } = new Dictionary<Sku, int>();

        public int GetTotalPrice() => Basket.Sum(o => o.Key.CalculatePrice(o.Value));

        public void Scan(string item)
        {
            Sku sku = priceList[item];

            if (Basket.ContainsKey(sku))
            {
                Basket[sku] += 1;
            } else
            {
                Basket.Add(sku, 1);
            }
        }
    }
}
