using System;
using System.Collections.Generic;

namespace SurveyMonkey
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
        public string RowName { get; set; }
    }

    public class DemographicAnswer
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class DateTimeAnswer
    {
        public List<DateTimeAnswerRow> Rows { get; set; }
    }

    public class DateTimeAnswerRow
    {
        public DateTime TimeStamp { get; set; }
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
        public Dictionary<long, MatrixMenuAnswerColumn> Columns { get; set; }
    }

    public class MatrixMenuAnswerColumn
    {
        public string ColumnName { get; set; }
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
        public SurveyMonkeyApi.QuestionFamily QuestionFamily { get; set; }
        public SurveyMonkeyApi.QuestionSubtype QuestionSubtype { get; set; }
    }
}