﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace クスリのナカヤマ薬局ツール.共通.File
{
    public static class GeneralMethods
    {
        /// <summary>
        /// ディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        public static void CopyDirectory(string sourceDirName, string destDirName,bool copySubDirectory)
        {
            //コピー先のディレクトリがないときは作る
            if (!System.IO.Directory.Exists(destDirName))
            {
                System.IO.Directory.CreateDirectory(destDirName);
                //属性もコピー
                System.IO.File.SetAttributes(destDirName,
                    System.IO.File.GetAttributes(sourceDirName));
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (destDirName[destDirName.Length - 1] !=
                    System.IO.Path.DirectorySeparatorChar)
                destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = System.IO.Directory.GetFiles(sourceDirName);
            foreach (string file in files)
                System.IO.File.Copy(file,
                    destDirName + System.IO.Path.GetFileName(file), true);

            if (copySubDirectory)
            {
                //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
                string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);
                foreach (string dir in dirs)
                    CopyDirectory(dir, destDirName + System.IO.Path.GetFileName(dir),true);
            }
        }
    }
}
