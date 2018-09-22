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
using SilverlightFX.UserInterface;


namespace View.Core.TopPage.Tab.在庫管理
{
    public partial class 現在庫DataGrid : UserControl
    {
        public 現在庫DataGrid()
        {
            InitializeComponent();

            SingletonInstances.現在庫DataGridInstance = this;

            //Set原本Itemsource();

            this.name現在庫DataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGrid1_LoadingRow);
            this.LayoutUpdated += new EventHandler(現在庫DataGrid_LayoutUpdated);


            Dispatcher.BeginInvoke
            (() =>
                {
                    name現在庫DataGrid.Width = LayoutRoot.ActualWidth;
                    name現在庫DataGrid.Height = LayoutRoot.ActualHeight;
                }
            );


        }

        void 現在庫DataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke
            (() =>
            {
                name現在庫DataGrid.Width = LayoutRoot.ActualWidth;
                name現在庫DataGrid.Height = LayoutRoot.ActualHeight;
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
            var e = this.name現在庫DataGrid.Columns[columnIndex].GetCellContent(this.rowContainer[rowIndex]);
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




        public ObservableCollection<現在庫データ> 原本itemsource;
        private void Set原本Itemsource()
        {
            View.Util.ServiceUtil.Call.FileReader fr = new View.Util.ServiceUtil.Call.FileReader();
            fr.CallFileReader("");
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

        private void TextBlock_KeyUp(object sender, KeyEventArgs e)
        {
            TextBlock tbl = sender as TextBlock;
            if (tbl == null)
            {
                return;
            }

            if (e.Key == Key.Space)
            {
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

        private void TextBlock_KeyUp_1(object sender, KeyEventArgs e)
        {

            TextBlock tbl = sender as TextBlock;
            if (tbl == null)
            {
                return;
            }

            if (e.Key == Key.Space)
            {
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
}
