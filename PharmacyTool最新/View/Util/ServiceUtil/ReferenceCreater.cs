using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;


namespace View.Util.ServiceUtil
{
    public class ReferenceCreater
    {
        public static Service.File.Reader.FileReaderClient GetFileReaderClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Reader/FileReader.svc");
            EndpointAddress endpoint = new EndpointAddress(svcUri);

            Service.File.Reader.FileReaderClient client = new Service.File.Reader.FileReaderClient(binding, endpoint);

            return client;
 
            //BasicHttpBinding binding = new BasicHttpBinding(
            //    Application.Current.Host.Source.Scheme.Equals("https", StringComparison.InvariantCultureIgnoreCase)
            //    ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);
            //binding.MaxReceivedMessageSize = int.MaxValue;
            //binding.MaxBufferSize = int.MaxValue;
            //return new Service.File.Reader.FileReaderClient(binding, new EndpointAddress(
            //    new Uri(Application.Current.Host.Source, "../Service/File/Reader/FileReader.svc")));
        }

        public static Service.DAO.PharmacyTool.LoginCheckClient GetログインチェックClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/DAO/PharmacyTool/LoginCheck.svc");
            EndpointAddress endpoint = new EndpointAddress(svcUri);

            Service.DAO.PharmacyTool.LoginCheckClient client = new Service.DAO.PharmacyTool.LoginCheckClient(binding, endpoint);

            return client;

        }


        public static Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient GetUserManagementClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/DAO/PharmacyTool/ユーザー管理/UserManagement.svc");
            EndpointAddress endpoint = new EndpointAddress(svcUri);

            Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient client = new Service.DAO.PharmacyTool.ユーザー管理.UserManagementClient(binding, endpoint);

            return client;
        }


        public static Service.DAO.PharmacyTool.アクセス数管理.AccessManagementClient GetAccessManagementClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/DAO/PharmacyTool/アクセス数管理/AccessManagement.svc");
            EndpointAddress endpoint = new EndpointAddress(svcUri);

            Service.DAO.PharmacyTool.アクセス数管理.AccessManagementClient client = new Service.DAO.PharmacyTool.アクセス数管理.AccessManagementClient(binding, endpoint);

            return client;
        }





        public static Service.File.TreeView.TreeViewManagerClient GetTreeViewManagerClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/TreeView/TreeViewManager.svc");
            EndpointAddress endpoint = new EndpointAddress(svcUri);

            Service.File.TreeView.TreeViewManagerClient client = new Service.File.TreeView.TreeViewManagerClient(binding, endpoint);

            return client;

        }


        public static Service.File.Writer.FileWriterClient GetFileWriterClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Writer/FileWriter.svc");
            EndpointAddress endpoint = new EndpointAddress(svcUri);

            Service.File.Writer.FileWriterClient client = new Service.File.Writer.FileWriterClient(binding, endpoint);

            return client;

        }


        public static Service.DAO.PharmacyTool.店舗.StoreDataClient GetStoreDataClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/DAO/PharmacyTool/店舗/StoreData.svc");
            EndpointAddress endpoint = new EndpointAddress(svcUri);

            Service.DAO.PharmacyTool.店舗.StoreDataClient client = new Service.DAO.PharmacyTool.店舗.StoreDataClient(binding, endpoint);

            return client;

        }



    }
}
