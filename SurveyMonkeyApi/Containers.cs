using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace SurveyMonkeyApi
{

    public class Survey
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
        //fields from [get_survey_list], [get_survey_details], [get_collector_list], and possibly [get_response_counts]
    }

    public class Response
    {
        //fields from [get_responses]
    }

    public class Respondent
    {
        //fields from [get_respondent_list]
    }

    public class GetSurveyListSettings
    {
        public int page;
        public int page_size;
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

    public class RequestSettings : Dictionary<string, object>
    {
    }
}