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
            new() {
                Id = 1,
                Text = "I Ate Breakfast Today",
                ImageSource = "breakfast.png",
                Answers =
                [
                    new() { Id = 1, Text = "True", Score = 1 },
                    new() { Id = 2, Text = "False", Score = 0 },
                ]
            },
            new() {
                Id = 2,
                Text = "I Coded Today",
                ImageSource="coding.png",
                Answers =
                [
                    new() { Id = 3, Text = "True", Score = 1 },
                    new() { Id = 4, Text = "False", Score = 0 },
                ]
            },
            new() {
                Id = 3,
                Text = "My Favorite Sports Team is the Milwaukee Bucks",
                ImageSource="bucks.png",
                Answers =
                [
                    new Answer { Id = 5, Text = "True", Score = 1 },
                    new Answer { Id = 6, Text = "False", Score = 0 },
                ]
            },
             new() {
                Id = 4,
                Text = "I Drank 5 Glasses of Water Today",
                ImageSource="water.png",
                Answers =
                [
                    new() { Id = 7, Text = "True", Score = 1 },
                    new() { Id = 8, Text = "False", Score = 0 },
                ]
            },
              new() {
                Id = 5,
                Text = "I Wear Contacts/Glasses",
                ImageSource="glasses.png",
                Answers =
                [
                    new() { Id = 9, Text = "True", Score = 1 },
                    new() { Id = 10, Text = "False", Score = 0 },
                ]
            }
        };

        _quizService = new QuizService(questions);
        LoadQuestion();

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

    private void OnTrueTapped (object sender, EventArgs e)
    {
        var question = _quizService.GetCurrentQuestion();
        var trueAnswer = question.Answers.FirstOrDefault(a => a.Text == "True");

        if (trueAnswer != null)
        {
            ProcessAnswer(trueAnswer);
        }
    }

    private void OnFalseTapped (object sender, EventArgs e)
    {
        var question = _quizService.GetCurrentQuestion();
        var falseAnswer = question.Answers.FirstOrDefault(a => a.Text == "False");

        if (falseAnswer != null)
        {
            ProcessAnswer(falseAnswer);
        }
    }

    private void OnSwipedRight(object sender, SwipedEventArgs e)
    {
        var question = _quizService.GetCurrentQuestion();
        var trueAnswer = question.Answers.FirstOrDefault(a => a.Text == "True");

        if (trueAnswer != null)
        {
            ProcessAnswer(trueAnswer);  
        }
    }

    private void OnSwipedLeft(object sender, SwipedEventArgs e)
    {
        var question = _quizService.GetCurrentQuestion();
        var falseAnswer = question.Answers.FirstOrDefault(a => a.Text == "False");

        if (falseAnswer != null)
        {
            ProcessAnswer(falseAnswer);
        }
    }

    private async void ProcessAnswer(Answer selectedAnswer)
    {
        var question = _quizService.GetCurrentQuestion();
        _quizService.SelectAnswer(question.Id, selectedAnswer.Id);

        await ShowAnswerFeedback(selectedAnswer.Text == "True");

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

    private CancellationTokenSource _colorResetTokenSource = new CancellationTokenSource();

    private async Task ShowAnswerFeedback(bool isTrue)
    {

        //cancel pending color resets
        _colorResetTokenSource?.Cancel();
        _colorResetTokenSource = new CancellationTokenSource();
        var token = _colorResetTokenSource.Token;

        Color originalColor = Colors.DarkGray;
        try
        {
            // apply animation
            await QuestionImage.ScaleTo(0.8, 100);
            await QuestionImage.ScaleTo(1.0, 100);

            this.BackgroundColor = isTrue ? Colors.LightGreen : Colors.LightCoral;

            //wait for the specified delay w/ cancellation support
            await Task.Delay(200, token);
        }
        catch (TaskCanceledException)
        {
            
        }
        finally
        {
            //Always reset the background color if this specific token was't canceled 
            this.BackgroundColor = originalColor;
        }
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
        }
    }
}