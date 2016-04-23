using System;
using System.Collections.Generic;
using System.Linq;

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

            parameters.Add("fields", new List<string>
            {
                "title",
                "analysis_url",
                "preview_url",
                "date_created",
                "date_modified",
                "language_id",
                "question_count",
                "num_responses"
            });

            return parameters;
        }

        public GetSurveyListSettings()
        {
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            Title = "";
            RecipientEmail = "";
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

            parameters.Add("fields", new List<string>
            {
                "url",
                "open",
                "type",
                "name",
                "date_created",
                "date_modified"
            });

            return parameters;
        }

        public GetCollectorListSettings()
        {
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            Name = "";
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

            parameters.Add("fields", new List<string>
            {
                "date_start",
                "date_modified",
                "collector_id",
                "collection_mode",
                "custom_id",
                "email",
                "first_name",
                "last_name",
                "ip_address",
                "status",
                "analysis_url",
                "recipient_id"
            });

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

    public class SendFlowSettingsCollector : CreateAndSendFlowSettingsCollector
    {
    }

    public class SendFlowSettingsEmailMessage : CreateAndSendFlowSettingsEmailMessage
    {
    }

    #endregion

    #region CreateFlow

    public class CreateFlowSettings
    {
        public CreateFlowSettingsSurvey Survey { get; set; }
        public CreateFlowSettingsCollector Collector { get; set; }
        public CreateFlowSettingsEmailMessage EmailMessage { get; set; }

        internal RequestSettings Serialize()
        {
            var parameters = new RequestSettings();
            parameters.Add("survey", Survey.Serialize());
            parameters.Add("collector", Collector.Serialize());
            parameters.Add("email_message", EmailMessage.Serialize());
            return parameters;
        }
    }

    public class CreateFlowSettingsSurvey
    {
        public long TemplateId { get; set; }
        public long FromSurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyNickname { get; set; }
        public Language LanguageId { get; set; }

        internal RequestSettings Serialize()
        {
            if (String.IsNullOrEmpty(SurveyTitle) || String.IsNullOrEmpty(SurveyNickname))
            {
                throw new ArgumentException("SurveyTitle and SurveyNickname must both be populated.");
            }
            
            if ((TemplateId == 0 && FromSurveyId == 0) || (TemplateId > 0 && FromSurveyId > 0))
            {
                throw new ArgumentException("You must populate either TemplateId or FromSurveyId, and not both.");
            }

            var parameters = new RequestSettings();
            
            if (TemplateId != 0)
            {
                parameters.Add("template_id", TemplateId.ToString());
            }
            if (FromSurveyId != 0)
            {
                parameters.Add("from_survey_id", FromSurveyId.ToString());
            }

            parameters.Add("survey_title", SurveyTitle);
            parameters.Add("survey_nickname", SurveyNickname);
            
            if (LanguageId != Language.NotSet)
            {
                parameters.Add("language_id", (int)LanguageId);
            }
            
            return parameters;
        }
    }

    public class CreateFlowSettingsCollector : CreateAndSendFlowSettingsCollector
    {
    }

    public class CreateFlowSettingsEmailMessage : CreateAndSendFlowSettingsEmailMessage
    {
    }

    #endregion

    #region CreateFlow and SendFlow common

    public class CreateAndSendFlowSettingsCollector
    {
        public CreateAndSendFlowSettingsCollector()
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
            parameters.Add("recipients", Recipients.Select(r => r.Serialize()));
            parameters.Add("send", true);

            return parameters;
        }
    }

    public class CreateAndSendFlowSettingsEmailMessage
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