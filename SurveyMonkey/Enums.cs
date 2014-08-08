using Newtonsoft.Json;

namespace SurveyMonkey
{
    [JsonConverter(typeof(LaxEnumJsonConverter))]
    public enum QuestionFamily
    {
        NotSet = 0,
        SingleChoice,
        MultipleChoice,
        Matrix,
        OpenEnded,
        Demographic,
        DateTime,
        Presentation
    }

    [JsonConverter(typeof(LaxEnumJsonConverter))]
    public enum QuestionSubtype
    {
        NotSet = 0,
        Menu,
        Vertical,
        VerticalTwoCol,
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
        DateOnly,
        TimeOnly,
        DescriptiveText,
        Image,
        Numerical
    }

    public enum AnswerType
    {
        NotSet = 0,
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
        NotSet = 0,
        Url,
        Embedded,
        Email,
        Facebook,
        Audience
    }

    [JsonConverter(typeof(LaxEnumJsonConverter))]
    public enum RespondentCollectionMode
    {
        NotSet = 0,
        Normal,
        Manual,
        SurveyPreview,
        Edited
    }

    public enum RespondentStatus
    {
        NotSet = 0,
        Completed,
        Partial,
        Disqualified
    }

    public enum Language
    {
        NotSet = 0,
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