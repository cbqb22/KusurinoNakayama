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
using OASystem.ViewModel.Common.DataConvert;
using OASystem.ViewModel.File;


namespace OASystem.View.Windows
{
    /// <summary>
    /// IndividualBasedMedicineSelectMaker.xaml の相互作用ロジック
    /// </summary>
    public partial class OrderMedicineManagementProtectAdd : Window
    {

        // 優先移動リストのスナップショット
        // 保護・優先の排他チェック
        private List<PriorityMoveEntity> _PriorityMoveListSnapShot;
        public List<PriorityMoveEntity> PriorityMoveListSnapShot
        {
            get { return _PriorityMoveListSnapShot; }
            set { _PriorityMoveListSnapShot = value; }
        }


        private bool _AddFlag;
        public bool AddFlag
        {
            get { return _AddFlag; }
            set { _AddFlag = value; }
        }


        private List<string> _すでに追加されているレセプト電算コード;

        public List<string> すでに追加されているレセプト電算コード
        {
            get { return _すでに追加されているレセプト電算コード; }
            set { _すでに追加されているレセプト電算コード = value; }
        }


        public OrderMedicineManagementProtectAdd()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OrderMedicineManagementProtectAdd_Loaded);
        }

        void OrderMedicineManagementProtectAdd_Loaded(object sender, RoutedEventArgs e)
        {
            tbキーワード.Focus();
        }

        private void btnキーワード検索_Click(object sender, RoutedEventArgs e)
        {
            if (this.tbキーワード.Text == "")
            {
                MessageBox.Show("キーワードが入力されておりません。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var keysplit = tbキーワード.Text.Replace('　', ' ').Split(' ');


            List<ProtectEntity> entlist = new List<ProtectEntity>();


            var 現在庫list = OASystem.ViewModel.OrderCenter.CheckExpAndDead.Load現在庫Total();
            var 自店舗の現在庫list = 現在庫list.Where(x => x.店名 == OASystem.Model.DI.自店舗名).ToList();

            foreach (var row in 自店舗の現在庫list)
            {
                bool nokey = false;

                // 現状ではレセプト電算コードが入っていないと、他店のデッド品期限切迫品と紐付けができない為、不可。
                // 対象はバイアグラなどの自費医薬品のみ。　ペンニードルなどの医療材料はレセ電ある為OK
                if (row.レセプト電算コード == "")
                {
                    continue;
                }

                foreach (var key in keysplit)
                {
                    if (!row.医薬品名.Contains(key) && !row.名称２.Contains(key))
                    {
                        nokey = true;
                        break;
                    }
                }

                if (!nokey)
                {
                    ProtectEntity pe = new ProtectEntity();
                    pe.レセプト電算コード = row.レセプト電算コード;
                    pe.医薬品名 = string.IsNullOrEmpty(row.名称２) ? row.医薬品名 : string.Format("{0}（{1}）", row.医薬品名, row.名称２);
                    entlist.Add(pe);
                }
            }



            if (entlist.Count == 0)
            {
                MessageBox.Show("キーワードに該当する医薬品名が見つかりませんでした。\r\n細かいキーワードで検索してください。\r\n漢字も使用可能です。カナは全角で入力してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            this.lvMedicineInfo.ItemsSource = entlist;

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

            ProtectEntity ent = lvMedicineInfo.SelectedValue as ProtectEntity;
            if (ent == null)
            {
                MessageBox.Show("医薬品が選択されてません。医薬品を表示されたリストから選択してください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            bool hasData = false;
            foreach (var snaprow in すでに追加されているレセプト電算コード)
            {
                if (snaprow == ent.レセプト電算コード)// && !snaprow.IsJAN管理 )
                {
                    hasData = true;
                    break;
                }
            }
            if (hasData)
            {
                MessageBox.Show("この医薬品はすでに保護リストに追加されてます。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            bool hasDataPriority = false;
            foreach (var snaprowpriority in _PriorityMoveListSnapShot)
            {
                if (snaprowpriority.レセプト電算コード == ent.レセプト電算コード)// && !snaprowpriority.IsJAN管理 )
                {
                    hasDataPriority = true;
                    break;
                }
            }
            if (hasDataPriority)
            {
                MessageBox.Show("この医薬品は優先移動リストに追加されてます。\r\n追加する場合は、先に優先移動リストより削除を行ってください。", "確認", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            AddFlag = true;

            this.Close();
            
        }


    }
}
