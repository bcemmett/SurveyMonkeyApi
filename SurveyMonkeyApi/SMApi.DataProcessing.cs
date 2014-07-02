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
        #region Fill missing survey details

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

        #endregion

        #region Fill missing collector details

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

        #endregion

        #region Fill missing response details

        public void FillMissingResponseDetails(Survey survey)
        {
            List<Response> responses = GetAllSurveyResponses(survey);

            //This will fail if you create your own collector for a survey without using GetCollectorList (eg finding CollectorIds through GetRespondentList then populating with GetResponseCounts)
            List<Collector> collectors = survey.collectors ?? GetCollectorList(survey.survey_id);

            //Need to initialise responses before adding to them
            foreach (var collector in collectors)
            {
                collector.responses = new List<Response>();
            }
            
            Dictionary<long, Collector> collectorLookup = collectors.ToDictionary(c => c.collector_id, c => c);
            foreach (var response in responses)
            {
                collectorLookup[response.respondent.collector_id].responses.Add(response);
            }

            survey.collectors = collectorLookup.Values.ToList();
        }

        private List<Response> GetAllSurveyResponses(Survey survey)
        {
            const int maxRespondentsPerPage = 100;
            List<Respondent> respondents = GetRespondentList(survey.survey_id);
            Dictionary<long, Respondent> respondentLookup = respondents.ToDictionary(r => r.respondent_id, r => r);
            var responses = new List<Response>();

            //page through the respondents
            bool moreRespondents = true;
            int page = 0;
            while (moreRespondents)
            {
                List<long> respondentIds = respondents.Skip(page * maxRespondentsPerPage).Take(maxRespondentsPerPage).Select(rp => rp.respondent_id).ToList();
                if (respondentIds.Count > 0)
                {
                    List<Response> newResponses = GetResponses(survey.survey_id, respondentIds);

                    foreach (var newResponse in newResponses)
                    {
                        newResponse.respondent = respondentLookup[newResponse.respondent_id];
                    }
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

        public void FillMissingResponseDetails(List<Survey> surveys)
        {
            foreach (var survey in surveys)
            {
                FillMissingResponseDetails(survey);
            }
        }

        #endregion

        #region Fill missing response counts

        public void FillMissingResponseCounts(Survey survey)
        {
            survey.collectors = survey.collectors ?? GetCollectorList(survey.survey_id);
            foreach (var collector in survey.collectors)
            {
                Collector countDetails = GetResponseCounts(collector.collector_id);
                collector.completed = countDetails.completed;
                collector.started = countDetails.started;
            }
        }

        public void FillMissingResponseCounts(List<Survey> surveys)
        {
            foreach (var survey in surveys)
            {
                FillMissingResponseCounts(survey);
            }
        }

        #endregion

        #region Match answers to questions

        public void MatchResponsesToQuestions(Survey survey, List<Response> responses)
        {
            survey.ProcessedResponses = new List<ProcessedResponse>();
            foreach (var response in responses)
            {
                var processedResponse = new ProcessedResponse();
                processedResponse.ProcessedQuestions = MatchSingleResponseToQuestion(survey, response);
                survey.ProcessedResponses.Add(processedResponse);
            }
        }

        private List<ProcessedQuestion> MatchSingleResponseToQuestion(Survey survey, Response response)
        {
            var questionReplies = new List<ProcessedQuestion>();
         
            Dictionary<long, Question> questionsLookup = survey.questions.ToDictionary(q => q.question_id, q => q);

            foreach (var responseQuestion in response.questions)
            {
                QuestionFamilies responseQuestionFamily = questionsLookup[responseQuestion.question_id].type.family;
                QuestionSubtypes responseQuestionSubtype = questionsLookup[responseQuestion.question_id].type.subtype;

                var processedResponse = new ProcessedQuestion();
                processedResponse.QuestionId = responseQuestion.question_id;

                if (responseQuestionFamily == QuestionFamilies.single_choice)
                {
                    processedResponse.QuestionType = typeof (SingleChoiceAnswer);
                    processedResponse.Response = MatchSingleChoiceQuestion(questionsLookup[responseQuestion.question_id], responseQuestion.answers);
                }
                questionReplies.Add(processedResponse);
            }
            return questionReplies;
        }

        private SingleChoiceAnswer MatchSingleChoiceQuestion(Question question, List<AnswerResponse> answerResponses)
        {
            var reply = new SingleChoiceAnswer();

            Dictionary<long, QuestionAnswer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);
            
            foreach (var answerResponse in answerResponses)
            {
                if (answersLookup[answerResponse.row].type == AnswerTypes.row)
                {
                    reply.Choice = answersLookup[answerResponse.row].text;
                }
                if (answersLookup[answerResponse.row].type == AnswerTypes.other)
                {
                    reply.OtherComment = answerResponse.text;
                    if (reply.Choice == null)
                    {
                        reply.Choice = answersLookup[answerResponse.row].text;
                    }
                }
            }
            return reply;
        }

        #endregion
    }
}