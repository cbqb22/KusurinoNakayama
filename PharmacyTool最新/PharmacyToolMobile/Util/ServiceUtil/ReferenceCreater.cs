using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using PharmacyToolMobile.Service.DAO.PharmacyTool;



namespace PharmacyToolMobile.Util.ServiceUtil
{
    public class ReferenceCreater
    {
        public static Service.File.Reader.FileReaderClient GetFileReaderClient()
        {
#if DEBUG

        //Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Reader/FileReader.svc");
        Uri svcFileReaderUri = new Uri("http://localhost:56305/Service/File/Reader/FileReader.svc");


#elif NAKAYAMA

            //Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Reader/FileReader.svc");
            Uri svcFileReaderUri = new Uri("http://www.kusurinonakayama.jp/PharmacyTool/Service/File/Reader/FileReader.svc");

#else

        //Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Reader/FileReader.svc");
        Uri svcFileReaderUri = new Uri("http://localhost:56305/Service/File/Reader/FileReader.svc");

#endif


            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            EndpointAddress endpoint = new EndpointAddress(svcFileReaderUri);
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;


            Service.File.Reader.FileReaderClient client = new Service.File.Reader.FileReaderClient(binding, endpoint);
            client.ClientCredentials.UserName.UserName = "nakayama";
            client.ClientCredentials.UserName.Password = "honnmono";

            //BasicHttpBinding binding = new BasicHttpBinding();
            //binding.MaxReceivedMessageSize = int.MaxValue;
            //binding.MaxBufferSize = int.MaxValue;
            //Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Reader/FileReader.svc");
            //EndpointAddress endpoint = new EndpointAddress(svcUri);

            //Service.File.Reader.FileReaderClient client = new Service.File.Reader.FileReaderClient(binding, endpoint);

            return client;

            //BasicHttpBinding binding = new BasicHttpBinding(
            //    Application.Current.Host.Source.Scheme.Equals("https", StringComparison.InvariantCultureIgnoreCase)
            //    ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);
            //binding.MaxReceivedMessageSize = int.MaxValue;
            //binding.MaxBufferSize = int.MaxValue;
            //return new Service.File.Reader.FileReaderClient(binding, new EndpointAddress(
            //    new Uri(Application.Current.Host.Source, "../Service/File/Reader/FileReader.svc")));
        }
        public static Service.File.Writer.FileWriterClient GetFileWriterClient()
        {

#if DEBUG

            //Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Writer/FileWriter.svc");
            Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/File/Writer/FileWriter.svc");


#elif NAKAYAMA

            //Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Writer/FileWriter.svc");
            Uri svcFileWriterUri = new Uri("http://www.kusurinonakayama.jp/PharmacyTool/Service/File/Writer/FileWriter.svc");

#else

        //Uri svcUri = new Uri(Application.Current.Host.Source, "../Service/File/Writer/FileWriter.svc");
        Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/File/Writer/FileWriter.svc");

#endif


            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            EndpointAddress endpoint = new EndpointAddress(svcFileWriterUri);
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;


            Service.File.Writer.FileWriterClient client = new Service.File.Writer.FileWriterClient(binding, endpoint);
            client.ClientCredentials.UserName.UserName = "nakayama";
            client.ClientCredentials.UserName.Password = "honnmono";


            return client;

        }
        public static Service.DAO.PharmacyTool.店舗.StoreDataClient GetStoreDataClient()
        {

#if DEBUG

            Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/DAO/PharmacyTool/店舗/StoreData.svc");


#elif NAKAYAMA

            Uri svcFileWriterUri = new Uri("http://www.kusurinonakayama.jp/PharmacyTool/Service/DAO/PharmacyTool/店舗/StoreData.svc");

#else

            Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/DAO/PharmacyTool/店舗/StoreData.svc");

#endif


            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            EndpointAddress endpoint = new EndpointAddress(svcFileWriterUri);
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;


            Service.DAO.PharmacyTool.店舗.StoreDataClient client = new Service.DAO.PharmacyTool.店舗.StoreDataClient(binding, endpoint);
            client.ClientCredentials.UserName.UserName = "nakayama";
            client.ClientCredentials.UserName.Password = "honnmono";


            return client;

        }
        public static Service.DAO.PharmacyTool.LoginCheckClient GetログインチェックClient()
        {

#if DEBUG

            Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/DAO/PharmacyTool/LoginCheck.svc");


#elif NAKAYAMA

            Uri svcFileWriterUri = new Uri("http://www.kusurinonakayama.jp/PharmacyTool/Service/DAO/PharmacyTool/LoginCheck.svc");

#else

            Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/DAO/PharmacyTool/LoginCheck.svc");

#endif


            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            EndpointAddress endpoint = new EndpointAddress(svcFileWriterUri);
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;


            LoginCheckClient client = new LoginCheckClient(binding, endpoint);
            client.ClientCredentials.UserName.UserName = "nakayama";
            client.ClientCredentials.UserName.Password = "honnmono";


            return client;

        }


        public static Service.DAO.PharmacyTool.アクセス数管理.AccessManagementClient GetAccessManagementClient()
        {

#if DEBUG

            Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/DAO/PharmacyTool/アクセス数管理/AccessManagement.svc");


#elif NAKAYAMA

            Uri svcFileWriterUri = new Uri("http://www.kusurinonakayama.jp/PharmacyTool/Service/DAO/PharmacyTool/アクセス数管理/AccessManagement.svc");

#else

            Uri svcFileWriterUri = new Uri("http://localhost:56305/Service/DAO/PharmacyTool/アクセス数管理/AccessManagement.svc");

#endif


            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            EndpointAddress endpoint = new EndpointAddress(svcFileWriterUri);
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;


            Service.DAO.PharmacyTool.アクセス数管理.AccessManagementClient client = new Service.DAO.PharmacyTool.アクセス数管理.AccessManagementClient(binding, endpoint);
            client.ClientCredentials.UserName.UserName = "nakayama";
            client.ClientCredentials.UserName.Password = "honnmono";

            return client;
        }


    }
}