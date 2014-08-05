using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SurveyMonkeyApi
{
    public partial class SMApi
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum QuestionFamily
        {
            [EnumMember(Value = "single_choice")]
            SingleChoice,
            [EnumMember(Value = "multiple_choice")]
            MultipleChoice,
            Matrix,
            [EnumMember(Value = "open_ended")]
            OpenEnded,
            Demographic,
            DateTime,
            Presentation
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum QuestionSubtype
        {
            Menu,
            Vertical,
            [EnumMember(Value = "vertical_two_col")]
            VerticalTwoCol,
            [EnumMember(Value = "vertical_three_col")]
            VerticalThreeCol,
            Horiz,
            Ranking,
            Rating,
            Single,
            Multi,
            Essay,
            International,
            US,
            Both,
            [EnumMember(Value = "date_only")]
            DateOnly,
            [EnumMember(Value = "time_only")]
            TimeOnly,
            [EnumMember(Value = "descriptive_text")]
            DescriptiveText,
            Image,
            Numerical
        }

        public enum AnswerType
        {
            Row,
            Col,
            Other,
            Img,
            Label,
            Name,
            Company,
            Address,
            Address2,
            City,
            State,
            Zip,
            Country,
            Email,
            Phone
        }

        public enum CollectorType
        {
            Url,
            Embedded,
            Email,
            Facebook,
            Audience
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum RespondentCollectionMode
        {
            Normal,
            Manual,
            [EnumMember(Value = "survey_preview")]
            SurveyPreview,
            Edited
        }

        public enum RespondentStatus
        {
            Completed,
            Partial,
            Disqualified
        }

        public enum Language
        {
            English = 1,
            ChineseSimplified = 2,
            ChineseTraditional = 3,
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
            PortugueseIberian = 16,
            PortugueseBrazilian = 17,
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