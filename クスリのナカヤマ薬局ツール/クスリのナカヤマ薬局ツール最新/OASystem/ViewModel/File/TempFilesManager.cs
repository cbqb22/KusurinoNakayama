using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OASystem.Properties;

namespace OASystem.ViewModel.File
{
    public static class TempFilesManager
    {
        public static void FolderCheck()
        {
            if (!Directory.Exists(Settings.Default.TempFilesFolderPath))
            {
                Directory.CreateDirectory(Settings.Default.TempFilesFolderPath);
            }
        }
    }
}
