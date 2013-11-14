using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public string Text { get; set; }
        public int options { get; set; }
        public string Instructions { get; set; }
        public Survey Survey { get; set; }
        public List<Response> Responses { get; set; }
    }
}