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

    public class MultipleChoiceAnswer
    {
        public List<string> Choices { get; set; }
        public string OtherComment { get; set; } 
    }

    public class ProcessedAnswer
    {
        public object Response { get; set; }
        public System.Type QuestionType { get; set; }
        public SMApi.QuestionFamilies QuestionFamily { get; set; }
        public SMApi.QuestionSubtypes QuestionSubtype { get; set; }
    }
}