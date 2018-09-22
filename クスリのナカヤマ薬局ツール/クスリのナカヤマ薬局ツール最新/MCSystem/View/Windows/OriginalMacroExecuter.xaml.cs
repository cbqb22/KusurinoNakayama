using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Reflection;
using WF = System.Windows.Forms;
using MCSystem.Model;
using MCSystem.ViewModel;
using MCSystem.ViewModel.Common.MessageBox;
using MCSystem.ViewModel.Common.Image;

namespace MCSystem.View.Windows
{
    /// <summary>
    /// OriginalMacroExecuter.xaml の相互作用ロジック
    /// </summary>
    public partial class OriginalMacroExecuter : Window
    {


        public OriginalMacroExecuter()
        {
            InitializeComponent();
            this.Closed += new EventHandler(OriginalMacroExecuter_Closed);
        }

        void OriginalMacroExecuter_Closed(object sender, EventArgs e)
        {
            SingletonWindows.BalanceChangeMenuWindow.Show();
            SingletonWindows.BalanceChangeMenuWindow.Activate();
        }

        /// <summary>
        /// プライマリースクリーン画像を保存
        /// </summary>
        /// <returns>成功したらtrue</returns>
        private bool SavePrintScreen(int counter)
        {
            Rect screenAllRect = new Rect(0, 0, WF.Screen.PrimaryScreen.Bounds.Width, WF.Screen.PrimaryScreen.Bounds.Height); //全画面
            string saveDatePath = System.IO.Path.Combine(@"C:\Users\poohace\Desktop\SBIマクロ\スクリーニング", DateTime.Now.ToString("yyyy.MM.dd"));
            string savePath = System.IO.Path.Combine(saveDatePath, counter.ToString().PadLeft(5, '0') + ".jpg");


            //日付フォルダがない場合は新しく作成
            if (!System.IO.Directory.Exists(saveDatePath))
            {
                System.IO.Directory.CreateDirectory(saveDatePath);
            }


            // 画像を保存
            return BitmapChecker.CaptureScreenAndSave(screenAllRect, savePath);


        }

        public void DoFromCmd(string path)
        {

            var omEnt = ControlOriginalMacro.LoadOriginalMacroFromFile(path);

            if (!string.IsNullOrEmpty(omEnt.データファイルパス))
            {
                一括データからマクロ実行(omEnt, true);
            }
            else
            {
                ひとつずつマクロを実行(omEnt, true);
            }




        }


        private void btnマクロ実行_Click(object sender, RoutedEventArgs e)
        {

            this.WindowState = System.Windows.WindowState.Minimized;

            var omEnt = ControlOriginalMacro.LoadOriginalMacroFromFile(tbデータファイルPath.Text);

            if (!string.IsNullOrEmpty(omEnt.データファイルパス))
            {
                一括データからマクロ実行(omEnt, false);
            }
            else
            {
                ひとつずつマクロを実行(omEnt, false);
            }


            MessageBoxTop.Show("マクロ処理が完了しました。", "正常終了", MessageBoxButton.OK, MessageBoxImage.Information);

            this.WindowState = System.Windows.WindowState.Normal;



        }

