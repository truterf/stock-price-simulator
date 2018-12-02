using StockPriceSimulator.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockPriceSimulator.Domain.MockImplementation
{
    internal class MockStockPrice : StockPrice
    {
        public MockStockPrice(string ticker, decimal price, DateTime priceTime) : base(ticker, price, priceTime)
        {
        }

        internal void MovePrice(decimal priceMovement)
        {
            Price += priceMovement;
            PriceTime = DateTime.Now;
        }
    }
}
