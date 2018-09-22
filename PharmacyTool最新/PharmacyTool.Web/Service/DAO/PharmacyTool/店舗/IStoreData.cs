using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmacyTool.Web.Service.DAO.PharmacyTool.店舗
{
    // メモ: ここでインターフェイス名 "IStoreData" を変更する場合は、Web.config で "IStoreData" への参照も更新する必要があります。
    [ServiceContract]
    public interface IStoreData
    {
        [OperationContract]
        List<PT店舗名リターンデータセット> 店舗名取得();

        [OperationContract]
        string 新規店舗名作成(string 作成店舗名);

        [OperationContract]
        string 店舗名削除(string 削除店舗名);
    }
}
