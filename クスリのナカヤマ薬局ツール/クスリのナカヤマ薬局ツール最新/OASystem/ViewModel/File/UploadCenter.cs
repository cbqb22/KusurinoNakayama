﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using OASystem.Common;

namespace OASystem.ViewModel.File
{
    public static class UploadCenter
    {
        public static void Upload帳合先チェックマスタ_メーカー別(string sourceFilePath)
        {
            string uri = Settings.UploadPath帳合先チェックマスタメーカー別CSV;
            string myFile = sourceFilePath;

            WebRequest req = WebRequest.Create(uri);
            NetworkCredential nc = new NetworkCredential(OASystem.Common.Settings.FtpId, OASystem.Common.Settings.FtpCredential);
            req.Credentials = nc;
            req.Method = WebRequestMethods.Ftp.UploadFile;
            using (Stream st = req.GetRequestStream())
            using (FileStream fs = new FileStream(myFile, FileMode.Open))
            {
                Byte[] buf = new Byte[1024];
                int count = 0;

                do
                {
                    count = fs.Read(buf, 0, buf.Length);
                    st.Write(buf, 0, count);
                } while (count != 0);
            }


        }

        public static void Upload帳合先チェックマスタ_医薬品別(string sourceFilePath)
        {
            string uri = OASystem.Common.Settings.UploadPath帳合先チェックマスタ医薬品別CSV;
            string myFile = sourceFilePath;

            WebRequest req = WebRequest.Create(uri);
            NetworkCredential nc = new NetworkCredential(OASystem.Common.Settings.FtpId, OASystem.Common.Settings.FtpCredential);
            req.Credentials = nc;
            req.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream st = req.GetRequestStream())
            using (FileStream fs = new FileStream(myFile, FileMode.Open))
            {
                Byte[] buf = new Byte[1024];
                int count = 0;

                do
                {
                    count = fs.Read(buf, 0, buf.Length);
                    st.Write(buf, 0, count);
                } while (count != 0);
            }

        }


        public static void UploadMEDIS_HOT13(string sourceFilePath)
        {
            string uri = OASystem.Common.Settings.UploadMEDIS_HOT13FilePath;
            string myFile = sourceFilePath;

            WebRequest req = WebRequest.Create(uri);
            NetworkCredential nc = new NetworkCredential(OASystem.Common.Settings.FtpId, OASystem.Common.Settings.FtpCredential);
            req.Credentials = nc;
            req.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream st = req.GetRequestStream())
            using (FileStream fs = new FileStream(myFile, FileMode.Open))
            {
                Byte[] buf = new Byte[1024];
                int count = 0;

                do
                {
                    count = fs.Read(buf, 0, buf.Length);
                    st.Write(buf, 0, count);
                } while (count != 0);
            }


        }


        public static void Upload保護リスト(string sourceFilePath,string 自店舗名)
        {
            string uri = OASystem.Common.Settings.UploadFolderPath保護リスト + "/" + 自店舗名 + ".csv";
            string myFile = sourceFilePath;

            WebRequest req = WebRequest.Create(uri);
            NetworkCredential nc = new NetworkCredential(OASystem.Common.Settings.FtpId, OASystem.Common.Settings.FtpCredential);
            req.Credentials = nc;
            req.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream st = req.GetRequestStream())
            using (FileStream fs = new FileStream(myFile, FileMode.Open))
            {
                Byte[] buf = new Byte[1024];
                int count = 0;

                do
                {
                    count = fs.Read(buf, 0, buf.Length);
                    st.Write(buf, 0, count);
                } while (count != 0);
            }


        }

        public static void Upload優先移動リスト(string sourceFilePath, string 自店舗名)
        {
            string uri = OASystem.Common.Settings.UploadFolderPath優先移動リスト + "/" + 自店舗名 + ".csv";
            string myFile = sourceFilePath;

            WebRequest req = WebRequest.Create(uri);
            NetworkCredential nc = new NetworkCredential(OASystem.Common.Settings.FtpId, OASystem.Common.Settings.FtpCredential);
            req.Credentials = nc;
            req.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream st = req.GetRequestStream())
            using (FileStream fs = new FileStream(myFile, FileMode.Open))
            {
                Byte[] buf = new Byte[1024];
                int count = 0;

                do
                {
                    count = fs.Read(buf, 0, buf.Length);
                    st.Write(buf, 0, count);
                } while (count != 0);
            }


        }

    }
}
