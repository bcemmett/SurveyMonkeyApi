using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkeyApi
{
    #region GetSurveyList

    public class GetSurveyListSettings
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string title { get; set; }
        public string recipient_email { get; set; }
        public bool order_asc { get; set; }
        public GetSurveyListSettingsOptionalData OptionalData { get; set; }
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

                var properties = typeof (GetSurveyListSettingsOptionalData).GetProperties();
                List<string> optionalProperties =
                    (from property in properties where (bool) property.GetValue(OptionalData) select property.Name).ToList();
                if (optionalProperties.Count > 0)
                {
                    parameters.Add("fields", optionalProperties);
                }

                return parameters;
            }
        }

        public GetSurveyListSettings()
        {
            start_date = DateTime.MaxValue;
            end_date = DateTime.MinValue;
            title = "";
            recipient_email = "";
            order_asc = false;
            OptionalData = new GetSurveyListSettingsOptionalData();    
        }
    }

    public class GetSurveyListSettingsOptionalData
    {
        public bool title { get; set; }
        public bool analysis_url { get; set; }
        public bool preview_url { get; set; }
        public bool date_created { get; set; }
        public bool date_modified { get; set; }
        public bool language_id { get; set; }
        public bool question_count { get; set; }
        public bool num_responses { get; set; }

        public GetSurveyListSettingsOptionalData()
        {
            title = true;
            analysis_url  = true;
            preview_url  = true;
            date_created  = true;
            date_modified  = true;
            language_id  = true;
            question_count  = true;
            num_responses  = true;
        }
    }

    #endregion

    #region GetCollectorList

    public class GetCollectorListSettings
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string name { get; set; }
        public bool order_asc { get; set; }
        public GetCollectorListSettingsOptionalData OptionalData { get; set; }
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
                if (!String.IsNullOrEmpty(name))
                {
                    parameters.Add("name", name);
                }
                if (order_asc)
                {
                    parameters.Add("order_asc", "True");
                }

                var properties = typeof(GetCollectorListSettingsOptionalData).GetProperties();
                List<string> optionalProperties =
                    (from property in properties where (bool)property.GetValue(OptionalData) select property.Name).ToList();
                if (optionalProperties.Count > 0)
                {
                    parameters.Add("fields", optionalProperties);
                }

                return parameters;
            }
        }

        public GetCollectorListSettings()
        {
            start_date = DateTime.MaxValue;
            end_date = DateTime.MinValue;
            name = "";
            order_asc = false;
            OptionalData = new GetCollectorListSettingsOptionalData();
        }
    }

    public class GetCollectorListSettingsOptionalData
    {
        public bool url { get; set; }
        public bool open { get; set; }
        public bool type { get; set; }
        public bool name { get; set; }
        public bool date_created { get; set; }
        public bool date_modified { get; set; }

        public GetCollectorListSettingsOptionalData()
        {
            url = true;
            open = true;
            type = true;
            name = true;
            date_created = true;
            date_modified = true;
        }
    }

    #endregion

    #region GetRespondentList

    public class GetRespondentListSettings
    {
        public enum OrderBy
        {
            respondent_id,
            date_modified,
            date_start
        };

        public long collector_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime start_modified_date { get; set; }
        public DateTime end_modified_date { get; set; }
        public bool order_asc { get; set; }
        public OrderBy order_by { get; set; }
        public GetRespondentListSettingsOptionalData OptionalData { get; set; }
        internal RequestSettings Serialized
        {
            get
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

                var properties = typeof(GetRespondentListSettingsOptionalData).GetProperties();
                List<string> optionalProperties =
                    (from property in properties where (bool)property.GetValue(OptionalData) select property.Name).ToList();
                if (optionalProperties.Count > 0)
                {
                    parameters.Add("fields", optionalProperties);
                }

                return parameters;
            }
        }

        public GetRespondentListSettings()
        {
            collector_id = 0;
            start_date = DateTime.MaxValue;
            end_date = DateTime.MinValue;
            start_modified_date = DateTime.MaxValue;
            end_modified_date = DateTime.MinValue;
            order_asc = false;
            order_by = OrderBy.respondent_id;
            OptionalData = new GetRespondentListSettingsOptionalData();
        }
    }

    public class GetRespondentListSettingsOptionalData
    {
        public bool date_start {get; set;}
        public bool date_modified {get; set;}
        public bool collector_id {get; set;}
        public bool collection_mode {get; set;}
        public bool custom_id {get; set;}
        public bool email {get; set;}
        public bool first_name {get; set;}
        public bool last_name {get; set;}
        public bool ip_address {get; set;}
        public bool status {get; set;}
        public bool analysis_url {get; set;}
        public bool recipient_id { get; set; }

        public GetRespondentListSettingsOptionalData()
        {
            date_start = true;
            date_modified = true;
            collector_id = true;
            collection_mode = true;
            custom_id = true;
            email = true;
            first_name = true;
            last_name = true;
            ip_address = true;
            status = true;
            analysis_url = true;
            recipient_id = true;
        }
    }

    #endregion

    internal class RequestSettings : Dictionary<string, object>
    {
    }
}