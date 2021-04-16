
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class KnapsackProblemActivity : Activity
    {
        Button buttonStart;
        SeekBar seekBar;
        TextView speedOfAlgo;
        TextView code1;
        TextView code2;
        bool pause = true;
        bool stop = false;
        TableLayout tableLayout;
        TableLayout tableLayoutW;
        TableLayout tableLayoutC;
        int N = 4;
        int W = 7;
        int[] weights = new int[4] {1, 3, 4, 5 };
        int[] costs = new int[4] { 1, 4, 5, 7 };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.knapsack_problem_activity);
            buttonStart = FindViewById<Button>(Resource.Id.buttonStart);
            seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            speedOfAlgo = FindViewById<TextView>(Resource.Id.speedOfAlgo);
            code1 = FindViewById<TextView>(Resource.Id.textViewCode1);
            code2 = FindViewById<TextView>(Resource.Id.textViewCode2);
            seekBar.ProgressChanged += new EventHandler<SeekBar.ProgressChangedEventArgs>(seekBarProgressChanged);
            int counter2 = 0;
            tableLayout = CreateTable();
            tableLayoutW = CreateTableWeights();
            tableLayoutC = CreateTableCosts();
            buttonStart.Click += delegate {
                counter2++;
                if (pause == true)
                {
                    buttonStart.Text = "Стоп";
                    pause = false;
                }
                else
                {
                    buttonStart.Text = "Продолжить";
                    pause = true;
                }
                if (counter2 < 2)
                {
                    DoAlgorithm(tableLayout);
                }
            };
        }

        private void seekBarProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            speedOfAlgo.Text = ((e.Progress / 1000.0)).ToString() + " секунда";
        }

        private async void DoAlgorithm(TableLayout tableLayout)
        {
            int[,] D = new int[N + 1, W + 1];
            for (int i = 0; i < N + 1; i++)
            {
                TableRow row = (TableRow)tableLayout.GetChildAt(i + 1);
                TableRow rowW = (TableRow)tableLayoutW.GetChildAt(0);
                TableRow rowC = (TableRow)tableLayoutC.GetChildAt(0);
                for (int j = 0; j < W + 1; j++)
                {
                    code1.SetTextColor(Android.Graphics.Color.Black);
                    code2.SetTextColor(Android.Graphics.Color.Black);
                    ResetTableColor(tableLayout);
                    ResetTableColorW(tableLayoutW);
                    ResetTableColorC(tableLayoutC);
                    double valueOfSeekBar = seekBar.Progress;
                    if (i == 0 || j == 0)
                    {
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        D[i, 0] = 0;
                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        await Pause();
                        cell.Text = D[i, j].ToString();
                    }
                    else if (j - weights[i-1] >= 0)
                    {
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        TableRow row2 = (TableRow)tableLayout.GetChildAt(i);
                        TextView cell2 = (TextView)row2.GetChildAt(j+1);
                        TableRow row3 = (TableRow)tableLayout.GetChildAt(i);
                        TextView cell3 = (TextView)row3.GetChildAt(j - weights[i-1] + 1);
                        TextView cellW = (TextView)rowW.GetChildAt(i-1);
                        TextView cellC = (TextView)rowC.GetChildAt(i - 1);
                        cellW.SetBackgroundResource(Resource.Drawable.cubBlue);
                        cellC.SetBackgroundResource(Resource.Drawable.cubBlue);
                        D[i, j] = Math.Max(D[i - 1, j], D[i - 1, j - weights[i-1]] + costs[i-1]);
                        code1.SetTextColor(Android.Graphics.Color.Red);
                        cell2.SetBackgroundResource(Resource.Drawable.cubRed);
                        cell3.SetBackgroundResource(Resource.Drawable.cubRed);
                        await Pause();
                        await Task.Delay((int)valueOfSeekBar);
                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        await Pause();
                        await Task.Delay((int)valueOfSeekBar);
                        cell.Text = D[i, j].ToString();
                    }
                    else
                    {
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        await Pause();
                        code2.SetTextColor(Android.Graphics.Color.Red);
                        D[i, j] = D[i - 1, j];
                        cell.Text = D[i, j].ToString();
                    }
                    await Task.Delay((int)valueOfSeekBar);
                    await Pause();
                    if (stop)
                    {
                        return;
                    }
                }
            }
        }
        private TableLayout CreateTable()
        {
            TableLayout tableLayout = new TableLayout(this);

            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

            TableRow row = new TableRow(this);
            for (int j = 0; j < W + 2; j++)
            {
                TextView textView = new TextView(this);
                textView.SetTextColor(Android.Graphics.Color.White);
                textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                textView.Gravity = GravityFlags.Center;
                if (j == 0)
                {
                    textView.Text = "";
                }
                else
                {
                    textView.Text = (j-1).ToString();
                }
                row.AddView(textView);
            }
            tableLayout.AddView(row);
            linearLayout.AddView(tableLayout);
            for (int i = 0; i < N + 1; i++)
            {
                TableRow tableRow = new TableRow(this);
                tableLayout.AddView(tableRow);

                for (int j = -1; j < W + 1; j++)
                {
                    TextView textView = new TextView(this);
                    textView.SetTextColor(Android.Graphics.Color.White);
                    textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                    textView.Gravity = GravityFlags.Center;

                    if (j == -1)
                    {
                        textView.Text = i.ToString();
                    }

                    tableRow.AddView(textView);
                }
            }
            return tableLayout;
        }

        private TableLayout CreateTableWeights()
        {
            TableLayout tableLayout = new TableLayout(this);

            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout2);
            TableRow row = new TableRow(this);
            for (int i = 0; i < weights.Length; i++)
            {
                TextView textView = new TextView(this);
                textView.SetTextColor(Android.Graphics.Color.White);
                textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                textView.Gravity = GravityFlags.Center;
                textView.Text = weights[i].ToString();
                row.AddView(textView);
            }
            tableLayout.AddView(row);
            tableLayout.SetGravity(GravityFlags.CenterHorizontal);
            linearLayout.AddView(tableLayout);
            return tableLayout;
        }
        private TableLayout CreateTableCosts()
        {
            TableLayout tableLayout = new TableLayout(this);

            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout3);
            TableRow row = new TableRow(this);
            for (int i = 0; i < costs.Length; i++)
            {
                TextView textView = new TextView(this);
                textView.SetTextColor(Android.Graphics.Color.White);
                textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                textView.Gravity = GravityFlags.Center;
                textView.Text = costs[i].ToString();
                row.AddView(textView);
            }
            tableLayout.AddView(row);
            tableLayout.SetGravity(GravityFlags.CenterHorizontal);
            linearLayout.AddView(tableLayout);
            return tableLayout;

        }

        private void ResetTableColor(TableLayout tableLayout)
        {
            for (int i = 0; i < N + 1; i++)
            {
                TableRow row = (TableRow)tableLayout.GetChildAt(i + 1);
                for (int j = 0; j < W + 1; j++)
                {
                    TextView cell = (TextView)row.GetChildAt(j + 1);
                    cell.SetBackgroundResource(Resource.Drawable.cubGrey2);
                }
            }
        }
        private void ResetTableColorW(TableLayout tableLayout)
        {
            TableRow row = (TableRow)tableLayout.GetChildAt(0);
            for (int i = 0; i < weights.Length; i++)
            {
                TextView cell = (TextView)row.GetChildAt(i);
                cell.SetBackgroundResource(Resource.Drawable.cubGrey2);
            }
        }
        private void ResetTableColorC(TableLayout tableLayout)
        {
            TableRow row = (TableRow)tableLayout.GetChildAt(0);
            for (int i = 0; i < costs.Length; i++)
            {
                TextView cell = (TextView)row.GetChildAt(i);
                cell.SetBackgroundResource(Resource.Drawable.cubGrey2);
            }
        }
        private async Task Pause()
        {
            while (pause)
            {
                await Task.Delay(100);
            }
        }
    }
}
