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
    public partial class SMApi
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
                             + ": "
                             + (string) o["errmsg"]
                             + " (https://developer.surveymonkey.com/mashery/requests_responses)";
                throw new WebException(msg);
            }
        }
        #endregion
    }
}