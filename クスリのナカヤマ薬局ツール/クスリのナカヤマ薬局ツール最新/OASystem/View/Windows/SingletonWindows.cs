using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OASystem.View.Windows
{
    public static class SingletonWindows
    {

        // [帳合先チェック登録ウィンドウ]
        private static BalancingAccountsCheckRegister _BalancingAccountsCheckRegisterWindow;
        public static BalancingAccountsCheckRegister BalancingAccountsCheckRegisterWindow
        {
            get
            {
                if (_BalancingAccountsCheckRegisterWindow == null)
                {
                    BalancingAccountsCheckRegisterWindow = new BalancingAccountsCheckRegister();
                }
                else if (_BalancingAccountsCheckRegisterWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _BalancingAccountsCheckRegisterWindow = new BalancingAccountsCheckRegister();
                }

                return _BalancingAccountsCheckRegisterWindow;
            }

            set
            {
                if (_BalancingAccountsCheckRegisterWindow == null)
                {
                    _BalancingAccountsCheckRegisterWindow = value;
                }
            }
        }

        // [マスタ管理ウィンドウ]
        private static MasterDataManagement _MasterDataManagementWindow;
        public static MasterDataManagement MasterDataManagementWindow
        {
            get
            {
                if (_MasterDataManagementWindow == null)
                {
                    MasterDataManagementWindow = new MasterDataManagement();
                }
                else if (_MasterDataManagementWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _MasterDataManagementWindow = new MasterDataManagement();
                }

                return _MasterDataManagementWindow;
            }

            set
            {
                if (_MasterDataManagementWindow == null)
                {
                    _MasterDataManagementWindow = value;
                }
            }
        }

        // [発注書作成ウィンドウ]
        private static OrderCenter _OrderCenterWindow;
        public static OrderCenter OrderCenterWindow
        {
            get
            {
                if (_OrderCenterWindow == null)
                {
                    OrderCenterWindow = new OrderCenter();
                }
                else if (_OrderCenterWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _OrderCenterWindow = new OrderCenter();
                }

                return _OrderCenterWindow;
            }

            set
            {
                if (_OrderCenterWindow == null)
                {
                    _OrderCenterWindow = value;
                }
            }
        }

        // [発注履歴ウィンドウ]
        private static OrderLogs _OrderLogsWindow;
        public static OrderLogs OrderLogsWindow
        {
            get
            {
                if (_OrderLogsWindow == null)
                {
                    OrderLogsWindow = new OrderLogs();
                }
                else if (_OrderLogsWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _OrderLogsWindow = new OrderLogs();
                }

                return _OrderLogsWindow;
            }

            set
            {
                if (_OrderLogsWindow == null)
                {
                    _OrderLogsWindow = value;
                }
            }
        }


        // [発注医薬品管理ウィンドウ]
        private static OrderMedicineManagement _OrderMedicineManagementWindow;

        public static OrderMedicineManagement OrderMedicineManagementWindow
        {
            get
            {
                if (_OrderMedicineManagementWindow == null)
                {
                    OrderMedicineManagementWindow = new OrderMedicineManagement();
                }
                else if (_OrderMedicineManagementWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _OrderMedicineManagementWindow = new OrderMedicineManagement();
                }

                //System.Diagnostics.Debug.WriteLine(_OrderMedicineManagementWindow.tblStoreNamerTitleProtext.Text + _OrderMedicineManagementWindow.GetHashCode() + "Singleton");

                return _OrderMedicineManagementWindow;
            }

            set
            {
                if (_OrderMedicineManagementWindow == null)
                {
                    _OrderMedicineManagementWindow = value;
                }
            }
        }

        // [設定ウィンドウ]
        private static Settings _SettingsWindow;
        public static Settings SettingsWindow
        {
            get
            {
                if (_SettingsWindow == null)
                {
                    SettingsWindow = new Settings();
                }
                else if (_SettingsWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _SettingsWindow = new Settings();
                }

                return _SettingsWindow;
            }

            set
            {
                if (_SettingsWindow == null)
                {
                    _SettingsWindow = value;
                }
            }
        }


        // [設定ウィンドウ]
        private static BalancingAccountsCheckResult _BalancingAccountsCheckResultWindow;
        public static BalancingAccountsCheckResult BalancingAccountsCheckResultWindow
        {
            get
            {
                if (_BalancingAccountsCheckResultWindow == null)
                {
                    BalancingAccountsCheckResultWindow = new BalancingAccountsCheckResult();
                }
                else if (_BalancingAccountsCheckResultWindow.IsClosed == true) //閉じられていたら作り直し
                {
                    _BalancingAccountsCheckResultWindow = new BalancingAccountsCheckResult();
                }

                return _BalancingAccountsCheckResultWindow;
            }

            set
            {
                if (_BalancingAccountsCheckResultWindow == null)
                {
                    _BalancingAccountsCheckResultWindow = value;
                }
            }
        }

        /// <summary>
        /// 指定のWindowすでに開いているかどうか
        /// </summary>
        public static bool CheckHasOpenOwnWindow(Type t)
        {
            // IsLoadedはOpenしているということかどうか確認
            if (t == typeof(BalancingAccountsCheckResult) && BalancingAccountsCheckResultWindow.IsLoaded)
            {
                return true;
            }

            if (t == typeof(BalancingAccountsCheckRegister) && BalancingAccountsCheckRegisterWindow.IsLoaded)
            {
                return true;
            }

            if (t == typeof(MasterDataManagement) && MasterDataManagementWindow.IsLoaded)
            {
                return true;
            }

            if (t == typeof(OrderCenter) && OrderCenterWindow.IsLoaded)
            {
                return true;
            }

            if (t == typeof(OrderLogs) && OrderLogsWindow.IsLoaded)
            {
                return true;
            }

            if (t == typeof(OrderMedicineManagement) && OrderMedicineManagementWindow.IsLoaded)
            {
                return true;
            }


            if (t == typeof(Settings) && SettingsWindow.IsLoaded)
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
            if (BalancingAccountsCheckRegisterWindow.IsLoaded)
            {
                return true;
            }

            if (MasterDataManagementWindow.IsLoaded)
            {
                return true;
            }

            if (OrderCenterWindow.IsLoaded)
            {
                return true;
            }

            if (OrderLogsWindow.IsLoaded)
            {
                return true;
            }


            if (OrderMedicineManagementWindow.IsLoaded)
            {
                return true;
            }


            if (SettingsWindow.IsLoaded)
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
            if (t != typeof(BalancingAccountsCheckResult) && BalancingAccountsCheckResultWindow.IsLoaded)
            {
                return true;
            }

            if (t != typeof(BalancingAccountsCheckRegister) &&  BalancingAccountsCheckRegisterWindow.IsLoaded)
            {
                return true;
            }

            if (t != typeof(MasterDataManagement) && MasterDataManagementWindow.IsLoaded)
            {
                return true;
            }

            if (t != typeof(OrderCenter) && OrderCenterWindow.IsLoaded)
            {
                return true;
            }

            if (t != typeof(OrderLogs) && OrderLogsWindow.IsLoaded)
            {
                return true;
            }

            if (t != typeof(OrderMedicineManagement) && OrderMedicineManagementWindow.IsLoaded)
            {
                return true;
            }


            if (t != typeof(Settings) && SettingsWindow.IsLoaded)
            {
                return true;
            }


            return false;


        }


    }
}
