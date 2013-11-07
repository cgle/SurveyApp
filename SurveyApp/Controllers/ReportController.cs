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

        public ActionResult Details(string uniqueid, int surveyid)
        {
            var responses = db.Responses.Include(q => q.Question).Include(s => s.Question.Survey).Include(u => u.User).Where(s => s.Question.SurveyId == surveyid).ToList();
            //Dictionary<string, List<Response>> response_dict = new Dictionary<string, List<Response>>();
            Dictionary<string, List<List<Object>>> json_dict = new Dictionary<string, List<List<object>>>();
            foreach (var r in responses)
            {
                if (!json_dict.Keys.Contains(r.UniqueId))
                {
                    //response_dict.Add(r.UniqueId, new List<Response>());
                    json_dict.Add(r.UniqueId, new List<List<object>>());
                }
                //response_dict[r.UniqueId].Add(r);
                json_dict[r.UniqueId].Add(new List<object>(new object[] {r.Question.Text, r.Text, r.Value}));
                
            }

            return View(json_dict);
        }

        public float meanScore(List<int> scores)
        {
            return scores.Sum() / scores.Count();
        }

    }
}
