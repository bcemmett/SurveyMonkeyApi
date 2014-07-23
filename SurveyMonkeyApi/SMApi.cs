using System;
using System.Net;
using System.Text;
using System.Threading;
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
        private DateTime m_LastRequestTime = DateTime.MinValue;
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
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + m_OAuthSecret);
                webClient.QueryString.Add("api_key", m_ApiKey);
                result = webClient.UploadString(url, "POST", serializedParameters);
            }

            m_RequestsMade++;
            
            var o = JObject.Parse(result);
            CheckSurveyMonkeyResponseIsValid(o);
            return o["data"];
        }

        private void RateLimit()
        {
            TimeSpan timeSpan = DateTime.Now - m_LastRequestTime;
            int elapsedTime = (int)timeSpan.TotalMilliseconds;
            int remainingTime = m_RequestDelay - elapsedTime;
            if ((m_LastRequestTime != DateTime.MinValue) && (remainingTime > 0))
            {
                Thread.Sleep(remainingTime);
            }
            m_LastRequestTime = DateTime.Now;
        }

        private void CheckSurveyMonkeyResponseIsValid(JObject o)
        {
            if ((int)o["status"] != 0)
            {
                string msg = String.Format("Problem querying the Survey Monkey API, error code {0}: {1}", (string) o["status"], (string) o["errmsg"]);
                throw new WebException(msg);
            }
        }

        #endregion
    }
}