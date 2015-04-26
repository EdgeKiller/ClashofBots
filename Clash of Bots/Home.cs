using BotLibNet;
using CocFunctions;
using IniParser;
using IniParser.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Clash_of_Bots
{
    public enum Status
    {
        Idle,
        Train,
        Search,
        Attack,
        Free,
        Collect,
        Locating
    }

    public partial class Home : Form
    {
        private long runningTimeSecond = 0;
		public static int goldGain = 0, elixirGain = 0, darkGain = 0;

        public static BotProcess bsProcess;
        static bool isHide;
        public Status botStatusVar = new Status();
        private Status botStatus
        {
            get { return botStatusVar; }
            set
            {
                botStatusVar = value;
                Log.SetStatus(botStatusVar.ToString());
            }
        }

        #region BGWORKER_REM
        //BGWORKER_UNZOOM
        BackgroundWorker bg_unzoom = new BackgroundWorker();
        //BGWORKER_COLLECTRESOURCES
        BackgroundWorker bg_collectResources = new BackgroundWorker();
        //BGWORKER_TRAINTROOPS
        BackgroundWorker bg_trainTroops = new BackgroundWorker();
        //BGWORKER_SEARCHVILLAGE
        BackgroundWorker bg_searchVillage = new BackgroundWorker();
        //BGWORKER_ATTACK
        BackgroundWorker bg_attack = new BackgroundWorker();
        //BGWORKER_PLAY
        AbortableBackgroundWorker bg_play = new AbortableBackgroundWorker();
        #endregion

        public Home()
        {
            InitializeComponent();
            formSkin1.Text = Settings.name + " • " + Settings.version + " • Home";
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            if (!BotProcess.processExist("HD-Frontend"))
            {
                MessageBox.Show("Bluestacks is not open !");
                Application.Exit();
                this.Close();
                Environment.Exit(0);
            }
            bsProcess = new BotProcess("HD-Frontend");
            #region VerifyWindowBorderSize
            Size dif = Window.GetBorderSize(bsProcess.image.GetWindowImage());
            Settings.xDif = dif.Width;
            Settings.yDif = dif.Height;
            #endregion
            Log.Init(flatListBox_log, flatStatusBar_status);

            #region VerifyConfigFile
            // Check to make sure config.ini exists.
            if (!System.IO.File.Exists(Application.StartupPath + "\\config.ini"))
            {
                CreateFile.Create("config.ini");
            }

            // Check to make sure config.ini exists.
            if (!System.IO.File.Exists(Application.StartupPath + "\\860x720.reg"))
            {
                CreateFile.Create("860x720.reg");
            }
            #endregion

            #region BGWORKERS_INIT
            //BGWORKER_UNZOOM
            bg_unzoom.DoWork += new DoWorkEventHandler(bgWorker_unzoom_DoWork);
            bg_unzoom.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_unzoom_Finish);
            //BGWORKER_COLLECTRESOURCES
            bg_collectResources.DoWork += new DoWorkEventHandler(bgWorker_collectresources_DoWork);
            bg_collectResources.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_collectresources_Finish);
            //BGWORKER_TRAINTROOPS
            bg_trainTroops.DoWork += new DoWorkEventHandler(bgWorker_traintroops_DoWork);
            bg_trainTroops.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_traintroops_Finish);
            //BGWORKER_SEARCHVILLAGE
            bg_searchVillage.DoWork += new DoWorkEventHandler(bgWorker_searchvillage_DoWork);
            bg_searchVillage.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_searchvillage_Finish);
            //BGWORKER_ATTACK
            bg_attack.DoWork += new DoWorkEventHandler(bgWorker_attack_DoWork);
            bg_attack.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_attack_Finish);
            //BGWORKER_PLAY
            bg_play.DoWork += new DoWorkEventHandler(bgWorker_play_DoWork);
            bg_play.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_play_Finish);
            #endregion
            botStatus = Status.Free;

            #region LoadConfig
            Log.SetLog("Loading config file...");
            LoadConfig.Load(flatComboBox_barrack1, flatComboBox_barrack2, flatComboBox_barrack3, flatComboBox_barrack4,
                flatTextBox_gold, flatTextBox_elixir, flatTextBox_dark, flatTextBox_trophy,
                flatCheckBox_gold, flatCheckBox_elixir, flatCheckBox_dark, flatCheckBox_trophy,
                flatCheckBox_alert, flatComboBox_sidesToAttack, flatCheckBox_maxTrophy, flatTextBox_maxTrophy, flatComboBox_attackMode,
                flatNumeric_deployTime);
            Log.SetLog("Load complete.");
            #endregion
            bg_unzoom.RunWorkerAsync();

            flatNumeric_deployTime.ValueChange += new EventHandler(flatNumeric_deployTime_ValueChange);
        }

        #region BUTTONS
        //HIDE BUTTON
        private void flatButton_hide_Click(object sender, EventArgs e)
        {
            if (isHide)
            {
                bsProcess.window.SetPosition(0, 0);
                flatButton_hide.BaseColor = Color.FromArgb(35, 168, 109);
                flatButton_hide.Text = "Hide";
                isHide = false;
            }
            else
            {
                bsProcess.window.SetPosition(-10000, -10000);
                flatButton_hide.BaseColor = Color.FromArgb(168, 35, 35);
                flatButton_hide.Text = "Show";
                isHide = true;
            }
        }

        //CLEAR LOG BUTTON
        private void flatButton_clearLog_Click(object sender, EventArgs e)
        {
            Log.Clear();
        }

        //LOCATE COLLECTORS BUTTON
        private void flatButton_locateCollectors_Click(object sender, EventArgs e)
        {
            if (botStatus == Status.Free)
            {
                botStatus = Status.Locating;
                Log.SetLog("Locating collectors...");

                LocateCollectors.Locate();
                Log.SetLog("Localization complete.");
                botStatus = Status.Free;
            }
        }

        //LOCATE BARRACKS BUTTON
        private void flatButton_locateBarracks_Click(object sender, EventArgs e)
        {
            if (botStatus == Status.Free)
            {
                botStatus = Status.Locating;
                Log.SetLog("Locating barracks...");

                LocateBarracks.Locate();
                Log.SetLog("Localization complete.");
                botStatus = Status.Free;
            }
        }

        //COLLECT RESOURCES BUTTON
        private void flatButton_collectResources_Click(object sender, EventArgs e)
        {
            if(!bg_collectResources.IsBusy && botStatus == Status.Free)
                bg_collectResources.RunWorkerAsync();
        }

        //TRAIN TROOPS BUTTON
        private void flatButton_trainTroops_Click(object sender, EventArgs e)
        {
            if (!bg_trainTroops.IsBusy && botStatus == Status.Free)
                bg_trainTroops.RunWorkerAsync();
        }

        //SEARCH BASE BUTTON
        private void flatButton_searchMode_Click(object sender, EventArgs e)
        {
            if (!bg_searchVillage.IsBusy && botStatus == Status.Free)
                bg_searchVillage.RunWorkerAsync();
        }

        //ATTACK BASE BUTTON
        private void flatButton_attack_Click(object sender, EventArgs e)
        {
            if (!bg_attack.IsBusy && botStatus == Status.Free)
                bg_attack.RunWorkerAsync();
        }

        //PLAY BASE BUTTON
        private void flatButton_start_Click(object sender, EventArgs e)
        {
            if (!bg_play.IsBusy && botStatus == Status.Free)
            {
                bg_play.RunWorkerAsync();
                flatButton_start.BaseColor = Color.FromArgb(168, 35, 35);
                flatButton_start.Text = "Stop";
                flatButton_start.Refresh();
                runningTimeSecond = 0;
                timer_runningTime.Start();
            }
            else
            {
                timer_runningTime.Stop();
                bg_play.Abort();
                bg_play.Dispose();
                bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
                Thread.Sleep(100);
                bsProcess.mouse.SendClick(WButton.Left, new Point(0, 0), false);
            }
        }

        //CHECK VERSION BUTTON
        private void flatButton_checkVersion_Click(object sender, EventArgs e)
        {
            string lastVersion = new System.Net.WebClient().DownloadString("http://clashofbots.edgekiller.fr/lastversion");
            if(lastVersion == Settings.version)
                MessageBox.Show("Your bot is up to date", "Up to date");
            else
                if(MessageBox.Show("Your bot is outdated, do you want to download the last version ?", "Outdated", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    Process.Start("http://clashofbots.edgekiller.fr/forum/index.php?/forum/5-releases/");
        }

        //NEED HELP BUTTON
        private void flatButton_needHelp_Click(object sender, EventArgs e)
        {
            Process.Start("http://clashofbots.edgekiller.fr/forum/");
        }

        //RESET STATS BUTTON
        private void flatButton_resetStats_Click(object sender, EventArgs e)
        {
            goldGain = 0;
            elixirGain = 0;
            darkGain = 0;
            flatLabel_statsGold.Text = goldGain.ToString();
            flatLabel_statsElixir.Text = elixirGain.ToString();
            flatLabel_statsDark.Text = darkGain.ToString();
            flatLabel_statsDark.Refresh();
            flatLabel_statsElixir.Refresh();
            flatLabel_statsGold.Refresh();
        }

        #endregion

        #region BGWORKER_UNZOOM
        private void bgWorker_unzoom_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.SetLog("Unzooming...");
            Zoom.UnZoom();
        }
        private void bgWorker_unzoom_Finish(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.SetLog("Unzoom complete.");
            bg_unzoom.Dispose();
        }
        #endregion

        #region BGWORKER_COLLECTRESOURCES
        private void bgWorker_collectresources_DoWork(object sender, DoWorkEventArgs e)
        {
            Zoom.UnZoom();
            Log.SetLog("Collecting resources...");
            botStatus = Status.Collect;
            CollectResources.Collect();
        }
        private void bgWorker_collectresources_Finish(object sender, RunWorkerCompletedEventArgs e)
        {
            botStatus = Status.Free;
            Log.SetLog("Collect complete.");
            bg_collectResources.Dispose();
        }
        #endregion

        #region BGWORKER_TRAINTROOPS
        private void bgWorker_traintroops_DoWork(object sender, DoWorkEventArgs e)
        {
            Zoom.UnZoom();
            Log.SetLog("Training troops...");
            botStatus = Status.Train;
            TrainTroops.Train();
        }
        private void bgWorker_traintroops_Finish(object sender, RunWorkerCompletedEventArgs e)
        {
            botStatus = Status.Free;
            Log.SetLog("Train complete.");
            bg_trainTroops.Dispose();
        }
        #endregion

        #region BGWORKER_SEARCHVILLAGE
        private void bgWorker_searchvillage_DoWork(object sender, DoWorkEventArgs e)
        {
            Zoom.UnZoom();
            Log.SetLog("Searching village...");
            botStatus = Status.Search;
            SearchVillage.Search();
        }
        private void bgWorker_searchvillage_Finish(object sender, RunWorkerCompletedEventArgs e)
        {
            botStatus = Status.Free;
            Log.SetLog("Search complete.");
            bg_searchVillage.Dispose();
        }
        #endregion

        #region BGWORKER_ATTACK
        private void bgWorker_attack_DoWork(object sender, DoWorkEventArgs e)
        {
            Zoom.UnZoom();
            Log.SetLog("Attacking village...");
            botStatus = Status.Attack;
            AttackVillage.Attack();
        }
        private void bgWorker_attack_Finish(object sender, RunWorkerCompletedEventArgs e)
        {
            botStatus = Status.Free;
            Log.SetLog("Attack complete.");
            bg_attack.Dispose();
        }
        #endregion

        #region BGWORKER_PLAY
        private void bgWorker_play_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isReadyToAttack = false;
            Zoom.UnZoom();
            while (bg_play.IsBusy)
            {
                isReadyToAttack = false;
                Zoom.UnZoom();
                botStatus = Status.Collect;
                Log.SetLog("Collecting resources...");
                CollectResources.Collect();
                Log.SetLog("Collect complete.");
                while (!isReadyToAttack)
                {
                    Zoom.UnZoom();
                    botStatus = Status.Train;
                    Log.SetLog("Training troops...");
                    TrainTroops.Train();
                    Log.SetLog("Train complete.");
                    Zoom.UnZoom();
                    botStatus = Status.Idle;
                    Log.SetLog("Waiting for full army...");
                    isReadyToAttack = TrainTroops.IsTroopsReady();
                    if(!isReadyToAttack)
                        Thread.Sleep(60000);
                }
                Thread.Sleep(1000);
                Zoom.UnZoom();
                botStatus = Status.Search;
                Log.SetLog("Searching base...");
                SearchVillage.Search();
                Log.SetLog("Search complete.");
                Thread.Sleep(5000);
                Zoom.UnZoom();
                botStatus = Status.Attack;
                Log.SetLog("Attacking base...");
                AttackVillage.Attack();
                Log.SetLog("Attack complete.");
                Thread.Sleep(1000);
                flatLabel_statsGold.Text = goldGain.ToString();
                flatLabel_statsElixir.Text = elixirGain.ToString();
                flatLabel_statsDark.Text = darkGain.ToString();
                Thread.Sleep(500);
                Zoom.UnZoom();
                while(flatCheckBox_maxTrophy.Checked && Trophy.Get() > Convert.ToInt32(flatTextBox_maxTrophy.Text))
                {
                    Log.SetLog("Losing trophy...");
                    AttackLoseTrophy.Attack();
                    Log.SetLog("Lose complete.");
                }
                Zoom.UnZoom();
                botStatus = Status.Idle;
                Thread.Sleep(5000);
            }          
        }

        private void bgWorker_play_Finish(object sender, RunWorkerCompletedEventArgs e)
        {
            botStatus = Status.Free;
            bg_play.Dispose();
            flatButton_start.BaseColor = Color.FromArgb(35, 168, 109);
            flatButton_start.Text = "Start";
            flatButton_start.Refresh();
            
        }
        #endregion

        #region SETTINGS_CHANGED
        private void flatComboBox_barrack1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["troops"]["barrack1"] = flatComboBox_barrack1.SelectedIndex.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatComboBox_barrack2_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["troops"]["barrack2"] = flatComboBox_barrack2.SelectedIndex.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatComboBox_barrack3_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["troops"]["barrack3"] = flatComboBox_barrack3.SelectedIndex.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatComboBox_barrack4_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["troops"]["barrack4"] = flatComboBox_barrack4.SelectedIndex.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatTextBox_gold_TextChanged(object sender, EventArgs e)
        {
                FileIniDataParser parser = new FileIniDataParser();
                IniData data = parser.ReadFile("config.ini");
                data["search"]["gold"] = flatTextBox_gold.Text;
                parser.WriteFile("config.ini", data);
        }

        private void flatTextBox_elixir_TextChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["elixir"] = flatTextBox_elixir.Text;
            parser.WriteFile("config.ini", data);
        }

        private void flatTextBox_dark_TextChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["dark"] = flatTextBox_dark.Text;
            parser.WriteFile("config.ini", data);
        }

        private void flatTextBox_trophy_TextChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["trophy"] = flatTextBox_trophy.Text;
            parser.WriteFile("config.ini", data);
        }

        private void flatCheckBox_gold_CheckedChanged(object sender)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["bgold"] = flatCheckBox_gold.Checked.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatCheckBox_elixir_CheckedChanged(object sender)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["belixir"] = flatCheckBox_elixir.Checked.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatCheckBox_dark_CheckedChanged(object sender)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["bdark"] = flatCheckBox_dark.Checked.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatCheckBox_trophy_CheckedChanged(object sender)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["btrophy"] = flatCheckBox_trophy.Checked.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatCheckBox_alert_CheckedChanged(object sender)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["search"]["alert"] = flatCheckBox_alert.Checked.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatComboBox_attackMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["attack"]["sides"] = flatComboBox_sidesToAttack.SelectedIndex.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatCheckBox_maxTrophy_CheckedChanged(object sender)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["attack"]["bmaxtrophy"] = flatCheckBox_maxTrophy.Checked.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatTextBox_maxTrophy_TextChanged(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["attack"]["maxtrophy"] = flatTextBox_maxTrophy.Text;
            parser.WriteFile("config.ini", data);
        }

        private void flatComboBox_attackMode_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["attack"]["mode"] = flatComboBox_attackMode.SelectedIndex.ToString();
            parser.WriteFile("config.ini", data);
        }

        private void flatNumeric_deployTime_Click(object sender, EventArgs e)
        {
            
        }

        private void flatNumeric_deployTime_ValueChange(object sender, EventArgs e)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            data["attack"]["deploytime"] = flatNumeric_deployTime.Value.ToString();
            parser.WriteFile("config.ini", data);
        }

        #endregion

        private void timer_runningTime_Tick(object sender, EventArgs e)
        {
            runningTimeSecond++;
            TimeSpan t = TimeSpan.FromSeconds(runningTimeSecond);
            string answer = string.Format("{3:D2}d:{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds,
                            t.Days);
            flatLabel_runningTime.Text = "Running time : " + answer;
        }

        

       

        

        

       
        

        
   
    }
}
