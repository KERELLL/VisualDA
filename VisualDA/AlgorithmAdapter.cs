using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
namespace VisualDA
{
    public class AlgorithmAdapter:ArrayAdapter<Algorithm>
    {
        private Context mContext;
        private int mResource;
        public AlgorithmAdapter(Context context, int resource, List<Algorithm> algorithms) : base(context, resource, algorithms)
        {
            this.mContext = context;
            this.mResource = resource;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(mContext);

            convertView = layoutInflater.Inflate(mResource, parent, false);

            ImageView imageView = convertView.FindViewById<ImageView>(Resource.Id.image);

            TextView textView = convertView.FindViewById<TextView>(Resource.Id.text);

            TextView textViewSub = convertView.FindViewById<TextView>(Resource.Id.textSub);

            TextView textViewHighscore = convertView.FindViewById<TextView>(Resource.Id.textHighscore);

            imageView.SetImageResource(GetItem(position).Image);

            textView.Text = GetItem(position).Name;
    
            textViewSub.Text = GetItem(position).SubTitle;

            if(textViewSub.Text == "Hard")
            {
                textViewSub.SetTextColor(Android.Graphics.Color.Red);
            }
            else if (textViewSub.Text == "Medium")
            {
                textViewSub.SetTextColor(Android.Graphics.Color.Orange);
            }
            else if (textViewSub.Text == "Easy")
            {
                string colorStr = "#43CA17"; 
                Color color = Color.ParseColor(colorStr);
                textViewSub.SetTextColor(color);
            }
            return convertView;
        }
    }
}
