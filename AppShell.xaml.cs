namespace personality_quiz_1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {

            InitializeComponent();

            Routing.RegisterRoute(nameof(ResultsPage), typeof(ResultsPage));
        }
    }
}
