using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        public Language Language { get; set; }
        public int QuestionCount { get; set; }
        public int NumResponses { get; set; }
        public string AnalysisUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string Nickname { get; set; }
        public List<Page> Pages { get; set; }
        public List<Collector> Collectors { get; set; }
        public int CustomVariableCount { get; set; }
        public List<CustomVariable> CustomVariables { get; set; }
        internal Dictionary<long, CustomVariable> CustomVariablesLookup { get; set; }
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
        public QuestionFamily Family { get; set; }
        public QuestionSubtype Subtype { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Answer
    {
        public long AnswerId { get; set; }
        public int Position { get; set; }
        public string Text { get; set; }
        public AnswerType Type { get; set; }
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
        public CollectorType Type { get; set; }
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
        public RespondentCollectionMode CollectionMode { get; set; }
        public string CustomId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IpAddress { get; set; }
        public RespondentStatus Status { get; set; }
        public string AnalysisUrl { get; set; }
        public string RecipientId { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class CustomVariable
    {
        public long QuestionId { get; set; }
        public string VariableLabel { get; set; }
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

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class Recipient
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomId { get; set; }

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();
                
            if (String.IsNullOrEmpty(Email))
            {
                throw new ArgumentException("A recipient email address cannot be empty");
            }

            parameters.Add("email", Email);
            if (!String.IsNullOrEmpty(FirstName))
            {
                parameters.Add("first_name", FirstName);
            }
            if (!String.IsNullOrEmpty(LastName))
            {
                parameters.Add("last_name", LastName);
            }
            if (!String.IsNullOrEmpty(CustomId))
            {
                parameters.Add("custom_id", CustomId);
            }

            return parameters;
        }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class CreateRecipientsResponse
    {
        public Collector Collector { get; set; }
        public RecipientsReport RecipientsReport { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class RecipientsReport
    {
        public List<Recipient>  Recipients { get; set; }
        public int ValidEmailsCount { get; set; }
        public List<string> InvalidEmails { get; set; }
        public List<string> DuplicateEmails { get; set; }
        public List<string> BouncedEmails { get; set; }
        public List<string> OptedOutEmails { get; set; }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    public class SendFlowResponse
    {
        public Survey Survey { get; set; }
        public Collector Collector { get; set; }
        public RecipientsReport RecipientsReport { get; set; }
    }

    [Serializable]
    public class SurveyMonkeyException : Exception
    {
        public SurveyMonkeyException()
        {
        }

        public SurveyMonkeyException(string message)
            : base(message)
        {
        }

        public SurveyMonkeyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SurveyMonkeyException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}