using StockPriceSimulator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPriceSimulator.Website.Controllers
{
    public class StockPricesController : Controller
    {
        IStockPriceFeed _stockPriceFeed;

        public StockPricesController(IStockPriceFeed stockPriceFeed)
        {
            ViewBag.Search = string.Empty;
            ViewBag.PageSize = 20;

            _stockPriceFeed = stockPriceFeed;
        }

        // GET: StockPrices
        public ActionResult Index(string search = "")
        {
            ViewBag.Search = search?.ToUpper() ?? string.Empty;
            IEnumerable<StockPrice> stockPrices = _stockPriceFeed.GetStockPrices(ViewBag.Search);
            return View(stockPrices);
        }
    }
}