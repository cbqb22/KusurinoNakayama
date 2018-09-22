using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OASystem.Properties;

namespace OASystem.ViewModel.File
{
    public static class OASystemCenter
    {
        public static void FolderCheck()
        {
            if (!Directory.Exists(Settings.Default.OASystemRootPath))
            {
                Directory.CreateDirectory(Settings.Default.OASystemRootPath);
            }
        }
    }
}
