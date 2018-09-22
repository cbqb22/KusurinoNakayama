using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmacyTool.Web.Service.File.TreeView
{
    // メモ: ここでインターフェイス名 "ITreeViewManager" を変更する場合は、Web.config で "ITreeViewManager" への参照も更新する必要があります。
    [ServiceContract]
    public interface ITreeViewManager
    {
        [OperationContract]
        List<TreeViewItemData> Create資料TreeView();
    }
}
