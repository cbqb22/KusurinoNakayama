using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OASystem.Properties;
using OASystem.Common;

namespace OASystem.ViewModel.File
{
    public static class TempFilesManager
    {
        public static void FolderCheck()
        {
            if (!Directory.Exists(Settings.TempFilesFolderPath))
            {
                Directory.CreateDirectory(Settings.TempFilesFolderPath);
            }
        }
    }
}
