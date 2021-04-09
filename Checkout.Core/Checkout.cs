using System.Collections.Generic;
using System.Linq;

namespace Checkout.Core
{
    public class Checkout : ICheckout
    {
        private readonly IDictionary<string, Sku> _priceList;

        public Checkout(IDictionary<string, Sku> priceList)
        {
            _priceList = priceList;
        }

        public Dictionary<Sku, int> Basket { get;} = new Dictionary<Sku, int>();
 

        public int GetTotalPrice() => Basket.Sum(o => o.Key.CalculatePrice(o.Value));

        public void Scan(string item)
        {
            Sku sku = _priceList[item];

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