        private void ひとつずつマクロを実行(OriginalMacroEntity omEnt, bool FromCmd)
        {
            // 開始準備完了メッセージ
            if (FromCmd || MessageBoxTop.Show("処理を開始する場合は、このメッセージウィンドウの後ろに処理するウィンドウが表示されている状態で、\r\n『はい』を選択してください。\r\n\r\n処理を中断する場合はキャンセルをクリックしてください。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Thread.Sleep(50);

                MessageBoxResult answer;

                if (FromCmd)
                {
                    answer = MessageBoxResult.No;
                }
                else
                {
                    answer = MessageBox.Show("繰り返し処理を行いますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);
                }

                Thread.Sleep(2000);


                //画像一致がないか調べる
                Dictionary<int, byte[]> imDic = new Dictionary<int, byte[]>(); //操作行と画像Byte[]を保存しておく。
                int rowcounter = 0;
                foreach (var data in omEnt.ListDetail)
                {
                    rowcounter++;

                    if (data.OperationEnum == Enum.MacroOperationEnum.ImageMatch)
                    {
                        var bit = BitmapChecker.CaptureScreenAndGetMD5(data.操作座標);
                        imDic.Add(rowcounter, bit);
                    }

                }

                while (true)
                {


                    //BCMacroRoutines.TestClickPoint();


                    int innercounter = 0;
                    bool WhileUnMatchEvent = false;
                    // ルーチンスタート
                    foreach (var data in omEnt.ListDetail)
                    {
                        innercounter++;

                        Thread.Sleep(500);

                        //画像不一致操作ONの場合は、UnMatch操作が終わるまで続行
                        if (WhileUnMatchEvent)
                        {
                            //不一致操作終了
                            if (!data.IsUnMatchOperation)
                            {
                                WhileUnMatchEvent = false;
                            }
                        }
                        else
                        {
                            //不一致操作OFFで不一致操作の場合は、continue
                            if (data.IsUnMatchOperation)
                            {
                                continue;
                            }
                        }

                        if (data.OperationEnum == Enum.MacroOperationEnum.Click)
                        {
                            BCMacroRoutines.ClickPoint(data.操作座標);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.Input)
                        {
                            BCMacroRoutines.InputVKFromByte(MockKeyboard.VK_DELETE);
                            BCMacroRoutines.InputString(data.入力データ);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.Sleep)
                        {
                            Thread.Sleep(data.待機時間);

                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.Drag)
                        {
                            BCMacroRoutines.Drag(data.Drag座標);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.DragFast)
                        {
                            BCMacroRoutines.DragFast(data.Drag座標);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.ImageMatch)
                        {
                            if (!imDic.ContainsKey(innercounter))
                            {
                                MessageBox.Show("画像一致操作中にエラーが発生しました。\r\n操作する画像一致の画像が存在しません。", "画像一致操作エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            var bit = BitmapChecker.CaptureScreenAndGetMD5(data.操作座標);

                            //画像が一致しなかったら
                            if (BitmapChecker.CheckByteArraysEqual(bit, imDic[innercounter]) == false)
                            {
                                //MessageBox.Show("画像が不一致");
                                WhileUnMatchEvent = true;
                            }

                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.ScreenShot)
                        {
                            Rect screenAllRect = new Rect(0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height); //全画面

                            // スクリーンイメージの取得
                            using (var bmp = new System.Drawing.Bitmap((int)screenAllRect.Width, (int)screenAllRect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                            using (var graph = System.Drawing.Graphics.FromImage(bmp))
                            {
                                // 画面をコピーする
                                graph.CopyFromScreen(new System.Drawing.Point((int)screenAllRect.X, (int)screenAllRect.Y), new System.Drawing.Point(), bmp.Size);

                                //イメージの保存
                                string folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                                string savePath = System.IO.Path.Combine(folder, innercounter + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
                                bmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                                //bmp.Save(System.IO.Path.ChangeExtension(System.IO.Path.Combine(folder, "image"), "png"), System.Drawing.Imaging.ImageFormat.Png);
                            }

                        }


                        //マクロの一番最後だったら画像を保存
                        if (innercounter == omEnt.ListDetail.Count)
                        {
                            SavePrintScreen(innercounter);
                        }

                        // エラー時の補足
                    }

                    //ループなしの場合は終了
                    if (answer != MessageBoxResult.Yes)
                    {
                        break;
                    }
                }
            }


        }

        private void 一括データからマクロ実行(OriginalMacroEntity omEnt, bool FromCmd)
        {
            List<string> d = new List<string>();
            using (StreamReader sr = new StreamReader(omEnt.データファイルパス, Encoding.GetEncoding(932)))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var sepa = line.Split(',');
                    if (sepa.Count() == 0)
                    {
                        continue;
                    }

                    d.Add(sepa[0]);
                }
            }

            // 開始準備完了メッセージ
            if (FromCmd || MessageBoxTop.Show("処理を開始する場合は、このメッセージウィンドウの後ろに処理するウィンドウが表示されている状態で、\r\n『はい』を選択してください。\r\n\r\n処理を中断する場合はキャンセルをクリックしてください。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Thread.Sleep(50);


                //画像一致がないか調べる
                Dictionary<int, byte[]> imDic = new Dictionary<int, byte[]>(); //操作行と画像Byte[]を保存しておく。
                int rowcounter = 0;
                foreach (var data in omEnt.ListDetail)
                {
                    rowcounter++;

                    if (data.OperationEnum == Enum.MacroOperationEnum.ImageMatch)
                    {
                        var bit = BitmapChecker.CaptureScreenAndGetMD5(data.操作座標);
                        imDic.Add(rowcounter, bit);
                    }

                }

                //BCMacroRoutines.TestClickPoint();

                int counter = 0;
                foreach (var inputdata in d)
                {
                    counter++;

                    int innercounter = 0;
                    // ルーチンスタート

                    bool WhileUnMatchEvent = false;
                    foreach (var data in omEnt.ListDetail)
                    {
                        innercounter++;

                        Thread.Sleep(500);

                        //画像不一致操作ONの場合は、UnMatch操作が終わるまで続行
                        if (WhileUnMatchEvent)
                        {
                            //不一致操作終了
                            if (!data.IsUnMatchOperation)
                            {
                                WhileUnMatchEvent = false;
                            }
                        }
                        else
                        {
                            //不一致操作OFFで不一致操作の場合は、continue
                            if (data.IsUnMatchOperation)
                            {
                                continue;
                            }
                        }

                        if (data.OperationEnum == Enum.MacroOperationEnum.Click)
                        {
                            BCMacroRoutines.ClickPoint(data.操作座標);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.Input)
                        {
                            BCMacroRoutines.InputVKFromByte(MockKeyboard.VK_DELETE);
                            BCMacroRoutines.InputString(inputdata);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.Sleep)
                        {
                            Thread.Sleep(data.待機時間);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.Drag)
                        {
                            BCMacroRoutines.Drag(data.Drag座標);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.DragFast)
                        {
                            BCMacroRoutines.DragFast(data.Drag座標);
                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.ImageMatch)
                        {
                            if (!imDic.ContainsKey(innercounter))
                            {
                                MessageBox.Show("画像一致操作中にエラーが発生しました。\r\n操作する画像一致の画像が存在しません。", "画像一致操作エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            var bit = BitmapChecker.CaptureScreenAndGetMD5(data.操作座標);

                            //画像が一致しなかったら
                            if (BitmapChecker.CheckByteArraysEqual(bit, imDic[innercounter]) == false)
                            {
                                WhileUnMatchEvent = true;
                            }

                        }
                        else if (data.OperationEnum == Enum.MacroOperationEnum.ScreenShot)
                        {
                            Rect screenAllRect = new Rect(0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height); //全画面

                            // スクリーンイメージの取得
                            using (var bmp = new System.Drawing.Bitmap((int)screenAllRect.Width, (int)screenAllRect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                            using (var graph = System.Drawing.Graphics.FromImage(bmp))
                            {
                                // 画面をコピーする
                                graph.CopyFromScreen(new System.Drawing.Point((int)screenAllRect.X, (int)screenAllRect.Y), new System.Drawing.Point(), bmp.Size);

                                //イメージの保存
                                string folder = data.入力データ;
                                string savePath = System.IO.Path.Combine(folder, innercounter + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
                                bmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                                //bmp.Save(System.IO.Path.ChangeExtension(System.IO.Path.Combine(folder, "image"), "png"), System.Drawing.Imaging.ImageFormat.Png);
                            }

                        }


                        //マクロの一番最後だったら画像を保存
                        if (innercounter == omEnt.ListDetail.Count)
                        {
                            SavePrintScreen(counter);
                        }

                        // エラー時の補足
                    }
                }
            }

        }

        private void btn閉じる_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnデータファイル参照_Click(object sender, RoutedEventArgs e)
        {
            WF.OpenFileDialog ofd = new WF.OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            if (ofd.ShowDialog() == WF.DialogResult.OK)
            {
                tbデータファイルPath.Text = ofd.FileName;
            }

        }





    }
}
