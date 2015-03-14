using FlatUITheme;
using IniParser;
using IniParser.Model;
using System;

namespace Clash_of_Bots
{
    class LoadConfig
    {
        public static void Load(FlatComboBox comboBoxBarrack1, FlatComboBox comboBoxBarrack2, 
            FlatComboBox comboBoxBarrack3, FlatComboBox comboBoxBarrack4,
            FlatTextBox textboxGold, FlatTextBox textboxElixir,
            FlatTextBox textboxDark, FlatTextBox textboxTrophy,
            FlatCheckBox checkboxGold, FlatCheckBox checkboxElixir,
            FlatCheckBox checkboxDark, FlatCheckBox checkboxTrophy,
            FlatCheckBox checkboxAlert, FlatComboBox comboBoxAttackSides,
            FlatCheckBox checkboxMaxTrophy, FlatTextBox textboxMaxTrophy,
            FlatComboBox comboBoxAttackMode)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            int intBarrack1 = Convert.ToInt32(data["troops"]["barrack1"]);
            int intBarrack2 = Convert.ToInt32(data["troops"]["barrack2"]);
            int intBarrack3 = Convert.ToInt32(data["troops"]["barrack3"]);
            int intBarrack4 = Convert.ToInt32(data["troops"]["barrack4"]);
            Action action1 = () => comboBoxBarrack1.SelectedIndex = intBarrack1;
            comboBoxBarrack1.Invoke(action1);
            Action action2 = () => comboBoxBarrack2.SelectedIndex = intBarrack2;
            comboBoxBarrack2.Invoke(action2);
            Action action3 = () => comboBoxBarrack3.SelectedIndex = intBarrack3;
            comboBoxBarrack3.Invoke(action3);
            Action action4 = () => comboBoxBarrack4.SelectedIndex = intBarrack4;
            comboBoxBarrack4.Invoke(action4);
            int gold = Convert.ToInt32(data["search"]["gold"]);
            int elixir = Convert.ToInt32(data["search"]["elixir"]);
            int dark = Convert.ToInt32(data["search"]["dark"]);
            int trophy = Convert.ToInt32(data["search"]["trophy"]);
            Action action5 = () => textboxGold.Text = gold.ToString();
            textboxGold.Invoke(action5);
            Action action6 = () => textboxElixir.Text = elixir.ToString();
            textboxElixir.Invoke(action6);
            Action action7 = () => textboxDark.Text = dark.ToString();
            textboxDark.Invoke(action7);
            Action action8 = () => textboxTrophy.Text = trophy.ToString();
            textboxTrophy.Invoke(action8);
            bool bgold = Convert.ToBoolean(data["search"]["bgold"]);
            bool belixir = Convert.ToBoolean(data["search"]["belixir"]);
            bool bdark = Convert.ToBoolean(data["search"]["bdark"]);
            bool btrophy = Convert.ToBoolean(data["search"]["btrophy"]);
            bool alert = Convert.ToBoolean(data["search"]["alert"]);
            Action action9 = () => checkboxGold.Checked = bgold;
            checkboxGold.Invoke(action9);
            Action action10 = () => checkboxElixir.Checked = belixir;
            checkboxElixir.Invoke(action10);
            Action action11 = () => checkboxDark.Checked = bdark;
            checkboxDark.Invoke(action11);
            Action action12 = () => checkboxTrophy.Checked = btrophy;
            checkboxTrophy.Invoke(action12);
            Action action13 = () => checkboxAlert.Checked = alert;
            checkboxAlert.Invoke(action13);
            int attackSides = Convert.ToInt32(data["attack"]["sides"]);
            Action action14 = () => comboBoxAttackSides.SelectedIndex = attackSides;
            comboBoxAttackSides.Invoke(action14);
            bool bmaxtrophy = Convert.ToBoolean(data["attack"]["bmaxtrophy"]);
            int maxtrophy = Convert.ToInt32(data["attack"]["maxtrophy"]);
            Action action15 = () => checkboxMaxTrophy.Checked = bmaxtrophy;
            checkboxMaxTrophy.Invoke(action15);
            Action action16 = () => textboxMaxTrophy.Text = maxtrophy.ToString();
            textboxMaxTrophy.Invoke(action16);
            int attackMode = Convert.ToInt32(data["attack"]["mode"]);
            Action action17 = () => comboBoxAttackMode.SelectedIndex = attackMode;
            comboBoxAttackMode.Invoke(action17);
        }

    }
}
