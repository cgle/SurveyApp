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
    [InitializeSimpleMembership]
    public class ResponseController : Controller
    {
        private SurveyEntities db = new SurveyEntities();

        //
        // GET: /Response/

        public ActionResult Index()
        {
            var responses = new List<Response>();
            ViewBag.surveys = db.Surveys.ToList();
            var uid = new List<string>();
            foreach (var r in db.Responses.Include(r => r.Question).Include(r => r.User).Include(r => r.Question.Survey).ToList())
            {
                if (!uid.Contains(r.UniqueId))
                {
                    responses.Add(r);
                    uid.Add(r.UniqueId);
                }
            }

            return View(responses);
        }

        //
        // GET: /Response/Details/5

        public ActionResult Summary(string uniqueid)
        {
            List<Response> responses = db.Responses.Include(q => q.Question).Include(s=>s.Question.Survey).Where(id => id.UniqueId == uniqueid).ToList();
            ViewBag.uniqueid = uniqueid;
            if (responses == null)
            {
                return HttpNotFound();
            }
            return View(responses);
        }

        //
        // GET: /Response/Create
        [Authorize]
        public ActionResult Take(int surveyid)
        {
            var surveys = db.Surveys.Include(q => q.Questions);
            var survey = surveys.FirstOrDefault(s => s.SurveyId == surveyid);
            var responses = new List<Response>();
            foreach (var q in survey.Questions)
            {
                responses.Add(new Response { User = db.UserProfiles.Find(WebSecurity.CurrentUserId), Question = q, Value = 1});
            }
            ViewBag.Survey = survey;
            ViewBag.Questions = survey.Questions;
            return View(responses);
        }

        //
        // POST: /Response/Create
        [Authorize]
        [HttpPost]
        public ActionResult Take(List<Response> responses, int surveyid)
        {

            var surveys = db.Surveys.Include(q => q.Questions);
            var survey = surveys.FirstOrDefault(s => s.SurveyId == surveyid);
            responses = new List<Response>();
            var uniqueid = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            foreach (var q in survey.Questions)
            {
                responses.Add(new Response { User = db.UserProfiles.Find(WebSecurity.CurrentUserId), Question = q, UniqueId = uniqueid});
            }
            ViewBag.Survey = survey;
            ViewBag.Questions = survey.Questions;
            if (ModelState.IsValid)
            {
                foreach (var response in responses){
                    if (response.Question.options == 0) { response.Text = Request.Form["question_"+response.Question.QuestionId.ToString()]; }
                    else {response.Value = Convert.ToInt16(Request.Form["question_"+response.Question.QuestionId.ToString()]);}
                    db.Responses.Add(response);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(responses);
        }

        //
        // GET: /Response/Edit/5
        [AuthorizeResponse]
        public ActionResult Edit(string uniqueid)
        {
            ViewBag.uniqueid = uniqueid;
            List<Response> responses = db.Responses.Include(r => r.Question).Include(r => r.User).Include(r => r.Question.Survey).Where(id => id.UniqueId == uniqueid).ToList();
            if (responses == null)
            {
                return HttpNotFound();
            }
            ViewBag.Survey = responses.First().Question.Survey;
        
            return View(responses);
        }

        //
        // POST: /Response/Edit/5
        [AuthorizeResponse]
        [HttpPost]
        public ActionResult Edit(List<Response> responses, string uniqueid)
        {
            responses = db.Responses.Include(r => r.Question).Include(r => r.User).Include(r => r.Question.Survey).Where(id => id.UniqueId == uniqueid).ToList();
            ViewBag.Survey = responses.First().Question.Survey;
            if (ModelState.IsValid)
            {
                foreach (var response in responses)
                {
                    if (response.Question.options == 0) { response.Text = Request.Form["question_" + response.Question.QuestionId.ToString()]; }
                    else { response.Value = Convert.ToInt16(Request.Form["question_" + response.Question.QuestionId.ToString()]); }
                    db.Responses.Attach(response);
                    db.Entry(response).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(responses);
        }

        //
        // GET: /Response/Delete/5
        [AuthorizeResponse]
        public ActionResult Delete(string uniqueid)
        {
            return View();
        }

        //
        // POST: /Response/Delete/5
        [AuthorizeResponse]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string uniqueid)
        {
            var rid_list = new List<int>();
            var responses = db.Responses.Where(r => r.UniqueId == uniqueid).ToList();

            foreach (var r in responses)
            {
                rid_list.Add(r.ResponseId);
            }

            foreach (var rid in rid_list)
            {
                var r_del = db.Responses.Find(rid);
                db.Responses.Remove(r_del);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}