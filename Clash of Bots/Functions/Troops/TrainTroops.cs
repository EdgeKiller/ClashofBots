using System;
using System.Drawing;
using System.Threading;
using BotLibNet;
using IniParser;
using IniParser.Model;
using CocFunctions;

namespace Clash_of_Bots
{
    class TrainTroops
    {
        public static void Train()
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            for (int i = 1; i <= 4; i++)
            {
                string stringPosition = data["barracks"]["barrack" + i];
                Point pointPosition = new Point(Convert.ToInt32(stringPosition.Split(';')[0]), Convert.ToInt32(stringPosition.Split(';')[1]));
                string stringTroop = data["troops"]["barrack" + i];
                int intTroop = Convert.ToInt32(stringTroop);
                if(pointPosition.X != -1 || pointPosition.Y != -1)
                {
                    Home.bsProcess.mouse.SendClick(WButton.Left, pointPosition, false);
                    Thread.Sleep(1000);
                    if (ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(Buttons.GetPos("5_5", Settings.xDif, Settings.yDif)), Color.FromArgb(112, 140, 176)))
                        Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("5_5", Settings.xDif, Settings.yDif), false);
                    else if(ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(Buttons.GetPos("3_3", Settings.xDif, Settings.yDif)), Color.FromArgb(161, 184, 207)))
                        Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("3_3", Settings.xDif, Settings.yDif), false);
                    else
                        Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("4_4", Settings.xDif, Settings.yDif), false);

                    Thread.Sleep(1000);
                    while (!SameRGB(Home.bsProcess.image.GetPixelColor(CocFunctions.Buttons.GetPos("troop" + intTroop, Settings.xDif, Settings.yDif))))
                    {
                        Home.bsProcess.mouse.SendClick(WButton.Left, CocFunctions.Buttons.GetPos("troop" + intTroop, Settings.xDif, Settings.yDif), false);
                        Thread.Sleep(100);
                    }
                    Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0,0), false);
                    Thread.Sleep(500);
                }
            }
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
        }

        public static bool IsTroopsReady()
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            string stringPosition = data["barracks"]["barrack1"];
            Point pointPosition = new Point(Convert.ToInt32(stringPosition.Split(';')[0]), Convert.ToInt32(stringPosition.Split(';')[1]));
            if (pointPosition.X != -1 || pointPosition.Y != -1)
            {
                Home.bsProcess.mouse.SendClick(WButton.Left, pointPosition, false);
                Thread.Sleep(500);
                if (ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(Buttons.GetPos("5_5", Settings.xDif, Settings.yDif)), Color.FromArgb(112, 140, 176)))
                    Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("5_5", Settings.xDif, Settings.yDif), false);
                else
                    Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("4_4", Settings.xDif, Settings.yDif), false);
                Thread.Sleep(500);
                if (ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(Buttons.GetPos("troop_ready", Settings.xDif, Settings.yDif)), Color.FromArgb(208, 68, 80)))
                {
                    Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
                    Thread.Sleep(500);
                    Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
                    return true;
                }
            }
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
            Thread.Sleep(500);
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
            return false;
        }

        private static bool SameRGB(Color color)
        {
            int colorDiff1 = color.R - color.G;
            int colorDiff2 = color.R - color.B;
            if (colorDiff1 < 5 && colorDiff2 < 5)
                return true;
            return false;
        }
    }
}
