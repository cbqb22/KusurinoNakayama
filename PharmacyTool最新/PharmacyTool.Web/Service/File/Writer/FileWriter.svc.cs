using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace PharmacyTool.Web.Service.File.Writer
{
    // メモ: ここでクラス名 "FileWriter" を変更する場合は、Web.config で "FileWriter" への参照も更新する必要があります。
    public class FileWriter : IFileWriter
    {
        public string FileWriter実行(string SendPath, ディレクトリ操作モード mode, タイプ type, string NewName)
        {
#if DEBUG
            string path = PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPathDEBUG;

            if (!SendPath.Equals("\\"))
            {
                path = Path.Combine(path, SendPath);
            }
#elif NAKAYAMA

            string path = PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPathNAKAYAMA;
            if (!SendPath.Equals("\\"))
            {
                path = Path.Combine(path, SendPath);
            }
#else

            string path = PharmacyTool.Web.Properties.Settings.Default.UploadFileRootPath;
            if (!SendPath.Equals("\\"))
            {
                path = Path.Combine(path, SendPath);
            }

#endif


            if (string.IsNullOrEmpty(path))
            {
                return "ファイルパスが入力されていません。";
            }

            if (type == タイプ.ファイル)
            {
                if (mode == ディレクトリ操作モード.Create)
                {
                    return "フォルダを指定して下さい。";
                }
                else if (mode == ディレクトリ操作モード.Delete)
                {
                    return DeleteFile(path);
                }
                else if (mode == ディレクトリ操作モード.Rename)
                {
                    if (string.IsNullOrEmpty(NewName))
                    {
                        return "変更後の名前が指定されていません。";
                    }

                    string[] sepa = path.Split('\\');
                    int ct = sepa.Count();

                    int counter = 1;
                    StringBuilder sb = new StringBuilder();
                    foreach (var s in sepa)
                    {
                        if (ct == 1)
                        {
                            sb.Append("");
                            break;
                        }

                        if (counter < ct)
                        {
                            if (sb.ToString().Equals(""))
                            {
                                sb.Append(s);
                            }
                            else
                            {
                                sb.Append("\\");
                                sb.Append(s);
                            }
                        }
                        else
                        {
                            break;
                        }

                        counter++;

                    }

                    return ModifyFile(path, Path.Combine(sb.ToString(), NewName));


                }
            }
            else
            {
                if (mode == ディレクトリ操作モード.Create)
                {
                    return CreateDirectory(Path.Combine(path, NewName));
                }
                else if (mode == ディレクトリ操作モード.Delete)
                {
                    return DeleteDirectory(path);
                }
                else if (mode == ディレクトリ操作モード.Rename)
                {
                    if (string.IsNullOrEmpty(NewName))
                    {
                        return "変更後の名前が指定されていません。";
                    }


                    string[] sepa = path.Split('\\');
                    int ct = sepa.Count();

                    int counter = 1;
                    StringBuilder sb = new StringBuilder();
                    foreach (var s in sepa)
                    {
                        if (ct == 1)
                        {
                            sb.Append("");
                            break;
                        }

                        if (counter < ct)
                        {
                            if (sb.ToString().Equals(""))
                            {
                                sb.Append(s);
                            }
                            else
                            {
                                sb.Append("\\");
                                sb.Append(s);
                            }
                        }
                        else
                        {
                            break;
                        }

                        counter++;

                    }

                    return ModifyDirectory(path, Path.Combine(sb.ToString(), NewName));

                }
            }

            return "操作に失敗しました。";
        }


        public string テロップ変更実行(string テロップ記事)
        {
#if DEBUG
            string path = PharmacyTool.Web.Properties.Settings.Default.TeropFilePathDEBUG;

#elif NAKAYAMA

            string path = PharmacyTool.Web.Properties.Settings.Default.TeropFilePathNAKAYAMA;

#else

            string path = PharmacyTool.Web.Properties.Settings.Default.TeropFilePath;

#endif

            if (string.IsNullOrEmpty(path))
            {
                return "エラーが発生しました。ファイルパスが存在しません。";
            }

            return OverrideOrCreateToTerop(path, テロップ記事);

        }



        public 掲示板結果リターンEntity 掲示板データ書込(string カテゴリ名, string タイトル, string 投稿者, string 記事, 掲示板書込タイプ タイプ, string 記事No, string HomepageUrl, string Email, List<string> 添付画像パスリスト, string 暗証キー, 文字色 文字色)
        {
            掲示板結果リターンEntity ent = new 掲示板結果リターンEntity();

            if (string.IsNullOrEmpty(カテゴリ名) ||
                string.IsNullOrEmpty(タイトル) ||
                string.IsNullOrEmpty(投稿者) ||
                string.IsNullOrEmpty(記事) ||
                (タイプ == 掲示板書込タイプ.返信投稿) && string.IsNullOrEmpty(記事No) ||
                (タイプ == 掲示板書込タイプ.記事修正) && string.IsNullOrEmpty(記事No)
                )
            {
                ent.書込成功か = false;
                ent.エラーメッセージ = "未入力の項目がある為、投稿出来ませんでした。\r\n再度投稿して下さい。";
                return ent;
            }


#if DEBUG
            string RootPath = @"C:\PharmacyTools\ClientBin\掲示板";
            string DirectoryRootPath = Path.Combine(RootPath, カテゴリ名);
#elif NAKAYAMA
            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPathNAKAYAMA;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);
#else
            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPath;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);
#endif


            // スレッド修正または削除でデータがない場合
            if (!Directory.Exists(DirectoryRootPath))
            {
                //Directory.CreateDirectory(DirectoryRootPath);

                ent.書込成功か = false;
                ent.エラーメッセージ = "選択したスレッドカテゴリがありません。再度ページを更新して下さい。";
                return ent;

            }

            try
            {


                // 掲示板書込みキューに追加する
                ApplicationScope.掲示板書込みキュー.Enqueue(this.GetHashCode());
                while (ApplicationScope.掲示板書込みキュー.Peek() != this.GetHashCode())
                {
                    // 待機する
                }


                List<int> list = new List<int>();

                // すべての.txtファイルの記事番号を取得
                // 数値に変換できなかったらエラー
                Directory.GetFiles(DirectoryRootPath, "*.txt", SearchOption.AllDirectories).ToList().ForEach
                    (str =>
                        list.Add(int.Parse(str.Split('.')[str.Split('.').Length - 2].Substring(str.Split('.')[str.Split('.').Length - 2].Length - 8, 8)))
                    );

                // フォルダ名（記事No）の最大番号 + 1

                int MaxDirectoryNo = 0;

                if (list.Count < 1)
                {
                    MaxDirectoryNo = 1;
                }
                else
                {
                    MaxDirectoryNo = list.Max() + 1;
                }

                string CreateDirectoryPath = Path.Combine(DirectoryRootPath, MaxDirectoryNo.ToString());

                // 新規投稿の場合
                if (タイプ == 掲示板書込タイプ.新規投稿)
                {
                    // もしタイミングが重なって番号が重複したら、次の番号で作成する。
                    while (Directory.Exists(CreateDirectoryPath))
                    {
                        MaxDirectoryNo += MaxDirectoryNo;
                        CreateDirectoryPath = Path.Combine(DirectoryRootPath, (MaxDirectoryNo).ToString());
                    }

                    // 新規投稿用のフォルダを作成
                    Directory.CreateDirectory(CreateDirectoryPath);
                    Directory.CreateDirectory(Path.Combine(CreateDirectoryPath, "メイン"));
                    Directory.CreateDirectory(Path.Combine(CreateDirectoryPath, "レス"));


                    // 記事データの書込
                    string filepath = Path.Combine(Path.Combine(CreateDirectoryPath, "メイン"), MaxDirectoryNo.ToString().PadLeft(8, '0') + ".txt");

                    using (StreamWriter sw = new StreamWriter(filepath, false, Encoding.GetEncoding("Shift_JIS")))
                    {
                        // 1行目は記事の付加情報
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", タイトル, 投稿者, System.DateTime.Now.ToString("yyyy/MM/dd(ddd) HH:mm:ss"), HomepageUrl, Email, 暗証キー, 文字色));
                        StringBuilder sb画像パス = new StringBuilder();
                        sb画像パス.Append("[ImagePath]=");
                        bool 初回 = true;

                        // 2行目は画像パス カンマ区切りで指定
                        foreach (var 書込み画像パス in 添付画像パスリスト)
                        {
                            if (初回)
                            {
                                sb画像パス.Append(書込み画像パス);
                                初回 = false;
                            }
                            else
                            {
                                sb画像パス.Append(",");
                                sb画像パス.Append(書込み画像パス);
                            }
                        }
                        sw.WriteLine(sb画像パス.ToString());

                        // 3行目以降は記事になる(CRLFを許可のため)
                        sw.WriteLine(記事);

                        sw.Flush();
                    }

                    ent.書込成功か = true;
                    ent.書込み記事No = MaxDirectoryNo.ToString();
                    ent.書込みカテゴリ = カテゴリ名;
                    return ent;
                }
                // 返信の場合
                else if (タイプ == 掲示板書込タイプ.返信投稿)
                {
                    int result;
                    if (int.TryParse(記事No, out result) == false)
                    {
                        ent.書込成功か = false;
                        ent.エラーメッセージ = "記事Noに誤りがある為、投稿出来ませんでした。\r\n再度、投稿して下さい。";
                        return ent;
                    }

                    List<int> list2 = new List<int>();

                    Directory.GetFiles(DirectoryRootPath, "*.txt", SearchOption.AllDirectories).ToList().ForEach
                    (str =>
                        list2.Add(記事No8ケタ文字列to数字((str.Split('\\')[str.Split('\\').GetLength(0) - 1]).Substring(0, 8)))
                    );

                    int counter = 1;
                    string Max記事No = "";

                    if (list2.Count == 0)
                    {
                        // 最小値を使用
                        Max記事No = "00000001";
                    }
                    else
                    {
                        Max記事No = (list2.Max() + counter).ToString().PadLeft(8, '0');
                    }

                    // 返信用の記事のフォルダパス
                    string 返信用DirectoryPath = Path.Combine(DirectoryRootPath, 記事No);

                    // 記事データの書込
                    string filepath = Path.Combine(Path.Combine(返信用DirectoryPath, "レス"), Max記事No + ".txt");

                    // タイミングで書き込みが重なったときは、次の番号を付与。
                    if (System.IO.File.Exists(filepath))
                    {
                        counter++;
                        Max記事No = (記事No8ケタ文字列to数字(Max記事No) + counter).ToString().PadLeft(8, '0');
                        filepath = Path.Combine(Path.Combine(返信用DirectoryPath, "レス"), Max記事No + ".txt");
                    }

                    using (StreamWriter sw = new StreamWriter(filepath, false, Encoding.GetEncoding("Shift_JIS")))
                    {
                        // 1行目は記事の付加情報

                        // [0]タイトル
                        // [1]投稿者名
                        // [2]投稿日付
                        // [3]HomepageURL
                        // [4]Email
                        // [5]暗証キー
                        // [6]文字色
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", タイトル, 投稿者, System.DateTime.Now.ToString("yyyy/MM/dd(ddd) HH:mm:ss"), HomepageUrl, Email, 暗証キー, 文字色));
                        StringBuilder sb画像パス = new StringBuilder();
                        sb画像パス.Append("[ImagePath]=");
                        bool 初回 = true;

                        // 2行目は画像パス カンマ区切りで指定
                        foreach (var 書込み画像パス in 添付画像パスリスト)
                        {
                            if (初回)
                            {
                                sb画像パス.Append(書込み画像パス);
                                初回 = false;
                            }
                            else
                            {
                                sb画像パス.Append(",");
                                sb画像パス.Append(書込み画像パス);
                            }
                        }
                        sw.WriteLine(sb画像パス.ToString());

                        // 3行目以降は記事になる(CRLFを許可のため)
                        sw.WriteLine(記事);

                        sw.Flush();
                    }

                    ent.書込成功か = true; ;
                    ent.書込み記事No = int.Parse(Max記事No).ToString();
                    ent.書込みカテゴリ = カテゴリ名;

                    return ent;

                }
                // 記事修正の場合
                else
                {
                    string[] filearray = System.IO.Directory.GetFiles(DirectoryRootPath, "*.txt", SearchOption.AllDirectories);

                    string 修正記事FilePath = "";
                    foreach (var file in filearray)
                    {
                        string[] presepa = file.Split('\\');
                        if (presepa.Length < 1)
                        {
                            continue;
                        }

                        string[] sepa5 = presepa[presepa.Length - 1].Split('.');
                        if (sepa5.Length < 2)
                        {
                            continue;
                        }

                        if (sepa5[sepa5.Length - 2].Equals(記事No.PadLeft(8, '0')))
                        {
                            修正記事FilePath = file;
                        }
                    }

                    if (修正記事FilePath.Equals(""))
                    {
                        ent.書込成功か = false;
                        ent.エラーメッセージ = "記事データがない為、修正出来ませんでした。\r\n再度、ページを読み込んで下さい。";
                        return ent;
                    }


                    if (!System.IO.File.Exists(修正記事FilePath))
                    {
                        ent.書込成功か = false;
                        ent.エラーメッセージ = "記事データがない為、修正出来ませんでした。\r\n再度、ページを読み込んで下さい。";
                        return ent;
                    }

                    DateTime 修正前の最終書込み時間 = System.IO.File.GetLastWriteTime(修正記事FilePath);
                    using (StreamWriter sw = new StreamWriter(修正記事FilePath, false, Encoding.GetEncoding(932)))
                    {
                        // １行目

                        // [0]タイトル
                        // [1]投稿者名
                        // [2]投稿日付
                        // [3]HomepageURL
                        // [4]Email
                        // [5]暗証キー
                        // [6]文字色
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", タイトル, 投稿者, System.DateTime.Now.ToString("yyyy/MM/dd(ddd) HH:mm:ss"), HomepageUrl, Email, 暗証キー, 文字色));

                        // 2行目は画像パス カンマ区切りで指定
                        StringBuilder sb画像パス = new StringBuilder();
                        sb画像パス.Append("[ImagePath]=");
                        bool 初回 = true;
                        foreach (var 書込み画像パス in 添付画像パスリスト)
                        {
                            if (初回)
                            {
                                sb画像パス.Append(書込み画像パス);
                                初回 = false;
                            }
                            else
                            {
                                sb画像パス.Append(",");
                                sb画像パス.Append(書込み画像パス);
                            }
                        }
                        sw.WriteLine(sb画像パス.ToString());

                        // 3行目以降は記事になる(CRLFを許可のため)
                        sw.WriteLine(記事);
                        sw.Flush();
                    }

                    // 記事修正は記事表示順序を変更しない為、書込み時間を変更しない。
                    System.IO.File.SetLastWriteTime(修正記事FilePath, 修正前の最終書込み時間);

                    ent.書込成功か = true;
                    ent.書込み記事No = 記事No;
                    ent.書込みカテゴリ = カテゴリ名;

                    return ent;
                }

            }
            catch (Exception ex)
            {
                ent.書込成功か = false;
                ent.エラーメッセージ = "エラーが発生しました。\r\n原因:" + ex.Message + ex.StackTrace;
                return ent;

            }
            finally
            {
                // キューから削除する
                ApplicationScope.掲示板書込みキュー.Dequeue();

            }


        }

        public string 掲示板データ削除(string カテゴリ名, string 記事No, string 暗証キー, bool 管理者か)
        {
            if (string.IsNullOrEmpty(カテゴリ名))
            {
                return "カテゴリ名が不明の為、削除出来ませんでした。";
            }

            if (string.IsNullOrEmpty(記事No))
            {
                return "記事Noが不明の為、削除出来ませんでした。";
            }

            if (!管理者か && string.IsNullOrEmpty(暗証キー))
            {
                return "暗証キーが不明の為、削除出来ませんでした。";
            }

#if DEBUG
            string RootPath = @"C:\PharmacyTools\ClientBin\掲示板";
            string DirectoryRootPath = Path.Combine(RootPath, カテゴリ名);

#elif NAKAYAMA

            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPathNAKAYAMA;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);
