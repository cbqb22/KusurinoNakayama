using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCSystem.View.Windows
{
    public static class SingletonWindows
    {

        // [帳合変更設定ウィンドウ]
        private static BalanceChangeMenu _BalanceChangeMenuWindow;
        public static BalanceChangeMenu BalanceChangeMenuWindow
        {
            get
            {
                if (_BalanceChangeMenuWindow == null)
                {
                    BalanceChangeMenuWindow = new BalanceChangeMenu();
                }
                else if (_BalanceChangeMenuWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _BalanceChangeMenuWindow = new BalanceChangeMenu();
                }

                return _BalanceChangeMenuWindow;
            }

            set
            {
                if (_BalanceChangeMenuWindow == null)
                {
                    _BalanceChangeMenuWindow = value;
                }
            }
        }


        // [帳合変更設定ウィンドウ]
        private static BalanceChangeSettings _BalanceChangeSettingsWindow;
        public static BalanceChangeSettings BalanceChangeSettingsWindow
        {
            get
            {
                if (_BalanceChangeSettingsWindow == null)
                {
                    BalanceChangeSettingsWindow = new BalanceChangeSettings();
                }
                else if (_BalanceChangeSettingsWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _BalanceChangeSettingsWindow = new BalanceChangeSettings();
                }

                return _BalanceChangeSettingsWindow;
            }

            set
            {
                if (_BalanceChangeSettingsWindow == null)
                {
                    _BalanceChangeSettingsWindow = value;
                }
            }
        }

        // [XY座標取得ウィンドウ]
        private static XYLocation _XYLocationWindow;
        public static XYLocation XYLocationWindow
        {
            get
            {
                if (_XYLocationWindow == null)
                {
                    XYLocationWindow = new XYLocation();
                }
                else if (_XYLocationWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _XYLocationWindow = new XYLocation();
                }

                return _XYLocationWindow;
            }

            set
            {
                if (_XYLocationWindow == null)
                {
                    _XYLocationWindow = value;
                }
            }
        }


        // [独自マクロ作成ウィンドウ]
        private static OriginalMacroMaker _OriginalMacroMakerWindow;
        public static OriginalMacroMaker OriginalMacroMakerWindow
        {
            get
            {
                if (_OriginalMacroMakerWindow == null)
                {
                    OriginalMacroMakerWindow = new OriginalMacroMaker();
                }
                else if (_OriginalMacroMakerWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _OriginalMacroMakerWindow = new OriginalMacroMaker();
                }

                return _OriginalMacroMakerWindow;
            }

            set
            {
                if (_OriginalMacroMakerWindow == null)
                {
                    _OriginalMacroMakerWindow = value;
                }
            }
        }


        /// <summary>
        /// 指定のWindowすでに開いているかどうか
        /// </summary>
        public static bool CheckHasOpenOwnWindow(Type t)
        {
            // IsLoadedはOpenしているということかどうか確認
            if (t == typeof(BalanceChangeSettings) && BalanceChangeSettingsWindow.IsLoaded)
            {
                return true;
            }

            if (t == typeof(OriginalMacroMaker) && OriginalMacroMakerWindow.IsLoaded)
            {
                return true;
            }


            if (t == typeof(XYLocation) && XYLocationWindow.IsLoaded)
            {
                return true;
            }


            return false;


        }



        /// <summary>
        /// どれかひとつでも開いたWindowがあるかチェック
        /// </summary>
        public static bool CheckHasOpenWindow()
        {

            // IsLoadedはOpenしているということかどうか確認
            if (BalanceChangeSettingsWindow.IsLoaded)
            {
                return true;
            }

            if (XYLocationWindow.IsLoaded)
            {
                return true;
            }


            return false;


        }


        /// <summary>
        /// 自Window以外がひとつでも開いているかどうか
        /// </summary>
        public static bool CheckHasOpenWindow(Type t)
        {

            // IsLoadedはOpenしているということかどうか確認
            if (t != typeof(BalanceChangeSettings) && BalanceChangeSettingsWindow.IsLoaded)
            {
                return true;
            }

            if (t != typeof(XYLocation) && XYLocationWindow.IsLoaded)
            {
                return true;
            }

            return false;


        }


    }
}
