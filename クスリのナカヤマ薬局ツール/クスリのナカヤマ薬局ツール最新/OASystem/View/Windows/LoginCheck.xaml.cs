using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OASystem.View.Windows
{
    /// <summary>
    /// LoginCheck.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginCheck : Window
    {
        private bool _LoginSuccess;

        public bool LoginSuccess
        {
            get { return _LoginSuccess; }
            set { _LoginSuccess = value; }
        }

        public LoginCheck()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(LoginCheck_Loaded);
        }

        void LoginCheck_Loaded(object sender, RoutedEventArgs e)
        {
            tbID.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //btnLogin.Focus();

            if (tbID.Text == "")
            {
                MessageBox.Show("IDが入力されておりません。", "入力チェック", MessageBoxButton.OK,MessageBoxImage.Exclamation);
                tbID.Focus();
                _FromLoginEvent = true;
                return;
            }

            if (pbPassword.Password == "")
            {
                MessageBox.Show("Passwordが入力されておりません。", "入力チェック", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                pbPassword.Focus();
                _FromLoginEvent = true;
                return;
            }

            if (tbID.Text == "Honnbu" && pbPassword.Password == "HonnbuGo")
            {
                LoginSuccess = true;
            }
            else
            {
                MessageBox.Show("IDまたはPasswordが違います。", "入力チェック", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                tbID.Focus();
                _FromLoginEvent = true;
                return;
            }

            this.Close();

        }

        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tbID_KeyUp(object sender, KeyEventArgs e)
        {
            if (_FromLoginEvent)
            {
                _FromLoginEvent = false;
            }
            else if (e.Key == Key.Enter)
            {
                pbPassword.Focus();
            }
        }

        private bool _FromLoginEvent;

        private void pbPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (_FromLoginEvent)
            {
                _FromLoginEvent = false;
            }
            else if (e.Key == Key.Enter)
            {
                btnLogin_Click(null, null);
            }
        }
    }
}
