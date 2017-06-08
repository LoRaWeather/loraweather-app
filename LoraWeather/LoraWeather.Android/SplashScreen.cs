using Android.App;
using Android.OS;

namespace LoraWeather.Droid
{
    [Activity(Theme = "@style/SplashTheme", Label = "LoraWeather", Icon = "@drawable/icon",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));
        }
    }
}