using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmacyTool.Web.Service.File.Reader
{
    // メモ: ここでインターフェイス名 "IFileReader" を変更する場合は、Web.config で "IFileReader" への参照も更新する必要があります。
    [ServiceContract]
    public interface IFileReader
    {
        [OperationContract]
        List<現在庫データ> OpenCSV(string Filepath);

        [OperationContract]
        在庫リターンデータセット<薬局使用量データ> Open使用量CSV(string 検索文字列, bool 全期間, int 期限加算月);

        [OperationContract]
        在庫リターンデータセット<薬局使用量データ> Open使用量2CSV(string 検索文字列, bool 全期間, int 期限加算月);

        [OperationContract]
        在庫リターンデータセット<不動品データ> Open不動品CSV(string 検索文字列, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月);

        [OperationContract]
        在庫リターンデータセット<現在庫データ> Get現在庫検索データ(string 検索ワード, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月);

        [OperationContract]
        在庫リターンデータセット<現在庫データ> Get後発品検索データ(string YJコード, bool 全期限, bool 期限内, bool 期限切, bool 期限指定か, bool 以内指定か, int 期限加算月, bool 他規格・剤形も表示する);

        [OperationContract]
        掲示板リターンデータセット Open掲示板データ(string カテゴリ名, int グループNo);

        [OperationContract]
        掲示板リターンデータセット Open掲示板データキーワード検索(string キーワード, string カテゴリ名);

        [OperationContract]
        掲示板記事修正確認Entity 掲示板記事修正確認チェック(string カテゴリ名, string 記事No, string 暗証キー, bool 管理者か);


        [OperationContract]
        List<ThreadTitlesEntity> GetThreadTitles();

        [OperationContract]
        テロップ記事リターンEntity テロップ記事読み込み();

        [OperationContract]
        在庫MergeリターンEntity DoMerge(MergeType タイプ);
        
        [OperationContract]
        最終更新日時リターンデータセット GetMEDISデータ最終更新日時();
    }
}
