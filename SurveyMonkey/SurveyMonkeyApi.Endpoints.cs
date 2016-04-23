using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyMonkey
{
    public partial class SurveyMonkeyApi
    {
        #region GetSurveyList endpoint

        //Auto-paging
        public List<Survey> GetSurveyList(GetSurveyListSettings settings)
        {
            var surveys = new List<Survey>();
            bool cont = true;
            int page = 1;
            while (cont)
            {
                RequestSettings parameters = settings.Serialize();
                parameters.Add("page", page);
                var newSurveys = GetSurveyListRequest(parameters);
                if (newSurveys.Count > 0)
                {
                    surveys.AddRange(newSurveys);
                }
                if (newSurveys.Count < 1000)
                {
                    cont = false;
                }
                page++;
            }
            return surveys;
        }

        public List<Survey> GetSurveyList()
        {
            return GetSurveyList(new GetSurveyListSettings());
        }


        ///No limit on page size
        public List<Survey> GetSurveyList(int page, GetSurveyListSettings settings)
        {
            if (page < 1)
            {
                throw new ArgumentException("Page must be greater than 0.");
            }
            return GetSurveyListPage(page, 0, false, settings);
        }

        public List<Survey> GetSurveyList(int page)
        {
            return GetSurveyList(page, new GetSurveyListSettings());
        }


        //Limit the page size returned
        public List<Survey> GetSurveyList(int page, int pageSize, GetSurveyListSettings settings)
        {
            if (pageSize < 1 || pageSize > 1000)
            {
                throw new ArgumentException("Page size must be between 1 and 1000.");
            }
            return GetSurveyListPage(page, pageSize, true, settings);
        }

        public List<Survey> GetSurveyList(int page, int pageSize)
        {
            return GetSurveyList(page, pageSize, new GetSurveyListSettings());
        }


        private List<Survey> GetSurveyListPage(int page, int pageSize, bool limitPageSize, GetSurveyListSettings settings)
        {
            RequestSettings parameters = settings.Serialize();
            parameters.Add("page", page);
            if (limitPageSize)
            {
                parameters.Add("page_size", pageSize);
            }
            return GetSurveyListRequest(parameters);
        }

        private List<Survey> GetSurveyListRequest(RequestSettings parameters)
        {
            try
            {
                const string endPoint = "/surveys/get_survey_list";
                var o = MakeApiRequest(endPoint, parameters);
                List<JsonDeserializeGetSurveyList> rawSurveys = o["surveys"].ToObject<List<JsonDeserializeGetSurveyList>>();
                List<Survey> surveys = rawSurveys.Select(x => x.ToSurvey()).ToList();
                return surveys;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region GetSurveyDetails endpoint

        public Survey GetSurveyDetails(long surveyId)
        {
            try {
                const string endPoint = "/surveys/get_survey_details";
                var parameters = new RequestSettings();
                parameters.Add("survey_id", surveyId.ToString());
                var o = MakeApiRequest(endPoint, parameters);
                JsonDeserializeGetSurveyDetails rawSurvey = o.ToObject<JsonDeserializeGetSurveyDetails>();
                Survey survey = rawSurvey.ToSurvey();
                return survey;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region GetCollectorList endpoint

        //Auto-paging
        public List<Collector> GetCollectorList(long surveyId, GetCollectorListSettings settings)
        {
            var collectors = new List<Collector>();
            bool cont = true;
            int page = 1;
            while (cont)
            {
                RequestSettings parameters = settings.Serialize();
                parameters.Add("survey_id", surveyId.ToString());
                parameters.Add("page", page);
                var newCollectors = GetCollectorListRequest(parameters);
                if (newCollectors.Count > 0)
                {
                    collectors.AddRange(newCollectors);
                }
                if (newCollectors.Count < 1000)
                {
                    cont = false;
                }
                page++;
            }
            return collectors;
        }

        public List<Collector> GetCollectorList(long surveyId)
        {
            return GetCollectorList(surveyId, new GetCollectorListSettings());
        }

        ///No limit on page size
        public List<Collector> GetCollectorList(long surveyId, int page, GetCollectorListSettings settings)
        {
            if (page < 1)
            {
                throw new ArgumentException("Page must be greater than 0.");
            }
            return GetCollectorListPage(surveyId, page, 0, false, settings);
        }

        public List<Collector> GetCollectorList(long surveyId, int page)
        {
            return GetCollectorList(surveyId, page, new GetCollectorListSettings());
        }

        //Limit the page size returned
        public List<Collector> GetCollectorList(long surveyId, int page, int pageSize, GetCollectorListSettings settings)
        {
            if (pageSize < 1 || pageSize > 1000)
            {
                throw new ArgumentException("Page size must be between 1 and 1000.");
            }
            return GetCollectorListPage(surveyId, page, pageSize, true, settings);
        }

        public List<Collector> GetCollectorList(long surveyId, int page, int pageSize)
        {
            return GetCollectorList(surveyId, page, pageSize, new GetCollectorListSettings());
        }

        private List<Collector> GetCollectorListPage(long surveyId, int page, int pageSize, bool limitPageSize, GetCollectorListSettings settings)
        {
            RequestSettings parameters = settings.Serialize();
            parameters.Add("survey_id", surveyId.ToString());
            parameters.Add("page", page);
            if (limitPageSize)
            {
                parameters.Add("page_size", pageSize);
            }
            return GetCollectorListRequest(parameters);
        }

        private List<Collector> GetCollectorListRequest(RequestSettings parameters)
        {
            try {
                const string endPoint = "/surveys/get_collector_list";
                var o = MakeApiRequest(endPoint, parameters);
                List<Collector> collectors = o["collectors"].ToObject<List<Collector>>();
                return collectors;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region GetRespondentList endpoint

        //TODO: deal with account upgrade notifications

        //Auto-paging
        public List<Respondent> GetRespondentList(long surveyId, GetRespondentListSettings settings)
        {
            var respondents = new List<Respondent>();
            bool cont = true;
            int page = 1;
            while (cont)
            {
                RequestSettings parameters = settings.Serialize();
                parameters.Add("survey_id", surveyId.ToString());
                parameters.Add("page", page);
                var newRespondents = GetRespondentListRequest(parameters);
                if (newRespondents.Count > 0)
                {
                    respondents.AddRange(newRespondents);
                }
                if (newRespondents.Count < 1000)
                {
                    cont = false;
                }
                page++;
            }
            return respondents;
        }

        public List<Respondent> GetRespondentList(long surveyId)
        {
            return GetRespondentList(surveyId, new GetRespondentListSettings());
        }

        ///No limit on page size
        public List<Respondent> GetRespondentList(long surveyId, int page, GetRespondentListSettings settings)
        {
            if (page < 1)
            {
                throw new ArgumentException("Page must be greater than 0.");
            }
            return GetRespondentListPage(surveyId, page, 0, false, settings);
        }

        public List<Respondent> GetRespondentList(long surveyId, int page)
        {
            return GetRespondentList(surveyId, page, new GetRespondentListSettings());
        }


        //Limit the page size returned
        public List<Respondent> GetRespondentList(long surveyId, int page, int pageSize, GetRespondentListSettings settings)
        {
            if (pageSize < 1 || pageSize > 1000)
            {
                throw new ArgumentException("Page size must be between 1 and 1000.");
            }
            return GetRespondentListPage(surveyId, page, pageSize, true, settings);
        }

        public List<Respondent> GetRespondentList(long surveyId, int page, int pageSize)
        {
            return GetRespondentList(surveyId, page, pageSize, new GetRespondentListSettings());
        }


        private List<Respondent> GetRespondentListPage(long surveyId, int page, int pageSize, bool limitPageSize, GetRespondentListSettings settings)
        {
            RequestSettings parameters = settings.Serialize();
            parameters.Add("survey_id", surveyId.ToString());
            parameters.Add("page", page);
            if (limitPageSize)
            {
                parameters.Add("page_size", pageSize);
            }
            return GetRespondentListRequest(parameters);
        }

        private List<Respondent> GetRespondentListRequest(RequestSettings parameters)
        {
            try {
                const string endPoint = "/surveys/get_respondent_list";
                var o = MakeApiRequest(endPoint, parameters);
                List<Respondent> respondents = o["respondents"].ToObject<List<Respondent>>();
                return respondents;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region GetResponses endpoint

        public List<Response> GetResponses(long surveyId, List<long> respondents)
        {
            var responses = new List<Response>();
            const int maxRespondentsPerPage = 100;
            
            bool moreRespondents = true;
            int page = 0;

            while (moreRespondents)
            {
                List<long> respondentIds = respondents.Skip(page * maxRespondentsPerPage).Take(maxRespondentsPerPage).ToList();
                if (respondentIds.Count > 0)
                {
                    List<Response> newResponses = GetResponsesRequest(surveyId, respondentIds);
                    responses.AddRange(newResponses);
                }
                if (respondentIds.Count < 100)
                {
                    moreRespondents = false;
                }

                page++;
            }
            return responses;
        }
        
        private List<Response> GetResponsesRequest(long surveyId, List<long> respondents)
        {
            try
            {
                const string endPoint = "/surveys/get_responses";
                var parameters = new RequestSettings();
                parameters.Add("survey_id", surveyId.ToString());
                parameters.Add("respondent_ids", respondents.Select(r => r.ToString()));
                var o = MakeApiRequest(endPoint, parameters);
                List<Response> responses = o.ToObject<List<Response>>();
                return responses;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region GetResponseCounts endpoint

        public Collector GetResponseCounts(long collectorId)
        {
            try {
                const string endPoint = "/surveys/get_response_counts";
                var parameters = new RequestSettings();
                parameters.Add("collector_id", collectorId.ToString());
                var o = MakeApiRequest(endPoint, parameters);
                Collector collector = o.ToObject<Collector>();
                return collector;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region GetUserDetails endpoint

        public UserDetails GetUserDetails()
        {
            try {
                const string endPoint = "/user/get_user_details";
                var parameters = new RequestSettings();
                var o = MakeApiRequest(endPoint, parameters);
                UserDetails userDetails = o["user_details"].ToObject<UserDetails>();
                return userDetails;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region GetTemplateList
        
        public List<Template> GetTemplateList()
        {
            return GetTemplateList(new GetTemplateListSettings());
        }

        public List<Template> GetTemplateList(GetTemplateListSettings settings)
        {
            RequestSettings parameters = settings.Serialize();
            parameters.Add("page", 1);
            parameters.Add("page_size", 1000);
            return GetTemplateListRequest(parameters);
        }
        
        private List<Template> GetTemplateListRequest(RequestSettings parameters)
        {
            try
            {
                const string endPoint = "/templates/get_template_list";
                var o = MakeApiRequest(endPoint, parameters);
                List<Template> templates = o["templates"].ToObject<List<Template>>();
                return templates;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region CreateRecipients endpoint

        public CreateRecipientsResponse CreateRecipients(long collectorId, long emailMessageId, List<Recipient> recipients)
        {
            var parameters = new RequestSettings();
            parameters.Add("collector_id", collectorId.ToString());
            parameters.Add("email_message_id", emailMessageId.ToString());
            parameters.Add("send", true);
            parameters.Add("recipients", recipients.Select(r => r.Serialize()));
            try
            {
                const string endPoint = "/collectors/create_recipients";
                var o = MakeApiRequest(endPoint, parameters);
                
                var response = new CreateRecipientsResponse();
                response.Collector = o["collector"].ToObject<Collector>();
                response.RecipientsReport = o["recipients_report"].ToObject<RecipientsReport>();
                return response;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion

        #region SendFlow

        public SendFlowResponse SendFlow(long surveyId, SendFlowSettings settings)
        {
            var parameters = settings.Serialize();
            parameters.Add("survey_id", surveyId.ToString());
            try
            {
                const string endPoint = "/batch/send_flow";
                var o = MakeApiRequest(endPoint, parameters);

                var response = new SendFlowResponse();
                JsonDeserializeGetSurveyList rawSurvey = o["survey"].ToObject<JsonDeserializeGetSurveyList>();
                response.Survey = rawSurvey.ToSurvey();
                response.Collector = o["collector"].ToObject<Collector>();
                response.RecipientsReport = o["recipients_report"].ToObject<RecipientsReport>();
                return response;
            }
            catch (Exception e)
            {
                throw new SurveyMonkeyException("Error communicating with endpoint", e);
            }
        }

        #endregion
    }
}