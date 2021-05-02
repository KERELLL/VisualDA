
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public class KnapsackProblemActivity : Activity
    {
        private static int REQUEST_CODE_TEST = 1;
        public static string SHARED_PREFS = "sharedPrefs";
        public static string KEY_HIGHSCORE = "keyHighscore";
        ImageButton buttonStart;
        ImageButton buttonPrevStep;
        ImageButton buttonNextStep;
        Button testButton;
        LinearLayout linearLayout;
        SeekBar seekBar;
        TextView speedOfAlgo;
        TextView code1;
        TextView code2;
        TextView code3;
        TextView textHighscore;
        TextView action;
        TextView startMenu;
        bool pause = true;
        bool stop = false;
        int step;
        TableLayout tableLayout;
        TableLayout tableLayoutW;
        TableLayout tableLayoutC;
        int N = 4;
        int W = 7;
        int[] weights = new int[4] {1, 3, 4, 5 };
        int[] costs = new int[4] { 1, 4, 5, 7 };
        int highscore;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.knapsack_problem_activity);
            buttonStart = FindViewById<ImageButton>(Resource.Id.buttonStart);
            buttonPrevStep = FindViewById<ImageButton>(Resource.Id.buttonPrevStep);
            buttonNextStep = FindViewById<ImageButton>(Resource.Id.buttonNextStep);
            testButton = FindViewById<Button>(Resource.Id.buttonTest);
            seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            speedOfAlgo = FindViewById<TextView>(Resource.Id.speedOfAlgo);
            code1 = FindViewById<TextView>(Resource.Id.textViewCode1);
            code2 = FindViewById<TextView>(Resource.Id.textViewCode2);
            code3 = FindViewById<TextView>(Resource.Id.textViewCode3);
            action = FindViewById<TextView>(Resource.Id.action);
            startMenu = FindViewById<TextView>(Resource.Id.startMenu);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            textHighscore = FindViewById<TextView>(Resource.Id.textHighscore);
            LoadHighscore();
            
            seekBar.ProgressChanged += new EventHandler<SeekBar.ProgressChangedEventArgs>(seekBarProgressChanged);
            Typeface tf = Typeface.CreateFromAsset(Assets, "OpenSans-Regular.ttf");
            action.SetTypeface(tf, TypefaceStyle.Normal);
            startMenu.SetTypeface(tf, TypefaceStyle.Normal);
            int counter2 = 0;
            step = 0;
            tableLayout = CreateTable();
            tableLayoutW = CreateTableWeights();
            tableLayoutC = CreateTableCosts();

            CancellationTokenSource prevTokenSource = new CancellationTokenSource();
            buttonStart.Click += delegate {
                counter2++;
                if (pause == true)
                {
                    buttonStart.SetBackgroundResource(Resource.Drawable.pause);
                    pause = false;
                }
                else
                {
                    buttonStart.SetBackgroundResource(Resource.Drawable.play);
                    pause = true;
                }
                if (counter2 < 2)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    prevTokenSource = cts;
                    DoAlgorithm(tableLayout, cts.Token);
                }
            };
            buttonPrevStep.Click += delegate {
                if (pause)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    prevTokenSource.Cancel();
                    linearLayout.RemoveAllViews();
                    int currentStep = step;
                    step = 0;
                    DoAlgorithm(CreateTable(), cts.Token, currentStep - 1);
                    prevTokenSource = cts;
                }
            };

            buttonNextStep.Click += delegate {
                if (pause)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    prevTokenSource.Cancel();
                    linearLayout.RemoveAllViews();
                    int currentStep = step;
                    step = 0;
                    DoAlgorithm(CreateTable(), cts.Token, currentStep + 1);
                    prevTokenSource = cts;
                }
            };
            testButton.Click += delegate {
                Intent intent = new Intent(this, typeof(TestKnapsackActivity));
                StartActivityForResult(intent, REQUEST_CODE_TEST);
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if(requestCode == REQUEST_CODE_TEST)
            {
                if(resultCode == Result.Ok)
                {
                    int score = data.GetIntExtra(TestKnapsackActivity.EXTRA_SCORE, 0);
                    if(score > highscore)
                    {
                        UpdateHighscore(score);
                    }
                }
            }
        }

        private void LoadHighscore()
        {
            ISharedPreferences prefs = GetSharedPreferences(SHARED_PREFS, FileCreationMode.Private);
            highscore = prefs.GetInt(KEY_HIGHSCORE, 0);
            textHighscore.Text = "Highscore: " + highscore;
        }
        private void UpdateHighscore(int highscoreNew)
        {
            highscore = highscoreNew;
            textHighscore.Text = "Highscore: " + highscore;
            ISharedPreferences prefs = GetSharedPreferences(SHARED_PREFS, FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutInt(KEY_HIGHSCORE, highscore);
            editor.Apply();
        }

        private void seekBarProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            speedOfAlgo.Text = ((e.Progress / 1000.0)).ToString() + " секунда";
        }

        private async void DoAlgorithm(TableLayout tableLayout, CancellationToken token, int stepToGo = 0)
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
                    code3.SetTextColor(Android.Graphics.Color.Black);
                    ResetTableColor(tableLayout);
                    ResetTableColorW(tableLayoutW);
                    ResetTableColorC(tableLayoutC);
                    double valueOfSeekBar = seekBar.Progress;
                    if (i == 0 || j == 0)
                    {
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        step++;
                        code3.SetTextColor(Android.Graphics.Color.Red);
                        D[i, 0] = 0;
                        action.Text = "A[i, 0] = 0";
                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        cell.Text = D[i, j].ToString();
                        if (step >= stepToGo)
                        {
                            await Pause();
                            await Task.Delay((int)valueOfSeekBar);
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                        }
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
                        step++;
                        D[i, j] = Math.Max(D[i - 1, j], D[i - 1, j - weights[i-1]] + costs[i-1]);
                        action.Text = $"Max({D[i - 1, j]}, {D[i - 1, j - weights[i - 1]]} + {costs[i - 1]}(ЦЕНА)) = {Math.Max(D[i - 1, j], D[i - 1, j - weights[i - 1]] + costs[i - 1])}";
                        code1.SetTextColor(Android.Graphics.Color.Red);
                        cell2.SetBackgroundResource(Resource.Drawable.cubRed);
                        cell3.SetBackgroundResource(Resource.Drawable.cubRed);
                        if (step >= stepToGo)
                        {
                            await Pause();
                            await Task.Delay((int)valueOfSeekBar);
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        cell.Text = D[i, j].ToString();
                        step++;
                        if (step >= stepToGo)
                        {
                            await Pause();
                            await Task.Delay((int)valueOfSeekBar);
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        step++;
                        code2.SetTextColor(Android.Graphics.Color.Red);
                        D[i, j] = D[i - 1, j];
                        action.Text = $"A[i, j] = D[i - 1, j] = {D[i - 1, j]}";
                        cell.Text = D[i, j].ToString();
                        if (step >= stepToGo)
                        {
                            await Pause();
                            await Task.Delay((int)valueOfSeekBar);
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                    }
                    if (stop)
                    {
                        return;
                    }
                }
            }
            code1.SetTextColor(Android.Graphics.Color.Black);
            code2.SetTextColor(Android.Graphics.Color.Black);
            ResetTableColor(tableLayout);
            ResetTableColorW(tableLayoutW);
            ResetTableColorC(tableLayoutC);
            action.Text = "Алгоритм закончен";
        }
        private TableLayout CreateTable()
        {
            TableLayout tableLayout = new TableLayout(this);


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
