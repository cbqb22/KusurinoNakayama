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
using View.Core.TopPage.Tab.在庫管理;
using View.Core.TopPage;
using View.Core.TopPage.Tab.設定.SubTab.ユーザー管理;
using View.Core.TopPage.Tab.設定.SubTab.店舗管理;
using View.Core.TopPage.Tab.設定.SubTab.その他;
using View.Core.TopPage.Tab.掲示板.メイン;
using View.Core.TopPage.Tab.在庫管理.在庫更新;
using View.Core.TopPage.Tab.資料;


namespace View.Core.共通
{
    public partial class SingletonInstances
    {
        private static MainPage instance;

        public static MainPage Instance
        {
            get
            {
                return instance;
            }
            set
            {
                if (instance == null)
                {
                    instance = value;
                }
            }
        }


        private static TabItemControl _TabItemControlInstance;

        public static TabItemControl TabItemControlInstance
        {
            get { return SingletonInstances._TabItemControlInstance; }
            set { SingletonInstances._TabItemControlInstance = value; }
        }


        private static 在庫管理Frame _在庫管理FrameInstance;

        public static 在庫管理Frame 在庫管理FrameInstance
        {
            get { return SingletonInstances._在庫管理FrameInstance; }
            set { SingletonInstances._在庫管理FrameInstance = value; }
        }


        private static 現在庫DataGrid _現在庫DataGridInstance;

        public static 現在庫DataGrid 現在庫DataGridInstance
        {
            get
            {
                return SingletonInstances._現在庫DataGridInstance;
            }
            set
            {
                if (_現在庫DataGridInstance == null)
                {
                    SingletonInstances._現在庫DataGridInstance = value;
                }
            }
        }

        private static 後発品DataGrid _後発品DataGridInstance;

        public static 後発品DataGrid 後発品DataGridInstance
        {
            get
            {
                return SingletonInstances._後発品DataGridInstance;
            }
            set
            {
                if (_後発品DataGridInstance == null)
                {
                    SingletonInstances._後発品DataGridInstance = value;
                }
            }
        }

        private static 使用量DataGrid _使用量DataGridInstance;

        public static 使用量DataGrid 使用量DataGridInstance
        {
            get
            {
                return SingletonInstances._使用量DataGridInstance;
            }
            set
            {
                if (_使用量DataGridInstance == null)
                {
                    SingletonInstances._使用量DataGridInstance = value;
                }
            }
        }


        private static 不動品DataGrid _不動品DataGridInstance;

        public static 不動品DataGrid 不動品DataGridInstance
        {
            get
            {
                return SingletonInstances._不動品DataGridInstance;
            }
            set
            {
                if (_不動品DataGridInstance == null)
                {
                    SingletonInstances._不動品DataGridInstance = value;
                }
            }
        }


        private static ログインユーザー更新 _ログインユーザー更新Instance;

        public static ログインユーザー更新 ログインユーザー更新Instance
        {
            get
            {
                return SingletonInstances._ログインユーザー更新Instance;
            }
            set
            {
                if (_ログインユーザー更新Instance == null)
                {
                    SingletonInstances._ログインユーザー更新Instance = value;
                }
            }
        }

        private static ユーザー管理TabItem _ユーザー管理TabItemInstance;

        public static ユーザー管理TabItem ユーザー管理TabItemInstance
        {
            get
            {
                return SingletonInstances._ユーザー管理TabItemInstance;
            }
            set
            {
                if (_ユーザー管理TabItemInstance == null)
                {
                    SingletonInstances._ユーザー管理TabItemInstance = value;
                }
            }
        }


        private static 店舗管理TabItem _店舗管理TabItemInstance;

        public static 店舗管理TabItem 店舗管理TabItemInstance
        {
            get { return SingletonInstances._店舗管理TabItemInstance; }
            set { SingletonInstances._店舗管理TabItemInstance = value; }
        }

        private static その他TabItem _その他TabItemInstance;

        public static その他TabItem その他TabItemInstance
        {
            get { return SingletonInstances._その他TabItemInstance; }
            set { SingletonInstances._その他TabItemInstance = value; }
        }

        private static 掲示板Frame _掲示板FrameInstance;

        public static 掲示板Frame 掲示板FrameInstance
        {
            get 
            {
                return SingletonInstances._掲示板FrameInstance; 
            }
            set
            {
                if (_掲示板FrameInstance == null)
                {
                    SingletonInstances._掲示板FrameInstance = value;
                }
            }
        }

        private static 在庫更新 _在庫更新Instance;

        public static 在庫更新 在庫更新Instance
        {
            get
            {
                return SingletonInstances._在庫更新Instance;
            }
            set
            {
                if (_在庫更新Instance == null)
                {
                    SingletonInstances._在庫更新Instance = value;
                }
            }
        }

        private static 資料TreeView _資料Instance;

        public static 資料TreeView 資料Instance
        {
            get
            {
                return SingletonInstances._資料Instance;
            }
            set
            {
                if (_資料Instance == null)
                {
                    SingletonInstances._資料Instance = value;
                }
            }
        }

    }
}
