using AForge.Imaging;
using System.Drawing;

namespace Clash_of_Bots.Functions
{
    public static class TroopsDetector
    {
        public static Point Detect(int id, Bitmap img)
        {
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.85f);
            TemplateMatch[] matchings = tm.ProcessImage(img, System.Drawing.Image.FromFile("images/troops/" + id + ".png") as Bitmap);

            if (matchings.Length < 1)
                return new Point(-1, -1);
            else
                return new Point(matchings[0].Rectangle.X + 37, matchings[0].Rectangle.Y + 580);
        }
    }
}