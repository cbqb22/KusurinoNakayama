using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using View.Core.共通;
using System.Collections.ObjectModel;
using View.Service.File.Reader;

namespace View.Core.TopPage.Tab.在庫管理
{
    public partial class 使用量DataGrid : UserControl
    {
        public 使用量DataGrid()
        {
            InitializeComponent();

            SingletonInstances.使用量DataGridInstance = this;

            //Set原本Itemsource();

            this.name使用量DataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGrid1_LoadingRow);
            this.LayoutUpdated += new EventHandler(使用量DataGrid_LayoutUpdated);

            Dispatcher.BeginInvoke
            (() =>
            {
                name使用量DataGrid.Width = LayoutRoot.ActualWidth;
                name使用量DataGrid.Height = LayoutRoot.ActualHeight;
            }
            );

        }

        void 使用量DataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke
            (() =>
            {
                name使用量DataGrid.Width = LayoutRoot.ActualWidth;
                name使用量DataGrid.Height = LayoutRoot.ActualHeight;
            }
            );
        }
        private Dictionary<int, DataGridRow> rowContainer = new Dictionary<int, DataGridRow>();


        void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            this.rowContainer[e.Row.GetIndex()] = e.Row;

        }

        private FrameworkElement GetDataGridCell(int columnIndex, int rowIndex)
        {
            var e = this.name使用量DataGrid.Columns[columnIndex].GetCellContent(this.rowContainer[rowIndex]);
            while (true)
            {
                if (e == null)
                {
                    return null;
                }
                if (e is DataGridCell)
                {
                    return e;
                }
                e = e.Parent as FrameworkElement;
            }
        }


        public void SetDataGridItemSource(string 検索文字列, bool 全期間, int 期限加算月)
        {
            View.Util.ServiceUtil.Call.FileReader fr = new View.Util.ServiceUtil.Call.FileReader();
            //fr.CallFileReader2(検索文字列,全期間, 期限加算月); //使用量検索
            fr.CallFileReader4(検索文字列, 全期間, 期限加算月);  //使用量2検索
        }

        //public ObservableCollection<薬局使用量データ> 原本itemsource;
        //private void Set原本Itemsource()
        //{
        //    View.Util.ServiceUtil.Call.FileReader fr = new View.Util.ServiceUtil.Call.FileReader();
        //    fr.CallFileReader2("");
        //}

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            TextBlock tbl = sender as TextBlock;

            if (tbl == null)
            {
                return;
            }


            FrameworkElement fe = VisualTreeHelper.GetParent(this.Parent) as FrameworkElement;
            while (true)
            {
                if (fe == null)
                {
                    return;
                }
                if (fe is 在庫管理Frame)
                {
                    ((在庫管理Frame)fe).SearchTextBox1.Text = tbl.Text;

                    return;
                }
                fe = fe.Parent as FrameworkElement;
            }
        }

    }
}
