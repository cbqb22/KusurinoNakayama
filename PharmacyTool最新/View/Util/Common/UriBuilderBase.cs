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

namespace View.Util.Common
{
    public class UriBuilderBase : UriBuilder
    {
        public UriBuilderBase(string uri)
            : base(uri)
        {
        }

        private string _Filepath;

        public string Filepath
        {
            get { return _Filepath; }
            set { _Filepath = value; }
        }

    }
}
