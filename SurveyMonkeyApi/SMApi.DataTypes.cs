namespace SurveyMonkeyApi
{
    public partial class SMApi
    {
        public enum QuestionFamilies
        {
            single_choice,
            multiple_choice,
            matrix,
            open_ended,
            Demographic,
            datetime,
            presentation
        }

        public enum QuestionSubtypes
        {
            menu,
            vertical,
            vertical_two_col,
            vertical_three_col,
            horiz,
            ranking,
            rating,
            single,
            multi,
            essay,
            international,
            us,
            both,
            date_only,
            time_only,
            descriptive_text,
            image,
            numerical
        }

        public enum AnswerTypes
        {
            row,
            col,
            other,
            img,
            label,
            name,
            company,
            address,
            address2,
            city,
            state,
            zip,
            country,
            email,
            phone
        }

        public enum CollectorTypes
        {
            url,
            embedded,
            email,
            facebook,
            audience
        }

        public enum RespondentCollectionModes
        {
            normal,
            manual,
            survey_preview,
            edited
        }

        public enum RespondentStatuses
        {
            completed,
            partial,
            disqualified
        }

        public enum LanguageIds
        {
            English = 1,
            Chinese_Simplified = 2,
            Chinese_Traditional = 3,
            Danish = 4,
            Dutch = 5,
            Finnish = 6,
            French = 7,
            German = 8,
            Greek = 9,
            Italian = 10,
            Japanese = 11,
            Korean = 12,
            Malay = 13,
            Norwegian = 14,
            Polish = 15,
            Portuguese_Iberian = 16,
            Portuguese_Brazilian = 17,
            Russian = 18,
            Spanish = 19,
            Swedish = 20,
            Turkish = 21,
            Ukrainian = 22,
            Reverse = 23,
            Albanian = 24,
            Arabic = 25,
            Armenian = 26,
            Basque = 27,
            Bengali = 28,
            Bosnian = 29,
            Bulgarian = 30,
            Catalan = 31,
            Croatian = 32,
            Czech = 33,
            Estonian = 34,
            Filipino = 35,
            Georgian = 36,
            Hebrew = 37,
            Hindi = 38,
            Hungarian = 39,
            Icelandic = 40,
            Indonesian = 41,
            Irish = 42,
            Kurdish = 43,
            Latvian = 44,
            Lithuanian = 45,
            Macedonian = 46,
            Malayalam = 47,
            Persian = 48,
            Punjabi = 49,
            Romanian = 50,
            Serbian = 51,
            Slovak = 52,
            Slovenian = 53,
            Swahili = 54,
            Tamil = 55,
            Telugu = 56,
            Thai = 57,
            Vietnamese = 58,
            Welsh = 59
        }
    }
}