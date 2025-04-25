using System.Threading.Tasks;
using Microsoft.Maui;
using personality_quiz_1.Models;
using personality_quiz_1.Service;

namespace personality_quiz_1;

public partial class MainPage : ContentPage
{
    private readonly QuizService _quizService;

    public MainPage()
    {
        InitializeComponent();

        var questions = new List<Question>
        {
            new Question
            {
                Id = 1,
                Text = "I ate breakfast today",
                ImageSource = "breakfast.png",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "True", Score = 1 },
                    new Answer { Id = 2, Text = "False", Score = 0 },
                }
            },
            new Question
            {
                Id = 2,
                Text = "I coded today",
                ImageSource="coding.png",
                Answers = new List<Answer>
                {
                    new Answer { Id = 3, Text = "True", Score = 1 },
                    new Answer { Id = 4, Text = "False", Score = 0 },
                }
            },
            new Question
            {
                Id = 3,
                Text = "My favorite sports team is the Milwaukee Bucks",
                ImageSource="bucks.png",
                Answers = new List<Answer>
                {
                    new Answer { Id = 5, Text = "True", Score = 1 },
                    new Answer { Id = 6, Text = "False", Score = 0 },
                }
            },
             new Question
            {
                Id = 4,
                Text = "I drank 2 glasses of water",
                ImageSource="water.png",
                Answers = new List<Answer>
                {
                    new Answer { Id = 7, Text = "True", Score = 1 },
                    new Answer { Id = 8, Text = "False", Score = 0 },
                }
            },
              new Question
            {
                Id = 5,
                Text = "I wear contacts/glasses",
                ImageSource="glasses.png",
                Answers = new List<Answer>
                {
                    new Answer { Id = 9, Text = "True", Score = 1 },
                    new Answer { Id = 10, Text = "False", Score = 0 },
                }
            }
        };

        _quizService = new QuizService(questions);
        LoadQuestion();

        //Reset the selection in the collection view
        AnswersCollection.SelectedItem = null;
    }

    private void LoadQuestion()
    {
        var question = _quizService.GetCurrentQuestion();
        QuestionLabel.Text = question.Text;

        if (!string.IsNullOrEmpty(question.ImageSource))
        {
            QuestionImage.Source = question.ImageSource;
            QuestionImage.IsVisible = true;
        }
        else
        {
            QuestionImage.IsVisible = false;
        }

            AnswersCollection.ItemsSource = question.Answers;
        NextButton.IsVisible = false;
    }

    private void OnAnswerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Answer selectedAnswer)
        {
            _quizService.SelectAnswer(_quizService.GetCurrentQuestion().Id, selectedAnswer.Id);
            if (_quizService.HasNextQuestion())
            {
                NextButton.IsVisible = true;
            }
            else
            {
                NextButton.IsVisible = false;
                _ = NavigateToResultsPageAsync();

            }
        }
    }

    private async Task NavigateToResultsPageAsync()
    {
        var result = _quizService.CalculateResult();
        await Shell.Current.GoToAsync($"//{nameof(ResultsPage)}?resultText={Uri.EscapeDataString(result.Text)}&imageSource={Uri.EscapeDataString(result.ImageSource)}");
    }


    private void OnNextClicked(object sender, EventArgs e)
    {
        if (_quizService.HasNextQuestion())
        {
            _quizService.MoveToNextQuestion();
            LoadQuestion();
        }
        else
        {
            _ = NavigateToResultsPageAsync();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Check to reset the quiz when returning from results

        if (_quizService != null)
        {
            _quizService.Reset();
            LoadQuestion();
            AnswersCollection.SelectedItem = null;
        }
    }
}