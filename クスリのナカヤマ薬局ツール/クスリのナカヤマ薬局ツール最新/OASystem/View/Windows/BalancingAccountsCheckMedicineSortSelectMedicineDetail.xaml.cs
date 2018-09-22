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
using OASystem.Model.Entity;

namespace OASystem.View.Windows
{
    /// <summary>
    /// BalancingAccountsCheckMedicineSortSelectMedicineDetail.xaml の相互作用ロジック
    /// </summary>
    public partial class BalancingAccountsCheckMedicineSortSelectMedicineDetail : Window
    {

        private bool _選択Flag;
        public bool 選択Flag
        {
            get { return _選択Flag; }
            set { _選択Flag = value; }
        }

        private bool _非JAN管理としても追加Flag;

        public bool 非JAN管理としても追加Flag
        {
            get { return _非JAN管理としても追加Flag; }
            set { _非JAN管理としても追加Flag = value; }
        }

        private bool _Is個別管理医薬品マスタ;

        public bool Is個別管理医薬品マスタ
        {
            get { return _Is個別管理医薬品マスタ; }
            set { _Is個別管理医薬品マスタ = value; }
        }

        // 前画面で選択された項目
        private BalancingAccountsCheckMedicineSortEntity _PreviousSelectedBACMSEnt;
        public BalancingAccountsCheckMedicineSortEntity PreviousSelectedBACMSEnt
        {
            get { return _PreviousSelectedBACMSEnt; }
            set { _PreviousSelectedBACMSEnt = value; }
        }


        // 帳合先チェックマスタ医薬品別のスナップショット
        // すでに追加されているものをチェックする為のスナップ
        List<BalancingAccountsCheckMedicineSortEntity> _BACMedicineSortListSnapShotCopy;
        public List<BalancingAccountsCheckMedicineSortEntity> BACMedicineSortListSnapShotCopy
        {
            get { return _BACMedicineSortListSnapShotCopy; }
            set { _BACMedicineSortListSnapShotCopy = value; }
        }

        List<BalancingAccountsCheckMedicineSortEntity> _JANList;
        public List<BalancingAccountsCheckMedicineSortEntity> JANList
        {
            get { return _JANList; }
            set { _JANList = value; }
        }


        public BalancingAccountsCheckMedicineSortSelectMedicineDetail()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BalancingAccountsCheckMedicineSortSelectMedicineDetail_Loaded);
        }

        void BalancingAccountsCheckMedicineSortSelectMedicineDetail_Loaded(object sender, RoutedEventArgs e)
        {
            SetInit();
        }

        private void SetInit()
        {
            this.lvMedicineInfo.ItemsSource = JANList;
        }


        private void btn中止_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btn追加_Click(object sender, RoutedEventArgs e)
        {
            if (lvMedicineInfo.SelectedIndex == -1)
            {
                MessageBox.Show("医薬品が選択されてません。\r\nキーワード検索後、医薬品を表示されたリストから選択してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var selected = lvMedicineInfo.SelectedValue as BalancingAccountsCheckMedicineSortEntity;

            bool hasJAN = false;
            foreach(var snap in BACMedicineSortListSnapShotCopy)
            {
                if (selected.JANコード == snap.JANコード)
                {
                    hasJAN = true;
                    break;
                }
            }

            if (hasJAN)
            {
                MessageBox.Show("この医薬品はすでに追加されています。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            bool has非JAN管理Data = false;
            foreach (var snaprow in _BACMedicineSortListSnapShotCopy)
            {
                if (snaprow.レセプト電算コード == selected.レセプト電算コード && !snaprow.IsJAN管理)
                {
                    has非JAN管理Data = true;
                    break;
                }
            }

            // 非JAN管理がされていなければ、それも追加するかメッセージボックス
            // 個別管理医薬品マスタの場合はスルー
            if (_Is個別管理医薬品マスタ == false)
            {
                if (!has非JAN管理Data)
                {
                    if (MessageBox.Show("この医薬品を非JAN管理としても追加しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        非JAN管理としても追加Flag = true;
                    }
                }
            }

            選択Flag = true;
            this.Close();
        }

    }
}
