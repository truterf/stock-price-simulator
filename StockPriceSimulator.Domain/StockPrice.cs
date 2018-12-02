using System;
using System.Collections.Generic;
using System.Text;

namespace StockPriceSimulator.Domain
{
    public class StockPrice
    {
        public string Ticker { get; protected set; }
        public decimal Price { get; protected set; }
        public DateTime PriceTime { get; protected set; }

        public StockPrice(string ticker, decimal price, DateTime priceTime)
        {
            Ticker = ticker;
            Price = price;
            PriceTime = priceTime;
        }
    }
}
