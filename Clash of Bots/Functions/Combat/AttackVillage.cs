using BotLibNet;
using CocFunctions;
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Clash_of_Bots
{
    class AttackVillage
    {


        public static void Attack()
        {
            List<Point> side_topleft = CocFunctions.Attack.topleft();
            List<Point> side_topright = CocFunctions.Attack.topright();
            List<Point> side_bottomleft = CocFunctions.Attack.bottomleft();
            List<Point> side_bottomright = CocFunctions.Attack.bottomright();
            List<Point> attackPoints = new List<Point>();
            List<int> attackTroops = new List<int>();
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            int sideToAttack = Convert.ToInt32(data["attack"]["sides"]);
            int attackMode = Convert.ToInt32(data["attack"]["mode"]);
            switch (sideToAttack)
            {
                case 0:
                    attackPoints.AddRange(side_topleft);
                    attackPoints.AddRange(side_topright);
                    break;
                case 1:
                    attackPoints.AddRange(side_topleft);
                    attackPoints.AddRange(side_topright);
                    attackPoints.AddRange(side_bottomleft);
                    break;
                case 2:
                    attackPoints.AddRange(side_topleft);
                    attackPoints.AddRange(side_topright);
                    attackPoints.AddRange(side_bottomleft);
                    attackPoints.AddRange(side_bottomright);
                    break;
                default:
                    break;
            }

            switch(attackMode)
            {
                case 0:
                    attackTroops.Add(0);
                    attackTroops.Add(1);
                    break;
                case 1:
                    attackTroops.Add(0);
                    attackTroops.Add(1);
                    attackTroops.Add(2);
                    break;
                case 2:
                    attackTroops.Add(0);
                    break;
                case 3:
                    attackTroops.Add(1);
                    break;
                default:
                    attackTroops.Add(0);
                    attackTroops.Add(1);
                    break;
            }

            for(int i = 0; i < 4; i++)
            {
                if (getTroopId(i) != -1 && attackTroops.Contains(getTroopId(i)))
                {
                    Home.bsProcess.mouse.SendClick(WButton.Left, getPos(i), false);
                    Thread.Sleep(100);
                    while (!SameRGB(Home.bsProcess.image.GetPixelColor(getPos(i))))
                    {
                        foreach (Point point in attackPoints)
                        {
                            Random randX = new Random();
                            Random randY = new Random();
                            Point randPoint = new Point(point.X + randX.Next(-2, 3) + Settings.xDif, point.Y + randY.Next(-2, 3) + Settings.yDif);
                            Home.bsProcess.mouse.SendClick(WButton.Left, randPoint, false);
                            Thread.Sleep(50);
                        }
                    }
                }
            }

            while(!ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(new Point(440 + Settings.xDif, 560 + Settings.yDif)), Color.FromArgb(200,230,104)))
            {
                Thread.Sleep(5000);
            }
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(440 + Settings.xDif, 560 + Settings.yDif), false);
        }

        private static int getTroopId(int troopcase)
        {
            Color color = Color.FromArgb(0, 0, 0);
            color = Home.bsProcess.image.GetPixelColor(getPos(troopcase));

            if (ColorDif.isCorrect(color, Color.FromArgb(248, 237, 63))) //BARBARIAN
                return 0;
            if (ColorDif.isCorrect(color, Color.FromArgb(233, 67, 118))) //ARCHER
                return 1;
            if (ColorDif.isCorrect(color, Color.FromArgb(82, 165, 60))) //GOBLIN
                return 2;
            if (ColorDif.isCorrect(color, Color.FromArgb(244, 122, 8))) //GIANT
                return 3;
            return -1;
        }

        private static Point getPos(int troopCase)
        {
            switch (troopCase)
            {
                case 0:
                    return new Point(77 + Settings.xDif, 624 + Settings.yDif);
                case 1:
                    return new Point(150 + Settings.xDif, 624 + Settings.yDif);
                case 2:
                    return new Point(222 + Settings.xDif, 624 + Settings.yDif);
                case 3:
                    return new Point(294 + Settings.xDif, 624 + Settings.yDif);
                default:
                    return new Point(-1, -1);
            }
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
