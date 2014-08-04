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
            Dictionary<long, Question> questionsLookup = survey.questions.ToDictionary(q => q.question_id, q => q);
            foreach (var collector in survey.collectors)
            {
                MatchCollectorsToSurveyStructure(questionsLookup, collector);
            }
        }

        private void MatchCollectorsToSurveyStructure(Dictionary<long, Question> questionsLookup, Collector collector)
        {
            foreach (var response in collector.responses)
            {
                MatchIndividualResponseToSurveyStructure(questionsLookup, response);
            }
        }

        private void MatchIndividualResponseToSurveyStructure(Dictionary<long, Question> questionsLookup, Response response)
        {
            foreach (var responseQuestion in response.questions)
            {
                responseQuestion.ProcessedAnswer = new ProcessedAnswer
                {
                    QuestionFamily = questionsLookup[responseQuestion.question_id].type.family,
                    QuestionSubtype = questionsLookup[responseQuestion.question_id].type.subtype,
                    Response = MatchResponseQuestionToSurveyStructure(questionsLookup[responseQuestion.question_id], responseQuestion.answers)
                };                
            }
        }

        private object MatchResponseQuestionToSurveyStructure(Question question, List<ResponseAnswer> responseAnswers)
        {
            switch (question.type.family)
            {
                case QuestionFamily.single_choice:
                    return MatchSingleChoiceAnswer(question, responseAnswers);

                case QuestionFamily.multiple_choice:
                    return MatchMultipleChoiceAnswer(question, responseAnswers);

                case QuestionFamily.open_ended:
                    switch (question.type.subtype)
                    {
                        case QuestionSubtype.essay:
                        case QuestionSubtype.single:
                            return MatchOpenEndedSingleAnswer(question, responseAnswers);

                        case QuestionSubtype.multi:
                        case QuestionSubtype.numerical:
                            return MatchOpenEndedMultipleAnswer(question, responseAnswers);
                    }
                    break;

                case QuestionFamily.Demographic:
                    return MatchDemographicAnswer(question, responseAnswers);

                case QuestionFamily.datetime:
                    return MatchDateTimeAnswer(question, responseAnswers);

                case QuestionFamily.matrix:
                    switch (question.type.subtype)
                    {
                        case QuestionSubtype.menu:
                            return MatchMatrixMenuAnswer(question, responseAnswers);
                        case QuestionSubtype.ranking:
                            return MatchMatrixRankingAnswer(question, responseAnswers);
                        case QuestionSubtype.rating:
                            return MatchMatrixRatingAnswer(question, responseAnswers);
                    }
                    break;
            }
            return "No match";
        }

        private SingleChoiceAnswer MatchSingleChoiceAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new SingleChoiceAnswer();

            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);
            
            foreach (var responseAnswer in responseAnswers)
            {
                if (answersLookup[responseAnswer.row].type == AnswerType.row)
                {
                    reply.Choice = answersLookup[responseAnswer.row].text;
                }
                if (answersLookup[responseAnswer.row].type == AnswerType.other)
                {
                    reply.OtherComment = responseAnswer.text;
                    if (reply.Choice == null)
                    {
                        reply.Choice = answersLookup[responseAnswer.row].text;
                    }
                }
            }
            return reply;
        }

        private MultipleChoiceAnswer MatchMultipleChoiceAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new MultipleChoiceAnswer
            {
                Choices = new List<string>()
            };

            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var responseAnswer in responseAnswers)
            {
                if (answersLookup[responseAnswer.row].type == AnswerType.row)
                {
                    reply.Choices.Add(answersLookup[responseAnswer.row].text);
                }
                if (answersLookup[responseAnswer.row].type == AnswerType.other)
                {
                    reply.Choices.Add(answersLookup[responseAnswer.row].text);
                    reply.OtherComment = responseAnswer.text;
                }
            }
            return reply;
        }

        private OpenEndedSingleAnswer MatchOpenEndedSingleAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new OpenEndedSingleAnswer();

            reply.Text = responseAnswers.First().text;

            return reply;
        }

        private OpenEndedMultipleAnswer MatchOpenEndedMultipleAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new OpenEndedMultipleAnswer
            {
                Replies = new List<OpenEndedMultipleAnswerReply>()
            };

            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var responseAnswer in responseAnswers)
            {
                var openEndedMultipleAnswerReply = new OpenEndedMultipleAnswerReply();
                openEndedMultipleAnswerReply.AnswerId = answersLookup[responseAnswer.row].answer_id;
                openEndedMultipleAnswerReply.AnswerLabel = answersLookup[responseAnswer.row].text;
                openEndedMultipleAnswerReply.Text = responseAnswer.text;
                reply.Replies.Add(openEndedMultipleAnswerReply);
            }

            return reply;
        }

        private DemographicAnswer MatchDemographicAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new DemographicAnswer();

            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var responseAnswer in responseAnswers)
            {
                var propertyName = answersLookup[responseAnswer.row].type.ToString();
                typeof(DemographicAnswer).GetProperty(propertyName).SetValue(reply, responseAnswer.text);
            }
            return reply;
        }

        private DateTimeAnswer MatchDateTimeAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new DateTimeAnswer
            {
                Replies = new List<DateTimeAnswerReply>()
            };

            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var responseAnswer in responseAnswers)
            {
                var dateTimeAnswerReply = new DateTimeAnswerReply();
                dateTimeAnswerReply.AnswerId = answersLookup[responseAnswer.row].answer_id;
                dateTimeAnswerReply.AnswerLabel = answersLookup[responseAnswer.row].text;
                dateTimeAnswerReply.TimeStamp = DateTime.MinValue;

                DateTime timeStamp = DateTime.Parse(responseAnswer.text, CultureInfo.CreateSpecificCulture("en-US"));
                if (question.type.subtype == QuestionSubtype.time_only) //Where only a time is given, use date component from DateTime.MinValue
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

        private MatrixMenuAnswer MatchMatrixMenuAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new MatrixMenuAnswer
            {
                Rows = new Dictionary<long, MatrixMenuRowAnswer>()
            };

            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);
            Dictionary<long, string> choicesLookup = (from answerItem in answersLookup where answerItem.Value.items != null from item in answerItem.Value.items select item).ToDictionary(item => item.answer_id, item => item.text);

            foreach (var responseAnswer in responseAnswers)
            {
                if (responseAnswer.row == 0)
                {
                    reply.Other = responseAnswer.text;
                }
                else
                {
                    if (!reply.Rows.ContainsKey(responseAnswer.row))
                    {
                        reply.Rows.Add(responseAnswer.row, new MatrixMenuRowAnswer
                        {
                            Columns = new Dictionary<long, MatrixMenuColumnAnswer>()
                        });
                    }
                    if (!reply.Rows[responseAnswer.row].Columns.ContainsKey(responseAnswer.col))
                    {
                        reply.Rows[responseAnswer.row].Columns.Add(responseAnswer.col, new MatrixMenuColumnAnswer());
                    }

                    reply.Rows[responseAnswer.row].RowId = responseAnswer.row;
                    reply.Rows[responseAnswer.row].Name = answersLookup[responseAnswer.row].text;
                    reply.Rows[responseAnswer.row].Columns[responseAnswer.col].ColumnId = responseAnswer.col;
                    reply.Rows[responseAnswer.row].Columns[responseAnswer.col].Name = answersLookup[responseAnswer.col].text;
                    reply.Rows[responseAnswer.row].Columns[responseAnswer.col].Choice = choicesLookup[responseAnswer.col_choice];
                }   
            }

            return reply;
        }

        private MatrixRankingAnswer MatchMatrixRankingAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new MatrixRankingAnswer
            {
                Ranking = new Dictionary<int, string>(),
                NotApplicable = new List<string>()
            };
            
            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var responseAnswer in responseAnswers)
            {
                if (answersLookup[responseAnswer.col].weight == 0)
                {
                    reply.NotApplicable.Add(answersLookup[responseAnswer.row].text);
                }
                else
                {
                    reply.Ranking.Add(answersLookup[responseAnswer.col].weight, answersLookup[responseAnswer.row].text);
                }
            }
            return reply;
        }

        private MatrixRatingAnswer MatchMatrixRatingAnswer(Question question, List<ResponseAnswer> responseAnswers)
        {
            var reply = new MatrixRatingAnswer
            {
                Rows = new List<MatrixRatingRowAnswer>()
            };

            Dictionary<long, Answer> answersLookup = question.answers.ToDictionary(a => a.answer_id, a => a);

            foreach (var responseAnswer in responseAnswers)
            {
                if (responseAnswer.row == 0)
                {
                    reply.Other = responseAnswer.text;
                }
                else
                {
                    var row = new MatrixRatingRowAnswer();
                    row.Name = answersLookup[responseAnswer.row].text;

                    if (responseAnswer.col != 0)
                    {
                        row.Choice = answersLookup[responseAnswer.col].text;
                    }
                    
                    if (!String.IsNullOrEmpty(responseAnswer.text))
                    {
                        row.Other = responseAnswer.text;
                    }
                    reply.Rows.Add(row);
                }
            }

            return reply;
        }

        #endregion
    }
}