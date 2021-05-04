
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace VisualDA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class FibonacciSequenceActivity : Activity
    {
        private static int REQUEST_CODE_TEST = 1;
        public static string SHARED_PREFS = "sharedPrefs";
        public static string KEY_HIGHSCORE = "keyHighscore";
        Button startButton;
        Button infoButton;
        Button testButton;
        int highscore;
        ImageButton buttonPrevStep;
        ImageButton buttonNextStep;
        LinearLayout linearLayout;
        TextView textHighscore;
        SeekBar seekBar;
        EditText editTextFb;
        TextView textView;
        TextView speedOfAlgo;
        TableLayout tableLayout;
        bool pause = true;
        bool stop = false;
        int counter2 = 0;
        int step;
        int num;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.fibonacci_sequence_activity);
            startButton = FindViewById<Button>(Resource.Id.buttonStart);
            //buttonPrevStep = FindViewById<ImageButton>(Resource.Id.buttonPrevStep);
            //buttonNextStep = FindViewById<ImageButton>(Resource.Id.buttonNextStep);
            editTextFb = FindViewById<EditText>(Resource.Id.editText1);
            textView = FindViewById<TextView>(Resource.Id.textView2);
            seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            speedOfAlgo = FindViewById<TextView>(Resource.Id.speedOfAlgo);
            infoButton = FindViewById<Button>(Resource.Id.buttonInfo);
            textHighscore = FindViewById<TextView>(Resource.Id.textHighscore);
            testButton = FindViewById<Button>(Resource.Id.buttonTest);
            seekBar.ProgressChanged += new EventHandler<SeekBar.ProgressChangedEventArgs>(seekBarProgressChanged);
            StartVisualizing();
            testButton.Click += delegate {
                Intent intent = new Intent(this, typeof(TestKnapsackActivity));
                StartActivityForResult(intent, REQUEST_CODE_TEST);
            };
            infoButton.Click += delegate {
                GoToInfoActivity();
            };
        }

        private void seekBarProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            speedOfAlgo.Text = ((e.Progress / 1000.0)).ToString() + " секунда";
        }
        private void GoToInfoActivity()
        {
            Intent intent = new Intent(this, typeof(InfoKnapsackActivity));
            StartActivity(intent);
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

        private void StartVisualizing()
        {
            step = 0;
            CancellationTokenSource prevTokenSource = new CancellationTokenSource();
            startButton.Click += delegate
            {
                if (editTextFb.Text == "")
                {
                    Toast.MakeText(this, "Введите число", ToastLength.Long).Show();
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
                        Toast.MakeText(this, "Ваше число не входит в [1, 10]", ToastLength.Long).Show();
                    }
                    else
                    {
                        counter2++;
                        if (pause == true)
                        {
                            startButton.Text = "Стоп";
                            pause = false;
                        }
                        else
                        {
                            startButton.Text = "Продолжить";
                            pause = true;
                        }
                        editTextFb.Focusable = false;
                        editTextFb.FocusableInTouchMode = false;
                        editTextFb.InputType = Android.Text.InputTypes.Null;
                        CancellationTokenSource cts = new CancellationTokenSource();
                        prevTokenSource = cts;
                        if(counter2 < 2)
                        {
                            tableLayout = CreateTable();
                            DoAlgorithm(tableLayout, cts.Token);
                        }
                    }
                }
            };
        }

        private TableLayout CreateTable()
        {
            TableLayout tableLayout = new TableLayout(this);
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

        private async void DoAlgorithm(TableLayout tableLayout, CancellationToken token, int stepToGo = 0)
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
                double valueOfSeekBar = seekBar.Progress;
                ResetTableColor(tableLayout);
                TextView cell = (TextView)row.GetChildAt(i);
                TextView cell1 = (TextView)row.GetChildAt(i - 1);
                TextView cell2 = (TextView)row.GetChildAt(i - 2);
                sequence[i] = sequence[i - 2] + sequence[i - 1];
                step++;
                cell1.SetBackgroundResource(Resource.Drawable.cubBlue);
                cell2.SetBackgroundResource(Resource.Drawable.cubBlue);
                if (step >= stepToGo)
                {
                    await Pause();
                    await Task.Delay((int)valueOfSeekBar);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
                step++;
                cell.SetBackgroundResource(Resource.Drawable.cubRed);
                if (step >= stepToGo)
                {
                    await Pause();
                    await Task.Delay((int)valueOfSeekBar);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
                cell.Text = sequence[i].ToString();
            }
            ResetTableColor(tableLayout);
            startButton.Enabled = false;
            startButton.Text = "Алгоритм закончен";
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
