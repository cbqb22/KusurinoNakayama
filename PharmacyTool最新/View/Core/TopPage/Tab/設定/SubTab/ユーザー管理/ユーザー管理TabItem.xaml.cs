﻿using System;
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

namespace View.Core.TopPage.Tab.設定.SubTab.ユーザー管理
{
    public partial class ユーザー管理TabItem : UserControl
    {
        public ユーザー管理TabItem()
        {
            InitializeComponent();

            SingletonInstances.ユーザー管理TabItemInstance = this;
        }
    }
}
