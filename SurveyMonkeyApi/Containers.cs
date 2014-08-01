using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyMonkeyApi
{
    internal class JsonSerializeGetSurveyList
    {
        public long survey_id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public string title { get; set; }
        public SMApi.Language language_id { get; set; }
        public int question_count { get; set; }
        public int num_responses { get; set; }
        public string analysis_url { get; set; }
        public string preview_url { get; set; }

        public Survey ToSurvey()
        {
            var survey = new Survey()
            {
                survey_id = survey_id,
                date_created = date_created,
                date_modified = date_modified,
                nickname = title,
                language_id = language_id,
                question_count = question_count,
                num_responses = num_responses,
                analysis_url = analysis_url,
                preview_url = preview_url
            };
            return survey;
        }
    }

    internal class JsonSerializeGetSurveyDetails
    {
        public long survey_id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public SMApi.Language language_id { get; set; }
        public int num_responses { get; set; }
        public int question_count { get; set; }
        public string nickname { get; set; }
        public Title title { get; set; }
        public List<Page> pages { get; set; }

        public Survey ToSurvey()
        {
            var survey = new Survey()
            {
                survey_id = survey_id,
                date_created = date_created,
                date_modified = date_modified,
                title_text = title.text,
                title_enabled = title.enabled,
                language_id = language_id,
                question_count = question_count,
                num_responses = num_responses,
                nickname = nickname,
                pages = pages
            };
            return survey;
        }
    }

    public class Survey
    {
        public long survey_id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public string title_text { get; set; }
        public bool title_enabled { get; set; }
        public SMApi.Language language_id { get; set; }
        public int question_count { get; set; }
        public int num_responses { get; set; }
        public string analysis_url { get; set; }
        public string preview_url { get; set; }
        public string nickname { get; set; }
        public List<Page> pages { get; set; }
        public List<Collector> collectors { get; set; }
        public List<Response> responses
        {
            get
            {
                return collectors == null ? null : collectors.SelectMany(collector => collector.responses).ToList();
            }
        }
        public List<Question> questions
        {
            get
            {
                return pages == null ? null : pages.SelectMany(page => page.questions).ToList();
            }
        }
    }

    internal class Title
    {
        public bool enabled { get; set; }
        public string text { get; set; }
    }

    public class Page
    {
        public long page_id { get; set; }
        public string heading { get; set; }
        public string sub_heading { get; set; }
        public List<Question> questions { get; set; }
    }

    public class Question
    {
        public long question_id { get; set; }
        public string heading { get; set; }
        public int position { get; set; }
        public QuestionType type { get; set; }
        public List<Answer> answers { get; set; }
    }

    public class QuestionType
    {
        public SMApi.QuestionFamily family { get; set; }
        public SMApi.QuestionSubtype subtype { get; set; }
    }

    public class Answer
    {
        public long answer_id { get; set; }
        public int position { get; set; }
        public string text { get; set; }
        public SMApi.AnswerType type { get; set; }
        public bool visible { get; set; }
        public int weight { get; set; }
        public bool apply_all_rows { get; set; }
        public bool is_answer { get; set; }
        public List<AnswerItem> items { get; set; }
    }

    public class AnswerItem
    {
        public long answer_id { get; set; }
        public int position { get; set; }
        public string type { get; set; }
        public string text { get; set; }
    }

    public class Collector
    {
        public long collector_id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public string name { get; set; }
        public bool open { get; set; }
        public SMApi.CollectorType type { get; set; }
        public string url { get; set; }
        public int completed { get; set; }
        public int started { get; set; }
        public List<Response> responses { get; set; } 
    }

    public class Response
    {
        public long respondent_id { get; set; }
        public List<ResponseQuestion> questions { get; set; }
        public Respondent respondent { get; set; } 
    }

    public class ResponseQuestion
    {
        public long question_id { get; set; }
        public List<ResponseAnswer> answers { get; set; }
        public ProcessedAnswer ProcessedAnswer { get; set; }
    }

    public class ResponseAnswer
    {
        public long row { get; set; }
        public long col { get; set; }
        public long col_choice { get; set; }
        public string text { get; set; }
    }

    public class Respondent
    {
        public long respondent_id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_modified { get; set; }
        public long collector_id { get; set; }
        public SMApi.RespondentCollectionMode collection_mode { get; set; }
        public string custom_id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string ip_address { get; set; }
        public SMApi.RespondentStatus status { get; set; }
        public string analysis_url { get; set; }
        public string recipient_id { get; set; }
    }

    public class UserDetails
    {
        public bool is_paid_account { get; set; }
        public bool is_enterprise_user { get; set; }
        public string username { get; set; }
        public EnterpriseDetails enterprise_details { get; set; }
    }

    public class EnterpriseDetails
    {
        public int total_seats_invoices { get; set; }
        public int total_seats_active { get; set; }
    }
}