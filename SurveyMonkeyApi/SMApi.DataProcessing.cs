using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SurveyMonkeyApi
{
    public partial class SMApi
    {
        #region Fill all missing survey information

        public void FillMissingSurveyInformation(List<Survey> surveys)
        {
            foreach (var survey in surveys)
            {
                FillMissingSurveyInformation(survey);
            }
        }

        public void FillMissingSurveyInformation(Survey survey)
        {
            FillMissingCollectorDetails(survey);
            FillMissingResponseCounts(survey);
            FillMissingSurveyDetails(survey);
            FillMissingResponseDetails(survey);
            MatchResponsesToSurveyStructure(survey);
        }

        #endregion

        #region Fill missing survey details

        private void FillMissingSurveyDetails(Survey survey)
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

        #endregion

        #region Fill missing collector details

        private void FillMissingCollectorDetails(Survey survey)
        {
            survey.collectors = GetCollectorList(survey.survey_id);
        }

        #endregion

        #region Fill missing response details

        private void FillMissingResponseDetails(Survey survey)
        {
            if (survey.collectors == null) return; //need to fill missing collector details first

            List<Response> responses = GetAllSurveyResponses(survey);

            //Need to initialise responses before adding to them
            foreach (var collector in survey.collectors)
            {
                collector.responses = new List<Response>();
            }
            
            Dictionary<long, Collector> collectorLookup = survey.collectors.ToDictionary(c => c.collector_id, c => c);
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

        #endregion

        #region Fill missing response counts

        private void FillMissingResponseCounts(Survey survey)
        {
            if (survey.collectors == null) return; //need to fill missing collector details first
            foreach (var collector in survey.collectors)
            {
                Collector countDetails = GetResponseCounts(collector.collector_id);
                collector.completed = countDetails.completed;
                collector.started = countDetails.started;
            }
        }

        #endregion

        #region Match answers to questions

        private void MatchResponsesToSurveyStructure(Survey survey)
        {
            if (survey.responses == null || survey.collectors == null) return;

            Dictionary<long, Question> surveyStructureLookup = survey.questions.ToDictionary(q => q.question_id, q => q);
            foreach (var collector in survey.collectors)
            {
                MatchCollectorsToSurveyStructure(surveyStructureLookup, collector);
            }
        }

        private void MatchCollectorsToSurveyStructure(Dictionary<long, Question> surveyStructureLookup, Collector collector)
        {
            foreach (var response in collector.responses)
            {
                MatchIndividualResponseToSurveyStructure(surveyStructureLookup, response);
            }
        }

        private void MatchIndividualResponseToSurveyStructure(Dictionary<long, Question> surveyStructureLookup, Response response)
        {
            foreach (var questionResponse in response.questions)
            {
                MatchAnswerToSurveyStructure(surveyStructureLookup[questionResponse.question_id], questionResponse);
            }
        }

        private void MatchAnswerToSurveyStructure(Question questionStructure, QuestionResponse questionResponse)
        {         
            questionResponse.ProcessedAnswer = new ProcessedAnswer
            {
                QuestionFamily = questionStructure.type.family,
                QuestionSubtype = questionStructure.type.subtype
            };

            switch (questionResponse.ProcessedAnswer.QuestionFamily)
            {
                case QuestionFamilies.single_choice:
                    questionResponse.ProcessedAnswer.Response = MatchSingleChoiceAnswer(questionStructure, questionResponse.answers);
                    break;

                case QuestionFamilies.multiple_choice:
                    questionResponse.ProcessedAnswer.Response = MatchMultipleChoiceAnswer(questionStructure, questionResponse.answers);
                    break;

                case QuestionFamilies.open_ended:
                    switch (questionResponse.ProcessedAnswer.QuestionSubtype)
                    {
                        case QuestionSubtypes.essay:
                        case QuestionSubtypes.single:
                            questionResponse.ProcessedAnswer.Response = MatchOpenEndedSingleAnswer(questionStructure, questionResponse.answers);
                            break;

                        case QuestionSubtypes.multi:
                        case QuestionSubtypes.numerical:
                            questionResponse.ProcessedAnswer.Response = MatchOpenEndedMultipleAnswer(questionStructure, questionResponse.answers);
                            break;
                    }
                    break;

                case QuestionFamilies.Demographic:
                    questionResponse.ProcessedAnswer.Response = MatchDemographicAnswer(questionStructure, questionResponse.answers);
                    break;

                case QuestionFamilies.datetime:
                    questionResponse.ProcessedAnswer.Response = MatchDateTimeAnswer(questionStructure, questionResponse.answers);
                    break;

                case QuestionFamilies.matrix:
                    switch (questionResponse.ProcessedAnswer.QuestionSubtype)
                    {
                        case QuestionSubtypes.menu:
                            questionResponse.ProcessedAnswer.Response = MatchMatrixMenuAnswer(questionStructure, questionResponse.answers);
                            break;
                    }
                    break;
            }
        }

        private SingleChoiceAnswer MatchSingleChoiceAnswer(Question question, List<AnswerResponse> answerResponses)
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

        private MultipleChoiceAnswer MatchMultipleChoiceAnswer(Question question, List<AnswerResponse> answerResponses)
        {
            var reply = new MultipleChoiceAnswer
            {
                Choices = new List<string>()
            };

            Dictionary<long, QuestionAnswer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var answerResponse in answerResponses)
            {
                if (answersLookup[answerResponse.row].type == AnswerTypes.row)
                {
                    reply.Choices.Add(answersLookup[answerResponse.row].text);
                }
                if (answersLookup[answerResponse.row].type == AnswerTypes.other)
                {
                    reply.Choices.Add(answersLookup[answerResponse.row].text);
                    reply.OtherComment = answerResponse.text;
                }
            }
            return reply;
        }

        private OpenEndedSingleAnswer MatchOpenEndedSingleAnswer(Question question, List<AnswerResponse> answerResponses)
        {
            var reply = new OpenEndedSingleAnswer();

            reply.Text = answerResponses.First().text;

            return reply;
        }

        private OpenEndedMultipleAnswer MatchOpenEndedMultipleAnswer(Question question, List<AnswerResponse> answerResponses)
        {
            var reply = new OpenEndedMultipleAnswer
            {
                Replies = new List<OpenEndedMultipleAnswerReply>()
            };

            Dictionary<long, QuestionAnswer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var answerResponse in answerResponses)
            {
                var openEndedMultipleAnswerReply = new OpenEndedMultipleAnswerReply();
                openEndedMultipleAnswerReply.AnswerId = answersLookup[answerResponse.row].answer_id;
                openEndedMultipleAnswerReply.AnswerLabel = answersLookup[answerResponse.row].text;
                openEndedMultipleAnswerReply.Text = answerResponse.text;
                reply.Replies.Add(openEndedMultipleAnswerReply);
            }

            return reply;
        }

        private DemographicAnswer MatchDemographicAnswer(Question question, List<AnswerResponse> answerResponses)
        {
            var reply = new DemographicAnswer();

            Dictionary<long, QuestionAnswer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var answerResponse in answerResponses)
            {
                var propertyName = answersLookup[answerResponse.row].type.ToString();
                typeof(DemographicAnswer).GetProperty(propertyName).SetValue(reply, answerResponse.text);
            }
            return reply;
        }

        private DateTimeAnswer MatchDateTimeAnswer(Question question, List<AnswerResponse> answerResponses)
        {
            var reply = new DateTimeAnswer
            {
                Replies = new List<DateTimeAnswerReply>()
            };

            Dictionary<long, QuestionAnswer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var answerResponse in answerResponses)
            {
                var dateTimeAnswerReply = new DateTimeAnswerReply();
                dateTimeAnswerReply.AnswerId = answersLookup[answerResponse.row].answer_id;
                dateTimeAnswerReply.AnswerLabel = answersLookup[answerResponse.row].text;
                dateTimeAnswerReply.TimeStamp = DateTime.MinValue;

                DateTime timeStamp = DateTime.Parse(answerResponse.text, CultureInfo.CreateSpecificCulture("en-US"));
                if (question.type.subtype == QuestionSubtypes.time_only) //Where only a time is given, use date component from DateTime.MinValue
                {
                    dateTimeAnswerReply.TimeStamp = dateTimeAnswerReply.TimeStamp.AddHours(timeStamp.Hour);
                    dateTimeAnswerReply.TimeStamp = dateTimeAnswerReply.TimeStamp.AddMinutes(timeStamp.Minute);
                }
                else
                {
                    dateTimeAnswerReply.TimeStamp = timeStamp;
                }

                reply.Replies.Add(dateTimeAnswerReply);
            }
            return reply;
        }

        private MatrixMenuAnswer MatchMatrixMenuAnswer(Question question, List<AnswerResponse> answerResponses)
        {
            var reply = new MatrixMenuAnswer
            {
                Rows = new Dictionary<long, MatrixMenuRowAnswer>()
            };

            Dictionary<long, QuestionAnswer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);
            Dictionary<long, string> choicesLookup = (from answerItem in answersLookup where answerItem.Value.items != null from item in answerItem.Value.items select item).ToDictionary(item => item.answer_id, item => item.text);

            foreach (var answerResponse in answerResponses)
            {
                if (answerResponse.row == 0)
                {
                    reply.Other = answerResponse.text;
                }
                else
                {
                    if (!reply.Rows.ContainsKey(answerResponse.row))
                    {
                        reply.Rows.Add(answerResponse.row, new MatrixMenuRowAnswer
                        {
                            Columns = new Dictionary<long, MatrixMenuColumnAnswer>()
                        });
                    }
                    if (!reply.Rows[answerResponse.row].Columns.ContainsKey(answerResponse.col))
                    {
                        reply.Rows[answerResponse.row].Columns.Add(answerResponse.col, new MatrixMenuColumnAnswer());
                    }

                    reply.Rows[answerResponse.row].RowId = answerResponse.row;
                    reply.Rows[answerResponse.row].Name = answersLookup[answerResponse.row].text;
                    reply.Rows[answerResponse.row].Columns[answerResponse.col].ColumnId = answerResponse.col;
                    reply.Rows[answerResponse.row].Columns[answerResponse.col].Name = answersLookup[answerResponse.col].text;
                    reply.Rows[answerResponse.row].Columns[answerResponse.col].Choice = choicesLookup[answerResponse.col_choice];
                }   
            }

            return reply;
        }

        #endregion
    }
}