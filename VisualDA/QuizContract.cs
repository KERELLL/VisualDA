using System;
using Android.Provider;

namespace VisualDA
{
    public class QuizContract
    {
        private QuizContract() { }
        public static class QuestionTable
        {
            public static string TABLE_NAME = "quiz_question";
            public static string _ID = "_id";
            public static string COLUMN_QUESTION = "question";
            public static string COLUMN_QUESTION_IMAGE = "question_image";
            public static string COLUMN_OPTION1 = "option1";
            public static string COLUMN_OPTION2 = "option2";
            public static string COLUMN_OPTION3 = "option3";
            public static string COLUMN_ANSWER_NR = "answer_nr";
        }
    }
}
