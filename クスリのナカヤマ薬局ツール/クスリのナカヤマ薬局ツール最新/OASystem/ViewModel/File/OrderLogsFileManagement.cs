using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;
using OASystem.Properties;
using System.Windows;


namespace OASystem.ViewModel.File
{
    public static class OrderLogsFileManagement
    {
        public static void FolderCheck()
        {
            if (!IO.Directory.Exists(Settings.Default.OrderLogsFolderPath))
            {
                IO.Directory.CreateDirectory(Settings.Default.OrderLogsFolderPath);
            }
        }

    }
}
