﻿namespace Checkout.Core
{
    public interface ICheckout
    {
        void Scan(string item);

        int GetTotalPrice();
    }
}
