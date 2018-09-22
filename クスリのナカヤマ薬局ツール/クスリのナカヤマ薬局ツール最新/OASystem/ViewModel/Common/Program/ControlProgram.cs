using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OASystem.Properties;
using System.Diagnostics;

namespace OASystem.ViewModel.Common.Program
{
    public static class ControlProgram
    {
        public static string StartProgram(string programPath,string processName)
        {
            //二重起動をチェックする
            if (Process.GetProcessesByName(processName).Length > 1)
            {
                return "すでに起動しています。";
            }

            //ProcessStartInfoオブジェクトを作成する
            ProcessStartInfo psi = new ProcessStartInfo();
            //起動するファイルのパスを指定する
            psi.FileName = programPath;
            //コマンドライン引数を指定する
            //psi.Arguments = @"""C:\test\1.txt""";

            Process proc = null;
            try
            {
                //アプリケーションを起動する
                proc = Process.Start(psi);
            }
            catch (Exception ex)
            {
                return string.Format("エラーが発生した為、{0}を起動できませんでした。\r\nエラー内容:{1}",processName,ex.Message);
            }

            if (proc == null)
            {
                return string.Format("{0}を起動できませんでした。",processName);
            }


            return null;
        }


        public static string EndProgram(string processName)
        {
            //notepadのプロセスを取得
            Process[] ps = Process.GetProcessesByName(processName);

            foreach (System.Diagnostics.Process p in ps)
            {
                //クローズメッセージを送信する
                p.CloseMainWindow();

              
                //プロセスが終了するまで最大1秒待機する
                //bool b = p.WaitForExit(3000);
                //p.WaitForExit();
                p.Kill();
                
                //プロセスが終了したか確認する
                //if (p.HasExited)
                //{
                //    return "";
                //}
                //else
                //{
                //    return string.Format("{0}を終了できませんでした。", processName);
                //}
            }

            return "";

        }


        /// <summary>
        /// 二重起動の確認
        /// </summary>
        /// <param name="processName"></param>
        /// <returns>true = 起動可能, false = 起動不可</returns>
        public static bool CheckMutexOperation(string processName)
        {
            //二重起動をチェックする
            if (Process.GetProcessesByName(processName).Length > 1)
            {
                return false;
            }

            return true;

        }
    }
}
