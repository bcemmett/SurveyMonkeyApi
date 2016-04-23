using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SurveyMonkey
{
    #region GetSurveyList

    public class GetSurveyListSettings
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string RecipientEmail { get; set; }
        public bool? OrderAsc { get; set; }
        public GetSurveyListSettingsOptionalData OptionalData { get; set; }
        internal RequestSettings Serialize()
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
            if (OrderAsc.HasValue)
            {
                parameters.Add("order_asc", OrderAsc.Value);
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

        public GetSurveyListSettings()
        {
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            Title = "";
            RecipientEmail = "";
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
        public bool? OrderAsc { get; set; }
        public GetCollectorListSettingsOptionalData OptionalData { get; set; }
        internal RequestSettings Serialize()
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
            if (OrderAsc.HasValue)
            {
                parameters.Add("order_asc", OrderAsc.Value);
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

        public GetCollectorListSettings()
        {
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            Name = "";
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
            NotSet = 0,
            RespondentId,
            DateModified,
            DateStart
        };

        public long CollectorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartModifiedDate { get; set; }
        public DateTime EndModifiedDate { get; set; }
        public bool? OrderAsc { get; set; }
        public Order OrderBy { get; set; }
        public GetRespondentListSettingsOptionalData OptionalData { get; set; }
        internal RequestSettings Serialize()
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
                parameters.Add("start_modified_date", StartModifiedDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (EndModifiedDate != DateTime.MinValue)
            {
                parameters.Add("end_modified_date", EndModifiedDate.ToString("yyyy-MM-dd HH':'mm':'ss"));
            }
            if (OrderAsc.HasValue)
            {
                parameters.Add("order_asc", OrderAsc.Value);
            }

            if (OrderBy != Order.NotSet)
            {
                string orderByString = "";
                if (OrderBy == Order.RespondentId)
                {
                    orderByString = "respondent_id";
                }
                if (OrderBy == Order.DateStart)
                {
                    orderByString = "date_start";
                }
                if (OrderBy == Order.DateModified)
                {
                    orderByString = "date_modified";
                }
                parameters.Add("order_by", orderByString);
            }

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

        public GetRespondentListSettings()
        {
            CollectorId = 0;
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            StartModifiedDate = DateTime.MaxValue;
            EndModifiedDate = DateTime.MinValue;
            OrderBy = Order.NotSet;
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

    #region GetTemplateList

    public class GetTemplateListSettings
    {
        public Language LanguageId { get; set; }
        public long CategoryId { get; set; }
        public bool? ShowOnlyAvailableToCurrentUser { get; set; }

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();
            if (LanguageId != Language.NotSet)
            {
                parameters.Add("language_id", (int)LanguageId);
            }
            if (CategoryId != 0)
            {
                parameters.Add("category_id", CategoryId.ToString());
            }
            if (ShowOnlyAvailableToCurrentUser.HasValue)
            {
                parameters.Add("show_only_available_to_current_user", ShowOnlyAvailableToCurrentUser.Value);
            }
            parameters.Add("fields", new List<string>
            {
                "language_id",
                "title",
                "short_description",
                "long_description",
                "is_available_to_current_user",
                "is_featured",
                "is_certified",
                "page_count",
                "question_count",
                "preview_url",
                "category_id",
                "category_name",
                "category_description",
                "date_modified",
                "date_created"
            });
            return parameters;
        }
    }

    #endregion

    #region SendFlow

    public class SendFlowSettings
    {
        public SendFlowSettingsCollector Collector { get; set; }
        public SendFlowSettingsEmailMessage EmailMessage { get; set; }

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();
            parameters.Add("collector", Collector.Serialize());
            parameters.Add("email_message", EmailMessage.Serialize());
            return parameters;
        }
    }

    public class SendFlowSettingsCollector
    {
        public SendFlowSettingsCollector()
        {
            Recipients = new List<Recipient>();
        }

        public string Name { get; set; }
        public string ThankYouMessage { get; set; }
        public string RedirectUrl { get; set; }
        public List<Recipient> Recipients { get; set; }

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();
            parameters.Add("type", "email"); //only email collectors are supported
            if(!String.IsNullOrEmpty(Name))
            {
                parameters.Add("name", Name);
            }
            if (!String.IsNullOrEmpty(ThankYouMessage))
            {
                parameters.Add("thank_you_message", ThankYouMessage);
            }
            if (!String.IsNullOrEmpty(RedirectUrl))
            {
                parameters.Add("redirect_url", RedirectUrl);
            }
            parameters.Add("recipients", Recipients.Select(r => r.Serialize()));
            parameters.Add("send", true);

            return parameters;
        }
    }

    public class SendFlowSettingsEmailMessage
    {
        public string ReplyEmail { get; set; }
        public string Subject { get; set; }
        public string BodyText { get; set; } //Must contain [SurveyLink], [OptOutLink], and [FooterLink]
        public bool? DisableFooter { get; set; }

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();
            parameters.Add("reply_email", ReplyEmail);
            parameters.Add("subject", Subject);
            if (!String.IsNullOrEmpty(BodyText))
            {
                parameters.Add("body_text", BodyText);
            }
            if (DisableFooter.HasValue)
            {
                parameters.Add("disable_footer", DisableFooter);    
            }
            return parameters;
        }
    }

    #endregion

    #region CreateCollector

    public class CreateCollectorSettings
    {
        public string Name { get; set; }
        public string ThankYouMessage { get; set; }
        public string RedirectUrl { get; set; }

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();
            if (!String.IsNullOrEmpty(Name))
            {
                parameters.Add("name", Name);
            }
            if (!String.IsNullOrEmpty(ThankYouMessage))
            {
                parameters.Add("thank_you_message", ThankYouMessage);
            }
            if (!String.IsNullOrEmpty(RedirectUrl))
            {
                parameters.Add("redirect_url", RedirectUrl);
            }
            parameters.Add("type", "weblink");
            return parameters;
        }
    }

    #endregion

    internal class RequestSettings : Dictionary<string, object>
    {
    }
}