using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace MediWebOrderWatcher
{
    public partial class NotifyIconWrapper : Component
    {
        public NotifyIconWrapper()
        {
            InitializeComponent();

            toolStripMenuItemShow.Click += new EventHandler(toolStripMenuItemShow_Click);
            toolStripMenuItemExit.Click += new EventHandler(toolStripMenuItemExit_Click);

            System.IO.Stream iconstream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/desktop.ico")).Stream;
            this.notifyIcon1.Icon = new System.Drawing.Icon(iconstream);
        }

        private MainWindow win = new MainWindow();

        void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        void toolStripMenuItemShow_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void ShowWindow()
        {
            // ウィンドウ表示＆最前面に持ってくる
            if (win.WindowState == System.Windows.WindowState.Minimized)
            {
                win.WindowState = System.Windows.WindowState.Normal;
            }

            win.Show();
            win.Activate();

            win.ShowInTaskbar = true;
        }

        //public NotifyIconWrapper(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}

        //private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        //{

        //}
    }
}
