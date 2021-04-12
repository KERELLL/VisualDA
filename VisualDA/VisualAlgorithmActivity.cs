using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System.Timers;
using Android.Widget;
using Android.Views;
using System.Threading.Tasks;
using AndroidX.AppCompat.App;

namespace VisualDA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class VisualAlgorithmActivity : Activity
    {
        Button buttonStart;
        TextView line;
        TextView lcs;
        TextView column;
        SeekBar seekBar;
        TextView speedOfAlgo;
        bool pause = true;
        bool stop = false;
        int counter;
        TextView action;
        TextView count;
        TableLayout tableLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.visualization_algorithm_activity);

            line = FindViewById<TextView>(Resource.Id.line);
            column = FindViewById<TextView>(Resource.Id.column);
            lcs = FindViewById<TextView>(Resource.Id.lcs);
            buttonStart = FindViewById<Button>(Resource.Id.buttonStart);
            seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            speedOfAlgo = FindViewById<TextView>(Resource.Id.speedOfAlgo);
            action = FindViewById<TextView>(Resource.Id.action);
            count = FindViewById<TextView>(Resource.Id.count);
            counter = 0;

            
            seekBar.ProgressChanged += new EventHandler<SeekBar.ProgressChangedEventArgs>(seekBarProgressChanged);
            

            line.SetTextColor(Android.Graphics.Color.Black);
            column.SetTextColor(Android.Graphics.Color.Black);
            lcs.SetTextColor(Android.Graphics.Color.Black);

            line.Text = Intent.GetStringExtra("line");
            column.Text = Intent.GetStringExtra("column");
            

            
            tableLayout = CreateTable();
            int counter2 = 0;

            buttonStart.Click += delegate {
                counter2++;
                if(pause == true)
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
                    DoAlgortihm(tableLayout);
                }
            };
        }

        private void seekBarProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            speedOfAlgo.Text = ((e.Progress/1000.0)).ToString() + " секунда";
        }

        /// <summary>
        /// Метод, который визуализирует алгоритм
        /// Подсвечивание чисел, заполнение таблицы
        /// </summary>
        /// <param name="tableLayout">передаем пустую таблицу</param>
        private async void DoAlgortihm(TableLayout tableLayout)
        {
            string lineText = line.Text;
            string columnText = column.Text;
            int lineLen = lineText.Length;
            int columnLen = columnText.Length;
            string speedOfal = speedOfAlgo.Text;

            int[][] subsequence = new int[columnLen + 1][];

            for (int i = 0; i < columnLen + 1; i++)
            {
                subsequence[i] = new int[lineLen + 1];
            }
            for (int i = 0; i < columnLen + 1; i++)
            {
                TableRow row = (TableRow)tableLayout.GetChildAt(i + 1);
                for (int j = 0; j < lineLen + 1; j++)
                {
                    ResetTableColor(tableLayout);
                    double valueOfSeekBar = seekBar.Progress;
                    if (i == 0 || j == 0)
                    {
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        subsequence[i][j] = 0;

                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        counter++;
                        await Pause();
                        count.Text = counter.ToString();
                        cell.Text = subsequence[i][j].ToString();
                        action.Text = "Записываем 0, так как индекс строки или столбца - нулевой";
                    }
                    else if (column.Text[i - 1] == line.Text[j - 1])
                    {
                        TableRow row2 = (TableRow)tableLayout.GetChildAt(i);
                        TextView cell2 = (TextView)row2.GetChildAt(j);
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        subsequence[i][j] = subsequence[i - 1][j - 1] + 1;

                        cell2.SetBackgroundResource(Resource.Drawable.cubRed);
                        counter++;

                        await Pause();
                        count.Text = counter.ToString();
                        await Task.Delay((int)valueOfSeekBar);
                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        counter++;
                        await Pause();
                        count.Text = counter.ToString();
                        cell.Text = subsequence[i][j].ToString();
                        action.Text = $"Совпадение символа {column.Text[i - 1]}, прибовляем еденицу";
                    }
                    else
                    {
                        TableRow row2 = (TableRow)tableLayout.GetChildAt(i);
                        TextView cell = (TextView)row.GetChildAt(j + 1);
                        TextView cell1 = (TextView)row2.GetChildAt(j + 1);
                        TextView cell2 = (TextView)row.GetChildAt(j);

                        subsequence[i][j] = Math.Max(subsequence[i - 1][j], subsequence[i][j - 1]);
                        action.Text = $"Берем максимум - {subsequence[i][j]}";

                        cell1.SetBackgroundResource(Resource.Drawable.cubRed);
                        counter++;
                        await Pause();
                        count.Text = counter.ToString();
                        await Task.Delay((int)valueOfSeekBar);

                        cell2.SetBackgroundResource(Resource.Drawable.cubRed);
                        counter++;
                        await Pause();
                        count.Text = counter.ToString();
                        await Task.Delay((int)valueOfSeekBar);

                        cell.SetBackgroundResource(Resource.Drawable.cubBlue);
                        counter++;
                        await Pause();
                        count.Text = counter.ToString();
                        await Task.Delay((int)valueOfSeekBar);

                        cell.Text = subsequence[i][j].ToString();
                    }
                    await Task.Delay((int)valueOfSeekBar);
                    await Pause();
                    if (stop)
                    {
                        return;
                    }
                }
            }
            OutputSubsequence(line.Text, column.Text, subsequence, tableLayout);
        }

        private async Task Pause()
        {
            while (pause)
            {
                await Task.Delay(100);
            }
        }
        /// <summary> 
        /// Метод, обновляющий таблицу
        /// </summary>
        /// <param name="tableLayout"></param>
        private void ResetTableColor(TableLayout tableLayout)
        {
            int lineLen = line.Text.Length;
            int columnLen = column.Text.Length;
            for(int i = 0; i < columnLen + 1;i++)
            {
                TableRow row = (TableRow)tableLayout.GetChildAt(i+1);
                for (int j = 0; j < lineLen + 1;j++)
                {
                    TextView cell = (TextView)row.GetChildAt(j + 1);
                    cell.SetBackgroundResource(Resource.Drawable.cubGrey2);
                }
            }
        }

        /// <summary>
        /// Метод, создающий пустую таблицу
        /// </summary>
        /// <returns>Возврощает пустую таблицу</returns>
        private TableLayout CreateTable()
        {
            TableLayout tableLayout = new TableLayout(this);

            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);


            string lineText = line.Text;
            string columnText = column.Text;
            int lineLen = line.Text.Length;

            int columnLen = column.Text.Length;

            TableRow row = new TableRow(this);
            for (int j = 0; j < lineLen + 2; j++)
            {
                TextView textView = new TextView(this);
                textView.SetTextColor(Android.Graphics.Color.White);
                textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                textView.Gravity = GravityFlags.Center;

                if (j == 0 || j == 1)
                {
                    textView.Text = " ";
                }
                else
                {
                    textView.Text = lineText[j - 2].ToString();
                }
                row.AddView(textView);
            }
            tableLayout.AddView(row);
            linearLayout.AddView(tableLayout);
            for (int i = 0; i < columnLen + 1; i++)
            {
                TableRow tableRow = new TableRow(this);
                tableLayout.AddView(tableRow);

                for (int j = -1; j < lineLen + 1; j++)
                {
                    TextView textView = new TextView(this);
                    textView.SetTextColor(Android.Graphics.Color.White);
                    textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                    textView.Gravity = GravityFlags.Center;

                    if (j == -1)
                    {
                        if (i == 0)
                        {
                            textView.Text = " ";
                        }
                        else
                        {
                            textView.Text = columnText[i - 1].ToString();
                        }
                    }

                    tableRow.AddView(textView);
                }
            }
            return tableLayout;
        }

        #region предыдущая версия
        /// <summary>
        /// Метод, который выводит первую строку таблицы.
        /// </summary>
        private void SymbolOutput()
        {
            TableLayout tableLayout = new TableLayout(this);
            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

            string lineText = line.Text;
            int lineLen = line.Text.Length;

            TableRow tableRow = new TableRow(this);

            for (int j = 0; j < lineLen+2; j++)
            {
                TextView textView = new TextView(this);
                textView.SetTextColor(Android.Graphics.Color.White);
                textView.SetBackgroundResource(Resource.Drawable.cubGrey2);
                textView.Gravity = GravityFlags.Center;

                if (j == 0 || j == 1)
                {
                    textView.Text = " ";
                }
                else
                {
                    textView.Text = lineText[j-2].ToString();
                }

                tableRow.AddView(textView);
            }
            tableLayout.AddView(tableRow);
            linearLayout.AddView(tableLayout);
        }
        #endregion

        /// <summary>
        /// Метод, который визуализирует обратный алгоритм lcs
        ///  Подсвечивание чисел.
        /// </summary>
        /// <param name="lineText">Строка</param>
        /// <param name="columnText">Столбец</param>
        /// <param name="subsequence">Массив чисел из алгоритма</param>
        /// <param name="tableLayout">Заполненная табоица</param>
        private async void OutputSubsequence(string lineText, string columnText, int[][] subsequence, TableLayout tableLayout)
        {
            string finalSubsequence = "";
            int i = columnText.Length;
            int j = lineText.Length;

            while (i >= 1 && j >= 1)
            {
                TableRow row = (TableRow)tableLayout.GetChildAt(i+1);
                double valueOfSeekBar = seekBar.Progress;
                if (columnText[i - 1] == lineText[j - 1])
                {
                    TextView cell = (TextView)row.GetChildAt(j + 1);

                    finalSubsequence = columnText[i - 1] + finalSubsequence;

                    cell.SetBackgroundResource(Resource.Drawable.cubRed);
                    counter++;
                    await Pause();
                    await Task.Delay((int)valueOfSeekBar);

                    count.Text = counter.ToString();
                    action.Text = $"Совпадение символа {column.Text[i - 1]}, поэтому идем по диагонале";
                    i--;
                    j--;
                }
                else if (subsequence[i - 1][j] > subsequence[i][j - 1])
                {
                    TableRow row2 = (TableRow)tableLayout.GetChildAt(i);
                    TextView cell = (TextView)row.GetChildAt(j + 1);
                    TextView cell1 = (TextView)row2.GetChildAt(j + 1);
                    TextView cell2 = (TextView)row.GetChildAt(j);

                    action.Text = $"Ячейка верхня больше ячейки нижней, идем вверх";

                    cell1.SetBackgroundResource(Resource.Drawable.cubBlue);
                    counter++;
                    await Pause();
                    count.Text = counter.ToString();
                    await Task.Delay((int)valueOfSeekBar);

                    cell2.SetBackgroundResource(Resource.Drawable.cubBlue);
                    counter++;
                    await Pause();
                    count.Text = counter.ToString();
                    await Task.Delay((int)valueOfSeekBar);

                    cell.SetBackgroundResource(Resource.Drawable.cubRed);
                    counter++;
                    await Pause();
                    count.Text = counter.ToString();
                    await Task.Delay((int)valueOfSeekBar);

                    i--;
                }
                else
                {
                    TableRow row2 = (TableRow)tableLayout.GetChildAt(i);
                    TextView cell = (TextView)row.GetChildAt(j + 1);
                    TextView cell1 = (TextView)row2.GetChildAt(j + 1);
                    TextView cell2 = (TextView)row.GetChildAt(j);

                    action.Text = $"Ячейка нижняя больше ячейки верхней, идем влево";

                    cell1.SetBackgroundResource(Resource.Drawable.cubBlue);
                    counter++;
                    await Pause();
                    await Task.Delay((int)valueOfSeekBar);

                    cell2.SetBackgroundResource(Resource.Drawable.cubBlue);
                    counter++;
                    await Pause();
                    await Task.Delay((int)valueOfSeekBar);

                    cell.SetBackgroundResource(Resource.Drawable.cubRed);
                    counter++;
                    await Pause();
                    await Task.Delay((int)valueOfSeekBar);

                    j--;
                }
                await Pause();
                if (stop)
                    return;
            }
            TextView lcs = FindViewById<TextView>(Resource.Id.lcs);
            lcs.Text = finalSubsequence + " длина: " + finalSubsequence.Length.ToString();

            buttonStart.Text = "Вернуться назад";
            action.Text = "Алгоритм закончен";

            buttonStart.Click+=delegate {
                Intent intent = new Intent(this, typeof(LCSActivity));
                StartActivity(intent);
                buttonStart.Text = "Начать";
            };
        }
    }
}
