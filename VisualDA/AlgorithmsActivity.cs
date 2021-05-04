
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace VisualDA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class AlgorithmsActivity : Activity
    {
        ListView listView;
        List<Algorithm> algorithms;
        string highscore;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_algorithms);
            listView = FindViewById<ListView>(Resource.Id.listView);
            //Create data
    
            algorithms = new List<Algorithm>();
            algorithms.Add(new Algorithm(Resource.Drawable.fibonaccisequence, "Последовательность Фибоначчи", "Easy"));
            algorithms.Add(new Algorithm(Resource.Drawable.backpack, "Задача о рюкзаке", "Medium"));
            algorithms.Add(new Algorithm(Resource.Drawable.LCS2, "Поиск наибольшей общей подпоследовательности", "Hard"));

            AlgorithmAdapter algorithmAdapter = new AlgorithmAdapter(this, Resource.Layout.list_row, algorithms);

            listView.Adapter = algorithmAdapter;

            listView.ItemClick += (parent, args) =>
            {
                int x = args.Position;
                if (x == 1)
                {
                    Intent intent = new Intent(this, typeof(KnapsackProblemActivity));
                    StartActivity(intent);
                }
                if (x == 2)
                {
                    Intent intent = new Intent(this, typeof(LCSActivity));
                    StartActivity(intent);
                }
                if(x == 0)
                {
                    Intent intent = new Intent(this, typeof(FibonacciSequenceActivity));
                    StartActivity(intent);
                }
            };

        }

    }
}
