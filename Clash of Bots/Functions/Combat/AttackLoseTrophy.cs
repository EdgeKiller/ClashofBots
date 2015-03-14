using BotLibNet;
using CocFunctions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Clash_of_Bots
{
    class AttackLoseTrophy
    {
        public static void Attack()
        {
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

            List<Point> side_topleft = CocFunctions.Attack.topleft();
            List<Point> attackPoints = new List<Point>();
            attackPoints.AddRange(side_topleft);
            int troop1 = getTroopId(0);
            if (troop1 != -1)
            {
                Home.bsProcess.mouse.SendClick(WButton.Left, new Point(77 + Settings.xDif, 624 + Settings.yDif), false);
                Thread.Sleep(100);
                foreach (Point point in attackPoints)
                {
                    Random randX = new Random();
                    Random randY = new Random();
                    Point randPoint = new Point(point.X + randX.Next(-2, 3) + Settings.xDif, point.Y + randY.Next(-2, 3) + Settings.yDif);
                    Home.bsProcess.mouse.SendClick(WButton.Left, randPoint, false);
                    Thread.Sleep(50);
                }
            }

            Home.bsProcess.mouse.SendClick(WButton.Left, Buttons.GetPos("endbattle", Settings.xDif, Settings.yDif), false);
            Thread.Sleep(1000);
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(480 + Settings.xDif, 425 + Settings.yDif), false);
            Thread.Sleep(1000);

            while (!ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(new Point(440 + Settings.xDif, 560 + Settings.yDif)), Color.FromArgb(200, 230, 104)))
            {
                Thread.Sleep(5000);
            }
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(440 + Settings.xDif, 560 + Settings.yDif), false);
        
        }

        private static int getTroopId(int troopcase)
        {
            Color color = Color.FromArgb(0, 0, 0);
            switch (troopcase)
            {
                case 0:
                    color = Home.bsProcess.image.GetPixelColor(new Point(77 + Settings.xDif, 624 + Settings.yDif));
                    break;
                case 1:
                    color = Home.bsProcess.image.GetPixelColor(new Point(150 + Settings.xDif, 624 + Settings.yDif));
                    break;
                case 2:
                    color = Home.bsProcess.image.GetPixelColor(new Point(222 + Settings.xDif, 624 + Settings.yDif));
                    break;
                case 3:
                    color = Home.bsProcess.image.GetPixelColor(new Point(294 + Settings.xDif, 624 + Settings.yDif));
                    break;
            }
            if (ColorDif.isCorrect(color, Color.FromArgb(248, 237, 63)))
                return 0;
            if (ColorDif.isCorrect(color, Color.FromArgb(233, 67, 118)))
                return 1;
            if (ColorDif.isCorrect(color, Color.FromArgb(82, 165, 60)))
                return 2;
            if (ColorDif.isCorrect(color, Color.FromArgb(244, 122, 8)))
                return 3;
            return -1;
        }
    }
}
