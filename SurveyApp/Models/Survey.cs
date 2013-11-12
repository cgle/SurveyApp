using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Tips { get; set; }
        public int CreatorId { get; set; }
        public List<Question> Questions { get; set; }
    }
}