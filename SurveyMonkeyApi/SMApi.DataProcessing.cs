﻿using System;
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

        public void MatchResponsesToSurveyStructure(Survey survey)
        {
            if (survey.responses == null || survey.collectors == null) return;

            foreach (var collector in survey.collectors)
            {
                foreach (var response in collector.responses)
                {
                    MatchResponseToSurveyStructure(survey, response);
                }
            }
        }

        private void MatchResponseToSurveyStructure(Survey survey, Response response)
        {
            foreach (var question in response.questions)
            {
                MatchAnswerToSurveyStructure(survey, question);
            }
        }

        private void MatchAnswerToSurveyStructure(Survey survey, QuestionResponse question)
        {         
            Dictionary<long, Question> questionsLookup = survey.questions.ToDictionary(q => q.question_id, q => q);

            question.ProcessedAnswer = new ProcessedAnswer();
            question.ProcessedAnswer.QuestionFamily = questionsLookup[question.question_id].type.family;
            question.ProcessedAnswer.QuestionSubtype = questionsLookup[question.question_id].type.subtype;

            if (question.ProcessedAnswer.QuestionFamily == QuestionFamilies.single_choice)
            {
                question.ProcessedAnswer.QuestionType = typeof (SingleChoiceAnswer);
                question.ProcessedAnswer.Response = MatchSingleChoiceQuestion(questionsLookup[question.question_id], question.answers);
            }
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