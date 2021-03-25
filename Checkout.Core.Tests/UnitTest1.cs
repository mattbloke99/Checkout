using System;
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
    }

    public class EntryItem
    {
        public int Quantity { get; set; }
        public Sku Sku { get; set; }

        public int CalculateCost()
        {
            return Quantity * Sku.Cost;
        }
    }

    public class Sku
    {
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
