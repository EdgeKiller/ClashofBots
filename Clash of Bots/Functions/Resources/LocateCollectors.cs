using BotLibNet;
using IniParser;
using IniParser.Model;
using System.Drawing;
using System.Windows.Forms;

namespace Clash_of_Bots
{
    public static class LocateCollectors
    {
        public static void Locate()
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            for (int i = 1; i <= 17; i++)
            {
                if (MessageBox.Show("Please set the cursor on your collector number " + i + " and press enter. Click on cancel if not available", "Collectors localization", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Point collectorPos = FindCursorPos();
                    data["collectors"]["collector" + i] = collectorPos.X + ";" + collectorPos.Y;
                    Home.bsProcess.mouse.SendClick(WButton.Left, collectorPos, false);
                }
                else
                    data["collectors"]["collector" + i] = "-1;-1";
            }
            parser.WriteFile("config.ini", data);
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0,0), false);
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
