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
using クスリのナカヤマ薬局ツール.共通.Printing;

namespace クスリのナカヤマ薬局ツール.Windows
{
    /// <summary>
    /// DocumentViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class DocumentViewer : Window
    {
        UIElement _uielement = null;
        PrintCenter pc = null;

        public DocumentViewer(UIElement ui)
        {
            this._uielement = ui;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DocumentViewer_Loaded);

            this.Closed += new EventHandler(DocumentViewer_Closed);
        }

        void DocumentViewer_Closed(object sender, EventArgs e)
        {
            // 印刷UIを解放する
            if (this.pc != null)
            {
                pc.RemoveFixedPageChild();
            }


        }

        void DocumentViewer_Loaded(object sender, RoutedEventArgs e)
        {
            //pc = new PrintCenter(this.documentViewer, _uielement);
        }


    }
}
