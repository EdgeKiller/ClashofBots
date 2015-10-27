using IniParser;
using IniParser.Model;
using Clash_of_Bots.Statics;

namespace Clash_of_Bots.Utils
{
    public static class ConfigFileHelper
    {
        public static bool CheckIfComplete()
        {
            //Read ini file
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(AppSettings.Cfg.FilePath);

            //Check collectors section
            if (!data.Sections.ContainsSection("collectors"))
                return false;

            //Check collectors locations
            for (int i = 1; i <= 17; i++)
            {
                if (!data["collectors"].ContainsKey("collector" + i))
                    return false;
            }

            //Check troops section
            if (!data.Sections.ContainsSection("troops"))
                return false;

            //Add troops settings
            for (int i = 1; i <= 4; i++)
            {
                if (!data["troops"].ContainsKey("barrack" + i))
                    return false;
            }

            //Check search section
            if (!data.Sections.ContainsSection("search"))
                return false;

            //Check search settings
            if (!data["search"].ContainsKey("gold") ||
               !data["search"].ContainsKey("elixir") ||
               !data["search"].ContainsKey("dark") ||
               !data["search"].ContainsKey("trophy") ||
               !data["search"].ContainsKey("bgold") ||
               !data["search"].ContainsKey("belixir") ||
               !data["search"].ContainsKey("bdark") ||
               !data["search"].ContainsKey("btrophy") ||
               !data["search"].ContainsKey("alert"))
                return false;

            //Check global section
            if (!data.Sections.ContainsSection("game"))
                return false;

            //Check global settings
            if (!data["game"].ContainsKey("hidemode"))
                return false;

            //Check attack section
            if (!data.Sections.ContainsSection("attack"))
                return false;

            //Check attack settings
            if (!data["attack"].ContainsKey("topleft") ||
                !data["attack"].ContainsKey("topright") ||
                !data["attack"].ContainsKey("bottomleft") ||
                !data["attack"].ContainsKey("bottomright") ||
               !data["attack"].ContainsKey("mode") ||
               !data["attack"].ContainsKey("maxtrophy") ||
               !data["attack"].ContainsKey("bmaxtrophy") ||
               !data["attack"].ContainsKey("deploytime"))
                return false;

            return true;
        }

        public static void CreateDefault()
        {
            //Create ini file
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = new IniData();

            //Add global section
            data.Sections.AddSection("game");

            //Add global settings
            data["game"]["hidemode"] = "1";

            //Add collectors section
            data.Sections.AddSection("collectors");

            //Add collectors locations
            for (int i = 1; i <= 17; i++)
            {
                data["collectors"]["collector" + i] = "-1:-1";
            }

            //Add troops section
            data.Sections.AddSection("troops");

            //Add troops settings
            for (int i = 1; i <= 4; i++)
            {
                data["troops"]["barrack" + i] = "0";
            }

            //Add search section
            data.Sections.AddSection("search");

            //Add search settings
            data["search"]["gold"] = "50000";
            data["search"]["elixir"] = "50000";
            data["search"]["dark"] = "0";
            data["search"]["trophy"] = "0";
            data["search"]["bgold"] = "1";
            data["search"]["belixir"] = "1";
            data["search"]["bdark"] = "0";
            data["search"]["btrophy"] = "0";
            data["search"]["alert"] = "0";

            //Add attack section
            data.Sections.AddSection("attack");

            //Add attack settings
            data["attack"]["topleft"] = "True";
            data["attack"]["topright"] = "True";
            data["attack"]["bottomleft"] = "True";
            data["attack"]["bottomright"] = "True";
            data["attack"]["mode"] = "0";
            data["attack"]["maxtrophy"] = "1000";
            data["attack"]["bmaxtrophy"] = "False";
            data["attack"]["deploytime"] = "75";

            //Save the file
            parser.WriteFile(AppSettings.Cfg.FilePath, data);
        }

    }
}