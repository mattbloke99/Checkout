using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Checkout.Core.Tests
{
    public class SkuTests
    {
        [Fact]
        public void CalculateNormalPriceTest()
        {
            var sku = new Sku() { Name = "A", UnitPrice = 50 };

            int cost = sku.CalculatePrice(3);

            Assert.Equal(150, cost);
        }

        [Fact]
        public void CalculateSpecialPriceTest()
        {
            var specialPrice = new SpecialPrice() { SpecialPriceName = "3 for 130", Quantity = 3, OfferPrice = 130 };
            var sku = new Sku() { Name = "A", UnitPrice = 50, SpecialPrice = specialPrice };

            int cost = sku.CalculatePrice(3);

            Assert.Equal(130, cost);
        }

        [Fact]
        public void CalculateCombinationPricesTest()
        {
            var specialPrice = new SpecialPrice() { SpecialPriceName = "3 for 130", Quantity = 3, OfferPrice = 130 };
            var sku = new Sku() { Name = "A", UnitPrice = 50, SpecialPrice = specialPrice };

            int cost = sku.CalculatePrice(5);

            Assert.Equal(230, cost);
        }
    }
}
