
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace VisualDA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class LCSActivity : Activity
    {

        Button button;
        Button randomBtn;
        Button infoBtn;
        EditText edit1;
        EditText edit2;
        TextView font;
        static Random rnd = new Random();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_lcs);
            button = FindViewById<Button>(Resource.Id.button1);
            randomBtn = FindViewById<Button>(Resource.Id.randomGenerate);
            infoBtn = FindViewById<Button>(Resource.Id.infoBtn);
            edit1 = FindViewById<EditText>(Resource.Id.editText1);
            edit2 = FindViewById<EditText>(Resource.Id.editText2);
            font = FindViewById<TextView>(Resource.Id.textView3);


            GoToNextActivities();
        }

        private string GenerateString(string symbols, int len)
        {
            int position = 0;
            StringBuilder sb = new StringBuilder(len - 1);
            for (int i = 0; i < len; i++)
            {
                position = rnd.Next(0, symbols.Length - 1);
                sb.Append(symbols[position]);
            }
            return sb.ToString();
        }

        private void GoToNextActivities()
        {
            button.Click += delegate
            {
                if (edit1.Text.Length < 1 || edit1.Text.Length > 8 || edit2.Text.Length < 1 || edit2.Text.Length > 8)
                {
                    Toast.MakeText(this, "Текст должен быть от 1 до 10 символов", ToastLength.Long).Show();
                }
                else
                {
                    Intent intent = new Intent(this, typeof(VisualAlgorithmActivity));
                    intent.PutExtra("line", edit1.Text);
                    intent.PutExtra("column", edit2.Text);
                    StartActivity(intent);
                }
            };
            randomBtn.Click += delegate
            {
                Intent intent = new Intent(this, typeof(VisualAlgorithmActivity));
                string randomLine = GenerateString("QWERTYUIOPASDFGHJKLZXCVBNM", rnd.Next(2, 9));
                string randomColumn = GenerateString("QWERTYUIOPASDFGHJKLZXCVBNM", rnd.Next(2, 9));
                intent.PutExtra("line", randomLine);
                intent.PutExtra("column", randomColumn);
                StartActivity(intent);
            };
            infoBtn.Click += delegate
            {
                Intent intent = new Intent(this, typeof(InfoActivity));
                StartActivity(intent);
            };
        }
    }
}
