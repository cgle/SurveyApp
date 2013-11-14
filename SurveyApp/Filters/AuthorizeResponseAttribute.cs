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
    public class AuthorizeResponseAttribute: AuthorizeAttribute
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

            var uniqueid = httpContext.Request.RequestContext.HttpContext.Request["uniqueid"] as string;
 
            return IsUserOwnerOfArticle(username, uniqueid);
        }

        private bool IsUserOwnerOfArticle(string username, string uniqueid)
        {
            var responses = db.Responses.Include(r => r.User).Where(id => id.UniqueId == uniqueid);
            return username == responses.FirstOrDefault().User.UserName;
            throw new NotImplementedException();
        }
    }
}