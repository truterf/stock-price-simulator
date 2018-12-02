using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPriceSimulator.Website.Controllers
{
    public class StockPricesController : Controller
    {
        // GET: StockPrices
        public ActionResult Index()
        {
            return View();
        }
    }
}