#else
            string 掲示板RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPath;
            string DirectoryRootPath = Path.Combine(掲示板RootPath, カテゴリ名);
#endif

            string Search記事No = 記事No.PadLeft(8, '0');
            List<string> allFiles = Directory.GetFiles(DirectoryRootPath, "*.txt", SearchOption.AllDirectories).ToList();

            string TargetFilePath = "";
            foreach (var file in allFiles)
            {
                string[] sepa = file.Split('\\');
                string filename = sepa[sepa.Length - 1];

                string[] sepa2 = filename.Split('.');
                if (sepa2.Length < 2)
                {
                    return "記事データが壊れている為(拡張子がない)、削除できませんでした。";
                }
                string 名称 = sepa2[sepa2.Length - 2];

                if (Search記事No.Equals(名称))
                {
                    TargetFilePath = file;
                    break;
                }
            }

            if (string.IsNullOrEmpty(TargetFilePath))
            {
                return "削除対象の記事が存在しないの為、削除出来ませんでした。";
            }


            bool メイン記事か = false;
            if (TargetFilePath.Contains("メイン"))
            {
                メイン記事か = true;
            }


            if (メイン記事か)
            {
                // 2層上が記事のRootPath
                string 記事RootPath = Directory.GetParent(Directory.GetParent(TargetFilePath).FullName).FullName;


                string s = メインレス両削除(記事RootPath, 管理者か, 暗証キー, 記事No, カテゴリ名);
                if (s.Equals(""))
                {
                    return "削除しました。";
                }

                return s;
            }
            else
            {
                string s2 = レス記事のみ削除(TargetFilePath, 管理者か, 暗証キー, 記事No, カテゴリ名);
                if (s2.Equals(""))
                {
                    return "削除しました。";
                }

                return s2;
            }

        }



        /// <summary>
        /// スレッド新規追加、修正、削除
        /// </summary>
        /// <param name="InputThreadName">新規スレッド名称,削除するスレッド名</param>
        /// <param name="画像種類">画像の種類</param>
        /// <param name="操作タイプ">新規、修正、削除</param>
        /// <param name="SourceName">修正前のスレッド名</param>
        /// <returns></returns>
        public string WriteThreadTitles(string InputThreadName, 画像種類Enum 画像種類, スレッド操作タイプ 操作タイプ, string SourceName)
        {

#if DEBUG
            string RootPath = @"C:\PharmacyTools\ClientBin\掲示板";
            string 掲示板資料RootPath = PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathDEBUG;

#elif NAKAYAMA
            string RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPathNAKAYAMA;
            string 掲示板資料RootPath = PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathNAKAYAMA;
#else
            string RootPath = PharmacyTool.Web.Properties.Settings.Default.掲示板データRootPath;
            string 掲示板資料RootPath = PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPath;
#endif


            if (操作タイプ == スレッド操作タイプ.新規)
            {
                List<string> directorylist = Directory.GetDirectories(RootPath, "*", SearchOption.TopDirectoryOnly).ToList();


                foreach (var dir in directorylist)
                {
                    if (dir.Equals(InputThreadName))
                    {
                        return "同じ名前のスレッド名があります。ほかのスレッド名を入力して下さい。";
                    }

                }

                string createFolderPath = Path.Combine(RootPath, InputThreadName);
                if (!CreateDirectory(createFolderPath).Equals("フォルダを作成しました。"))
                {
                    return "同じ名前のスレッド名があります。ほかのスレッド名を入力して下さい。";
                }

                string imagefilepath = "";

                switch (画像種類)
                {
                    case 画像種類Enum.home: imagefilepath = "[image]=/etc/Icon/home.png";
                        break;

                    case 画像種類Enum.book1: imagefilepath = "[image]=/etc/Icon/book1.png";
                        break;

                    case 画像種類Enum.kinds1: imagefilepath = "[image]=/etc/Icon/kinds1.png";
                        break;

                    case 画像種類Enum.folder2: imagefilepath = "[image]=/etc/Icon/folder2.png";
                        break;

                    case 画像種類Enum.cross: imagefilepath = "[image]=/etc/Icon/cross.png";
                        break;

                    case 画像種類Enum.exclame: imagefilepath = "[image]=/etc/Icon/exclame.png";
                        break;

                    default: throw new Exception("画像の種類が存在しません。");

                }

                string datfilepath = Path.Combine(createFolderPath, "Thread.dat");
                using (StreamWriter sw = new StreamWriter(datfilepath))
                {
                    sw.WriteLine(imagefilepath);
                    sw.Flush();
                }

                return InputThreadName + " スレッドを追加しました。";
            }
            else if (操作タイプ == スレッド操作タイプ.修正)
            {

                // スレッド名に重複がないか確認。
                List<string> directorylist = Directory.GetDirectories(RootPath, "*", SearchOption.TopDirectoryOnly).ToList();
                foreach (var dir in directorylist)
                {
                    if (dir.Equals(InputThreadName))
                    {
                        return "同じ名前のスレッド名があります。ほかのスレッド名を入力して下さい。";
                    }

                }


                // 掲示板資料フォルダ内でスレッド名に重複がないか確認。
                List<string> directorylist2 = Directory.GetDirectories(掲示板資料RootPath, "*", SearchOption.TopDirectoryOnly).ToList();
                foreach (var dir in directorylist2)
                {
                    if (dir.Equals(InputThreadName))
                    {
                        return "掲示板資料フォルダに同じ名前のフォルダ名があります。ほかのスレッド名を入力して下さい。";
                    }

                }

                // スレッド名のフォルダを変更
                string str = ModifyDirectory(Path.Combine(RootPath, SourceName), Path.Combine(RootPath, InputThreadName));
                if (!str.Equals("フォルダを修正しました。"))
                {
                    return str;
                }

                // 掲示板資料のフォルダ名を変更
                string str2 = ModifyDirectory(Path.Combine(掲示板資料RootPath, SourceName), Path.Combine(掲示板資料RootPath, InputThreadName));
                if (!str2.Equals("フォルダを修正しました。"))
                {
                    return str2 + "\r\n" + InputThreadName + "スレッドに保存されているファイルの保存先が正常に変更できませんでした。\r\n管理者へご連絡下さい。";
                }

                string imagefilepath = "";

                switch (画像種類)
                {
                    case 画像種類Enum.home: imagefilepath = "[image]=/etc/Icon/home.png";
                        break;

                    case 画像種類Enum.book1: imagefilepath = "[image]=/etc/Icon/book1.png";
                        break;

                    case 画像種類Enum.kinds1: imagefilepath = "[image]=/etc/Icon/kinds1.png";
                        break;

                    case 画像種類Enum.folder2: imagefilepath = "[image]=/etc/Icon/folder2.png";
                        break;

                    case 画像種類Enum.cross: imagefilepath = "[image]=/etc/Icon/cross.png";
                        break;

                    case 画像種類Enum.exclame: imagefilepath = "[image]=/etc/Icon/exclame.png";
                        break;

                    default: throw new Exception("画像の種類が存在しません。");

                }

                string datfilepath = Path.Combine(Path.Combine(RootPath, InputThreadName), "Thread.dat");
                using (StreamWriter sw = new StreamWriter(datfilepath))
                {
                    sw.WriteLine(imagefilepath);
                    sw.Flush();
                }

                return "スレッドを修正しました。";

            }
            else if (操作タイプ == スレッド操作タイプ.削除)
            {
                DeleteDirectory(Path.Combine(RootPath, InputThreadName));
                return "スレッドを削除しました。";
            }
            else
            {
                throw new NotImplementedException("エラー：実装していないメソッドが呼び出されました。");
            }

        }





        #region クラス内部からの呼び出しメソッド

        /// <summary>
        /// テロップファイルの上書き
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="テロップ記事"></param>
        /// <returns></returns>
        public string OverrideOrCreateToTerop(string Path, string テロップ記事)
        {
            try
            {
#if DEBUG
                if (!System.IO.Directory.Exists(PharmacyTool.Web.Properties.Settings.Default.TeropRootPathDEBUG))
                {
                    System.IO.Directory.CreateDirectory(PharmacyTool.Web.Properties.Settings.Default.TeropRootPathDEBUG);
                }
#elif NAKAYAMA
                if (!System.IO.Directory.Exists(PharmacyTool.Web.Properties.Settings.Default.TeropRootPathNAKAYAMA))
                {
                    System.IO.Directory.CreateDirectory(PharmacyTool.Web.Properties.Settings.Default.TeropRootPathNAKAYAMA);
                }
#else
                if (!System.IO.Directory.Exists(PharmacyTool.Web.Properties.Settings.Default.TeropRootPath))
                {
                    System.IO.Directory.CreateDirectory(PharmacyTool.Web.Properties.Settings.Default.TeropRootPath);
                }
#endif

                using (StreamWriter sw = new StreamWriter(Path, false, Encoding.GetEncoding(932)))
                {
                    sw.WriteLine(テロップ記事);
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
                return e.Message + e.StackTrace;
            }


            return "テロップを編集しました。";
        }




        /// <summary>
        /// 新規作成（ディレクトリ）
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private string CreateDirectory(string Path)
        {
            try
            {
                if (System.IO.Directory.Exists(Path))
                {
                    return "フォルダが既に存在します。";
                }
            }
            catch (Exception e)
            {
                return e.Message + e.StackTrace;
            }


            System.IO.Directory.CreateDirectory(Path);

            return "フォルダを作成しました。";
        }


        /// <summary>
        /// 削除（ディレクトリ）
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private string DeleteDirectory(string Path)
        {
            try
            {
                System.IO.Directory.Delete(Path, true);
            }
            catch (Exception e)
            {
                return e.Message + e.StackTrace;
            }

            return "フォルダを削除しました。";
        }


        /// <summary>
        /// 削除（ファイル）
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private string DeleteFile(string Path)
        {
            try
            {
                System.IO.File.Delete(Path);
            }
            catch (Exception e)
            {
                return e.Message + e.StackTrace;
            }

            return "ファイルを削除しました。";
        }



        /// <summary>
        /// 修正（ディレクトリ）
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private string ModifyDirectory(string Path, string NewName)
        {
            try
            {
                if (System.IO.Directory.Exists(Path))
                {
                    if (System.IO.Directory.Exists(NewName))
                    {
                        return "既に存在するフォルダ名の為、修正出来ませんでした。";
                    }

                    System.IO.Directory.Move(Path, NewName);
                    return "フォルダを修正しました。";
                }
                else
                {
                    return "フォルダが存在しません。";
                }
            }
            catch (Exception e)
            {
                //return Path + "   " + NewName;
                return e.Message + e.StackTrace;
            }

        }


        /// <summary>
        /// 修正（ファイル）
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private string ModifyFile(string Path, string NewName)
        {
            try
            {
                if (System.IO.File.Exists(Path))
                {
                    System.IO.File.Move(Path, NewName);
                    return "ファイルを修正しました。";
                }
                else
                {
                    return "ファイルが存在しません。";
                }
            }
            catch (Exception e)
            {
                //return Path + "   " + NewName;
                return e.Message + e.StackTrace;
            }

        }

        #endregion

        /// <summary>
        /// メイン記事と添付資料を削除
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string メイン記事のみ削除(string filePath, bool 管理者か, string 暗証キー, string 記事No, string カテゴリ名)
        {
            try
            {
                // メイン記事添付の資料のパスを先に取得
                List<string> 掲示板資料ファイルパスリスト = new List<string>();
                暗証キーチェックと記事添付の資料パス取得メイン(filePath, 掲示板資料ファイルパスリスト, 管理者か ? false : true, 暗証キー);

                // メイン記事削除
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // メイン記事についていた資料を削除
                記事添付の資料削除メイン(掲示板資料ファイルパスリスト, 記事No, カテゴリ名);

            }
            catch (Exception exp)
            {
                return exp.Message + exp.StackTrace;
            }

            return "";
        }

        /// <summary>
        /// レス記事と添付資料を削除
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string レス記事のみ削除(string filePath, bool 管理者か, string 暗証キー, string 記事No, string カテゴリ名)
        {
            return メイン記事のみ削除(filePath, 管理者か, 暗証キー, 記事No, カテゴリ名);
        }

        /// <summary>
        /// メイン・レスの記事と添付資料を削除
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string メインレス両削除(string 記事RootPath, bool 管理者か, string 暗証キー, string 記事No, string カテゴリ名)
        {
            try
            {
                List<string> メイン記事添付資料ファイルリスト = new List<string>();
                Dictionary<int, string> レス記事添付資料ファイルリスト = new Dictionary<int, string>();
                string[] mainarray = System.IO.Directory.GetFiles(Path.Combine(記事RootPath, "メイン"));
                string[] resarray = System.IO.Directory.GetFiles(Path.Combine(記事RootPath, "レス"));

                // メイン記事の暗証と資料パス
                if (mainarray.Length != 0)
                {
                    string メイン記事Path = mainarray[0];
                    string s = 暗証キーチェックと記事添付の資料パス取得メイン(メイン記事Path, メイン記事添付資料ファイルリスト, 管理者か ? false : true, 暗証キー);

                    if (!s.Equals(""))
                    {
                        return s;
                    }
                }

                // レス記事の暗証と資料パス
                if (resarray.Length != 0)
                {
                    foreach (var resa in resarray)
                    {
                        string レス記事Path = resa;
                        // レスの方は、メインで暗証しているのでOK
                        string s2 = 暗証キーチェックと記事添付の資料パス取得レス(レス記事Path, レス記事添付資料ファイルリスト, false, 暗証キー);
                        if (!s2.Equals(""))
                        {
                            return s2;
                        }
                    }
                }

                // 記事RootPathを削除
                if (System.IO.Directory.Exists(記事RootPath))
                {
                    System.IO.Directory.Delete(記事RootPath, true);
                }

                string message = "";


                message += 記事添付の資料削除メイン(メイン記事添付資料ファイルリスト, 記事No, カテゴリ名);

                message += 記事添付の資料削除レス(レス記事添付資料ファイルリスト, カテゴリ名);

                return message;



            }
            catch (Exception exp)
            {
                return exp.Message + exp.StackTrace;
            }

        }


        /// <summary>
        /// メイン記事に添付されていた資料のパスを取得
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string 暗証キーチェックと記事添付の資料パス取得レス(string filePath, Dictionary<int, string> 掲示板資料ファイルパスリスト, bool 暗証キーチェックするか, string 暗証キー)
        {

            using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("Shift_JIS")))
            {
                string line = "";
                int counter = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    // １行目か
                    if (counter == 1)
                    {
                        // 管理者の場合は暗証チェックをスルー
                        if (!暗証キーチェックするか)
                        {
                            counter++;
                            continue;
                        }
                        else
                        {
                            // [,]区切りで7番目がパスワード
                            string[] sepa = line.Split(',');
                            if (sepa.Length != 7)
                            {
                                return "記事データが壊れている為、削除できませんでした。";
                            }
                            string pass = sepa[5];

                            // 暗証キーがない場合は削除可能
                            if (pass.Equals(""))
                            {
                                counter++;
                                continue;
                            }

                            if (pass.Equals(暗証キー))
                            {
                                counter++;
                                continue;
                            }
                            else
                            {
                                return "暗証キーが異なる為、削除できませんでした。";
                            }
                        }
                    }
                    else if (counter == 2)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            return "";
                        }

                        if (line.Length < 12)
                        {
                            return "指定の記事ファイルの画像パスが壊れている為、削除できませんでした。";
                        }

                        string removedStr = line.Substring(12);
                        if (!removedStr.Equals(""))
                        {
                            string[] sepa = removedStr.Split(',');

                            string[] spl = filePath.Split('\\');
                            string[] preNo = spl[spl.Length - 1].Split('.');
                            int 記事No;
                            if (int.TryParse(preNo[preNo.Length - 2], out 記事No) == false)
                            {
                                return "レス記事Noが正しくない為、削除できませんでした。";
                            }

                            foreach (var path in sepa)
                            {
                                掲示板資料ファイルパスリスト.Add(記事No, path);
                                // フォルダごと削除する為、１つでOK
                                break;
                            }
                        }

                        return "";
                    }
                }

                return "";
            }

        }

        /// <summary>
        /// メイン記事に添付されていた資料のパスを取得
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string 暗証キーチェックと記事添付の資料パス取得メイン(string filePath, List<string> 掲示板資料ファイルパスリスト, bool 暗証キーチェックするか, string 暗証キー)
        {

            using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("Shift_JIS")))
            {
                string line = "";
                int counter = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    // １行目か
                    if (counter == 1)
                    {
                        // 管理者の場合は暗証チェックをスルー
                        if (!暗証キーチェックするか)
                        {
                            counter++;
                            continue;
                        }
                        else
                        {
                            // [,]区切りで7番目がパスワード
                            string[] sepa = line.Split(',');
                            if (sepa.Length != 7)
                            {
                                return "記事データが壊れている為、削除できませんでした。";
                            }
                            string pass = sepa[5];

                            // 暗証キーがない場合は削除可能
                            if (pass.Equals(""))
                            {
                                counter++;
                                continue;
                            }

                            if (pass.Equals(暗証キー))
                            {
                                counter++;
                                continue;
                            }
                            else
                            {
                                return "暗証キーが異なる為、削除できませんでした。";
                            }
                        }
                    }
                    else if (counter == 2)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            return "";
                        }

                        if (line.Length < 12)
                        {
                            return "指定の記事ファイルの画像パスが壊れている為、削除できませんでした。";
                        }

                        string removedStr = line.Substring(12);
                        if (!removedStr.Equals(""))
                        {
                            string[] sepa = removedStr.Split(',');

                            foreach (var path in sepa)
                            {
                                掲示板資料ファイルパスリスト.Add(path);
                            }
                        }

                        return "";
                    }
                }

                return "";
            }

        }


        /// <summary>
        /// メイン記事に添付されていた資料を削除
        /// </summary>
        /// <param name="掲示板資料ファイルパスリスト"></param>
        /// <returns></returns>
        private string 記事添付の資料削除メイン(List<string> 掲示板資料ファイルパスリスト, string 記事No, string カテゴリ名)
        {
            foreach (var リスト in 掲示板資料ファイルパスリスト)
            {

#if DEBUG
                string prepath = Path.Combine(PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathDEBUG, カテゴリ名);
                string pre = Path.Combine(prepath, 記事No);
                // string delete資料パス = Path.Combine(pre, リスト);
#elif NAKAYAMA

                string prepath = Path.Combine(PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathNAKAYAMA, カテゴリ名);
                string pre = Path.Combine(prepath, 記事No);
                // string delete資料パス = Path.Combine(prepath, リスト);

#else 
                string prepath = Path.Combine(PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPath, カテゴリ名);
                string pre = Path.Combine(prepath, 記事No);
                // string delete資料パス = Path.Combine(prepath, リスト);
#endif
                if (System.IO.Directory.Exists(pre))
                {
                    System.IO.Directory.Delete(pre, true);
                }
            }

            return "";

        }


        /// <summary>
        /// レス記事に添付されていた資料を削除
        /// </summary>
        /// <param name="掲示板資料ファイルパスリスト"></param>
        /// <returns></returns>
        private string 記事添付の資料削除レス(Dictionary<int, string> 掲示板資料ファイルパスリスト, string カテゴリ名)
        {
            foreach (var リスト in 掲示板資料ファイルパスリスト)
            {

#if DEBUG
                string prepath = Path.Combine(PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathDEBUG, カテゴリ名);
                string pre = Path.Combine(prepath, リスト.Key.ToString());
                // string delete資料パス = Path.Combine(pre, リスト.Value);
#elif NAKAYAMA

                string prepath = Path.Combine(PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPathNAKAYAMA, カテゴリ名);
                string pre = Path.Combine(prepath,リスト.Key.ToString());
                // string delete資料パス = Path.Combine(pre, リスト.Value);

#else 

                string prepath = Path.Combine(PharmacyTool.Web.Properties.Settings.Default.Upload掲示板FileRootPath, カテゴリ名);
                string pre = Path.Combine(prepath,リスト.Key.ToString());
                // string delete資料パス = Path.Combine(pre, リスト.Value);

#endif
                if (System.IO.Directory.Exists(pre))
                {
                    System.IO.Directory.Delete(pre, true);
                }
            }

            return "";

        }



        private int 記事No8ケタ文字列to数字(string 記事No)
        {
            string no = 記事No.Substring(0, 8);
            while (no.Substring(0, 1).Equals("0"))
            {
                no = no.Remove(0, 1);
            }

            return int.Parse(no);
        }

    }

    public static class ApplicationScope
    {
        // キューに追加するのは、this.GetHashCode()
        private static Queue<int> _掲示板書込みキュー = new Queue<int>();

        public static Queue<int> 掲示板書込みキュー
        {
            get { return ApplicationScope._掲示板書込みキュー; }
            set { ApplicationScope._掲示板書込みキュー = value; }
        }
    }

    public enum 画像種類Enum
    {
        画像なし = -1,
        home = 0,
        book1 = 1,
        kinds1 = 2,
        folder2 = 3,
        cross = 4,
        exclame = 5
    }

    public enum ディレクトリ操作モード
    {
        Create = 0,
        Delete = 1,
        Rename = 2
    }

    public enum タイプ
    {
        ディレクトリ = 0,
        ファイル = 1
    }

    public enum 文字色
    {
        赤 = 0,
        茶色 = 1,
        緑 = 2,
        青 = 3,
        紫 = 4,
        ピンク = 5,
        オレンジ = 6,
        黒 = 7
    }

    public enum 掲示板書込タイプ
    {
        新規投稿 = 0,
        記事修正 = 1,
        返信投稿 = 2
    }

    #region 掲示板結果リターンEntity

    public class 掲示板結果リターンEntity
    {
        private bool _書込成功か;

        public bool 書込成功か
        {
            get { return _書込成功か; }
            set { _書込成功か = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }

        private string _書込み記事No;

        public string 書込み記事No
        {
            get { return _書込み記事No; }
            set { _書込み記事No = value; }
        }


        private string _書込みカテゴリ;

        public string 書込みカテゴリ
        {
            get { return _書込みカテゴリ; }
            set { _書込みカテゴリ = value; }
        }
    }

    public enum スレッド操作タイプ
    {
        新規 = 0,
        修正 = 1,
        削除 = 2

    }

    #endregion


}
