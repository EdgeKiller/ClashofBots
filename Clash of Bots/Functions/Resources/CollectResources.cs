using BotLibNet;
using IniParser;
using IniParser.Model;
using System;
using System.Drawing;
using System.Threading;

namespace Clash_of_Bots
{
    class CollectResources
    {
        public static void Collect()
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            for (int i = 1; i <= 17; i++)
            {
                string stringPosition = data["collectors"]["collector" + i];
                Point pointPosition = new Point(Convert.ToInt32(stringPosition.Split(';')[0]), Convert.ToInt32(stringPosition.Split(';')[1]));
                if (pointPosition.X != -1 && pointPosition.Y != -1)
                    Home.bsProcess.mouse.SendClick(WButton.Left, pointPosition, false);
                Thread.Sleep(250);
            }
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
        }

    }
}
