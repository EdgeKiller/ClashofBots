using AForge.Imaging;
using BotLibNet2;
using Clash_of_Bots.BotPackage;
using Clash_of_Bots.Statics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Clash_of_Bots.Functions
{
    public static class LocateCollectors
    {
        public static void Locate(Bot bot)
        {
            for (int i = 1; i <= 17; i++)
            {
                if (MessageBox.Show("Please set the cursor on your collector number " + i + " and press enter. Click on cancel if not available", "Collectors localization", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Point collectorPos = FindCursorPos(bot.bs);
                    bot.cfgData["collectors"]["collector" + i] = collectorPos.X + ":" + collectorPos.Y;
                    bot.bs.mouse.SendClick(WButton.Left, collectorPos);
                }
                else
                    bot.cfgData["collectors"]["collector" + i] = "-1:-1";
            }

            bot.bs.mouse.SendClick(WButton.Left, new Point(0, 0));
            bot.SaveConfig();
        }

        public static void AutoLocate(Bot bot)
        {
            for (int x = 1; x <= 17; x++)
            {
                bot.cfgData["collectors"]["collector" + x] = "-1:-1";
            }

            Bitmap sourceImage = bot.bs.image.GetWindowImage(!Convert.ToBoolean(bot.cfgData["game"]["hidemode"]));

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.85f);

            int i = 1;

            TemplateMatch[] matchings = tm.ProcessImage(sourceImage, System.Drawing.Image.FromFile(AppSettings.Images.goldMine) as Bitmap);
            bot.Log(matchings.Length + " gold mine(s) found.");
            foreach (TemplateMatch m in matchings)
            {
                bot.cfgData["collectors"]["collector" + i] = (m.Rectangle.Location.X + 5) + ":" + (m.Rectangle.Location.Y + 15);
                i++;
            }

            matchings = tm.ProcessImage(sourceImage, System.Drawing.Image.FromFile(AppSettings.Images.elixirExtractor) as Bitmap);
            bot.Log(matchings.Length + " elixir extractor(s) found.");
            foreach (TemplateMatch m in matchings)
            {
                bot.cfgData["collectors"]["collector" + i] = (m.Rectangle.Location.X + 5) + ":" + (m.Rectangle.Location.Y + 15);
                i++;
            }

            matchings = tm.ProcessImage(sourceImage, System.Drawing.Image.FromFile(AppSettings.Images.darkExtractor) as Bitmap);
            bot.Log(matchings.Length + " dark elixir extractor(s) found.");
            foreach (TemplateMatch m in matchings)
            {
                bot.cfgData["collectors"]["collector" + i] = (m.Rectangle.Location.X + 5) + ":" + (m.Rectangle.Location.Y + 15);
                i++;
            }

            bot.SaveConfig();
        }

        static Point FindCursorPos(BotProcess bs)
        {
            Point bluestacksPos = bs.window.GetPosition();
            Point pos = bs.mouse.GetPosition();
            pos.X = pos.X - bluestacksPos.X;
            pos.Y = pos.Y - bluestacksPos.Y;
            return pos;
        }
    }
}