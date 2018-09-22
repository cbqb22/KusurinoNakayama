using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;
using System.Drawing;
using MCSystem.ViewModel.Common.Image;
using MCSystem.Model;

namespace MCSystem.ViewModel
{

    // Win32APIを呼び出すためのクラス
    public class win32api
    {
        [DllImport("user32.dll")]
        public static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    }

    public static class BCMacroRoutines
    {


        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void SetCursorPos(int X, int Y);


        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;

        public static void TestClickPoint()
        {
            SetCursorPos(590, 150);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void ClickPoint(Rect 座標)
        {
            SetCursorPos((int)座標.X, (int)座標.Y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// 軌跡をゆっくりドラッグ
        /// </summary>
        /// <param name="dm"></param>
        public static void Drag(DragMeasure dm)
        {
            SetCursorPos((int)dm.StartX, (int)dm.StartY);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(1500);

            int divide = 200;
            for (int i = 0; i < divide; i++)
            {
                var x1 = (dm.EndX - dm.StartX) / divide;
                var y1 = (dm.EndY - dm.StartY) / divide;

                var addx = x1 * (i + 1);
                var addy = y1 * (i + 1);
                SetCursorPos((int)dm.StartX + (int)addx,(int)dm.StartY + (int)addy);
                Thread.Sleep(20);

            }

            //SetCursorPos((int)dm.EndX, (int)dm.EndY);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// 軌跡をすばやくドラッグ
        /// </summary>
        /// <param name="dm"></param>
        public static void DragFast(DragMeasure dm)
        {
            SetCursorPos((int)dm.StartX, (int)dm.StartY);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(200);
            SetCursorPos((int)dm.EndX, (int)dm.EndY);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }




        public static void InputVKFromByte(byte key)
        {
            win32api.keybd_event(key, 0, 0, (UIntPtr)0);
        }


        public static void InputString(string inputstring)
        {
            // キーの取りこぼしを防ぐための、各入力間の待機時間（ミリ秒単位）
            int iWaitTime = 30;

            var sepa = inputstring.ToCharArray();

            foreach (char pi in sepa)
            {


                // 入力対象の文字列かどうかを調べる
                if (checkKey(pi))
                {
                    // キーの押し下げをシミュレートする。
                    win32api.keybd_event((byte)pi, 0, 0, (UIntPtr)0);
                    //// キーの解放をシミュレートする。
                    win32api.keybd_event((byte)pi, 0, 2/*KEYEVENTF_KEYUP*/, (UIntPtr)0);

                    // dwWaitTimeミリ秒間待機する
                    // (キーの取りこぼしを防ぐため)
                    Thread.Sleep(iWaitTime);
                }
            }


        }

        public static bool checkKey(char c)
        {
            if ('0' <= c && c <= '9')
            { // 数字ならＯＫ
                return true;
            }
            else if ('a' <= c && c <= 'z')
            { // 英小文字ならＯＫ
                return true;
            }
            else if ('A' <= c && c <= 'Z')
            { // 英大文字ならＯＫ
                return true;
            }
            else if (c == ' ')
            { // スペースならＯＫ
                return true;
            }
            else if (c == '\t')
            { // タブならＯＫ
                return true;
            }

            // それら以外の場合はＮＧ
            return false;
        }





        public static bool Routines(Rect 検索名称座標, string 商品コード, Rect 検索完了座標, Rect 通常仕入先座標, string 通常仕入先コード, Rect 個別画面完了ボタン座標, Rect 在庫メンテナンス受付範囲, Rect 在庫メンテナンス範囲, byte[] bitmap受付MD5, byte[] bitmapMD5)
        {
            //在庫メンテナンス（受付）になっているかチェック
            if (在庫メンテナンス受付画面かチェック(在庫メンテナンス受付範囲, bitmap受付MD5) == false)
            {
                throw new Exception("在庫メンテナンス(受付)画面へ戻れませんでした。\r\n処理を中止します。" + "\r\n商品コード:" + 商品コード + "\r\n通常仕入先コード:" + 通常仕入先コード);
            }

            //ダブルクリック
            ClickPoint(検索名称座標);
            ClickPoint(検索名称座標);
            Thread.Sleep(100);

            //コード入力
            InputString(商品コード);
            Thread.Sleep(100);

            //ENTER入力
            InputVKFromByte(MockKeyboard.VK_RETURN); // ENTERが必要かチェック
            Thread.Sleep(100);

            //検索完了クリック
            ClickPoint(検索完了座標);
            Thread.Sleep(300);


            //在庫メンテナンスになっているかチェック
            if (在庫メンテナンス画面のチェック(在庫メンテナンス範囲, 在庫メンテナンス受付範囲, bitmap受付MD5, bitmapMD5) == false)
            {
                if (在庫メンテナンス受付画面へ(在庫メンテナンス受付範囲, bitmap受付MD5))
                {
                    return false;
                }
                else
                {
                    throw new Exception("在庫メンテナンス(受付)F12完了で、在庫メンテナンス(受付)画面へ戻れませんでした。\r\n処理を中止します。" + "\r\n商品コード:" + 商品コード + "\r\n通常仕入先コード:" + 通常仕入先コード);
                }
            }

            //通常仕入先ダブルクリック
            ClickPoint(通常仕入先座標); //ダブルクリックorDELETE
            ClickPoint(通常仕入先座標); //ダブルクリックorDELETE
            Thread.Sleep(300);


            //通常仕入先コード入力
            InputString(通常仕入先コード);
            Thread.Sleep(300);

            //ENTER入力
            InputVKFromByte(MockKeyboard.VK_RETURN); // ENTERが必要かチェック


            //完了ボタンクリック
            ClickPoint(個別画面完了ボタン座標);
            Thread.Sleep(100);

            //在庫メンテナンス受付画面でなかったら、何かしらのエラー
            if (在庫メンテナンス受付画面かチェック(在庫メンテナンス受付範囲, bitmap受付MD5) == false)
            {
                //在庫メンテナンス(受付)かどうか
                if (在庫メンテナンス受付画面へ(在庫メンテナンス受付範囲, bitmap受付MD5) == false)
                {
                    throw new Exception("在庫メンテナンスF12完了で、在庫メンテナンス(受付)画面へ戻れませんでした。\r\n処理を中止します。" + "\r\n商品コード:" + 商品コード + "\r\n通常仕入先コード:" + 通常仕入先コード);

                }
                else
                {
                    return false;
                }

            }

            return true;

        }

        private static bool 在庫メンテナンス受付画面へ(Rect 在庫メンテナンス受付範囲, byte[] bitmap受付)
        {
            int escapecounter = 0;

            while (!在庫メンテナンス受付画面かチェック(在庫メンテナンス受付範囲, bitmap受付))
            {
                //エスケープ４回押したら、処理は強制終了
                if (escapecounter == 4)
                {
                    return false;
                }

                InputVKFromByte(MockKeyboard.VK_ESCAPE);
                escapecounter++;
                Thread.Sleep(1000);
            }

            return true;

        }




        /// <summary>
        /// 在庫メンテナンス画面かどうかチェック
        /// </summary>
        /// <param name="在庫メンテナンス範囲"></param>
        /// <param name="在庫メンテナンス受付範囲"></param>
        /// <param name="bitmap受付"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static bool 在庫メンテナンス画面のチェック(Rect 在庫メンテナンス範囲, Rect 在庫メンテナンス受付範囲, byte[] bitmap受付, byte[] bitmap)
        {

            int counter = 0;
            while (true)
            {
                counter++;
                if (counter == 5)
                {
                    return false;
                }

                var bit = BitmapChecker.CaptureScreenAndGetMD5(在庫メンテナンス範囲);
                if (BitmapChecker.CheckByteArraysEqual(bitmap, bit) == false)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }

            return true;

        }


        public static bool 在庫メンテナンス受付画面かチェック(Rect 在庫メンテナンス受付範囲, byte[] bitmap受付)
        {
            int counter = 0;
            while (true)
            {
                counter++;
                if (counter == 10)
                {
                    return false;
                }

                var bit = BitmapChecker.CaptureScreenAndGetMD5(在庫メンテナンス受付範囲);
                if (BitmapChecker.CheckByteArraysEqual(bitmap受付, bit) == false)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }

            return true;



        }

        /// <summary>
        /// マクロテスト
        /// 画像取得する場合
        /// </summary>
        /// <param name="検索名称座標"></param>
        /// <param name="商品コード"></param>
        /// <param name="検索完了座標"></param>
        /// <param name="通常仕入先座標"></param>
        /// <param name="通常仕入先コード"></param>
        /// <param name="個別画面完了ボタン座標"></param>
        /// <param name="在庫メンテナンス受付範囲"></param>
        /// <param name="在庫メンテナンス範囲"></param>
        /// <param name="bitmap受付"></param>
        /// <param name="bitmap"></param>
        public static bool MacroTestRoutines(Rect 検索名称座標, string 商品コード, Rect 検索完了座標, Rect 通常仕入先座標, string 通常仕入先コード, Rect 個別画面完了ボタン座標, Rect 在庫メンテナンス受付範囲, Rect 在庫メンテナンス範囲, out byte[] bitmap受付, out byte[] bitmap)
        {
            //ダブルクリック
            ClickPoint(検索名称座標);
            ClickPoint(検索名称座標);
            Thread.Sleep(500);

            //あえてダブルクリックのあとにいれる。フォーカスがあたる為
            //在庫メンテナンス（受付）のチェック画像を取得
            bitmap受付 = BitmapChecker.CaptureScreenAndGetMD5(在庫メンテナンス受付範囲);


            //コード入力
            InputString(商品コード);
            Thread.Sleep(500);

            //ENTER入力
            InputVKFromByte(MockKeyboard.VK_RETURN); // ENTERが必要かチェック
            Thread.Sleep(500);

            //検索完了クリック
            ClickPoint(検索完了座標);
            Thread.Sleep(500);


            //通常仕入先ダブルクリック
            ClickPoint(通常仕入先座標); //ダブルクリックorDELETE
            ClickPoint(通常仕入先座標); //ダブルクリックorDELETE
            Thread.Sleep(500);

            //敢えて通常仕入先ダブルクリックの下にいれる。フォーカスがあたる為
            //在庫メンテナンスのチェック画像を取得
            bitmap = BitmapChecker.CaptureScreenAndGetMD5(在庫メンテナンス範囲);



            //通常仕入先コード入力
            InputString(通常仕入先コード);
            Thread.Sleep(500);

            //ENTER入力
            InputVKFromByte(MockKeyboard.VK_RETURN); // ENTERが必要かチェック
            Thread.Sleep(500);


            //完了ボタンクリック
            //ClickPoint(個別画面完了ボタン座標);

            // マクロTESTは
            // 完了せずにESCAOEでもどることにする。 JANでエラーになる場合もあるので
            SetCursorPos((int)個別画面完了ボタン座標.X, (int)個別画面完了ボタン座標.Y);
            InputVKFromByte(MockKeyboard.VK_ESCAPE);
            Thread.Sleep(1000);


            //在庫メンテナンス受付画面でなかったら、何かしらのエラー
            if (在庫メンテナンス受付画面かチェック(在庫メンテナンス受付範囲, bitmap受付) == false)
            {
                //在庫メンテナンス(受付)かどうか
                if (在庫メンテナンス受付画面へ(在庫メンテナンス受付範囲, bitmap受付) == false)
                {
                    throw new Exception("在庫メンテナンスF12完了で、在庫メンテナンス(受付)画面へ戻れませんでした。\r\n処理を中止します。" + "\r\n商品コード:" + 商品コード + "\r\n通常仕入先コード:" + 通常仕入先コード);

                }
                else
                {
                    return false;
                }

            }

            return true;



        }

        /// <summary>
        /// マクロテスト
        /// ２回目以降で画像取得しない場合
        /// </summary>
        /// <param name="検索名称座標"></param>
        /// <param name="商品コード"></param>
        /// <param name="検索完了座標"></param>
        /// <param name="通常仕入先座標"></param>
        /// <param name="通常仕入先コード"></param>
        /// <param name="個別画面完了ボタン座標"></param>
        /// <param name="在庫メンテナンス受付範囲"></param>
        /// <param name="在庫メンテナンス範囲"></param>
        public static bool MacroTestRoutines(Rect 検索名称座標, string 商品コード, Rect 検索完了座標, Rect 通常仕入先座標, string 通常仕入先コード, Rect 個別画面完了ボタン座標, Rect 在庫メンテナンス受付範囲, Rect 在庫メンテナンス範囲, byte[] bitmap受付, byte[] bitmap)
        {

            //ダブルクリック
            ClickPoint(検索名称座標);
            ClickPoint(検索名称座標);
            Thread.Sleep(500);

            //コード入力
            InputString(商品コード);
            Thread.Sleep(500);

            //ENTER入力
            InputVKFromByte(MockKeyboard.VK_RETURN); // ENTERが必要かチェック
            Thread.Sleep(500);

            //検索完了クリック
            ClickPoint(検索完了座標);
            Thread.Sleep(500);


            //通常仕入先ダブルクリック
            ClickPoint(通常仕入先座標); //ダブルクリックorDELETE
            ClickPoint(通常仕入先座標); //ダブルクリックorDELETE
            Thread.Sleep(500);


            //通常仕入先コード入力
            InputString(通常仕入先コード);
            Thread.Sleep(500);

            //ENTER入力
            InputVKFromByte(MockKeyboard.VK_RETURN); // ENTERが必要かチェック
            Thread.Sleep(500);


            //完了ボタンクリック
            //ClickPoint(個別画面完了ボタン座標);

            // マクロTESTは
            // 完了せずにESCAOEでもどることにする。 JANでエラーになる場合もあるので
            SetCursorPos((int)個別画面完了ボタン座標.X, (int)個別画面完了ボタン座標.Y);
            InputVKFromByte(MockKeyboard.VK_ESCAPE);
            Thread.Sleep(1000);



            //在庫メンテナンス受付画面でなかったら、何かしらのエラー
            if (在庫メンテナンス受付画面かチェック(在庫メンテナンス受付範囲, bitmap受付) == false)
            {
                //在庫メンテナンス(受付)かどうか
                if (在庫メンテナンス受付画面へ(在庫メンテナンス受付範囲, bitmap受付) == false)
                {
                    throw new Exception("在庫メンテナンスF12完了で、在庫メンテナンス(受付)画面へ戻れませんでした。\r\n処理を中止します。" + "\r\n商品コード:" + 商品コード + "\r\n通常仕入先コード:" + 通常仕入先コード);

                }
                else
                {
                    return false;
                }

            }

            return true;



        }
    }
}
