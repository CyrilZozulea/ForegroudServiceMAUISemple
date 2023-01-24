using Android.App;
using Android.Content;
using Android.OS;
using ForegroudServiceMAUISemple.Models;
using ForegroudServiceMAUISemple.Platforms.Android;

namespace ForegroudServiceMAUISemple;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
        InitializeComponent();
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
        Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(ForegroundService));
        Android.App.Application.Context.StartForegroundService(intent);

        SettingGrid.IsVisible = false;
        Timer.IsVisible = true;
        CounterBtn.IsVisible = false;

        TimerUpdate();
    }

    void OnHoursScroll(object item, ItemsViewScrolledEventArgs e)
	{
		string[] hours = (string[])((CollectionView)item).ItemsSource;
        App.timer.Hours = Int32.Parse(hours[e.FirstVisibleItemIndex]);
	}

    void OnMinutesScroll(object item, ItemsViewScrolledEventArgs e)
    {
        string[] minutes = (string[])((CollectionView)item).ItemsSource;
        App.timer.Minutes = Int32.Parse(minutes[e.FirstVisibleItemIndex]);
    }

    void OnSecondsScroll(object item, ItemsViewScrolledEventArgs e)
    {
        string[] seconds = (string[])((CollectionView)item).ItemsSource;
        App.timer.Seconds = Int32.Parse(seconds[e.FirstVisibleItemIndex]);
    }

    async void TimerUpdate()
    {
        TimeSpan Time = new TimeSpan(App.timer.Hours, App.timer.Minutes, App.timer.Seconds);

        HoursLabel.Text = Time.Hours.ToString();
        MinutesLabel.Text = Time.Minutes.ToString();
        SecondsLabel.Text = Time.Seconds.ToString();

        while (true)
        {
            await Task.Delay(1000);

            TimeSpan tmp = new TimeSpan(Time.Hours, Time.Minutes, Time.Seconds - 1);
            Time = tmp;

            if (tmp.ToString() == "-00:00:01")
            {
                SettingGrid.IsVisible = true;
                Timer.IsVisible = false;
                CounterBtn.IsVisible = true;

                break;
            }

            HoursLabel.Text = Time.Hours.ToString();
            MinutesLabel.Text = Time.Minutes.ToString();
            SecondsLabel.Text = Time.Seconds.ToString();
        }
    }
}

