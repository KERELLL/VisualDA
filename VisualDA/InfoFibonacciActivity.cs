
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
    [Activity(Label = "InfoFibonacciActivity")]
    public class InfoFibonacciActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.info_fibonacci_activity);
            TextView taskDescription = FindViewById<TextView>(Resource.Id.taskDescription);
            TextView taskFormula = FindViewById<TextView>(Resource.Id.formulaTextView);
            TextView taskFormulaDes = FindViewById<TextView>(Resource.Id.formulaTextViewDes);
            Typeface tf = Typeface.CreateFromAsset(Assets, "OpenSans-Regular.ttf");
            taskDescription.SetTypeface(tf, TypefaceStyle.Normal);
            taskFormula.SetTypeface(tf, TypefaceStyle.Normal);
            taskFormulaDes.SetTypeface(tf, TypefaceStyle.Normal);
        }
    }
}
