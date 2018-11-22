using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OASystem.Properties;
using OASystem.Common;

namespace OASystem.ViewModel.File
{
    public static class OASystemCenter
    {
        public static void FolderCheck()
        {
            if (!Directory.Exists(Settings.OASystemRootPath))
            {
                Directory.CreateDirectory(Settings.OASystemRootPath);
            }
        }
    }
}
