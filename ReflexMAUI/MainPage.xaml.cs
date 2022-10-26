
namespace ReflexMAUI;

public partial class MainPage : ContentPage
{
    int Score = 0;
    public int Timeout = 30;

    Random rnd = new Random();
    Thread TimerThread;
    public MainPage()
    {
        InitializeComponent();
        TimerThread = new Thread(StartCountdown);
        TimerThread.IsBackground = true;
        TimerEditor.Text = Timeout.ToString();
        SemanticScreenReader.Announce(TimerEditor.Text);
        ScoreLabel.Text = $"Score: {Score}";
        SemanticScreenReader.Announce(ScoreLabel.Text);



    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        Score++;
        ScoreLabel.Text = $"Score: {Score}";
        RandomPlace();

    }
    public void RandomPlace()
    {
        var rndwidth = rnd.Next(0, (int)(this.Width - TargetButton.WidthRequest));
        var rndheight = rnd.Next(0, (int)(this.Height - TargetButton.HeightRequest - 60));
        TargetButton.Margin = new Thickness(rndwidth, rndheight, 0, 0);
        ScoreLabel.Text = $"Score: {Score} ";
    }

    public void StartCountdown()
    {
        while (Timeout != 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TimerLabel.Text = $"Time left: {Timeout}";
                SemanticScreenReader.Announce(TimerEditor.Text);

            });
            Thread.Sleep(1000);
            Timeout--;
        }
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await DisplayAlert("Score:", $"{Score}", "Ok");
            StartButton.IsEnabled = true;
            StartButton.IsVisible = true;
            TimerEditor.IsVisible = true;
            TargetButton.IsVisible = false;
            TimerLabel.Text = $"Set Time:";
            Score = 0;
            SemanticScreenReader.Announce(ScoreLabel.Text);

        });
    }
    public void StartButtonClicked(object sender, EventArgs e)
    {
        TimerEditor.IsVisible = false;
        TimerLabel.IsVisible = true;
        TargetButton.IsVisible = true;
        Timeout = int.Parse(TimerEditor.Text);
        StartButton.IsVisible = false;
        {
            TimerThread = new Thread(StartCountdown);
            TimerThread.IsBackground = true;
        }
        TimerThread.Start();
    }

}

