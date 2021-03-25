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


    }

    public class SpecialPrice
    {
        //for the purposes of this kata it will be assumed that a sku can only have 0 or 1 special prices
        public SpecialPrice()
        {
        }

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
            if (Sku.SpecialPrice == null) { 
                return Quantity * Sku.Cost;
            } else
            {
                return CostOfItemsAtNormalPrice() + CostOfItemsAtSpecialPrice();
            }
        }

        private int CostOfItemsAtNormalPrice()
        {
            int quantityItemsAtNormalPrice = Quantity % Sku.SpecialPrice.Quantity;
            var cost = quantityItemsAtNormalPrice * Sku.Cost;
            return cost;
        }

        private int CostOfItemsAtSpecialPrice()
        {
            int quantityitemsAtSpecialPrice = Quantity / Sku.SpecialPrice.Quantity;
            var cost = quantityitemsAtSpecialPrice * Sku.SpecialPrice.Cost;
            return cost;
        }
    }

    public class Sku
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public SpecialPrice SpecialPrice { get; internal set; }
    }
}
