using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class Admin1Controller : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            var a = 5;
            var b = 5;
            return View();
        }
	}
}