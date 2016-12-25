using System;
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
    /// process_buffer.xaml 的交互逻辑
    /// </summary>
    public partial class process_buffer : MetroWindow
    {
        public delegate void PassValuesHandler(object sender, int e);
        public event PassValuesHandler PassValuesEvent;
        public process_buffer()
        {
            InitializeComponent();
        }
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void thousand_button_Click(object sender, RoutedEventArgs e)
        {
            PassValuesEvent(this, 1);
        }

        private void miller_rabin_button_Click(object sender, RoutedEventArgs e)
        {
            PassValuesEvent(this, 2);
        }

        private void show_button_Click(object sender, RoutedEventArgs e)
        {
            PassValuesEvent(this, 3);
        }
    }
}
