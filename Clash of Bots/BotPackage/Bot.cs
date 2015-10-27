using BotLibNet2;
using Clash_of_Bots.Functions;
using Clash_of_Bots.Statics;
using Clash_of_Bots.Utils;
using FlatUITheme;
using IniParser;
using IniParser.Model;
using Jint;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Clash_of_Bots.BotPackage
{
    public class Bot
    {
        public BotProcess bs;

        FileIniDataParser cfgParser;
        public IniData cfgData;

        FlatListBox logListBox;

        Engine js;
        AbortableBackgroundWorker botWorker;

        BotState bstate;
        public BotState botState
        {
            get
            {
                return bstate;
            }
            set
            {
                bstate = value;
                StateChanged(value);
            }
        }
        public delegate void BotStateChange(BotState state);
        public event BotStateChange OnStateChanged;

        public Bot(FlatListBox logLB)
        {
            //Attach Bluestacks process
            if (BotProcess.ProcessExist("HD-Frontend"))
            {
                bs = new BotProcess("HD-Frontend");
            }
            else
            {
                MessageBox.Show("Bluestacks is not open, please start it and restart the bot.");
                Environment.Exit(0);
            }

            //Set the default state
            botState = BotState.Free;

            //Set the listbox log
            logListBox = logLB;

            //Create the config
            cfgParser = new FileIniDataParser();

            //Worker configuration
            botWorker = new AbortableBackgroundWorker();
            botWorker.DoWork += botWorker_DoWork;
            botWorker.WorkerSupportsCancellation = true;

            //JS engine configuration
            js = new Engine(cfg => cfg.AllowClr()).SetValue("BotMouse", bs.mouse)
                .SetValue("BotKeyboard", bs.keyboard)
                .SetValue("BotImage", bs.image)
                .SetValue("BotWindow", bs.window)
                .SetValue("Config", cfgData)
                .SetValue("Bot", this);

            //Load the config
            ReloadConfig();
        }

        void StateChanged(BotState state)
        {
            //Invoke status changed if not null
            if (OnStateChanged != null)
                OnStateChanged.Invoke(state);
        }

        void botWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Unzoom
            Unzoom();

            //Always repeat
            while (true)
            {

                //Run collect script
                js.Execute(File.ReadAllText(AppSettings.Script.Collect));

                //Run train script
                js.Execute(File.ReadAllText(AppSettings.Script.Train));

                //Run search script
                js.Execute(File.ReadAllText(AppSettings.Script.Search));

                //Run attack script
                js.Execute(File.ReadAllText(AppSettings.Script.Attack));

            }

            Stop();

            botState = BotState.Free;
        }

        public void StartDebug()
        {
            // Run debug script
            js.Execute(File.ReadAllText(AppSettings.Script.Debug));
        }

        public void Unzoom()
        {
            while (bs.image.GetWindowImage(!Convert.ToBoolean(cfgData["game"]["hidemode"])).GetPixel(3, 25).R != 0)
            {
                bs.keyboard.SendKey(Keys.Down);
                Wait(50);
            }
        }

        public void SaveConfig()
        {
            cfgParser.WriteFile(AppSettings.Cfg.FilePath, cfgData);
        }

        public void ReloadConfig()
        {
            //Check config file
            if (!File.Exists(AppSettings.Cfg.FilePath) || !ConfigFileHelper.CheckIfComplete())
            {
                ConfigFileHelper.CreateDefault();
                Log("Config file not found or invalid, a default config file was created.");
            }

            //Load the config file
            cfgData = cfgParser.ReadFile(AppSettings.Cfg.FilePath);
            js.SetValue("Config", cfgData);
        }

        public void Start()
        {
            //Start the bot
            botWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            //Stop the bot
            botWorker.Abort();
            botWorker.Dispose();
            botState = BotState.Free;
        }

        public void Log(object message)
        {
            //Log message on the console
            if (logListBox.InvokeRequired)
            {
                Action action = () => logListBox.AddItem("[" + DateTime.Now + "] " + message);
                logListBox.Invoke(action);

                if (logListBox.CountItem() > 17)
                {
                    Action action1 = () => logListBox.SelectIndex(logListBox.CountItem() - 1);
                    logListBox.Invoke(action1);
                }
            }
            else
            {
                logListBox.AddItem("[" + DateTime.Now + "] " + message);
            }
        }

        public int Read(string type, Bitmap image)
        {
            return ResourcesReader.Read(type, image);
        }

        public int ReadTrophy(Bitmap image)
        {
            return ResourcesReader.ReadTrophy(image);
        }

        public Point DetectTroop(int id, Bitmap image)
        {
            return TroopsDetector.Detect(id, image);
        }

        public void Wait(int time)
        {
            //Wait
            Thread.Sleep(time);
        }

        public Bitmap BmpFromFile(string path)
        {
            return Image.FromFile(path) as Bitmap;
        }

        public bool IsWorking()
        {
            return botWorker.IsBusy;
        }


    }
}