namespace Clash_of_Bots.Statics
{
    public static class AppSettings
    {
        public static class App
        {
            public static string Name = "Clash of Bots",
                Dev = "EdgeKiller",
                Ver = "Alpha 0.5.0",
                Sep = " • ",
                LastVersionSite = "http://clashofbots.edgekiller.fr/lastversion",
                LastVersionDl = "http://clashofbots.edgekiller.fr/forum/index.php?/forum/5-releases/";
        }

        public static class Cfg
        {
            public static string FilePath = "cobconfig.ini";
        }

        public static class Images
        {
            public static string goldMine = "images/collectors/gold.png",
                elixirExtractor = "images/collectors/elixir.png",
                darkExtractor = "images/collectors/dark.png",
                resourcesPath = "images/resources/";
        }

        public static class Script
        {
            public static string ScriptFolder = "scripts/";

            public static string Train = ScriptFolder + "train.js",
                Search = ScriptFolder + "search.js",
                Attack = ScriptFolder + "attack.js",
                Collect = ScriptFolder + "collect.js";
        }
    }
}