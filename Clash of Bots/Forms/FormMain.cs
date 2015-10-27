using Clash_of_Bots.BotPackage;
using Clash_of_Bots.Functions;
using Clash_of_Bots.Statics;
using FlatUITheme;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Clash_of_Bots.Forms
{
    public partial class FormMain : Form
    {
        Bot bot;

        public FormMain()
        {
            InitializeComponent();
            formSkin_main.Text = AppSettings.App.Name + AppSettings.App.Sep + AppSettings.App.Ver + AppSettings.App.Sep + AppSettings.App.Dev;
            formSkin_main.Refresh();
            bot = new Bot(flatListBox_log);
            bot.OnStateChanged += bot_OnStateChanged;
            bot.Log("Welcome to " + AppSettings.App.Name + " " + AppSettings.App.Ver + ", " + Environment.UserName + " !");

            //Hide mode
            flatCheckBox_hideMode.Checked = Convert.ToBoolean(bot.cfgData["game"]["hidemode"]);

            //Sound alert
            flatCheckBox_soundAlert.Checked = Convert.ToBoolean(bot.cfgData["search"]["alert"]);

            //Resources
            flatNumeric_minGold.Value = Convert.ToInt64(bot.cfgData["search"]["gold"]);
            flatNumeric_minElixir.Value = Convert.ToInt64(bot.cfgData["search"]["elixir"]);
            flatNumeric_minDark.Value = Convert.ToInt64(bot.cfgData["search"]["dark"]);
            flatNumeric_minTrophy.Value = Convert.ToInt64(bot.cfgData["search"]["trophy"]);
            flatCheckBox_minGold.Checked = Convert.ToBoolean(bot.cfgData["search"]["bgold"]);
            flatCheckBox_minElixir.Checked = Convert.ToBoolean(bot.cfgData["search"]["belixir"]);
            flatCheckBox_minDark.Checked = Convert.ToBoolean(bot.cfgData["search"]["bdark"]);
            flatCheckBox_minTrophy.Checked = Convert.ToBoolean(bot.cfgData["search"]["btrophy"]);

            //Troops
            flatComboBox_barrack1.SelectedIndex = Convert.ToInt32(bot.cfgData["troops"]["barrack1"]);
            flatComboBox_barrack2.SelectedIndex = Convert.ToInt32(bot.cfgData["troops"]["barrack2"]);
            flatComboBox_barrack3.SelectedIndex = Convert.ToInt32(bot.cfgData["troops"]["barrack3"]);
            flatComboBox_barrack4.SelectedIndex = Convert.ToInt32(bot.cfgData["troops"]["barrack4"]);

            //Attack sides
            flatCheckBox_attackTopLeft.Checked = Convert.ToBoolean(bot.cfgData["attack"]["topleft"]);
            flatCheckBox_attackTopRight.Checked = Convert.ToBoolean(bot.cfgData["attack"]["topright"]);
            flatCheckBox_attackBottomLeft.Checked = Convert.ToBoolean(bot.cfgData["attack"]["bottomleft"]);
            flatCheckBox_attackBottomRight.Checked = Convert.ToBoolean(bot.cfgData["attack"]["bottomright"]);
        }

        private void bot_OnStateChanged(BotState state)
        {
            Action refreshTextAction = () => flatStatusBar.Text = "Status : " + state;
            flatStatusBar.Invoke(refreshTextAction);
        }

        private void FlatButton_startStopClick(object sender, EventArgs e)
        {
            if (bot.IsWorking())
            {
                bot.Stop();
                flatButton_startStop.Text = "Start";
                flatButton_startStop.BaseColor = Color.FromArgb(255, 35, 168, 109);
            }
            else
            {
                bot.Start();
                flatButton_startStop.Text = "Stop";
                flatButton_startStop.BaseColor = Color.FromArgb(255, 168, 35, 35);
            }
        }

        private void FlatButton_locateCollectorsClick(object sender, EventArgs e)
        {
            if (bot.botState == BotState.Free)
            {
                bot.botState = BotState.Locating;
                bot.Log("Locating collectors...");
                LocateCollectors.Locate(bot);
                bot.Log("Localization complete.");
                bot.botState = BotState.Free;
            }
        }

        private void FlatButton_autoLocateCollectorsClick(object sender, EventArgs e)
        {
            if (bot.botState == BotState.Free)
            {
                bot.botState = BotState.Locating;
                bot.Log("Auto-Locating collectors...");
                LocateCollectors.AutoLocate(bot);
                bot.Log("Auto-Localization complete.");
                bot.botState = BotState.Free;
            }
        }

        private void FlatButton_takeScreenshotClick(object sender, EventArgs e)
        {
            if (!Directory.Exists("debug"))
                Directory.CreateDirectory("debug");
            bot.bs.image.GetWindowImage().Save("debug/" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + "HMON.png");
        }

        private void flatButton_takeScreenshot2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("debug"))
                Directory.CreateDirectory("debug");
            bot.bs.image.GetWindowImage(true).Save("debug/" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + "HMOFF.png");
        }

        private void numeric_ValueChange(object sender, EventArgs e)
        {
            FlatNumeric fn = sender as FlatNumeric;
            bot.cfgData[fn.Tag.ToString().Split(':')[0]][fn.Tag.ToString().Split(':')[1]] = fn.Value.ToString();
            bot.SaveConfig();
        }

        private void checkBox_CheckedChange(object sender)
        {
            FlatCheckBox fcb = sender as FlatCheckBox;
            bot.cfgData[fcb.Tag.ToString().Split(':')[0]][fcb.Tag.ToString().Split(':')[1]] = fcb.Checked.ToString();
            bot.SaveConfig();
        }

        private void barrackComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FlatComboBox fcb = sender as FlatComboBox;
            bot.cfgData["troops"]["barrack" + fcb.Tag.ToString()] = fcb.SelectedIndex.ToString();
            bot.SaveConfig();
        }

        private void flatButton1_Click(object sender, EventArgs e)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = null;
                string lastVer = wc.DownloadString(AppSettings.App.LastVersionSite);
                if (lastVer.Equals(AppSettings.App.Ver))
                    MessageBox.Show("Your bot is up to date !", "No update available", MessageBoxButtons.OK);
                else
                    if(MessageBox.Show("A new version is available : " + lastVer +
                        ". Do you want to download it ?", "New update available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        Process.Start(AppSettings.App.LastVersionDl);
            }
        }

        private void flatCheckBoxAttackSide_CheckedChanged(object sender)
        {
            FlatCheckBox fcb = sender as FlatCheckBox;

            if (!flatCheckBox_attackTopLeft.Checked && !flatCheckBox_attackTopRight.Checked &&
               !flatCheckBox_attackBottomLeft.Checked && !flatCheckBox_attackBottomRight.Checked)
            {
                fcb.Checked = true;
                MessageBox.Show("You need to select at least one side to attack !");
            }

            bot.cfgData["attack"][fcb.Tag.ToString()] = fcb.Checked.ToString();
            bot.SaveConfig();
        }
    }
}
