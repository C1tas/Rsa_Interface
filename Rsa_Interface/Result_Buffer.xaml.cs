﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Rsa_Interface
{
    /// <summary>
    /// Result_Buffer.xaml 的交互逻辑
    /// </summary>
    public partial class Result_Buffer : MetroWindow
    {
        public Result_Buffer()
        {
            InitializeComponent();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void result_buffer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
