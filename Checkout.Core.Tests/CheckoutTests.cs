using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Checkout.Core.Tests
{
    public class CheckoutTests
    {
        readonly IDictionary<string, Sku> _priceList;

        public CheckoutTests()
        {
            _priceList = new Dictionary<string, Sku>();
            _priceList.Add("A", new Sku() { Name = "A", UnitPrice = 50, SpecialPrice = new SpecialPrice() { Quantity = 3, OfferPrice = 130 } });
            _priceList.Add("B", new Sku() { Name = "B", UnitPrice = 30, SpecialPrice = new SpecialPrice() { Quantity = 2, OfferPrice = 45 } });
            _priceList.Add("C", new Sku() { Name = "C", UnitPrice = 20 });
            _priceList.Add("D", new Sku() { Name = "D", UnitPrice = 15 });
        }

        [Fact]
        public void CheckoutTotalPrice1Test()
        {
            Checkout checkout = new Checkout(_priceList);

            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("B");

            Sku skuB = GetSku(checkout.Basket, "B");
            Sku skuA = GetSku(checkout.Basket, "A");
            Assert.Equal(2, checkout.Basket[skuB]);
            Assert.Equal(1, checkout.Basket[skuA]);
            Assert.Equal(95, checkout.GetTotalPrice());
        }

        [Fact]
        public void CheckoutTotalPrice2Test()
        {
            Checkout checkout = new Checkout(_priceList);

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

            Sku skuB = GetSku(checkout.Basket, "B");
            Sku skuA = GetSku(checkout.Basket, "A");
            Sku skuC = GetSku(checkout.Basket, "C");
            Sku skuD = GetSku(checkout.Basket, "D");
            Assert.Equal(3, checkout.Basket[skuB]);
            Assert.Equal(4, checkout.Basket[skuA]);
            Assert.Equal(2, checkout.Basket[skuC]);
            Assert.Equal(4, checkout.Basket[skuD]);
            Assert.Equal(130 + 50 + 45 + 30 + 2 * 20 + 4 * 15, checkout.GetTotalPrice());
        }

        Sku GetSku(Dictionary<Sku, int> basket, String skuName) => basket.SingleOrDefault(o => o.Key.Name == skuName).Key;
    }
}
