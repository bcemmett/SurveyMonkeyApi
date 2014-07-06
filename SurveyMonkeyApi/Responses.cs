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

    public class OpenEndedSingleAnswer
    {
        public string Text { get; set; }
    }

    public class OpenEndedMultipleAnswer
    {
        public List<OpenEndedMultipleAnswerReply> Replies { get; set; }
    }

    public class OpenEndedMultipleAnswerReply
    {
        public string Text { get; set; }
        public long AnswerId { get; set; }
        public string AnswerLabel { get; set; }
    }

    public class DemographicAnswer
    {
        public string name { get; set; }
        public string company { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class DateTimeAnswer
    {
        public List<DateTimeAnswerReply> Replies { get; set; }
    }

    public class DateTimeAnswerReply
    {
        public DateTime TimeStamp { get; set; }
        public long AnswerId { get; set; }
        public string AnswerLabel { get; set; }
    }

    public class ProcessedAnswer
    {
        public object Response { get; set; }
        public System.Type QuestionType { get; set; }
        public SMApi.QuestionFamilies QuestionFamily { get; set; }
        public SMApi.QuestionSubtypes QuestionSubtype { get; set; }
    }
}