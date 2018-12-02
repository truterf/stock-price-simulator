using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceSimulator.Domain
{
    public interface IStockPriceFeed
    {
        IEnumerable<StockPrice> GetStockPrices(string tickerFilter);
    }
}
