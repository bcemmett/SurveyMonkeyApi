using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace SurveyMonkeyApi
{
    internal class JsonSerializeGetSurveyList
    {
        public long survey_id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public string title { get; set; }
        public int language_id { get; set; }
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
                title_text = title,
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
        public int language_id { get; set; }
        public int num_responses { get; set; }
        public int question_count { get; set; }
        public string nickname { get; set; }
        public Title title { get; set; }
        public string analysis_url { get; set; }
        public string preview_url { get; set; }
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
                preview_url = preview_url,
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
        public int language_id { get; set; }
        public int question_count { get; set; }
        public int num_responses { get; set; }
        public string analysis_url { get; set; }
        public string preview_url { get; set; }
        public string nickname { get; set; }
        public List<Page> pages { get; set; }

        public List<Question> questions
        {
            get
            {
                List<Question> qs = new List<Question>();
                foreach (var page in pages)
                {
                    foreach (var question in page.questions)
                    {
                        qs.Add(question);
                    }
                }
                return qs;
            }
        }
    }

    public class Title
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
        public Type type { get; set; }
        public List<QuestionAnswer> answers { get; set; }
    }

    public class Type
    {
        public string family { get; set; }
        public string subtype { get; set; }
    }

    public class QuestionAnswer
    {
        public long answer_id { get; set; }
        public int position { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int weight { get; set; }
        public bool apply_all_rows { get; set; }
        public bool is_answer { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
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
        public string type { get; set; } //TODO: make enum
        public string url { get; set; }
        public int completed { get; set; }
        public int started { get; set; }
    }

    public class Response
    {
        public long respondent_id { get; set; }
        public List<QuestionResponse> questions { get; set; }
    }

    public class QuestionResponse
    {
        public long question_id { get; set; }
        public List<AnswerResponse> answers { get; set; }
    }

    public class AnswerResponse
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
        public string collection_mode { get; set; } //TODO: make enum
        public string custom_id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string ip_address { get; set; }
        public string status { get; set; } //TODO: make enum
        public string analysis_url { get; set; }
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