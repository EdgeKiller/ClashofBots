using BotLibNet;
using CocFunctions;
using IniParser;
using IniParser.Model;
using System;
using System.Drawing;
using System.Threading;

namespace Clash_of_Bots
{
    class SearchVillage
    {
        public static void Search()
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            bool alert = Convert.ToBoolean(data["search"]["alert"]);

            int goldFound, elixirFound, darkFound, trophyFound;
            bool hasFound = false;
            bool hasDark = false;
            Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("attack", Settings.xDif, Settings.yDif), false);
            Thread.Sleep(500);
            Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("find_match", Settings.xDif, Settings.yDif), false);
            Thread.Sleep(1000);
            if (ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(Buttons.GetPos("disable_shield", Settings.xDif, Settings.yDif)), Color.FromArgb(201, 232, 106)))
            {
                Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("disable_shield", Settings.xDif, Settings.yDif), false);
            }
            while (!ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(Buttons.GetPos("next", Settings.xDif, Settings.yDif)), Color.FromArgb(219, 87, 0)))
            {
                Thread.Sleep(500);
            }
            while (!hasFound)
            {
                goldFound = 0;
                elixirFound = 0;
                darkFound = 0;
                trophyFound = 0;
                if (ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(new Point(36 + Settings.xDif, 159 + Settings.yDif)), Color.FromArgb(88, 69, 96)))
                    hasDark = true;
                else
                    hasDark = false;
                goldFound = Read.GetNumberResources(Home.bsProcess.image.CaptureRegion(Buttons.GetResourcesRec("gold", Settings.xDif, Settings.yDif)));
                elixirFound = Read.GetNumberResources(Home.bsProcess.image.CaptureRegion(Buttons.GetResourcesRec("elixir", Settings.xDif, Settings.yDif)));
                if (hasDark)
                {
                    darkFound = Read.GetNumberResources(Home.bsProcess.image.CaptureRegion(Buttons.GetResourcesRec("dark", Settings.xDif, Settings.yDif)));
                    trophyFound = Read.GetNumberResources(Home.bsProcess.image.CaptureRegion(Buttons.GetResourcesRec("trophy_dark", Settings.xDif, Settings.yDif)));
                }
                else
                    trophyFound = Read.GetNumberResources(Home.bsProcess.image.CaptureRegion(Buttons.GetResourcesRec("trophy", Settings.xDif, Settings.yDif)));
                Log.SetLog("[G] : " + goldFound + " [E] : " + elixirFound + " [D] : " + darkFound + " [T] : " + trophyFound);
                if(CompareResources(goldFound, elixirFound, darkFound, trophyFound))
                {
                    hasFound = true;
                    if (alert)
                    {
                        Console.Beep(300, 250);
                        Console.Beep(300, 250);
                    }
                }
                else
                {
                    Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("next", Settings.xDif, Settings.yDif), false);
                    Thread.Sleep(500);
                    while (!ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(Buttons.GetPos("next", Settings.xDif, Settings.yDif)), Color.FromArgb(219, 87, 0)))
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

        private static bool CompareResources(int fgold, int felixir, int fdark, int ftrophy)
        {
            bool good = true;
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            bool bgold = Convert.ToBoolean(data["search"]["bgold"]);
            bool belixir = Convert.ToBoolean(data["search"]["belixir"]);
            bool bdark = Convert.ToBoolean(data["search"]["bdark"]);
            bool btrophy = Convert.ToBoolean(data["search"]["btrophy"]);
            int wgold = Convert.ToInt32(data["search"]["gold"]);
            int welixir = Convert.ToInt32(data["search"]["elixir"]);
            int wdark = Convert.ToInt32(data["search"]["dark"]);
            int wtrophy = Convert.ToInt32(data["search"]["trophy"]);
            if (bgold && fgold < wgold)
                good = false;
            if (belixir && felixir < welixir)
                good = false;
            if (bdark && fdark < wdark)
                good = false;
            if (btrophy && ftrophy < wtrophy)
                good = false;
            return good;
        }

    }
}
