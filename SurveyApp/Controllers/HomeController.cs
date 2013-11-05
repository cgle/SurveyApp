using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace SurveyApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (WebSecurity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Response");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}
