using CocFunctions;

namespace Clash_of_Bots
{
    class Trophy
    {

        public static int Get()
        {
            int trophy;
            trophy = Read.GetNumberResources(Home.bsProcess.image.CaptureRegion(Buttons.GetResourcesRec("player_trophy", Settings.xDif, Settings.yDif)));
            return trophy;
        }

    }
}
