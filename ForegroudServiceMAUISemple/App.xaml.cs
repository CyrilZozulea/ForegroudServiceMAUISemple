using ForegroudServiceMAUISemple.Models;
using ForegroudServiceMAUISemple.Platforms.Android;
using Plugin.Maui.Audio;

namespace ForegroudServiceMAUISemple;

public partial class App : Application
{
    public static TimerModel timer;
    public readonly IAudioManager audioManager;
    public App(IAudioManager audioManager)
	{
		InitializeComponent();
        timer = new TimerModel();

        MainPage = new MainPage();
        this.audioManager = audioManager;


    }

    public static void StopForegroundService()
    {
        Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(ForegroundService));
        Android.App.Application.Context.StopService(intent);
    }
}
