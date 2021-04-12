
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace VisualDA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class FibonacciSequenceActivity : Activity
    {
        Button startButton;
        EditText editTextFb;
        TextView textView;
        TableLayout tableLayout;
        bool pause = true;
        int num;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.fibonacci_sequence_activity);
            startButton = FindViewById<Button>(Resource.Id.buttonStart);
            editTextFb = FindViewById<EditText>(Resource.Id.editText1);
            textView = FindViewById<TextView>(Resource.Id.textView2);

            StartVisualizing();
        }

        private void StartVisualizing()
        {
            startButton.Click += delegate
            {
                if (editTextFb.Text == "")
                {
                    Toast.MakeText(this, "", ToastLength.Long).Show();
                    editTextFb.Focusable = true;
                    editTextFb.FocusableInTouchMode = true;
                    editTextFb.InputType = Android.Text.InputTypes.ClassText;
                    startButton.Enabled = true;
                }
                else
                {
                    num = int.Parse(editTextFb.Text);
                    if (num < 1 || num > 11)
                    {
                        Toast.MakeText(this, "", ToastLength.Long).Show();
                    }
                    else
                    {
                        editTextFb.Focusable = false;
                        editTextFb.FocusableInTouchMode = false;
                        editTextFb.InputType = Android.Text.InputTypes.Null;
                        startButton.Enabled = false;
                        tableLayout = CreateTable();
                        DoAlgorithm(tableLayout);
                    }
                }
            };
        }
        private TableLayout CreateTable()
        {
            TableLayout tableLayout = new TableLayout(this);
            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            TableRow row = new TableRow(this);
            for (int i = 0; i < num; i++)
            {
                TextView textView = new TextView(this);
                textView.SetTextColor(Android.Graphics.Color.White);
                textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                textView.Gravity = GravityFlags.Center;
                row.AddView(textView);
            }
            tableLayout.AddView(row);
            tableLayout.SetGravity(GravityFlags.CenterHorizontal);
            linearLayout.AddView(tableLayout);
            return tableLayout;
        }

        private async void DoAlgorithm(TableLayout tableLayout)
        {
            int[] sequence = new int[num];
            TableRow row = (TableRow)tableLayout.GetChildAt(0);
            for (int i = 0; i < sequence.Length; i++)
            {
                TextView cell = (TextView)row.GetChildAt(i);
                if(i == 0 || i == 1)
                {
                    sequence[i] = 1;
                }
                else
                {
                    sequence[i] = 0;
                }
                cell.Text = sequence[i].ToString();
            }
            for(int i = 2; i < sequence.Length; i++)
            {
                ResetTableColor(tableLayout);
                TextView cell = (TextView)row.GetChildAt(i);
                TextView cell1 = (TextView)row.GetChildAt(i - 1);
                TextView cell2 = (TextView)row.GetChildAt(i - 2);
                sequence[i] = sequence[i - 2] + sequence[i - 1];
                cell1.SetBackgroundResource(Resource.Drawable.cubBlue);
                cell2.SetBackgroundResource(Resource.Drawable.cubBlue);
                await Task.Delay(500);
                cell.SetBackgroundResource(Resource.Drawable.cubRed);
                cell.Text = sequence[i].ToString();
                await Task.Delay(500);
            }
            ResetTableColor(tableLayout);
        }
        private async Task Pause()
        {
            while (pause)
            {
                await Task.Delay(100);
            }
        }
        private void ResetTableColor(TableLayout tableLayout)
        {
            TableRow row = (TableRow)tableLayout.GetChildAt(0);
            for (int i = 2; i < num; i++)
            {
                TextView cell = (TextView)row.GetChildAt(i);
                TextView cell1 = (TextView)row.GetChildAt(i-1);
                TextView cell2 = (TextView)row.GetChildAt(i-2);
                cell.SetBackgroundResource(Resource.Drawable.cubGrey2);
                cell1.SetBackgroundResource(Resource.Drawable.cubGrey2);
                cell2.SetBackgroundResource(Resource.Drawable.cubGrey2);
            }
        }
    }
}
