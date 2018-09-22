using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;


namespace PharmacyTool.Web.Service.File.TreeView
{
    public class TreeViewManagerServiceHostFactory : ServiceHostFactory
    {

#if DEBUG
#elif NAKAYAMA
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost result = base.CreateServiceHost(serviceType,
                new Uri[] { new Uri(PharmacyTool.Web.Properties.Settings.Default.ServerHttpRootNAKAYAMA + "PharmacyTool/Service/File/TreeView/TreeViewManager.svc") });
            return result;
        }

#else
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost result = base.CreateServiceHost(serviceType,
                new Uri[] { new Uri(PharmacyTool.Web.Properties.Settings.Default.ServerHttpRoot +  "PharmacyTool/Service/File/TreeView/TreeViewManager.svc")});
            return result;
        }
#endif


    }
}
