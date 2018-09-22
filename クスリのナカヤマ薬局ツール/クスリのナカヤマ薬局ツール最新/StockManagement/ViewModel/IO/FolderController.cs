using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StockManagement.Const;

namespace StockManagement.ViewModel.IO
{
    public static class FolderController
    {
        public static void FolderCheck()
        {
            if (!Directory.Exists(SMConst.rootFolder))
            {
                Directory.CreateDirectory(SMConst.rootFolder);
            }

            if (!Directory.Exists(SMConst.downloadFolder))
            {
                Directory.CreateDirectory(SMConst.downloadFolder);
            }

            if (!Directory.Exists(SMConst.outputFolder))
            {
                Directory.CreateDirectory(SMConst.outputFolder);
            }

            if (!Directory.Exists(SMConst.outputOrderFolder))
            {
                Directory.CreateDirectory(SMConst.outputOrderFolder);
            }

            if (!Directory.Exists(SMConst.outputReceiveFolder))
            {
                Directory.CreateDirectory(SMConst.outputReceiveFolder);
            }

            if (!Directory.Exists(SMConst.outputAllStoreNoUseFolder))
            {
                Directory.CreateDirectory(SMConst.outputAllStoreNoUseFolder);
            }

            if (!Directory.Exists(SMConst.outputExpOrderFolder))
            {
                Directory.CreateDirectory(SMConst.outputExpOrderFolder);
            }


        }

    }
}
