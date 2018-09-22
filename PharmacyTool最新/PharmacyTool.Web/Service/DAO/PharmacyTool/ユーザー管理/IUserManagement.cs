using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmacyTool.Web.Service.DAO.PharmacyTool.ユーザー管理
{
    // メモ: ここでインターフェイス名 "UserManagement" を変更する場合は、Web.config で "UserManagement" への参照も更新する必要があります。
    [ServiceContract]
    public interface IUserManagement
    {
        [OperationContract]
        CreateNewUser結果 CreateNewUser実行(string UserID, string Confidential, int アクセス権限);

        [OperationContract]
        DeleteUser結果 DeleteUser実行(string UserID);

        [OperationContract]
        List<AllUser情報取得結果> AllUser情報取得実行();

        [OperationContract]
        string AllUser情報更新実行(List<AllUser情報取得結果> list);

        [OperationContract]
        string LoginUser情報更新実行(AllUser情報取得結果 lu);

    }
}
