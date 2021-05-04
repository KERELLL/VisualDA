using System;
namespace VisualDA
{
    public class Algorithm
    {
        int image;
        string name;
        string subTitle;

        public Algorithm(int image, string name, string subTitle)
        {
            Image = image;
            Name = name;
            SubTitle = subTitle;
        }

        public int Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string SubTitle
        {
            get
            {
                return subTitle;
            }
            set
            {
                subTitle = value;
            }
        }
    }
}
