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
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace View.Core.TopPage.Tab.シフト.表作成
{
    public partial class シフト表作成 : UserControl
    {
        public シフト表作成()
        {
            InitializeComponent();
            SetDatagrid();

        }

        public void SetDatagrid()
        {
            // シフト作成用のDataGrid
            DataGrid newdatagrid = new DataGrid();

            newdatagrid.Background = new SolidColorBrush(Colors.White);

            newdatagrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            newdatagrid.SelectedIndex = -1;
            newdatagrid.AutoGenerateColumns = false;
            newdatagrid.HeadersVisibility = DataGridHeadersVisibility.None;
            newdatagrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            Thickness tc2 = new Thickness(0d);
            newdatagrid.BorderThickness = tc2;


            #region カラムの追加

            DataGridTemplateColumn SummaryTemplate1 = new DataGridTemplateColumn();
            DataGridLength SummaryDGL1 = new DataGridLength(50d);
            SummaryTemplate1.CellTemplate = Summary1;
            SummaryTemplate1.Header = "カラム１";
            SummaryTemplate1.Width = SummaryDGL1;
            newdatagrid.Columns.Add(SummaryTemplate1);

            DataGridTemplateColumn SummaryTemplate2 = new DataGridTemplateColumn();
            DataGridLength SummaryDGL2 = new DataGridLength(50d);
            SummaryTemplate2.CellTemplate = Summary2;
            SummaryTemplate2.Header = "カラム２";
            SummaryTemplate2.Width = SummaryDGL2;
            newdatagrid.Columns.Add(SummaryTemplate2);

            DataGridTemplateColumn SummaryTemplate3 = new DataGridTemplateColumn();
            DataGridLength SummaryDGL3 = new DataGridLength(50d);
            SummaryTemplate3.CellTemplate = Summary3;
            SummaryTemplate3.Header = "カラム３";
            SummaryTemplate3.Width = SummaryDGL3;
            newdatagrid.Columns.Add(SummaryTemplate3);

            DataGridTemplateColumn SummaryTemplate4 = new DataGridTemplateColumn();
            DataGridLength SummaryDGL4 = new DataGridLength(50d);
            SummaryTemplate4.CellTemplate = Summary4;
            SummaryTemplate4.Header = "カラム４";
            SummaryTemplate4.Width = SummaryDGL4;
            newdatagrid.Columns.Add(SummaryTemplate4);

            DataGridTemplateColumn SummaryTemplate5 = new DataGridTemplateColumn();
            DataGridLength SummaryDGL5 = new DataGridLength(50d);
            SummaryTemplate5.CellTemplate = Summary5;
            SummaryTemplate5.Header = "カラム５";
            SummaryTemplate5.Width = SummaryDGL5;
            newdatagrid.Columns.Add(SummaryTemplate5);

            DataGridTemplateColumn SummaryTemplate6 = new DataGridTemplateColumn();
            DataGridLength SummaryDGL6 = new DataGridLength(50d);
            SummaryTemplate6.CellTemplate = Summary6;
            SummaryTemplate6.Header = "カラム６";
            SummaryTemplate6.Width = SummaryDGL6;
            newdatagrid.Columns.Add(SummaryTemplate6);

            //DataGridTemplateColumn SummaryTemplate7 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL7 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary7;
            //SummaryTemplate.Header = "カラム７";
            //SummaryTemplate.Width = SummaryDGL7;
            //newdatagrid.Columns.Add(SummaryTemplate7);

            //DataGridTemplateColumn SummaryTemplate8 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL8 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary8;
            //SummaryTemplate.Header = "カラム８";
            //SummaryTemplate.Width = SummaryDGL8;
            //newdatagrid.Columns.Add(SummaryTemplate8);

            //DataGridTemplateColumn SummaryTemplate9 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL9 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary9;
            //SummaryTemplate.Header = "カラム９";
            //SummaryTemplate.Width = SummaryDGL9;
            //newdatagrid.Columns.Add(SummaryTemplate9);

            //DataGridTemplateColumn SummaryTemplate10 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL10 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary10;
            //SummaryTemplate.Header = "カラム１０";
            //SummaryTemplate.Width = SummaryDGL10;
            //newdatagrid.Columns.Add(SummaryTemplate10);

            //DataGridTemplateColumn SummaryTemplate11 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL11 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary11;
            //SummaryTemplate.Header = "カラム１１";
            //SummaryTemplate.Width = SummaryDGL11;
            //newdatagrid.Columns.Add(SummaryTemplate11);

            //DataGridTemplateColumn SummaryTemplate12 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL12 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary12;
            //SummaryTemplate.Header = "カラム１２";
            //SummaryTemplate.Width = SummaryDGL12;
            //newdatagrid.Columns.Add(SummaryTemplate12);

            //DataGridTemplateColumn SummaryTemplate13 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL13 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary13;
            //SummaryTemplate.Header = "カラム１３";
            //SummaryTemplate.Width = SummaryDGL13;
            //newdatagrid.Columns.Add(SummaryTemplate13);

            //DataGridTemplateColumn SummaryTemplate14 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL14 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary14;
            //SummaryTemplate.Header = "カラム１４";
            //SummaryTemplate.Width = SummaryDGL14;
            //newdatagrid.Columns.Add(SummaryTemplate14);

            //DataGridTemplateColumn SummaryTemplate15 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL15 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary15;
            //SummaryTemplate.Header = "カラム１５";
            //SummaryTemplate.Width = SummaryDGL15;
            //newdatagrid.Columns.Add(SummaryTemplate15);

            //DataGridTemplateColumn SummaryTemplate16 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL16 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary16;
            //SummaryTemplate.Header = "カラム１６";
            //SummaryTemplate.Width = SummaryDGL16;
            //newdatagrid.Columns.Add(SummaryTemplate16);

            //DataGridTemplateColumn SummaryTemplate17 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL17 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary17;
            //SummaryTemplate.Header = "カラム１７";
            //SummaryTemplate.Width = SummaryDGL17;
            //newdatagrid.Columns.Add(SummaryTemplate17);

            //DataGridTemplateColumn SummaryTemplate18 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL18 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary18;
            //SummaryTemplate.Header = "カラム１８";
            //SummaryTemplate.Width = SummaryDGL18;
            //newdatagrid.Columns.Add(SummaryTemplate18);

            //DataGridTemplateColumn SummaryTemplate19 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL19 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary19;
            //SummaryTemplate.Header = "カラム１９";
            //SummaryTemplate.Width = SummaryDGL19;
            //newdatagrid.Columns.Add(SummaryTemplate19);

            //DataGridTemplateColumn SummaryTemplate20 = new DataGridTemplateColumn();
            //DataGridLength SummaryDGL20 = new DataGridLength(50d);
            //SummaryTemplate.CellTemplate = Summary20;
            //SummaryTemplate.Header = "カラム２０";
            //SummaryTemplate.Width = SummaryDGL20;
            //newdatagrid.Columns.Add(SummaryTemplate20);


            #endregion

            ObservableCollection<シフト表カラム名> source = new ObservableCollection<シフト表カラム名>();
            CreateSource(source);

            newdatagrid.ItemsSource = source;

            stackpanel1.Children.Add(newdatagrid);

        }

        public void CreateSource(ObservableCollection<シフト表カラム名> source)
        {
            {
                シフト表カラム名 item = new シフト表カラム名();
                item.カラム１ = "";
                item.カラム２ = "阿部";
                item.カラム３ = "井上";
                item.カラム４ = "上田";
                item.カラム５ = "江口";
                item.カラム６ = "太田";

                source.Add(item);
            }

            {
                シフト表カラム名 item = new シフト表カラム名();
                item.カラム１ = "";
                item.カラム２ = "9-23";
                item.カラム３ = "";
                item.カラム４ = "12-23";
                item.カラム５ = "14-24";
                item.カラム６ = "○";

                source.Add(item);
            }


        }
    }

    public class シフト表カラム名
    {
        private string _カラム１;

        public string カラム１
        {
            get { return _カラム１; }
            set { _カラム１ = value; }
        }
        private string _カラム２;

        public string カラム２
        {
            get { return _カラム２; }
            set { _カラム２ = value; }
        }
        private string _カラム３;

        public string カラム３
        {
            get { return _カラム３; }
            set { _カラム３ = value; }
        }
        private string _カラム４;

        public string カラム４
        {
            get { return _カラム４; }
            set { _カラム４ = value; }
        }
        private string _カラム５;

        public string カラム５
        {
            get { return _カラム５; }
            set { _カラム５ = value; }
        }
        private string _カラム６;

        public string カラム６
        {
            get { return _カラム６; }
            set { _カラム６ = value; }
        }
        private string _カラム７;

        public string カラム７
        {
            get { return _カラム７; }
            set { _カラム７ = value; }
        }
        private string _カラム８;

        public string カラム８
        {
            get { return _カラム８; }
            set { _カラム８ = value; }
        }
        private string _カラム９;

        public string カラム９
        {
            get { return _カラム９; }
            set { _カラム９ = value; }
        }
        private string _カラム１０;

        public string カラム１０
        {
            get { return _カラム１０; }
            set { _カラム１０ = value; }
        }
        private string _カラム１１;

        public string カラム１１
        {
            get { return _カラム１１; }
            set { _カラム１１ = value; }
        }
        private string _カラム１２;

        public string カラム１２
        {
            get { return _カラム１２; }
            set { _カラム１２ = value; }
        }
        private string _カラム１３;

        public string カラム１３
        {
            get { return _カラム１３; }
            set { _カラム１３ = value; }
        }
        private string _カラム１４;

        public string カラム１４
        {
            get { return _カラム１４; }
            set { _カラム１４ = value; }
        }
        private string _カラム１５;

        public string カラム１５
        {
            get { return _カラム１５; }
            set { _カラム１５ = value; }
        }
        private string _カラム１６;

        public string カラム１６
        {
            get { return _カラム１６; }
            set { _カラム１６ = value; }
        }
        private string _カラム１７;

        public string カラム１７
        {
            get { return _カラム１７; }
            set { _カラム１７ = value; }
        }
        private string _カラム１８;

        public string カラム１８
        {
            get { return _カラム１８; }
            set { _カラム１８ = value; }
        }
        private string _カラム１９;

        public string カラム１９
        {
            get { return _カラム１９; }
            set { _カラム１９ = value; }
        }
        private string _カラム２０;

        public string カラム２０
        {
            get { return _カラム２０; }
            set { _カラム２０ = value; }
        }
    }
}
