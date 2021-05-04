
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using AndroidX.Core.Content;

namespace VisualDA
{
    [Activity(Label = "TestKnapsackActivity")]
    public class TestLCSActivity : Activity
    {
        public static string EXTRA_SCORE = "extraScore";

        LinearLayout linearLayout;
        ImageView imageView;
        TextView textViewScore;
        TextView textViewQuestionCount;
        TextView textViewQuestionImage;
        RadioGroup radioGroup;
        RadioButton radioButton1;
        RadioButton radioButton2;
        RadioButton radioButton3;
        Button testButton;

        ColorStateList textColorDefaultRb;
        List<Question> questionList;

        private int questionCounter;
        private int questionCountTotal;
        private Question currentQuestion;
        private int score;
        private bool answered;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.test_LCS_activiy);
            testButton = FindViewById<Button>(Resource.Id.buttonTest);
            imageView = FindViewById<ImageView>(Resource.Id.imageViewTest);
            textViewScore = FindViewById<TextView>(Resource.Id.textViewScore);
            textViewQuestionCount = FindViewById<TextView>(Resource.Id.textViewQuestionCount);
            textViewQuestionImage = FindViewById<TextView>(Resource.Id.questionImage);
            radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup);
            radioButton1 = FindViewById<RadioButton>(Resource.Id.radioButton1);
            radioButton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            radioButton3 = FindViewById<RadioButton>(Resource.Id.radioButton3);
            Typeface tf = Typeface.CreateFromAsset(Assets, "OpenSans-Regular.ttf");
            textViewScore.SetTypeface(tf, TypefaceStyle.Normal);
            textViewQuestionCount.SetTypeface(tf, TypefaceStyle.Normal);
            textViewQuestionImage.SetTypeface(tf, TypefaceStyle.Normal);
            textColorDefaultRb = radioButton1.TextColors;

            TestLCSDBHelper testDBHelper = new TestLCSDBHelper(this);
            questionList = testDBHelper.GetAllQuestions();

            questionCountTotal = questionList.Count;

            ShowNextQuestion();

            testButton.Click += delegate {
                if (!answered)
                {
                    if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked)
                    {
                        CheckAnswer();
                    }
                    else
                    {
                        Toast.MakeText(this, "Пожалуйста выберете ответ", ToastLength.Short).Show();
                    }
                }
                else
                {
                    ShowNextQuestion();
                }

            };
        }
        private void ShowNextQuestion()
        {
            radioButton1.SetTextColor(textColorDefaultRb);
            radioButton2.SetTextColor(textColorDefaultRb);
            radioButton3.SetTextColor(textColorDefaultRb);
            radioGroup.ClearCheck();
            if (questionCounter < questionCountTotal)
            {
                currentQuestion = questionList[questionCounter];
                imageView.SetBackgroundResource(currentQuestion.Image);
                textViewQuestionImage.Text = currentQuestion.QuestionImage;
                radioButton1.Text = currentQuestion.OptionOne;
                radioButton2.Text = currentQuestion.OptionTwo;
                radioButton3.Text = currentQuestion.OptionThree;
                questionCounter++;
                textViewQuestionCount.Text = "Вопрос: " + questionCounter + "/" + questionCountTotal;
                answered = false;
                testButton.Text = "Confirm";
            }
            else
            {
                FinishQuiz();
            }
        }

        private void CheckAnswer()
        {
            answered = true;
            RadioButton rbSelected = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);
            int answerNr = radioGroup.IndexOfChild(rbSelected) + 1;
            if (answerNr == currentQuestion.CorrectAnswer)
            {
                score++;
                textViewScore.Text = "Очки: " + score;
            }
            ShowSolution();
        }
        private void ShowSolution()
        {
            radioButton1.SetTextColor(Color.Red);
            radioButton2.SetTextColor(Color.Red);
            radioButton3.SetTextColor(Color.Red);
            switch (currentQuestion.CorrectAnswer)
            {
                case 1:
                    radioButton1.SetTextColor(Color.Green);
                    //textViewQuestion.Text = "Answer 1 is correct";
                    break;
                case 2:
                    radioButton2.SetTextColor(Color.Green);
                    //textViewQuestion.setText("Answer 2 is correct");
                    break;
                case 3:
                    radioButton3.SetTextColor(Color.Green);
                    //textViewQuestion.setText("Answer 3 is correct");
                    break;
            }
            if (questionCounter < questionCountTotal)
            {
                testButton.Text = "Next";
            }
            else
            {
                testButton.Text = "Finish";
            }
        }
        private void FinishQuiz()
        {
            Intent resultIntent = new Intent();
            resultIntent.PutExtra(EXTRA_SCORE, score);
            SetResult(Result.Ok, resultIntent);
            Finish();

        }
    }

}
