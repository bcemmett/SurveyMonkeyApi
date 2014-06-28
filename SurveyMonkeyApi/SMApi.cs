using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SurveyMonkeyApi
{
    public class SMApi
    {
        #region Members
        private string m_ApiKey;
        private string m_OAuthSecret;
        private string m_BaseUrl = "https://api.surveymonkey.net/v2";
        private int m_RequestDelay = 600;
        private long m_LastRequestTime = 0;
        private int m_RequestsMade = 0;
        public int RequestsMade {get { return m_RequestsMade; }}
        #endregion

        #region Constructors

        public SMApi(string apiKey, string oAuthSecret)
        {
            SetSecretKeys(apiKey, oAuthSecret);
        }

        public SMApi(string apiKey, string oAuthSecret, string customUrl)
        {
            m_BaseUrl = customUrl;
            SetSecretKeys(apiKey, oAuthSecret);
        }

        public SMApi(string apiKey, string oAuthSecret, int customDelay)
        {
            m_RequestDelay = customDelay;
            SetSecretKeys(apiKey, oAuthSecret);
        }

        public SMApi(string apiKey, string oAuthSecret, string customUrl, int customDelay)
        {
            m_BaseUrl = customUrl;
            m_RequestDelay = customDelay;
            SetSecretKeys(apiKey, oAuthSecret);
        }

        private void SetSecretKeys(string apiKey, string oAuthSecret)
        {
            if (apiKey == null || oAuthSecret == null)
            {
                throw new ArgumentNullException();
            }
            m_ApiKey = apiKey;
            m_OAuthSecret = oAuthSecret;
        }

        #endregion

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
            const string endPoint = "/surveys/get_survey_list";
            var o = MakeApiRequest(endPoint, parameters);
            var surveysJson = o.SelectToken("surveys").ToString();
            var surveysProcessedJson = JsonConvert.DeserializeObject<List<JsonSerializeGetSurveyList>>(surveysJson);
            List<Survey> surveys = surveysProcessedJson.Select(x => x.ToSurvey()).ToList();
            return surveys;
        }

        #endregion

        #region GetSurveyDetails endpoint

        public Survey GetSurveyDetails(long surveyId)
        {
            const string endPoint = "/surveys/get_survey_details";
            var parameters = new RequestSettings();
            parameters.Add("survey_id", surveyId.ToString());
            var o = MakeApiRequest(endPoint, parameters);
            var surveyJson = o.ToString();
            var surveysProcessedJson = JsonConvert.DeserializeObject<JsonSerializeGetSurveyDetails>(surveyJson);
            Survey survey = surveysProcessedJson.ToSurvey();
            return survey;
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
            const string endPoint = "/surveys/get_collector_list";
            var o = MakeApiRequest(endPoint, parameters);
            var collectorsJson = o.SelectToken("collectors").ToString();
            List<Collector> collectors = JsonConvert.DeserializeObject<List<Collector>>(collectorsJson);
            return collectors;
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
            const string endPoint = "/surveys/get_respondent_list";
            var o = MakeApiRequest(endPoint, parameters);
            var respondentsJson = o.SelectToken("respondents").ToString();
            List<Respondent> respondents = JsonConvert.DeserializeObject<List<Respondent>>(respondentsJson);
            return respondents;
        }

        #endregion

        #region GetResponseCounts endpoint

        public Collector GetResponseCounts(long collectorId)
        {
            const string endPoint = "/surveys/get_response_counts";
            var parameters = new RequestSettings();
            parameters.Add("collector_id", collectorId.ToString());
            var o = MakeApiRequest(endPoint, parameters);
            var collectorJson = o.ToString();
            Collector collector = JsonConvert.DeserializeObject<Collector>(collectorJson);
            return collector;
        }

        #endregion

        #region API communication

        private JToken MakeApiRequest(string endPoint, RequestSettings data)
        {
            RateLimit();

            string url = m_BaseUrl + endPoint;
            var serializedParameters = JsonConvert.SerializeObject(data);
            string result;

            using (var webClient = new WebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + m_OAuthSecret);
                webClient.QueryString.Add("api_key", m_ApiKey);
                result = webClient.UploadString(url, "POST", serializedParameters);
            }

            m_RequestsMade++;
            
            var o = JObject.Parse(result);
            CheckSurveyMonkeyResponseIsValid(o);
            return o.SelectToken(("data"));
        }

        private void RateLimit()
        {
            //TODO: don't be as wasteful if <500ms but not 0ms of stuff happened since last request
            if (m_LastRequestTime != 0 && (DateTime.Now.Millisecond - m_LastRequestTime < m_RequestDelay))
            {
                Thread.Sleep(m_RequestDelay);
            }
            m_LastRequestTime = DateTime.Now.Millisecond;
        }

        private void CheckSurveyMonkeyResponseIsValid(JObject o)
        {
            if ((int)o["status"] != 0)
            {
                string msg = "Problem querying the Survey Monkey API, error code "
                             + (int) o["status"]
                             + " (https://developer.surveymonkey.com/mashery/requests_responses)";
                throw new WebException(msg);
            }
        }
        #endregion
    }
}