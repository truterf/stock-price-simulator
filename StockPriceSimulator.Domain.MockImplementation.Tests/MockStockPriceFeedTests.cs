using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StockPriceSimulator.Domain.MockImplementation.Tests
{
    [TestFixture]
    public class MockStockPriceFeedTests
    {
        [Test]
        public void GetStockPricesReturnsAllStocks()
        {
            const int NumberOfStocks = 10;
            const int TickerLength = 3;
            IEnumerable<StockPrice> prices;

            using (MockStockPriceFeed feed = new MockStockPriceFeed(NumberOfStocks, TickerLength, 10, 10000))
            {
                prices = feed.GetStockPrices(null);
            }

            Assert.AreEqual(NumberOfStocks, prices.Count(), "Number of stock prices not as expected");
        }

        [Test]
        public void GetStockPricesReturnsUniqueTickers()
        {
            const int NumberOfStocks = 200;
            const int TickerLength = 3;
            IEnumerable<StockPrice> prices;

            using (MockStockPriceFeed feed = new MockStockPriceFeed(NumberOfStocks, TickerLength, 10, 10000))
            {
                prices = feed.GetStockPrices(null);
            }

            Assert.False(prices.GroupBy(p => p.Ticker).Any(g => g.Count() > 1), "Tickers are not unique");
        }

        [Test]
        public void GetStockPricesReturnsTickersOfCorrectLength()
        {
            const int NumberOfStocks = 10;
            const int TickerLength = 3;
            IEnumerable<StockPrice> prices;

            using (MockStockPriceFeed feed = new MockStockPriceFeed(NumberOfStocks, TickerLength, 10, 10000))
            {
                prices = feed.GetStockPrices(null);
            }

            foreach (StockPrice stockPrice in prices)
            {
                Assert.AreEqual(TickerLength, stockPrice.Ticker.Length, "Ticker length not as expected {0}", stockPrice.Ticker);
            }
        }

        [Test]
        public void GetStockPricesFiltersTickersThatStartsWithValue()
        {
            const int NumberOfStocks = 10;
            const int TickerLength = 3;

            using (MockStockPriceFeed feed = new MockStockPriceFeed(NumberOfStocks, TickerLength, 10, 10000))
            {
                IEnumerable<StockPrice> allPrices = feed.GetStockPrices(null);

                foreach (char character in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                {
                    string filter = character.ToString();
                    int expected = allPrices.Count(p => p.Ticker.StartsWith(filter));
                    IEnumerable<StockPrice> filteredPrices = feed.GetStockPrices(filter);

                    Assert.AreEqual(expected, filteredPrices.Count(), "Number of stock prices that start with {0} not as expected", filter);
                    foreach(StockPrice stockPrice in filteredPrices)
                    {
                        Assert.True(stockPrice.Ticker.StartsWith(filter), "All tickers should start with {0}: {1}", filter, stockPrice.Ticker);
                    }
                }
            }
        }

        [Test]
        public void GetStockPricesReturnsChangedPrices()
        {
            const int NumberOfStocks = 10;
            const int TickerLength = 3;
            const int RepriceInterval = 100;

            IEnumerable<StockPrice> firstPrices;
            IEnumerable<StockPrice> secondPrices;

            using (MockStockPriceFeed feed = new MockStockPriceFeed(NumberOfStocks, TickerLength, 100, RepriceInterval))
            {
                firstPrices = feed.GetStockPrices(null).Select(sp => new StockPrice(sp.Ticker, sp.Price, sp.PriceTime)).ToArray();
                Thread.Sleep(RepriceInterval * 2);
                secondPrices = feed.GetStockPrices(null).Select(sp => new StockPrice(sp.Ticker, sp.Price, sp.PriceTime)).ToArray();
            }

            var query = from first in firstPrices
                        join second in secondPrices
                        on first.Ticker equals second.Ticker
                        where first.Price == second.Price
                        select $"{first.Ticker} | {first.Price} | {second.Price}";

            Assert.False(query.Any(), "Not all prices changed: {0}", string.Join(", ", query));
        }
    }
}
