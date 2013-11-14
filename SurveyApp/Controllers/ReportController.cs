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
            ViewBag.uniqueid = uniqueid;
            var responses = db.Responses.Include(q => q.Question).Include(s => s.Question.Survey).Include(u => u.User).Where(s => s.Question.SurveyId == surveyid).ToList();
            var responses_uniqueid = responses.Where(u => u.UniqueId == uniqueid);
            Dictionary<string, List<int>> question_dict = new Dictionary<string, List<int>>();
            Dictionary<string, List<object>> output_dict = new Dictionary<string, List<object>>();
            output_dict.Add("Question", new List<object>(new object[] {uniqueid,"mean","median"}) );

            foreach (var r in responses)
            {
                if (r.Question.options != 0)
                {
                    if (!question_dict.Keys.Contains(r.Question.Text))
                    {
                        question_dict.Add(r.Question.Text, new List<int>());
                    }
                    question_dict[r.Question.Text].Add(r.Value);
                }
            }

            foreach (var node in question_dict)
            {
                if (!output_dict.Keys.Contains(node.Key))
                {
                    output_dict.Add(node.Key, new List<object>(new object[] {responses_uniqueid.FirstOrDefault(q => q.Question.Text == node.Key).Value, double.Parse(meanScore(node.Value).ToString("#.##")), medianScore(node.Value)} ));
                }

                
            }

            var data = new List<List<object>>();
            foreach (var item in output_dict)
            {
                var k = new List<object>();
                k.Add(item.Key);
                foreach (var i in item.Value) { k.Add(i); }
                data.Add(k);

            }

            return View(data);
        }



        public double meanScore(List<int> scores)
        {
            return scores.Sum()*1.0/ scores.Count();
        }

        public double medianScore(List<int> scores)
        {
            scores.Sort();
            if (scores.Count() == 1) { return scores[0]; }
            else
            {
                if (scores.Count() % 2 == 0)
                { return (scores[scores.Count() / 2 - 1] + scores[scores.Count() / 2]) / 2.0; }
                else
                { return scores[scores.Count() / 2 - 1]; }
            }
        }

    }
}
