using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmacyTool.Web.Service.DAO.PharmacyTool
{
    // メモ: ここでインターフェイス名 "ILoginCheck" を変更する場合は、Web.config で "ILoginCheck" への参照も更新する必要があります。
    [ServiceContract]
    public interface ILoginCheck
    {
        [OperationContract]
        List<ログインチェック結果> LoginCheck実行(string 入力ユーザーID, string 入力コンフィデンシャル);
    }
}
