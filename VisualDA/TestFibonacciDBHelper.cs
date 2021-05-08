using System;
using Android.Content;
using Android.Database.Sqlite;
using System.Collections.Generic;
using Android.Graphics;

namespace VisualDA
{
    public class TestFibonacciDBHelper : SQLiteOpenHelper
    {
        private static string DATABASE_NAME = "MyFibTest.db";
        private static int DATABASE_VERSION = 1;

        private SQLiteDatabase db;
        public TestFibonacciDBHelper(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {

        }

        public override void OnCreate(SQLiteDatabase db)
        {
            this.db = db;
            string SQL_CREATE_QUESTIONS_TABLE = "CREATE TABLE "
                + QuizContract.QuestionTable.TABLE_NAME + " ( " +
                QuizContract.QuestionTable._ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                QuizContract.QuestionTable.COLUMN_QUESTION + " INTEGER, " +
                QuizContract.QuestionTable.COLUMN_QUESTION_IMAGE + " TEXT, " +
                QuizContract.QuestionTable.COLUMN_OPTION1 + " TEXT, " +
                QuizContract.QuestionTable.COLUMN_OPTION2 + " TEXT, " +
                QuizContract.QuestionTable.COLUMN_OPTION3 + " TEXT, " +
                QuizContract.QuestionTable.COLUMN_ANSWER_NR + " INTEGER" + ")";
            db.ExecSQL(SQL_CREATE_QUESTIONS_TABLE);
            FillQuestionTable();
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXIST " + QuizContract.QuestionTable.TABLE_NAME);
            OnCreate(db);
        }

        private void FillQuestionTable()
        {
            Question question1 = new Question(Resource.Drawable.fibPic1, "Какое число должно стоять в красной ячейке?", "18", "21", "23", 2);
            AddQuestion(question1);
            Question question2 = new Question(Resource.Drawable.fibPic2, "Какое число должно стоять в красной ячейке?", "79", "83", "89", 3);
            AddQuestion(question2);
        }

        private void AddQuestion(Question question)
        {
            ContentValues cv = new ContentValues();
            cv.Put(QuizContract.QuestionTable.COLUMN_QUESTION, question.Image);
            cv.Put(QuizContract.QuestionTable.COLUMN_QUESTION_IMAGE, question.QuestionImage);
            cv.Put(QuizContract.QuestionTable.COLUMN_OPTION1, question.OptionOne);
            cv.Put(QuizContract.QuestionTable.COLUMN_OPTION2, question.OptionTwo);
            cv.Put(QuizContract.QuestionTable.COLUMN_OPTION3, question.OptionThree);
            cv.Put(QuizContract.QuestionTable.COLUMN_ANSWER_NR, question.CorrectAnswer);
            db.Insert(QuizContract.QuestionTable.TABLE_NAME, null, cv);
        }

        public List<Question> GetAllQuestions()
        {
            List<Question> questionList = new List<Question>();
            db = ReadableDatabase;
            var c = db.RawQuery("SELECT * FROM " + QuizContract.QuestionTable.TABLE_NAME, null);
            if (c.MoveToFirst())
            {
                do
                {
                    Question question = new Question();
                    question.Image = c.GetInt(c.GetColumnIndex(QuizContract.QuestionTable.COLUMN_QUESTION));
                    question.QuestionImage = c.GetString(c.GetColumnIndex(QuizContract.QuestionTable.COLUMN_QUESTION_IMAGE));
                    question.OptionOne = c.GetString(c.GetColumnIndex(QuizContract.QuestionTable.COLUMN_OPTION1));
                    question.OptionTwo = c.GetString(c.GetColumnIndex(QuizContract.QuestionTable.COLUMN_OPTION2));
                    question.OptionThree = c.GetString(c.GetColumnIndex(QuizContract.QuestionTable.COLUMN_OPTION3));
                    question.CorrectAnswer = c.GetInt(c.GetColumnIndex(QuizContract.QuestionTable.COLUMN_ANSWER_NR));
                    questionList.Add(question);
                } while (c.MoveToNext());
            }
            c.Close();
            return questionList;
        }
    }
}
