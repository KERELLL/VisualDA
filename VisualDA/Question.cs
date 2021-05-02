using System;
namespace VisualDA
{
    public class Question
    {
        int image;
        string questionImage;
        string optionOne;
        string optionTwo;
        string optionThree;
        int correctAnswer;

        public Question()
        {

        }

        public Question(int image, string questionImage, string optionOne, string optionTwo, string optionThree, int correctAnswer)
        {
            Image = image;
            QuestionImage = questionImage;
            OptionOne = optionOne;
            OptionTwo = optionTwo;
            OptionThree = optionThree;
            CorrectAnswer = correctAnswer;
        }

        public int Image { get => image; set => image = value; }
        public string OptionOne { get => optionOne; set => optionOne = value; }
        public string OptionTwo { get => optionTwo; set => optionTwo = value; }
        public string OptionThree { get => optionThree; set => optionThree = value; }
        public int CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }
        public string QuestionImage { get => questionImage; set => questionImage = value; }
    }
}
