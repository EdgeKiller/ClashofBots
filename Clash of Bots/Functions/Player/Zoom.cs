using BotLibNet;
using CocFunctions;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Clash_of_Bots
{
    class Zoom
    {
        public static void UnZoom()
        {
            while(!ColorDif.isCorrect(Home.bsProcess.image.GetPixelColor(new Point(3 + Settings.xDif,25 + Settings.yDif)), Color.FromArgb(0, 0, 0)))
            {
                Home.bsProcess.keyboard.SendKeyStroke(Keys.Down);
                Thread.Sleep(250);
            }
            Home.bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
        }

    }
}
