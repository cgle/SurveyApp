using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class Response
    {
        public int ResponseId { get; set; }
        public string UniqueId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public Question Question { get; set; }
        public virtual UserProfile User { get; set; }

    }
}