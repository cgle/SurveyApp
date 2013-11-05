using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyApp.Models;
using WebMatrix.WebData;
using SurveyApp.Filters;

namespace SurveyApp.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        private SurveyEntities db = new SurveyEntities();

        public ActionResult Index()
        {
            var responses = new List<Response>();
            var uid = new List<string>();
            foreach (var r in db.Responses.Include(r => r.Question).Include(r => r.User).Include(r => r.Question.Survey).ToList())
            {
                if (!uid.Contains(r.UniqueId))
                {
                    responses.Add(r);
                    uid.Add(r.UniqueId);
                }
            }
            ViewBag.surveys = db.Surveys.Include(q => q.Questions).ToList();
            return View(responses);
        }

        public ActionResult Details(string uniqueid)
        {

            return View();
        }
    }
}
