using System;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SurveyMonkey
{
    public partial class SurveyMonkeyApi : ISurveyMonkeyApi
    {
        #region Members

        private string _apiKey;
        private string _oAuthSecret;
        private string _baseUrl = "https://api.surveymonkey.net/v2";
        private int _rateLimitDelay = 500;
        private DateTime _lastRequestTime = DateTime.MinValue;
        public int RequestsMade { get; private set; }
        public int QuotaAllotted { get; private set; }
        public int QuotaUsed { get; private set; }

        #endregion

        #region Constructors

        public SurveyMonkeyApi(string apiKey, string oAuthSecret)
        {
            SetSecretKeys(apiKey, oAuthSecret);
        }

        public SurveyMonkeyApi(string apiKey, string oAuthSecret, string baseUrl)
        {
            _baseUrl = baseUrl;
            SetSecretKeys(apiKey, oAuthSecret);
        }

        public SurveyMonkeyApi(string apiKey, string oAuthSecret, int rateLimitDelay)
        {
            _rateLimitDelay = rateLimitDelay;
            SetSecretKeys(apiKey, oAuthSecret);
        }

        public SurveyMonkeyApi(string apiKey, string oAuthSecret, string baseUrl, int rateLimitDelay)
        {
            _baseUrl = baseUrl;
            _rateLimitDelay = rateLimitDelay;
            SetSecretKeys(apiKey, oAuthSecret);
        }

        private void SetSecretKeys(string apiKey, string oAuthSecret)
        {
            if (apiKey == null || oAuthSecret == null)
            {
                throw new ArgumentNullException();
            }
            _apiKey = apiKey;
            _oAuthSecret = oAuthSecret;
        }

        #endregion

        #region API communication

        private JToken MakeApiRequest(string endPoint, RequestSettings data)
        {
            RateLimit();

            string url = _baseUrl + endPoint;
            var serializedParameters = JsonConvert.SerializeObject(data);
            string result;

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + _oAuthSecret);
                webClient.QueryString.Add("api_key", _apiKey);
                result = webClient.UploadString(url, "POST", serializedParameters);
                UpdateQuotaInformation(webClient.ResponseHeaders);
            }

            _lastRequestTime = DateTime.Now;
            RequestsMade++;
            
            JObject o = ParseApiResponse(result);
            return o["data"];
        }

        private void RateLimit()
        {
            TimeSpan timeSpan = DateTime.Now - _lastRequestTime;
            int elapsedTime = (int)timeSpan.TotalMilliseconds;
            int remainingTime = _rateLimitDelay - elapsedTime;
            if ((_lastRequestTime != DateTime.MinValue) && (remainingTime > 0))
            {
                Thread.Sleep(remainingTime);
            }
            _lastRequestTime = DateTime.Now; //Also setting here as otherwise if a WebException is thrown while making the request it wouldn't get set at all
        }

        private JObject ParseApiResponse(string apiResponse)
        {
            JObject o;
            
            try
            {
                o = JObject.Parse(apiResponse);
            }
            catch (JsonReaderException e)
            {
                string msg = String.Format("Could not parse the data returned by the API: {0}", apiResponse);
                throw new WebException(msg, e);
            }

            if ((int)o["status"] != 0)
            {
                string msg = String.Format("Problem querying the Survey Monkey API, error code {0}: {1}", (string) o["status"], (string) o["errmsg"]);
                throw new WebException(msg);
            }
            return o;
        }

        private void UpdateQuotaInformation(WebHeaderCollection headers)
        {
            try
            {
                QuotaAllotted = Int32.Parse(headers["X-Plan-Quota-Allotted"]);
                QuotaUsed = Int32.Parse(headers["X-Plan-Quota-Current"]);
            }
            catch (Exception)
            {
                //Just swallow anything. The information's not critical and I'm not sure the header's guaranteed to exist.
                //If there's an actual problem it'll be more helpful to throw in ParseApiResponse()
            }
        }

        #endregion
    }
}