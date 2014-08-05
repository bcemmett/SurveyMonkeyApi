using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SurveyMonkeyApi
{
    #region GetSurveyList

    public class GetSurveyListSettings
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string RecipientEmail { get; set; }
        public bool OrderAsc { get; set; }
        public GetSurveyListSettingsOptionalData OptionalData { get; set; }
        internal RequestSettings Serialized
        {
            get
            {
                var parameters = new RequestSettings();

                if (StartDate != DateTime.MaxValue)
                {
                    parameters.Add("start_date", StartDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (EndDate != DateTime.MinValue)
                {
                    parameters.Add("end_date", EndDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (!String.IsNullOrEmpty(Title))
                {
                    parameters.Add("title", Title);
                }
                if (!String.IsNullOrEmpty(RecipientEmail))
                {
                    parameters.Add("recipient_email", RecipientEmail);
                }
                if (OrderAsc)
                {
                    parameters.Add("order_asc", "True");
                }

                var properties = typeof (GetSurveyListSettingsOptionalData).GetProperties();
                List<string> optionalProperties = (from property in properties where (bool) property.GetValue(OptionalData) select property.Name).ToList();
                var snakeCaseProperties = new List<string>();
                foreach (var optionalProperty in optionalProperties)
                {
                    snakeCaseProperties.Add(Regex.Replace(optionalProperty, "(?<=.)([A-Z])", "_$0").ToLower());
                }
                if (snakeCaseProperties.Count > 0)
                {
                    parameters.Add("fields", snakeCaseProperties);
                }

                return parameters;
            }
        }

        public GetSurveyListSettings()
        {
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            Title = "";
            RecipientEmail = "";
            OrderAsc = false;
            OptionalData = new GetSurveyListSettingsOptionalData();    
        }
    }

    public class GetSurveyListSettingsOptionalData
    {
        public bool Title { get; set; }
        public bool AnalysisUrl { get; set; }
        public bool PreviewUrl { get; set; }
        public bool DateCreated { get; set; }
        public bool DateModified { get; set; }
        public bool LanguageId { get; set; }
        public bool QuestionCount { get; set; }
        public bool NumResponses { get; set; }

        public GetSurveyListSettingsOptionalData()
        {
            Title = true;
            AnalysisUrl  = true;
            PreviewUrl  = true;
            DateCreated  = true;
            DateModified  = true;
            LanguageId  = true;
            QuestionCount  = true;
            NumResponses  = true;
        }
    }

    #endregion

    #region GetCollectorList

    public class GetCollectorListSettings
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public bool OrderAsc { get; set; }
        public GetCollectorListSettingsOptionalData OptionalData { get; set; }
        internal RequestSettings Serialized
        {
            get
            {
                var parameters = new RequestSettings();

                if (StartDate != DateTime.MaxValue)
                {
                    parameters.Add("start_date", StartDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (EndDate != DateTime.MinValue)
                {
                    parameters.Add("end_date", EndDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (!String.IsNullOrEmpty(Name))
                {
                    parameters.Add("name", Name);
                }
                if (OrderAsc)
                {
                    parameters.Add("order_asc", "True");
                }

                var properties = typeof(GetCollectorListSettingsOptionalData).GetProperties();
                List<string> optionalProperties = (from property in properties where (bool)property.GetValue(OptionalData) select property.Name).ToList();
                var snakeCaseProperties = new List<string>();
                foreach (var optionalProperty in optionalProperties)
                {
                    snakeCaseProperties.Add(Regex.Replace(optionalProperty, "(?<=.)([A-Z])", "_$0").ToLower());
                }
                if (snakeCaseProperties.Count > 0)
                {
                    parameters.Add("fields", snakeCaseProperties);
                }

                return parameters;
            }
        }

        public GetCollectorListSettings()
        {
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            Name = "";
            OrderAsc = false;
            OptionalData = new GetCollectorListSettingsOptionalData();
        }
    }

    public class GetCollectorListSettingsOptionalData
    {
        public bool Url { get; set; }
        public bool Open { get; set; }
        public bool Type { get; set; }
        public bool Name { get; set; }
        public bool DateCreated { get; set; }
        public bool DateModified { get; set; }

        public GetCollectorListSettingsOptionalData()
        {
            Url = true;
            Open = true;
            Type = true;
            Name = true;
            DateCreated = true;
            DateModified = true;
        }
    }

    #endregion

    #region GetRespondentList

    public class GetRespondentListSettings
    {
        public enum Order
        {
            RespondentId,
            DateModified,
            DateStart
        };

        public long CollectorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartModifiedDate { get; set; }
        public DateTime EndModifiedDate { get; set; }
        public bool OrderAsc { get; set; }
        public Order OrderBy { get; set; }
        public GetRespondentListSettingsOptionalData OptionalData { get; set; }
        internal RequestSettings Serialized
        {
            get
            {
                var parameters = new RequestSettings();

                if (CollectorId != 0)
                {
                    parameters.Add("collector_id", CollectorId.ToString());
                }
                if (StartDate != DateTime.MaxValue)
                {
                    parameters.Add("start_date", StartDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (EndDate != DateTime.MinValue)
                {
                    parameters.Add("end_date", EndDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (StartModifiedDate != DateTime.MaxValue)
                {
                    parameters.Add("start_modified_date", StartDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (EndModifiedDate != DateTime.MinValue)
                {
                    parameters.Add("end_modified_date", EndDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
                }
                if (OrderAsc)
                {
                    parameters.Add("order_asc", "True");
                }

                string order_by_string = "";
                if (OrderBy == Order.RespondentId)
                {
                    order_by_string = "respondent_id";
                }
                if (OrderBy == Order.DateStart)
                {
                    order_by_string = "date_start";
                }
                if (OrderBy == Order.DateModified)
                {
                    order_by_string = "date_modified";
                }
                parameters.Add("order_by", order_by_string);

                var properties = typeof(GetRespondentListSettingsOptionalData).GetProperties();
                List<string> optionalProperties = (from property in properties where (bool)property.GetValue(OptionalData) select property.Name).ToList();
                var snakeCaseProperties = new List<string>();
                foreach (var optionalProperty in optionalProperties)
                {
                    snakeCaseProperties.Add(Regex.Replace(optionalProperty, "(?<=.)([A-Z])", "_$0").ToLower());
                }
                if (snakeCaseProperties.Count > 0)
                {
                    parameters.Add("fields", snakeCaseProperties);
                }

                return parameters;
            }
        }

        public GetRespondentListSettings()
        {
            CollectorId = 0;
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            StartModifiedDate = DateTime.MaxValue;
            EndModifiedDate = DateTime.MinValue;
            OrderAsc = false;
            OrderBy = Order.RespondentId;
            OptionalData = new GetRespondentListSettingsOptionalData();
        }
    }

    public class GetRespondentListSettingsOptionalData
    {
        public bool DateStart {get; set;}
        public bool DateModified {get; set;}
        public bool CollectorId {get; set;}
        public bool CollectionMode {get; set;}
        public bool CustomId {get; set;}
        public bool Email {get; set;}
        public bool FirstName {get; set;}
        public bool LastName {get; set;}
        public bool IpAddress {get; set;}
        public bool Status {get; set;}
        public bool AnalysisUrl {get; set;}
        public bool RecipientId { get; set; }

        public GetRespondentListSettingsOptionalData()
        {
            DateStart = true;
            DateModified = true;
            CollectorId = true;
            CollectionMode = true;
            CustomId = true;
            Email = true;
            FirstName = true;
            LastName = true;
            IpAddress = true;
            Status = true;
            AnalysisUrl = true;
            RecipientId = true;
        }
    }

    #endregion

    internal class RequestSettings : Dictionary<string, object>
    {
    }
}