using System;
using System.Windows.Forms;

namespace Clash_of_Bots
{
    class CreateFile
    {
        public static void Create(string file)
        {
            if (file == "config.ini")
            {
                string[] configLines = { 
                                        "[collectors]",
                                        "collector1 = -1;-1",
                                        "collector2 = -1;-1",
                                        "collector3 = -1;-1",
                                        "collector4 = -1;-1",
                                        "collector5 = -1;-1",
                                        "collector6 = -1;-1",
                                        "collector7 = -1;-1",
                                        "collector8 = -1;-1",
                                        "collector9 = -1;-1",
                                        "collector10 = -1;-1",
                                        "collector11 = -1;-1",
                                        "collector12 = -1;-1",
                                        "collector13 = -1;-1",
                                        "collector14 = -1;-1",
                                        "collector15 = -1;-1",
                                        "collector16 = -1;-1",
                                        "collector17 = -1;-1",
                                        "",
                                        "[barracks]",
                                        "barrack1 = -1;-1",
                                        "barrack2 = -1;-1",
                                        "barrack3 = -1;-1",
                                        "barrack4 = -1;-1",
                                        "",
                                        "[troops]",
                                        "barrack1 = 0",
                                        "barrack2 = 0",
                                        "barrack3 = 1",
                                        "barrack4 = 1",
                                        "",
                                        "[search]",
                                        "gold = 100000",
                                        "elixir = 100000",
                                        "dark = 0",
                                        "trophy = 0",
                                        "bgold = True",
                                        "belixir = True",
                                        "bdark = False",
                                        "btrophy = False",
                                        "alert = True",
                                        "",
                                        "[attack]",
                                        "sides = 2",
                                        "mode = 0",
                                        "bmaxtrophy = False",
                                        "maxtrophy = 1000",
                                        "deploytime = 50"
                                        };
                    System.IO.File.WriteAllLines(Application.StartupPath + "\\" + file, configLines);
                    Log.SetLog("Config.ini File Not Found - Creating it...");
            }
            // Create Reg File
            if (file == "860x720.reg")
            {
                string[] regLines = { 
                                            "Windows Registry Editor Version 5.00",
                                            "",
                                            "[HKEY_LOCAL_MACHINE\\SOFTWARE\\BlueStacks\\Guests\\Android\\FrameBuffer\\0]",
                                            "\"WindowWidth\"=dword:0000035C",
                                            "\"WindowHeight\"=dword:000002D0",
                                            "\"GuestWidth\"=dword:0000035C",
                                            "\"GuestHeight\"=dword:000002D0",
                                            "\"Depth\"=dword:00000010",
                                            "\"FullScreen\"=dword:00000000",
                                            "\"WindowState\"=dword:00000001",
                                            "\"HideBootProgress\"=dword:00000001"
                                         };

                System.IO.File.WriteAllLines(Application.StartupPath + "\\" + file, regLines);
                Log.SetLog("860x720.reg File Not Found - Creating it...");
            }
        }
    }
}
