using Microsoft.Maui.Controls;

namespace personality_quiz_1;

[QueryProperty(nameof(ResultText), "resultText")]
[QueryProperty(nameof(ImageSource), "imageSource")]
public partial class ResultsPage : ContentPage
{
    private string _resultText = string.Empty; // Initialize to avoid CS8618
    public string ResultText
    {
        get => _resultText;
        set
        {
            _resultText = value;
            ResultLabel.Text = _resultText;
        }
    }

    private string _imageSource = string.Empty;
    public string ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            if (!string.IsNullOrEmpty(_imageSource))
            {
                AnswerImage.Source = _imageSource;
                AnswerImage.IsVisible = true;

            }
            else
            {
                AnswerImage.IsVisible = false;
            }
        }
    }

    public ResultsPage()
    {
        InitializeComponent();
    }

    private async void OnRetakeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}
