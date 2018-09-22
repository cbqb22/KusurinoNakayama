using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace PharmacyTool.Web.Service.File.Writer
{
    // メモ: ここでインターフェイス名 "IFileWriter" を変更する場合は、Web.config で "IFileWriter" への参照も更新する必要があります。
    [ServiceContract]
    public interface IFileWriter
    {
        [OperationContract]
        string FileWriter実行(string SendPath, ディレクトリ操作モード mode, タイプ type, string NewName);

        [OperationContract]
        掲示板結果リターンEntity 掲示板データ書込(string カテゴリ名, string タイトル, string 投稿者, string 記事, 掲示板書込タイプ タイプ, string 記事No, string HomepageUrl, string Email, List<string> 添付画像パスリスト, string 暗証キー, 文字色 文字色);

        [OperationContract]
        string 掲示板データ削除(string カテゴリ名, string 記事No, string 暗証キー, bool 管理者か);

        [OperationContract]
        string WriteThreadTitles(string InputThreadName, 画像種類Enum 画像種類, スレッド操作タイプ 操作タイプ, string SourceName);

        [OperationContract]
        string テロップ変更実行(string テロップ記事);

    }
}
