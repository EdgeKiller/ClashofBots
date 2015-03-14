using System;
using FlatUITheme;

namespace Clash_of_Bots
{
    class Log
    {
        private static FlatListBox log;
        private static FlatStatusBar status;

        public static void Init(FlatListBox listlog, FlatStatusBar statusbar)
        {
            log = listlog;
            status = statusbar;
        }

        public static void SetLog(string text)
        {
            Action action = () => log.AddItem("[" + DateTime.Now + "] " + text);
            log.Invoke(action);
            if(log.CountItem() > 17)
            {
                Action action1 = () => log.SelectIndex(log.CountItem() - 1);
                log.Invoke(action1);
            }
        }

        public static void SetStatus(string text)
        {
            Action action = () => status.Text = "Status : " + text;
            status.Invoke(action);
        }

        public static void Clear()
        {
            log.Clear();
        }

    }
}
