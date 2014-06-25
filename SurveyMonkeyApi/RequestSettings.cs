using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkeyApi
{
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
            List<string> optionalFields = (from field in fields where (bool)field.GetValue(OptionalData) select field.Name).ToList();
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

    public class GetRespondentListSettings
    {
        public enum OrderBy
        {
            respondent_id,
            date_modified,
            date_start
        };

        public long collector_id;
        public DateTime start_date = DateTime.MaxValue;
        public DateTime end_date = DateTime.MinValue;
        public DateTime start_modified_date = DateTime.MaxValue;
        public DateTime end_modified_date = DateTime.MinValue;
        public bool order_asc;
        public OrderBy order_by = OrderBy.respondent_id;
        public GetRespondentListSettingsOptionalData OptionalData = new GetRespondentListSettingsOptionalData();

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();

            if (collector_id != 0)
            {
                parameters.Add("collector_id", collector_id.ToString());
            }
            if (start_date != DateTime.MaxValue)
            {
                parameters.Add("start_date", start_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (end_date != DateTime.MinValue)
            {
                parameters.Add("end_date", end_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (start_modified_date != DateTime.MaxValue)
            {
                parameters.Add("start_modified_date", start_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (end_modified_date != DateTime.MinValue)
            {
                parameters.Add("end_modified_date", end_date.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (order_asc)
            {
                parameters.Add("order_asc", "True");
            }

            string order_by_string = "";
            if (order_by == OrderBy.respondent_id)
            {
                order_by_string = "respondent_id";
            }
            if (order_by == OrderBy.date_start)
            {
                order_by_string = "date_start";
            }
            if (order_by == OrderBy.date_modified)
            {
                order_by_string = "date_modified";
            }
            parameters.Add("order_by", order_by_string);

            var fields = typeof(GetRespondentListSettingsOptionalData).GetFields();
            List<string> optionalFields = (from field in fields where (bool)field.GetValue(OptionalData) select field.Name).ToList();
            if (optionalFields.Count > 0)
            {
                parameters.Add("fields", optionalFields);
            }

            return parameters;
        }
    }

    public class GetRespondentListSettingsOptionalData
    {
        public bool date_start = true;
        public bool date_modified = true;
        public bool collector_id = true;
        public bool collection_mode = true;
        public bool custom_id = true;
        public bool email = true;
        public bool first_name = true;
        public bool last_name = true;
        public bool ip_address = true;
        public bool status = true;
        public bool analysis_url = true;
    }

    public class RequestSettings : Dictionary<string, object>
    {
    }
}
