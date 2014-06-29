using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SurveyMonkeyApi
{
    public partial class SMApi
    {
        public void FillMissingSurveyDetails(Survey survey)
        {
            Survey surveyDetails = GetSurveyDetails(survey.survey_id);
            survey.date_created = surveyDetails.date_created;
            survey.date_modified = surveyDetails.date_modified;
            survey.language_id = surveyDetails.language_id;
            survey.num_responses = surveyDetails.num_responses;
            survey.question_count = surveyDetails.question_count;
            survey.nickname = surveyDetails.nickname;
            survey.title_text = surveyDetails.title_text;
            survey.title_enabled = surveyDetails.title_enabled;
            survey.pages = surveyDetails.pages;
        }

        public void FillMissingSurveyDetails(List<Survey> surveys)
        {
            foreach (var survey in surveys)
            {
                FillMissingSurveyDetails(survey);
            }
        }

        public void FillMissingCollectorDetails(Survey survey)
        {
            survey.collectors = GetCollectorList(survey.survey_id);
        }

        public void FillMissingCollectorDetails(List<Survey> surveys)
        {
            foreach (var survey in surveys)
            {
                FillMissingCollectorDetails(survey);
            }
        }

        public void FillMissingResponseDetails(Survey survey)
        {
            const int maxRespondentsPerPage = 100;
            List<Respondent> respondents = GetRespondentList(survey.survey_id);
            var responses = new List<Response>();
            
            //page through the respondents
            bool moreRespondents = true;
            int page = 0;
            while (moreRespondents)
            {
                List<long> respondentIds = respondents.Skip(page * maxRespondentsPerPage).Take(maxRespondentsPerPage).Select(rp => rp.respondent_id).ToList();
                if (respondentIds.Count > 0)
                {
                    responses.AddRange(GetResponses(survey.survey_id, respondentIds));
                }
                if (respondentIds.Count <100)
                {
                    moreRespondents = false;
                }

                page++;
            }

            survey.responses = responses;
        }

        public void FillMissingResponseDetails(List<Survey> surveys)
        {
            foreach (var survey in surveys)
            {
                FillMissingSurveyDetails(survey);
            }
        }
    }
}