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
    public partial class 不動品DataGrid : UserControl
    {
        public 不動品DataGrid()
        {
            InitializeComponent();

            SingletonInstances.不動品DataGridInstance = this;

            //Set原本Itemsource();

            this.name不動品DataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGrid1_LoadingRow);
            this.LayoutUpdated += new EventHandler(不動品DataGrid_LayoutUpdated);

            Dispatcher.BeginInvoke
            (() =>
            {
                name不動品DataGrid.Width = LayoutRoot.ActualWidth;
                name不動品DataGrid.Height = LayoutRoot.ActualHeight;
            }
            );


        }

        void 不動品DataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke
            (() =>
            {
                name不動品DataGrid.Width = LayoutRoot.ActualWidth;
                name不動品DataGrid.Height = LayoutRoot.ActualHeight;
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
            var e = this.name不動品DataGrid.Columns[columnIndex].GetCellContent(this.rowContainer[rowIndex]);
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
