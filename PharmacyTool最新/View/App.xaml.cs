using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace View
{
    public partial class App : Application
    {

        public App()
        {
            // 必ず先に行っておく必要がある。
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            InitializeComponent();

            // Internet Explorer以外は未対応とする。
            // IE = Microsoft Internet Explorer
            // Chrome = Netscape
            if (System.Windows.Browser.HtmlPage.BrowserInformation.Name.Equals("Microsoft Internet Explorer") == false)
            {
                MessageBox.Show("Internet Explorer以外のブラウザは未対応の為、終了します。", "ブラウザの種類確認", MessageBoxButton.OK);

                // 401エラー認証失敗のページへ遷移
                string Error401Path = Core.共通.Settings.Error401Path;
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(Error401Path), "_self");

            }
            else
            {
                // ログイン確認
                Core.Login.LoginCheckDialog lcd = new View.Core.Login.LoginCheckDialog();
                lcd.Show();

                this.RootVisual = new MainPage();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
        }

        private void Application_Exit(object sender, EventArgs e)
        {
        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // アプリケーションがデバッガーの外側で実行されている場合、ブラウザーの
            // 例外メカニズムによって例外が報告されます。これにより、IE ではステータス バーに
            // 黄色の通知アイコンが表示され、Firefox にはスクリプト エラーが表示されます。
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // メモ : これにより、アプリケーションは例外がスローされた後も実行され続け、例外は
                // ハンドルされません。 
                // 実稼動アプリケーションでは、このエラー処理は、Web サイトにエラーを報告し、
                // アプリケーションを停止させるものに置換される必要があります。
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
