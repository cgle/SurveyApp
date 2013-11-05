using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyApp.Models;
using WebMatrix.WebData;

namespace SurveyApp.Controllers
{
    public class SurveyController : Controller
    {
        private SurveyEntities db = new SurveyEntities();

        //
        // GET: /Survey/

        public ActionResult Index()
        {
            return View(db.Surveys.Include(q => q.Questions).ToList());
        }

        //
        // GET: /Survey/Details/5

        public ActionResult Details(int id = 0)
        {
            Survey survey = db.Surveys.Include(q=>q.Questions).FirstOrDefault(q=>q.SurveyId == id);
            ViewBag.Questions = survey.Questions;
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        //
        // GET: /Survey/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Summary(int id = 0)
        {
            Survey survey = db.Surveys.Include(q => q.Questions).FirstOrDefault(q => q.SurveyId == id);
            ViewBag.survey = survey.Title;
            ViewBag.Questions = survey.Questions;
            var responses = new List<Response>();
            foreach (var r in db.Responses.Include(r => r.Question).Include(r => r.User).Include(r => r.Question.Survey).ToList())
            {
                if (r.Question.SurveyId == id)
                {
                    responses.Add(r);
                }
            }
            var responses_dict = new Dictionary<string,List<Response>>();
            var responders = new Dictionary<string, string>();

            var uid_list = new List<string>();
            foreach (var r in responses) 
            {
                if (!uid_list.Contains(r.UniqueId))
                {
                    uid_list.Add(r.UniqueId);
                }
            }

            foreach (var uid in uid_list)
            {
                responses_dict.Add(uid, new List<Response>());
                responders.Add(uid, responses.FirstOrDefault(uniqueid => uniqueid.UniqueId == uid).User.UserName);
            }

            foreach (var r in responses)
            {
                responses_dict[r.UniqueId].Add(r);
            }
            if (survey == null)
            {
                return HttpNotFound();
            }
            ViewBag.Responders = responders;
            return View(responses_dict);
        }

        //
        // POST: /Survey/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Survey survey)
        {
            if (ModelState.IsValid)
            {
                survey.CreatorId = WebSecurity.CurrentUserId;
                db.Surveys.Add(survey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(survey);
        }

        //
        // GET: /Survey/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        //
        // POST: /Survey/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(survey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(survey);
        }

        //
        // GET: /Survey/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        //
        // POST: /Survey/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Survey survey = db.Surveys.Include(q=>q.Questions).FirstOrDefault(s=>s.SurveyId == id);
            List<int> qid_list = new List<int>();

            foreach (var q in survey.Questions)
            {
                qid_list.Add(q.QuestionId);
            }
            foreach (var qid in qid_list)
            {
                var q_del = db.Questions.Find(qid);
                db.Questions.Remove(q_del);
            }
            db.Surveys.Remove(survey);
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