using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Imaging;
using Clash_of_Bots.Statics;

namespace Clash_of_Bots.Functions
{
    public static class ResourcesReader
    {
        public static int Read(string type, Bitmap image)
        {
            List<PointInt> res = new List<PointInt>();
            List<Point> actual = new List<Point>();

            string result = "";

            for (int i = 0; i <= 9; i++)
            {
                actual = findPositions(image, System.Drawing.Image.FromFile(AppSettings.Images.resourcesPath + type + i + ".png") as Bitmap);
                foreach (Point p in actual)
                {
                    res.Add(new PointInt(p, i));
                }
            }

            res.Sort(Comparison);

            foreach (PointInt p in res)
            {
                result += p.Value;
            }

            return Convert.ToInt32(result);
        }

        public static int ReadTrophy(Bitmap image)
        {
            List<PointInt> res = new List<PointInt>();
            List<Point> actual = new List<Point>();

            string result = "";

            for (int i = 0; i <= 9; i++)
            {
                actual = findPositions(image, System.Drawing.Image.FromFile(AppSettings.Images.resourcesPath + "w" + i + ".png") as Bitmap);
                foreach (Point p in actual)
                {
                    res.Add(new PointInt(p, i));
                }
            }

            res.Sort(Comparison);

            foreach (PointInt p in res)
            {
                result += p.Value;
            }

            actual = findPositions(image, System.Drawing.Image.FromFile(AppSettings.Images.resourcesPath + "w-.png") as Bitmap);

            if (actual.Count > 0)
                result = "-" + result;

            return Convert.ToInt32(result);
        }

        static List<Point> findPositions(Bitmap big, Bitmap small)
        {
            List<Point> result = new List<Point>();
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.96f);
            TemplateMatch[] matchings = tm.ProcessImage(big, small);
            foreach (TemplateMatch m in matchings)
            {
                result.Add(new Point(m.Rectangle.Location.X, m.Rectangle.Location.Y));
            }
            return result;
        }

        static int Comparison(PointInt pos1, PointInt pos2)
        {
            if (pos1.Position.X >= pos2.Position.X)
                return 1;
            return -1;
        }
    }

    class PointInt
    {
        public int Value;
        public Point Position;

        public PointInt(Point pos, int value)
        {
            Value = value;
            Position = pos;
        }
    }
}