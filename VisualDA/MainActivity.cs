using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace VisualDA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button button;
        Button infoBtn;
        TextView font;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            button = FindViewById<Button>(Resource.Id.button1);
            infoBtn = FindViewById<Button>(Resource.Id.infoBtn);
            font = FindViewById<TextView>(Resource.Id.textView3);

            Typeface tf = Typeface.CreateFromAsset(Assets, "AlegreyaSans-Regular.ttf");
            font.SetTypeface(tf, TypefaceStyle.Normal);

            GoToNextActivities();
        }

        private void GoToNextActivities()
        {
            button.Click += delegate
            {
                Intent intent = new Intent(this, typeof(AlgorithmsActivity));
                StartActivity(intent);
                
            };

        }
    }
}

