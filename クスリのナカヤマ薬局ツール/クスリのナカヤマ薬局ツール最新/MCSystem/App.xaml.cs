using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Diagnostics;
using MCSystem.View.Windows;
using MCSystem.ViewModel;
using MCSystem.Model;
using MCSystem.ViewModel.Common.Program;
using MCSystem.ViewModel.Common.MessageBox;


namespace MCSystem
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            if (CheckHasCmd())
            {
                DoCmd();
                App.Current.Shutdown();
            }
            else
            {
                // 二重起動チェック
                if (!ControlProgram.CheckMutexOperation(Process.GetCurrentProcess().ProcessName))
                {
                    MessageBox.Show("すでに起動しています。", "確認", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Shutdown();
                    return;
                }



                // TODO:AutoUpdate
                ControlBCSettings.SetSettingsToDI(ControlBCSettings.LoadSettingsFromFile());
                //var ee = DI.検索名称XY座標;


                this.MainWindow = SingletonWindows.BalanceChangeMenuWindow;
                this.MainWindow.ShowDialog();
            }

        }


        private bool CheckHasCmd()
        {
            string[] cmds = System.Environment.GetCommandLineArgs();

            if(cmds.Length <= 1)
            {
                return false;
            }

            return true;
        }

        private void DoCmd()
        {
            string[] cmds = System.Environment.GetCommandLineArgs();

            //実行パスのみはreturn
            if (cmds.Length <= 1)
            {
                return;
            }

            if (cmds.Length == 3 &&
                cmds[1] == "sbimacro")
            {

                OriginalMacroExecuter omExe1 = new OriginalMacroExecuter();
                string inipath = @"C:\Users\poohace\Desktop\SBIマクロ\Macro20160513020455.csv";
                omExe1.DoFromCmd(inipath);

                string path = cmds[2];
                OriginalMacroExecuter omExe2 = new OriginalMacroExecuter();

                omExe2.DoFromCmd(path);

                //SBILoginプロセスを終了
                var sbiLoginProcess = Process.GetProcessesByName("ETLogin");
                foreach (var p in sbiLoginProcess)
                {
                    p.Kill();
                }
                var sbiETCommGate = Process.GetProcessesByName("ETCommGate");
                foreach (var p in sbiETCommGate)
                {
                    p.Kill();
                }
 
                MessageBoxTop.Show("マクロ処理が完了しました。", "正常終了", MessageBoxButton.OK, MessageBoxImage.Information);


            }
        }
    }
}
