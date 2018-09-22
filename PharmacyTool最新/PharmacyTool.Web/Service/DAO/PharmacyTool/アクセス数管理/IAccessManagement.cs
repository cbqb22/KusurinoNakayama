using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmacyTool.Web.Service.DAO.PharmacyTool.アクセス数管理
{
    // メモ: ここでインターフェイス名 "ILoginCheck" を変更する場合は、Web.config で "ILoginCheck" への参照も更新する必要があります。
    [ServiceContract]
    public interface IAccessManagement
    {
        [OperationContract]
        アクセス数取得結果 Doアクセス数カウントアップ取得();
    }
}
