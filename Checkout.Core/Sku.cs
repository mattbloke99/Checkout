namespace Checkout.Core
{
    public class Sku
    {
        public string Name { get; set; }

        public int UnitPrice { get; set; }

        public SpecialPrice SpecialPrice { get; set; }

        private int QuantityOfItemsAtNormalPrice(int quantity) => SpecialPrice == null ? quantity : quantity % SpecialPrice.Quantity;

        private int CostOfItemsAtNormalPrice(int quantity) => QuantityOfItemsAtNormalPrice(quantity) * UnitPrice;

        private int CostOfItemsAtSpecialPrice(int quantity)
        {
            int quantityitemsAtSpecialPrice = quantity / SpecialPrice.Quantity;
            return quantityitemsAtSpecialPrice * SpecialPrice.OfferPrice;
        }

        public int CalculatePrice(int quantity)
        {

            int specialPrice = SpecialPrice == null ? 0 : CostOfItemsAtSpecialPrice(quantity);
            return specialPrice + CostOfItemsAtNormalPrice(QuantityOfItemsAtNormalPrice(quantity));
        }
    }
}
