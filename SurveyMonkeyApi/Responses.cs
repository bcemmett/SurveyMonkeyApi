using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkeyApi
{
    public class SingleChoiceAnswer
    {
        public string Choice { get; set; }
        public string OtherComment { get; set; }
    }

    public class ProcessedQuestion
    {
        public object Response { get; set; }
        public long QuestionId { get; set; }
        public System.Type QuestionType { get; set; }
    }

    public class ProcessedResponse
    {
        public List<ProcessedQuestion> ProcessedQuestions { get; set; }
        //and presumably some recipient info
    }
}