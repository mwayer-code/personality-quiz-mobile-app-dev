using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using personality_quiz_1.Models;

namespace personality_quiz_1.Service
{
    internal class QuizService
    {
        private List<Question> _questions;
        private int _currentQuestionIndex;
        private Dictionary<int, int> _selectedAnswers = new();

        public QuizService(List<Question> questions)
        {
            _questions = questions;
        }

        public Question GetCurrentQuestion()
        {
            return _questions[_currentQuestionIndex];
        }

        public bool HasNextQuestion()
        {
            return _currentQuestionIndex < _questions.Count - 1;
        }

        public void SelectAnswer(int questionId, int answerId)
        {
            _selectedAnswers[questionId] = answerId;
        }

        public void MoveToNextQuestion()
        {
            if (HasNextQuestion())
            {
                _currentQuestionIndex++;
            }
        }

        public QuizResult CalculateResult()
        {
            var totalScore = _selectedAnswers
                .Select(pair => _questions.First(q => q.Id == pair.Key)
                .Answers.First(a => a.Id == pair.Value).Score)
                .Sum();

            string message;
            string imageSource;

            if (totalScore >= 4)
            {
                imageSource = "ironman.png";
                message = "Your personality is Ironman";
            }
            else if (totalScore >= 2)
            {
                imageSource = "spiderman.png";
                message = "Your personality is Spiderman";
            }
            else
            {
                imageSource ="batman.png";// Correct usage of ImageSource
                message = "Your personality is Batman";
            }

            return new QuizResult
            {
                Id = 1,
                Text = message,
                ImageSource = imageSource,
                Answers = _questions.SelectMany(q => q.Answers).ToList()
            };
        }

        public void Reset()
        {
            _currentQuestionIndex = 0;
            _selectedAnswers.Clear();
        }
    }
}
