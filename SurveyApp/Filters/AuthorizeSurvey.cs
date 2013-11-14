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


namespace SurveyApp.Filters
{
    public class AuthorizeSurvey: AuthorizeAttribute
    {
        private SurveyEntities db = new SurveyEntities();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            var username = httpContext.User.Identity.Name;
            var id = httpContext.Request.RequestContext.HttpContext.Request["surveyid"] as string;
            if (string.IsNullOrEmpty(id)) { id = httpContext.Request.RequestContext.RouteData.Values["id"] as string; }
            return IsUserOwnerOfArticle(username, id);
        }

        private bool IsUserOwnerOfArticle(string username, string surveyid)
        {
            var survey = db.Surveys.Find(int.Parse(surveyid));
            var creatorid = survey.CreatorId;
            return username == db.UserProfiles.Find(survey.CreatorId).UserName;
            throw new NotImplementedException();
        }
    }
}