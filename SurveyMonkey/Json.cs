using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SurveyMonkey
{

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    internal class JsonDeserializeGetSurveyList
    {
        public long SurveyId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Title { get; set; }
        public Language LanguageId { get; set; }
        public int QuestionCount { get; set; }
        public int NumResponses { get; set; }
        public string AnalysisUrl { get; set; }
        public string PreviewUrl { get; set; }

        public Survey ToSurvey()
        {
            var survey = new Survey()
            {
                SurveyId = SurveyId,
                DateCreated = DateCreated,
                DateModified = DateModified,
                Nickname = Title,
                Language = LanguageId,
                QuestionCount = QuestionCount,
                NumResponses = NumResponses,
                AnalysisUrl = AnalysisUrl,
                PreviewUrl = PreviewUrl
            };
            return survey;
        }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    internal class JsonDeserializeGetSurveyDetails
    {
        public long SurveyId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Language LanguageId { get; set; }
        public int NumResponses { get; set; }
        public int QuestionCount { get; set; }
        public string Nickname { get; set; }
        public Title Title { get; set; }
        public List<Page> Pages { get; set; }
        public int CustomVariableCount { get; set; }
        public List<CustomVariable> CustomVariables { get; set; }

        public Survey ToSurvey()
        {
            var survey = new Survey()
            {
                SurveyId = SurveyId,
                DateCreated = DateCreated,
                DateModified = DateModified,
                TitleText = Title.Text,
                TitleEnabled = Title.Enabled,
                Language = LanguageId,
                QuestionCount = QuestionCount,
                NumResponses = NumResponses,
                Nickname = Nickname,
                Pages = Pages,
                CustomVariableCount = CustomVariableCount,
                CustomVariables = CustomVariables
            };
            return survey;
        }
    }

    [JsonConverter(typeof(LaxPropertyNameJsonConverter))]
    internal class Title
    {
        public bool Enabled { get; set; }
        public string Text { get; set; }
    }

    //Using a custom converter to ignore underscores in the json returned by SM
    //http://stackoverflow.com/questions/19792274/alternate-property-name-while-deserializing/19885911#19885911
    internal class LaxPropertyNameJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsClass;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            object instance = objectType.GetConstructor(Type.EmptyTypes).Invoke(null);
            PropertyInfo[] props = objectType.GetProperties();

            JObject jo = JObject.Load(reader);
            foreach (JProperty jp in jo.Properties())
            {
                string name = Regex.Replace(jp.Name, "[^A-Za-z0-9]+", "");

                PropertyInfo prop = props.FirstOrDefault(pi =>
                    pi.CanWrite && string.Equals(pi.Name, name, StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                    prop.SetValue(instance, jp.Value.ToObject(prop.PropertyType, serializer));
            }

            return instance;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    //http://stackoverflow.com/questions/22752075/how-can-i-ignore-unknown-enum-values-during-json-deserialization/22755077#22755077
    internal class LaxEnumJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            Type type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            return type.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullable = IsNullableType(objectType);
            Type enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            string[] names = Enum.GetNames(enumType);

            if (reader.TokenType == JsonToken.String)
            {
                string enumText = Regex.Replace(reader.Value.ToString(), "[^A-Za-z0-9]+", "");

                if (!string.IsNullOrEmpty(enumText))
                {
                    string match = names
                        .Where(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (match != null)
                    {
                        return Enum.Parse(enumType, match);
                    }
                }
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                int enumVal = Convert.ToInt32(reader.Value);
                int[] values = (int[])Enum.GetValues(enumType);
                if (values.Contains(enumVal))
                {
                    return Enum.Parse(enumType, enumVal.ToString());
                }
            }

            if (!isNullable)
            {
                string defaultName = names
                    .Where(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (defaultName == null)
                {
                    defaultName = names.First();
                }

                return Enum.Parse(enumType, defaultName);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        private bool IsNullableType(Type t)
        {
            return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}