using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Checkout.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CalculateNormalPriceForSingleEntryTest()
        {
            var sku = new Sku() { Name = "A", Cost = 50 };
            var entry = new EntryItem() { Quantity = 3, Sku = sku };

            int entryCost = entry.CalculateCost();

            Assert.Equal(150, entryCost);
        }

        [Fact]
        public void CalculateSpecialPriceForSingleEntryTest()
        {
            var specialPrice = new SpecialPrice() { SpecialPriceName = "3 for 130", Quantity = 3, Cost = 130 };
            var sku = new Sku() { Name = "A", Cost = 50, SpecialPrice = specialPrice };
            var entry = new EntryItem() { Quantity = 3, Sku = sku };

            int entryCost = entry.CalculateCost();

            Assert.Equal(130, entryCost);
        }

        [Fact]
        public void CalculatePriceForSingleEntryWhenSpecialPriceAndNormalPriceApplyTest()
        {
            var specialPrice = new SpecialPrice() { SpecialPriceName = "3 for 130", Quantity = 3, Cost = 130 };
            var sku = new Sku() { Name = "A", Cost = 50, SpecialPrice = specialPrice };
            var entry = new EntryItem() { Quantity = 5, Sku = sku };

            int entryCost = entry.CalculateCost();

            Assert.Equal(230, entryCost);
        }

        [Fact]
        public void CheckoutSumQuantityTest()
        {
            CheckoutClass checkout = new CheckoutClass();

            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("B");

            Sku skuB = GetSku(checkout.Basket, "B");
            Sku skuA = GetSku(checkout.Basket, "A");
            Assert.Equal(2, checkout.Basket[skuB]);
            Assert.Equal(1, checkout.Basket[skuA]);
        }

        [Fact]
        public void CheckoutTotalPrice1Test()
        {
            CheckoutClass checkout = new CheckoutClass();

            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("B");

            Assert.Equal(95, checkout.GetTotalPrice());
        }

        [Fact]
        public void CheckoutTotalPrice2Test()
        {
            CheckoutClass checkout = new CheckoutClass();

            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("C");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");

            Sku skuB = GetSku(checkout.Basket,"B");
            Sku skuA = GetSku(checkout.Basket, "A");
            Sku skuC = GetSku(checkout.Basket, "C");
            Sku skuD = GetSku(checkout.Basket, "D");
            Assert.Equal(3, checkout.Basket[skuB]);
            Assert.Equal(4, checkout.Basket[skuA]);
            Assert.Equal(2, checkout.Basket[skuC]);
            Assert.Equal(4, checkout.Basket[skuD]);
            Assert.Equal(130 + 50 + 45 + 30 + 2*20 + 4*15, checkout.GetTotalPrice());
        }

        Sku GetSku(Dictionary<Sku, int> basket, String skuName) => basket.SingleOrDefault(o => o.Key.Name == skuName).Key;
    }

    public class CheckoutClass : ICheckout
    {
        public Dictionary<Sku, int> Basket { get; set;} = new Dictionary<Sku, int>();

        readonly Dictionary<string, Sku> priceList = new Dictionary<string, Sku>();

        public CheckoutClass()
        {
            //this would normally stored elsewhere but for the purposes of this exercise it is initialised here
            priceList.Add("A", new Sku() { Name = "A", Cost = 50, SpecialPrice = new SpecialPrice() { Quantity = 3, Cost = 130 } });
            priceList.Add("B", new Sku() { Name = "B", Cost = 30, SpecialPrice = new SpecialPrice() { Quantity = 2, Cost = 45 } });
            priceList.Add("C", new Sku() { Name = "C", Cost = 20 });
            priceList.Add("D", new Sku() { Name = "D", Cost = 15 });
        }

        public int GetTotalPrice() => Basket.Sum(o => o.Key.CalculatePrice(o.Value));

        public void Scan(string item)
        {
            Sku sku = priceList[item];

            if(Basket.ContainsKey(sku))
            {
                Basket[sku] += 1;
            } else
            {
                Basket.Add(sku, 1);
            }
        }
    }

    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }


    public class SpecialPrice
    {
        //for the purposes of this kata it will be assumed that a sku can only have 0 or 1 special prices
        public string SpecialPriceName { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
    }

    public class EntryItem
    {
        public int Quantity { get; set; }
        public Sku Sku { get; set; }

        public int CalculateCost()
        {
            return Sku.CalculatePrice(Quantity);
        }
    }

    public class Sku
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public SpecialPrice SpecialPrice { get; internal set; }

        private int CostOfItemsAtNormalPrice(int quantity)
        {
            int quantityItemsAtNormalPrice = quantity % SpecialPrice.Quantity;
            var cost = quantityItemsAtNormalPrice * Cost;
            return cost;
        }

        private int CostOfItemsAtSpecialPrice(int quantity)
        {
            int quantityitemsAtSpecialPrice = quantity / SpecialPrice.Quantity;
            var cost = quantityitemsAtSpecialPrice * SpecialPrice.Cost;
            return cost;
        }

        public int CalculatePrice(int quantity)
        {
            if (SpecialPrice == null) {
                return quantity * Cost;
            } else
            {
                return CostOfItemsAtNormalPrice(quantity) + CostOfItemsAtSpecialPrice(quantity);
            }
        }
    }
}
