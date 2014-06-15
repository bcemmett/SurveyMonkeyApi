using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkeyApi
{

    public class Survey
    {
        public long survey_id;
        public DateTime date_created; //can json.net serialize these?
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

    public class SettingsGetSurveyList
    {
        public int page;
        public int page_size;
        public DateTime start_date = DateTime.MaxValue; //TODO change these to datetimes?
        public DateTime end_date = DateTime.MinValue;
        public string title;
        public string recipient_email;
        public bool order_asc;
        
        internal RequestSettings Serialized
        {
            get
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
                return parameters;
            }
        }

        public SettingsGetSurveyList()
        {
        }
    }

    public class RequestSettings : Dictionary<string, object>
    {
    }
}