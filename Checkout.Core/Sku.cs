namespace Checkout.Core
{
    public class Sku
    {
        public string Name { get; set; }

        public int UnitPrice { get; set; }

        public SpecialPrice SpecialPrice { get; set; }

        private int CostOfItemsAtNormalPrice(int quantity)
        {
            int quantityItemsAtNormalPrice = quantity % SpecialPrice.Quantity;
            return quantityItemsAtNormalPrice * UnitPrice;
        }

        private int CostOfItemsAtSpecialPrice(int quantity)
        {
            int quantityitemsAtSpecialPrice = quantity / SpecialPrice.Quantity;
            return quantityitemsAtSpecialPrice * SpecialPrice.OfferPrice;
        }

        public int CalculatePrice(int quantity)
        {
            if (SpecialPrice == null) {
                return quantity * UnitPrice;
            } else
            {
                return this.CostOfItemsAtNormalPrice(quantity) + CostOfItemsAtSpecialPrice(quantity);
            }
        }
    }
}
