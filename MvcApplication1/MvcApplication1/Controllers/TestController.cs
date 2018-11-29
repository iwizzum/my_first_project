using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult test1()
        {
            //b
            return View();
        }

        public ActionResult test2(string s1, string s2)
        {
            return View();
        }
    }
}
