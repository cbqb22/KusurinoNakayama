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
using System.Collections.ObjectModel;

namespace View
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            Core.共通.SingletonInstances.Instance = this;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

    }
}
