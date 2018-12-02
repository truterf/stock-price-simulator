using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StockPriceSimulator.Domain;
using System.Threading;
using System.Linq;

namespace StockPriceSimulator.Domain.MockImplementation
{
    public class MockStockPriceFeed : IStockPriceFeed, IDisposable
    {
        private const string ValidTickerCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private readonly MockStockPrice[] _stockPrices;
        private readonly Timer _repriceTimer;
        private readonly int _tickerLength;
        private readonly decimal _priceMovementRange;
        private readonly Random _random;

        private bool disposedValue = false;

        public MockStockPriceFeed(int numberOfStocks, int tickerLength, decimal priceMovementRange, int repriceIntervalMilliseconds)
        {
            _stockPrices = new MockStockPrice[numberOfStocks <= 0 ? 1 : numberOfStocks];
            _tickerLength = tickerLength <= 0 ? 1 : tickerLength;
            _priceMovementRange = priceMovementRange;
            _random = new Random();

            DateTime priceTime = DateTime.Now;

            for (int i = 0; i < _stockPrices.Length; i++)
            {
                string ticker = GenerateUniqueTicker();
                decimal price = new decimal(_random.NextDouble()) * 100;

                _stockPrices[i] = new MockStockPrice(ticker, price, priceTime);
            }

            _repriceTimer = new Timer(RepriceStocks, null, repriceIntervalMilliseconds, repriceIntervalMilliseconds);
        }

        public IEnumerable<StockPrice> GetStockPrices(string tickerFilter = null)
        {
            return _stockPrices.Where(sp => string.IsNullOrEmpty(tickerFilter) || sp.Ticker.StartsWith(tickerFilter));
        }

        private string GenerateUniqueTicker()
        {
            string ticker;

            do
            {
                ticker = _random.NextString(ValidTickerCharacters, _tickerLength);
            } while (_stockPrices.Any(sp => sp != null && sp.Ticker.Equals(ticker)));

            return ticker;
        }

        private void RepriceStocks(object stateInfo)
        {
            lock (_stockPrices)
            {
                foreach (MockStockPrice stockPrice in _stockPrices)
                {
                    stockPrice.MovePrice(stockPrice.Price * new decimal(_random.NextDouble()));
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _repriceTimer.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}
