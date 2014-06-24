using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace SurveyMonkeyApi
{
    public class JsonSerializeGetSurveyList
    {
        public long survey_id;
        public DateTime date_created;
        public DateTime date_modified;
        public string title;
        public int language_id;
        public int question_count;
        public int num_responses;
        public string analysis_url;
        public string preview_url;

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

    public class JsonSerializeGetSurveyDetails
    {
        public long survey_id;
        public DateTime date_created;
        public DateTime date_modified;
        public int language_id;
        public int num_responses;
        public int question_count;
        public string nickname;
        public Title title;
        public string analysis_url;
        public string preview_url;
        public List<Page> pages;

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
        public long survey_id;
        public DateTime date_created;
        public DateTime date_modified;
        public string title_text;
        public bool title_enabled;
        public int language_id;
        public int question_count;
        public int num_responses;
        public string analysis_url;
        public string preview_url;
        public string nickname;
        public List<Page> pages;

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
        public bool enabled;
        public string text;
    }

    public class Page
    {
        public long page_id;
        public string heading;
        public string sub_heading;
        public List<Question> questions;
    }

    public class Question
    {
        public long question_id;
        public string heading;
        public int position;
        public Type  type;
        public List<Answer> answers;
    }

    public class Type
    {
        public string family;
        public string subtype;
    }

    public class Answer
    {
        public long answer_id;
        public int position;
        public string text;
        public string type;
        public bool visible;
        public int weight;
        public bool apply_all_rows;
        public bool is_answer;
        public List<Item> items;
    }

    public class Item
    {
        public long answer_id;
        public int position;
        public string type;
        public string text;
    }

    public class Collector
    {
        public long collector_id;
        public DateTime date_created;
        public DateTime date_modified;
        public string name;
        public bool open;
        public string type; //TODO: make enum
        public string url;
    }

    public class Response
    {
        //fields from [get_responses]
    }

    public class Respondent
    {
        //fields from [get_respondent_list]
    }

    #region Settings for processing requests

    public class GetSurveyListSettings
    {
        public DateTime start_date = DateTime.MaxValue;
        public DateTime end_date = DateTime.MinValue;
        public string title;
        public string recipient_email;
        public bool order_asc;
        public GetSurveyListSettingsOptionalData OptionalData = new GetSurveyListSettingsOptionalData();

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();

            if (start_date != DateTime.MaxValue)
            {
                parameters.Add("start_date", start_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (end_date != DateTime.MinValue)
            {
                parameters.Add("end_date", end_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (!String.IsNullOrEmpty(title))
            {
                parameters.Add("title", title);
            }
            if (!String.IsNullOrEmpty(recipient_email))
            {
                parameters.Add("recipient_email", recipient_email);
            }
            if (order_asc)
            {
                parameters.Add("order_asc", "True");
            }

            var fields = typeof(GetSurveyListSettingsOptionalData).GetFields();
            List<string> optionalFields = (from field in fields where (bool) field.GetValue(OptionalData) select field.Name).ToList();
            if (optionalFields.Count > 0)
            {
                parameters.Add("fields", optionalFields);
            }

            return parameters;
        }
    }

    public class GetSurveyListSettingsOptionalData
    {
        public bool title = true;
        public bool analysis_url = true;
        public bool preview_url = true;
        public bool date_created = true;
        public bool date_modified = true;
        public bool language_id = true;
        public bool question_count = true;
        public bool num_responses = true;
    }

    public class GetCollectorListSettings
    {
        public DateTime start_date = DateTime.MaxValue;
        public DateTime end_date = DateTime.MinValue;
        public string name;
        public bool order_asc;
        public GetCollectorListSettingsOptionalData OptionalData = new GetCollectorListSettingsOptionalData();

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();

            if (start_date != DateTime.MaxValue)
            {
                parameters.Add("start_date", start_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (end_date != DateTime.MinValue)
            {
                parameters.Add("end_date", end_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (!String.IsNullOrEmpty(name))
            {
                parameters.Add("name", name);
            }
            if (order_asc)
            {
                parameters.Add("order_asc", "True");
            }

            var fields = typeof(GetCollectorListSettingsOptionalData).GetFields();
            List<string> optionalFields = (from field in fields where (bool)field.GetValue(OptionalData) select field.Name).ToList();
            if (optionalFields.Count > 0)
            {
                parameters.Add("fields", optionalFields);
            }

            return parameters;
        }
    }

    public class GetCollectorListSettingsOptionalData
    {
        public bool url = true;
        public bool open = true;
        public bool type = true;
        public bool name = true;
        public bool date_created = true;
        public bool date_modified = true;
    }

    public class RequestSettings : Dictionary<string, object>
    {
    }
#endregion

}