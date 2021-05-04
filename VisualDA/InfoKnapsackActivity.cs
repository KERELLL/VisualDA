
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

namespace VisualDA
{
    [Activity(Label = "InfoKnapsackActivity")]
    public class InfoKnapsackActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.info_knapsack_activity);
            TextView taskDescription = FindViewById<TextView>(Resource.Id.taskDescription);
            TextView codeView1 = FindViewById<TextView>(Resource.Id.codeView1);
            TextView codeView2 = FindViewById<TextView>(Resource.Id.codeView2);
            TextView codeView3 = FindViewById<TextView>(Resource.Id.codeView3);
            TextView taskMethod = FindViewById<TextView>(Resource.Id.taskMethod);
            Typeface tf = Typeface.CreateFromAsset(Assets, "OpenSans-Regular.ttf");
            taskDescription.SetTypeface(tf, TypefaceStyle.Normal);
            codeView1.SetTypeface(tf, TypefaceStyle.Normal);
            codeView2.SetTypeface(tf, TypefaceStyle.Normal);
            codeView3.SetTypeface(tf, TypefaceStyle.Normal);
            taskMethod.SetTypeface(tf, TypefaceStyle.Normal);
        }
    }
}
