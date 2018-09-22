using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Media.Imaging;
using SW = System.Windows;
using System.Windows.Media;




namespace MCSystem.ViewModel.Common.Image
{
    public static class BitmapChecker
    {
        public static byte[] GetBitmapMD5Hash(Bitmap bmp)
        {
            var bmpdata =  bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpdata.Scan0;
            int bytes = bmpdata.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);
            byte[] bmpHash = new MD5CryptoServiceProvider().ComputeHash(rgbValues);

            return bmpHash;
        }

        /// <summary>
        /// BitmapからBitmapSourceへ変換
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            IntPtr intPtr = bitmap.GetHbitmap();
            BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromEmptyOptions();
            BitmapSource bmpSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, SW.Int32Rect.Empty, sizeOptions);
            bmpSrc.Freeze();

            return bmpSrc;
        }

        /// <summary>
        /// 指定した範囲のスクリーンのキャプチャーをBitmapで取得
        /// </summary>
        /// <param name="captureRect"></param>
        public static byte[] CaptureScreenAndGetMD5(SW.Rect captureRect)
        {
            // スクリーンイメージの取得
            using (var bmp = new System.Drawing.Bitmap((int)captureRect.Width, (int)captureRect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            using (var graph = System.Drawing.Graphics.FromImage(bmp))
            {
                // 画面をコピーする
                graph.CopyFromScreen(new System.Drawing.Point((int)captureRect.X, (int)captureRect.Y), new System.Drawing.Point(), bmp.Size);

                return BitmapChecker.GetBitmapMD5Hash(bmp);
            }
        }

        public static bool CheckByteArraysEqual(byte[] first, byte[] second)
        {
            if (first.Count() != second.Count())
            {
                return false;
            }

            for (int i = 0; i < first.Count(); i++)
            {
                if (first[i] != second[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 指定した範囲のスクリーンのキャプチャーのBitmapを指定のパスで保存
        /// </summary>
        /// <param name="captureRect"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool CaptureScreenAndSave(SW.Rect captureRect, string savePath)
        {
            var sepa = savePath.Split('\\');
            if (2 <= sepa.Count())
            {
                string folderpath = savePath.Replace(sepa[sepa.Count() - 1], "");

                if (!System.IO.Directory.Exists(folderpath))
                {
                    System.IO.Directory.CreateDirectory(folderpath);
                }
            }



            // スクリーンイメージの取得
            try
            {
                using (var bmp = new System.Drawing.Bitmap((int)captureRect.Width, (int)captureRect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                using (var graph = System.Drawing.Graphics.FromImage(bmp))
                {
                    // 画面をコピーする
                    graph.CopyFromScreen(new System.Drawing.Point((int)captureRect.X, (int)captureRect.Y), new System.Drawing.Point(), bmp.Size);

                    bmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                return true;
            }
            catch (Exception ex)
            {
                //CsvWriter.OutputError(ex);
                return false;
            }
        }




    }
}
