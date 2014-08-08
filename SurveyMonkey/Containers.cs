using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SurveyMonkey
{
    public class Survey
    {
        public long SurveyId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string TitleText { get; set; }
        public bool TitleEnabled { get; set; }
        public SurveyMonkeyApi.Language Language { get; set; }
        public int QuestionCount { get; set; }
        public int NumResponses { get; set; }
        public string AnalysisUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string Nickname { get; set; }
        public List<Page> Pages { get; set; }
        public List<Collector> Collectors { get; set; }
        public List<Response> Responses
        {
            get
            {
                return Collectors == null ? null : Collectors.SelectMany(collector => collector.Responses).ToList();
            }
        }
        public List<Question> Questions
        {
            get
            {
                return Pages == null ? null : Pages.SelectMany(page => page.Questions).ToList();
            }
        }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Page
    {
        public long PageId { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public List<Question> Questions { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Question
    {
        public long QuestionId { get; set; }
        public string Heading { get; set; }
        public int Position { get; set; }
        public QuestionType Type { get; set; }
        public List<Answer> Answers { get; set; }
        internal Dictionary<long, Answer> AnswersLookup { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class QuestionType
    {
        public SurveyMonkeyApi.QuestionFamily Family { get; set; }
        public SurveyMonkeyApi.QuestionSubtype Subtype { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Answer
    {
        public long AnswerId { get; set; }
        public int Position { get; set; }
        public string Text { get; set; }
        public SurveyMonkeyApi.AnswerType Type { get; set; }
        public bool Visible { get; set; }
        public int Weight { get; set; }
        public bool ApplyAllRows { get; set; }
        public bool IsAnswer { get; set; }
        public List<AnswerItem> Items { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class AnswerItem
    {
        public long AnswerId { get; set; }
        public int Position { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Collector
    {
        public long CollectorId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Name { get; set; }
        public bool Open { get; set; }
        public SurveyMonkeyApi.CollectorType Type { get; set; }
        public string Url { get; set; }
        public int Completed { get; set; }
        public int Started { get; set; }
        public List<Response> Responses { get; set; } 
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Response
    {
        public long RespondentId { get; set; }
        public List<ResponseQuestion> Questions { get; set; }
        public Respondent Respondent { get; set; } 
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class ResponseQuestion
    {
        public long QuestionId { get; set; }
        public List<ResponseAnswer> Answers { get; set; }
        public ProcessedAnswer ProcessedAnswer { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class ResponseAnswer
    {
        public long Row { get; set; }
        public long Col { get; set; }
        public long ColChoice { get; set; }
        public string Text { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Respondent
    {
        public long RespondentId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateModified { get; set; }
        public long CollectorId { get; set; }
        public SurveyMonkeyApi.RespondentCollectionMode CollectionMode { get; set; }
        public string CustomId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IpAddress { get; set; }
        public SurveyMonkeyApi.RespondentStatus Status { get; set; }
        public string AnalysisUrl { get; set; }
        public string RecipientId { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class UserDetails
    {
        public bool IsPaidAccount { get; set; }
        public bool IsEnterpriseUser { get; set; }
        public string Username { get; set; }
        public EnterpriseDetails EnterpriseDetails { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class EnterpriseDetails
    {
        public int TotalSeatsInvoiced { get; set; }
        public int TotalSeatsActive { get; set; }
    }
}