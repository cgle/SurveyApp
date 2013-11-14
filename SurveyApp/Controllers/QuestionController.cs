using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyApp.Models;
using SurveyApp.Filters;

namespace SurveyApp.Controllers
{
    [InitializeSimpleMembership]
    public class QuestionController : Controller
    {
        private SurveyEntities db = new SurveyEntities();

        //
        // GET: /Question/

        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.Survey);
            return View(questions.ToList());
        }

        //
        // GET: /Question/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // GET: /Question/Create
        [AuthorizeSurvey]
        public ActionResult Create(int surveyid = 0)
        {
            ViewBag.SurveyId = new SelectList(db.Surveys, "SurveyId", "Title");
            return View();
        }

        //
        // POST: /Question/Create
        [AuthorizeSurvey]
        [HttpPost]
        public ActionResult Create(Question question, int surveyid)
        {
            var survey = db.Surveys.Find(surveyid);
            question.Survey = survey;
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                var responses = db.Responses.Include(s => s.Question.Survey).Include(u => u.User).Where(s => s.Question.SurveyId == surveyid).ToList();
                var uid_list = new List<string>();
                foreach (var r in responses)
                {
                    if(!uid_list.Contains(r.UniqueId))
                    {
                        uid_list.Add(r.UniqueId);
                    }
                }
                foreach (var u in uid_list)
                {
                    db.Responses.Add(new Response { UniqueId = u, Question = question, User = responses.FirstOrDefault(uid => uid.UniqueId == u).User, Text = "" });
                }

                db.SaveChanges();
                return RedirectToAction("Details", "Survey", new { id = question.Survey.SurveyId });
            }

            ViewBag.SurveyId = new SelectList(db.Surveys, "SurveyId", "Title", question.SurveyId);
            return View(question);
        }

        //
        // GET: /Question/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Question question = db.Questions.Include(s=>s.Survey).FirstOrDefault(q => q.QuestionId == id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /Question/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Question question, int id)
        {
            question = db.Questions.Include(s => s.Survey).FirstOrDefault(q => q.QuestionId == id);
            if (ModelState.IsValid)
            {
                question.Text = Request.Form["Text"];
                question.options = Convert.ToInt16(Request.Form["options"]);
                db.Questions.Attach(question);
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Survey", new { id = question.Survey.SurveyId });
            }
            return View(question);
        }

        //
        // GET: /Question/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Question question = db.Questions.Include(s => s.Survey).FirstOrDefault(i => i.QuestionId == id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /Question/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Include(s => s.Survey).FirstOrDefault(i => i.QuestionId == id);
            var sid = question.Survey.SurveyId;
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Details", "Survey", new { id = sid });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}