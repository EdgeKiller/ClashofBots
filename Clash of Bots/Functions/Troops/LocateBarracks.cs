using BotLibNet;
using IniParser;
using IniParser.Model;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Clash_of_Bots
{
    class LocateBarracks
    {
        public static void Locate()
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            for (int i = 1; i <= 4; i++)
            {
                if (MessageBox.Show("Please set the cursor on your barrack number " + i + " and press enter. Click on cancel if not available", "Barracks localization", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Point barrackPos = FindCursorPos();
                    data["barracks"]["barrack" + i] = barrackPos.X + ";" + barrackPos.Y;
                    Home.bsProcess.mouse.SendClick(WButton.Left, barrackPos, false);
                }
                else
                    data["barracks"]["barrack" + i] = "-1;-1";
            }
            parser.WriteFile("config.ini", data);
            Thread.Sleep(500);
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
        }

        private static Point FindCursorPos()
        {
            Point bluestacksPos = Home.bsProcess.window.GetPosition();
            Point pos = Home.bsProcess.mouse.GetPosition();
            pos.X = pos.X - bluestacksPos.X;
            pos.Y = pos.Y - bluestacksPos.Y;
            return pos;
        }
    }
}
