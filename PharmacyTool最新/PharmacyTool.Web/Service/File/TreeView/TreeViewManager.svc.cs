using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace PharmacyTool.Web.Service.File.TreeView
{
    // メモ: ここでクラス名 "TreeViewManager" を変更する場合は、Web.config で "TreeViewManager" への参照も更新する必要があります。
    public class TreeViewManager : ITreeViewManager
    {
        public List<TreeViewItemData> Create資料TreeView()
        {

#if DEBUG
            string FilePath = PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPathDEBUG;

#elif NAKAYAMA
            string FilePath = PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPathNAKAYAMA;

#else
            string FilePath = PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPath;
#endif


            string[] directoryArray = Directory.GetDirectories(FilePath, "*", SearchOption.AllDirectories);
            var sortdirectory = directoryArray.OrderBy(keyselector => keyselector);


            List<string> cleardirectory = new List<string>();
            foreach (var sd in sortdirectory)
            {
                cleardirectory.Add(sd.Replace(FilePath, ""));
            }


            List<Dictionary<string, List<TreeViewItemData>>> rootSource = new List<Dictionary<string, List<TreeViewItemData>>>();

            bool 最初かどうか = false;
            foreach (var cd in cleardirectory)
            {
                if (最初かどうか == false)
                {
                    List<TreeViewItemData> tvi = new List<TreeViewItemData>();
                    Dictionary<string, List<TreeViewItemData>> dic = new Dictionary<string, List<TreeViewItemData>>();
                    dic.Add(cd, tvi);

                    rootSource.Add(dic);
                    最初かどうか = true;
                    continue;
                }

                string[] sepa = cd.Split('\\');

                int ct = sepa.Count();

                string directoryname = sepa[ct - 1];
                int counter = 1;

                StringBuilder sb = new StringBuilder();
                foreach (var sp in sepa)
                {
                    if (counter < ct)
                    {
                        if (sb.ToString().Equals(""))
                        {
                            sb.Append(sp);
                        }
                        else
                        {
                            sb.Append("\\");
                            sb.Append(sp);
                        }
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }

                // 元の階層のTreeViewItemDataに参加
                foreach (var rs in rootSource)
                {
                    foreach (var d in rs)
                    {
                        if (d.Key.Equals(sb.ToString()))
                        {
                            TreeViewItemData tvi = new TreeViewItemData();
                            tvi.Name = directoryname;

                            // ディレクトリのIcon
                            tvi.Image = "/etc/Icon/folder1.png";
                            tvi.PathFromRoot = cd;
                            tvi.IsDirectory = true;
                            d.Value.Add(tvi);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                // 自分の階層を作る
                // countの数が階層　階層がListにない場合は追加する
                if (rootSource.Count < ct)
                {
                    Dictionary<string, List<TreeViewItemData>> dic = new Dictionary<string, List<TreeViewItemData>>();
                    List<TreeViewItemData> ltvi = new List<TreeViewItemData>();
                    dic.Add(cd, ltvi);
                    rootSource.Add(dic);
                }
                else
                {
                    List<TreeViewItemData> ltvi = new List<TreeViewItemData>();
                    rootSource[ct - 1].Add(cd, ltvi);
                }

            }


            List<TreeViewItemData> root = new List<TreeViewItemData>();

            int counter2 = 0;
            foreach (var rs in rootSource)
            {
                foreach (var r in rs)
                {
                    // ディレクトリ名と格納ディレクトリ名の取得
                    string[] sepa = r.Key.Split('\\');

                    int ct = sepa.Count();

                    // ディレクトリ名
                    string directoryname = sepa[ct - 1];
                    if (ct == 1)
                    {

                        string[] fileArray = System.IO.Directory.GetFiles(Path.Combine(FilePath, directoryname), "*", SearchOption.TopDirectoryOnly);
                        foreach (var f in fileArray)
                        {
                            TreeViewItemData localtvid = new TreeViewItemData();
                            string[] sp = f.Split('\\');
                            localtvid.Name = sp[sp.Count() - 1];
                            localtvid.PathFromRoot = f;
                            localtvid.IsDirectory = false;
                            localtvid.Image = GetImagePath(localtvid.Name);

                            r.Value.Add(localtvid);
                        }


                        // root直下のディレクトリ
                        TreeViewItemData tvid = new TreeViewItemData();
                        tvid.Name = r.Key;
                        tvid.Image = "/etc/Icon/folder1.png";
                        tvid.PathFromRoot = r.Key;
                        tvid.Children = r.Value;
                        tvid.IsDirectory = true;
                        root.Add(tvid);

                        continue;
                    }


                    int counter = 1;

                    // 格納ディレクトリ名
                    StringBuilder sb = new StringBuilder();
                    foreach (var sp in sepa)
                    {
                        if (counter < ct)
                        {
                            if (sb.ToString().Equals(""))
                            {
                                sb.Append(sp);
                            }
                            else
                            {
                                sb.Append("\\");
                                sb.Append(sp);
                            }
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                    }


                    List<TreeViewItemData> li = root;
                    int co2 = 0;
                    for (int a = 0; a < counter2; a++)
                    {
                        int co = 0;
                        foreach (var l in li)
                        {
                            string sep = sepa[co2];
                            if (l.Name.Equals(sep))
                            {
                                li = li[co].Children;
                                break;
                            }
                            co++;
                        }
                        co2++;
                    }

                    foreach (var l in li)
                    {
                        if (l.Name.Equals(directoryname))
                        {
                            // Fileの格納
                            string[] fileArray2 = System.IO.Directory.GetFiles(Path.Combine(FilePath, r.Key), "*", SearchOption.TopDirectoryOnly);
                            foreach (var f in fileArray2)
                            {
                                TreeViewItemData localtvid = new TreeViewItemData();
                                string[] sp = f.Split('\\');
                                localtvid.Name = sp[sp.Count() - 1];
                                localtvid.PathFromRoot = f;
                                localtvid.IsDirectory = false;
                                localtvid.Image = GetImagePath(localtvid.Name);

                                r.Value.Add(localtvid);
                            }

                            l.Children = r.Value;
                            break;
                        }
                    }






                }

                counter2++;
            }

            //rootの直下のFileを格納
            string[] fileArray3 = System.IO.Directory.GetFiles(FilePath, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in fileArray3)
            {
                TreeViewItemData localtvid = new TreeViewItemData();
                string[] sp = f.Split('\\');
                localtvid.Name = sp[sp.Count() - 1];
                localtvid.PathFromRoot = f;
                localtvid.IsDirectory = false;
                localtvid.Image = GetImagePath(localtvid.Name);

                root.Add(localtvid);
            }

            List<TreeViewItemData> rt = new List<TreeViewItemData>();
            TreeViewItemData tvidrt = new TreeViewItemData();
            tvidrt.Children = root;
            tvidrt.IsDirectory = true;
            tvidrt.Image = "/etc/Icon/folder1.png";
            tvidrt.Name = "Root";
            tvidrt.PathFromRoot = FilePath;
            rt.Add(tvidrt);

            return rt;

            //return root;
        }

        #region クラス内部で使用のメソッド

        private string GetImagePath(string FileName)
        {
            string[] splitpyriod = FileName.Split('.');

            string 拡張子 = splitpyriod[splitpyriod.Count() - 1];


            if (拡張子.ToUpper().Equals("CSV") || 拡張子.ToUpper().Equals("TXT"))
            {
                return "/etc/Icon/txt.png";
            }
            else if (拡張子.ToUpper().Equals("DOC") || 拡張子.ToUpper().Equals("DOCX"))
            {
                return "/etc/Icon/doc.png";
            }
            else if (拡張子.ToUpper().Equals("XLS") || 拡張子.ToUpper().Equals("XLSX"))
            {
                return "/etc/Icon/xls.png";
            }
            else if (拡張子.ToUpper().Equals("PPT") || 拡張子.ToUpper().Equals("PPTX"))
            {
                return "/etc/Icon/ppt.png";
            }
            else if (拡張子.ToUpper().Equals("PDF"))
            {
                return "/etc/Icon/pdf.png";
            }
            else
            {
                return "";
            }
        }

        #endregion


    }

    public class TreeViewData
    {
        public List<TreeViewItemData> TreeItemList { get; set; }

    }


    public class TreeViewItemData
    {
        public bool IsDirectory { get; set; }
        public string PathFromRoot { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public List<TreeViewItemData> Children { get; set; }
    }

    public class Phar : PharmacyTool.Web.DAO.PharmacyTool.ユーザー管理
    {
        public Phar()
            : base()
        {
        }
    }

}
