using System.Collections.Generic;

namespace SurveyMonkey
{
    public interface ISurveyMonkeyApi
    {
        int RequestsMade { get; }
        int QuotaAllotted { get; }
        int QuotaUsed { get; }

        //Endpoints
        List<Survey> GetSurveyList();
        List<Survey> GetSurveyList(GetSurveyListSettings settings);
        List<Survey> GetSurveyList(int page);
        List<Survey> GetSurveyList(int page, GetSurveyListSettings settings);
        List<Survey> GetSurveyList(int page, int pageSize);
        List<Survey> GetSurveyList(int page, int pageSize, GetSurveyListSettings settings);

        Survey GetSurveyDetails(long surveyId);

        List<Collector> GetCollectorList(long surveyId);
        List<Collector> GetCollectorList(long surveyId, GetCollectorListSettings settings);
        List<Collector> GetCollectorList(long surveyId, int page);
        List<Collector> GetCollectorList(long surveyId, int page, GetCollectorListSettings settings);
        List<Collector> GetCollectorList(long surveyId, int page, int pageSize);
        List<Collector> GetCollectorList(long surveyId, int page, int pageSize, GetCollectorListSettings settings);

        List<Respondent> GetRespondentList(long surveyId);
        List<Respondent> GetRespondentList(long surveyId, GetRespondentListSettings settings);
        List<Respondent> GetRespondentList(long surveyId, int page);
        List<Respondent> GetRespondentList(long surveyId, int page, GetRespondentListSettings settings);
        List<Respondent> GetRespondentList(long surveyId, int page, int pageSize);
        List<Respondent> GetRespondentList(long surveyId, int page, int pageSize, GetRespondentListSettings settings);

        List<Response> GetResponses(long surveyId, List<long> respondents);
        
        Collector GetResponseCounts(long collectorId);
        
        UserDetails GetUserDetails();

        CreateRecipientsResponse CreateRecipients(long collectorId, long emailMessageId, List<Recipient> recipients);
        
        //Data processing
        void FillMissingSurveyInformation(List<Survey> surveys);
        void FillMissingSurveyInformation(Survey survey);
    }
}