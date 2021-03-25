namespace Checkout.Core
{
    public class SpecialPrice
    {
        //for the purposes of this kata it will be assumed that a sku can only have 0 or 1 special prices
        public string SpecialPriceName { get; set; }
        public int Quantity { get; set; }
        public int OfferPrice { get; set; }
    }
}
