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

namespace View.Core.共通
{
    public static class Settings
    {
#if DEBUG
        public const string ClientBinRootPath = @"C:\PharmacyTools\ClientBin\";
        public const string TeropFilePath = @"C:\PharmacyTools\ClientBin\テロップ\Terop.dat";
        public const string UploadFileRootPath = @"C:\PharmacyTools\ClientBin\資料\";
        public const string Upload掲示板FileRootPath = @"C:\PharmacyTools\ClientBin\掲示板資料\";
        public const string GenericHandlerPath = @"http://localhost:56305/GenericHandler/FileUpload.ashx";
        public const string TopPageRoot = @"http://localhost:49107/";
        public const string Error401Path = @"http://localhost:49107/Error401.html";
        public const string ZaikoGenericHandlerPath = @"http://localhost:49107/GenericHandler/在庫データFileUpload.ashx";
        //public const string TopPageRoot = @"http://localhost:56305/";
        //public const string Error401Path = @"http://localhost:56305/Error401.html";
        //public const string ZaikoGenericHandlerPath = @"http://localhost:56305/GenericHandler/在庫データFileUpload.ashx";

        // Http Basic
        public const string BasicID = "";
        public const string BasicCredent = "";

#elif NAKAYAMA

        public const string ClientBinRootPath = @"D:\inetpub\vhosts\kusurinonakayama.jp\httpdocs\PharmacyTool\ClientBin\";
        public const string ServiceRootPath = @"D:\inetpub\vhosts\kusurinonakayama.jp\httpdocs\PharmacyTool\";
        public const string TeropFilePath = @"D:\inetpub\vhosts\kusurinonakayama.jp\httpdocs\PharmacyTool\ClientBin\テロップ\Terop.dat";
        public const string UploadFileRootPath = @"D:\inetpub\vhosts\kusurinonakayama.jp\httpdocs\PharmacyTool\ClientBin\資料\";
        public const string Upload掲示板FileRootPath = @"D:\inetpub\vhosts\kusurinonakayama.jp\httpdocs\PharmacyTool\ClientBin\掲示板資料\";
        public const string GenericHandlerPath = @"http://www.kusurinonakayama.jp/PharmacyTool/GenericHandler/FileUpload.ashx";
        public const string TopPageRoot = @"http://www.kusurinonakayama.jp/";
        public const string Error401Path = @"http://www.kusurinonakayama.jp/PharmacyTool/Error401.html";
        public const string ZaikoGenericHandlerPath = @"http://www.kusurinonakayama.jp/PharmacyTool/GenericHandler/在庫データFileUpload.ashx";

        // Http Basic
        public const string BasicID = "nakayama";
        public const string BasicCredent = "honnmono";

#else
        public const string ClientBinRootPath = @"D:\inetpub\vhosts\my-world.me\httpdocs\PharmacyTool\ClientBin\";
        public const string ServiceRootPath = @"D:\inetpub\vhosts\my-world.me\httpdocs\PharmacyTool\";
        public const string TeropFilePath = @"D:\inetpub\vhosts\my-world.me\httpdocs\PharmacyTool\ClientBin\テロップ\Terop.dat";
        public const string UploadFileRootPath = @"D:\inetpub\vhosts\my-world.me\httpdocs\PharmacyTool\ClientBin\資料\";
        public const string Upload掲示板FileRootPath = @"D:\inetpub\vhosts\my-world.me\httpdocs\PharmacyTool\ClientBin\掲示板資料\";
        public const string GenericHandlerPath = @"http://www.my-world.me/PharmacyTool/GenericHandler/FileUpload.ashx";
        public const string TopPageRoot = @"http://www.my-world.me/";
        public const string Error401Path = @"http://www.my-world.me/PharmacyTool/Error401.html";
        public const string ZaikoGenericHandlerPath = @"http://www.my-world.me/PharmacyTool/GenericHandler/在庫データFileUpload.ashx";

        // Http Basic
        public const string BasicID = "poohace";
        public const string BasicCredent = "cbqb22";

#endif

    }
}
