using System;
using System.Collections.Generic;

namespace SurveyMonkeyApi
{
    public class SingleChoiceAnswer
    {
        public string Choice { get; set; }
        public string OtherText { get; set; }
    }

    public class MultipleChoiceAnswer
    {
        public List<string> Choices { get; set; }
        public string OtherText { get; set; } 
    }

    public class OpenEndedSingleAnswer
    {
        public string Text { get; set; }
    }

    public class OpenEndedMultipleAnswer
    {
        public List<OpenEndedMultipleAnswerRow> Rows { get; set; }
    }

    public class OpenEndedMultipleAnswerRow
    {
        public string Text { get; set; }
        public long AnswerId { get; set; }
        public string RowName { get; set; }
    }

    public class DemographicAnswer
    {
        public string name { get; set; }
        public string company { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class DateTimeAnswer
    {
        public List<DateTimeAnswerRow> Rows { get; set; }
    }

    public class DateTimeAnswerRow
    {
        public DateTime TimeStamp { get; set; }
        public long AnswerId { get; set; }
        public string RowName { get; set; }
    }

    public class MatrixMenuAnswer
    {
        public Dictionary<long, MatrixMenuAnswerRow> Rows { get; set; }
        public string OtherText { get; set; }
    }

    public class MatrixMenuAnswerRow
    {
        public string RowName { get; set; }
        public long RowId { get; set; }
        public Dictionary<long, MatrixMenuAnswerColumn> Columns { get; set; }
    }

    public class MatrixMenuAnswerColumn
    {
        public string ColumnName { get; set; }
        public long ColumnId { get; set; }
        public string Choice {get; set; }
    }

    public class MatrixRankingAnswer
    {
        public Dictionary<int, string> Ranking { get; set; }
        public List<string> NotApplicable { get; set; }
    }

    public class MatrixRatingAnswer
    {
        public List<MatrixRatingAnswerRow> Rows { get; set; }
        public string OtherText { get; set; }
    }

    public class MatrixRatingAnswerRow
    {
        public string RowName { get; set; }
        public string Choice { get; set; }
        public string OtherText { get; set; }
    }

    public class MatrixSingleAnswer
    {
        public List<MatrixSingleAnswerRow> Rows { get; set; }
        public string OtherText { get; set; }
    }

    public class MatrixSingleAnswerRow
    {
        public string RowName { get; set; }
        public string Choice { get; set; }
    }

    public class MatrixMultiAnswer
    {
        public List<MatrixMultiAnswerRow> Rows { get; set; }
        public string OtherText { get; set; }
    }

    public class MatrixMultiAnswerRow
    {
        public string RowName { get; set; }
        public List<string> Choices { get; set; }
    }

    public class ProcessedAnswer
    {
        public object Response { get; set; }
        public SMApi.QuestionFamily QuestionFamily { get; set; }
        public SMApi.QuestionSubtype QuestionSubtype { get; set; }
    }
}