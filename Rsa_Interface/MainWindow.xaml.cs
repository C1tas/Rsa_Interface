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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace Rsa_Interface
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        [DllImport("Rsa_DLL.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static System.Int64 random_prime(System.Int64 a, System.Int64 b);

        [DllImport("Rsa_DLL.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static System.Int64 GCD(System.Int64 a, System.Int64 b);

        [DllImport("Rsa_DLL.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static System.Int64 ext_gcd(System.Int64 a, System.Int64 b);

        [DllImport("Rsa_DLL.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static System.Int64 encode(System.Int64 m, System.Int64 e, System.Int64 modulus);

        [DllImport("Rsa_DLL.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static System.Int64 decode(System.Int64 m, System.Int64 e, System.Int64 modulus);

        [DllImport("Rsa_DLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        extern static int return_string(StringBuilder tmp_str, StringBuilder tmp2_str, StringBuilder tmp3_str);

        StringBuilder strShp = new StringBuilder(20480);
        StringBuilder ts_result = new StringBuilder(20480);
        StringBuilder mr_result = new StringBuilder(20480);
        Result_Buffer RS_Buffer = new Result_Buffer();
        process_buffer pc_buffer = new process_buffer();
        
        miller_rabin_buffer mr_buffer = new miller_rabin_buffer();
        thousand_buffer ts_buffer = new thousand_buffer();
        Int64 prime_1;
        Int64 prime_2;
        Int64 d, n, fain;
        public void initialize(Int64 sign_content)
        {
            
            while (true)
            {
                strShp.Clear();
                prime_1 = random_prime(1024, 2048);
                prime_2 = random_prime(65536, 131072);

                n = prime_1 * prime_2;
                
                fain = (prime_1 - 1) * (prime_2 - 1);
                
                if (GCD(17, fain) == 1)
                {
                    
                    d = ext_gcd(17, fain);
                    this.public_key.Text = "(" + fain.ToString() + "," + "17" + ")";
                    this.private_key.Text = "(" + fain.ToString() + "," + d.ToString() + ")";
                    
                    break;
                }

            }

            return_string(strShp, ts_result, mr_result);
            StringBuilder prime1_Shp = new StringBuilder("The random_prime_1 is " + prime_1.ToString() + ".\n");
            StringBuilder prime2_Shp = new StringBuilder("The random_prime_2 is " + prime_2.ToString() + ".\n");
            strShp.Append(prime1_Shp);
            strShp.Append(prime2_Shp);


        }
        public MainWindow()
        {
            pc_buffer.PassValuesEvent += new process_buffer.PassValuesHandler(ReceiveValues);
            InitializeComponent();
            
        }
        private void ReceiveValues(object sender, int e)
        {
            if (e == 1)
            {
                ts_buffer.show_buffer.Text = ts_result.ToString();
                ts_buffer.Show();
            }
            else if(e == 2)
            {
                mr_buffer.show_buffer.Text = mr_result.ToString();
                mr_buffer.Show();
            }
            else if(e == 3)
            {
                pc_buffer.prime_1.Content = prime_1.ToString();
                pc_buffer.prime_2.Content = prime_2.ToString();

            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void show_RS_Buffer_button_Click(object sender, RoutedEventArgs e)
        {
            //RS_Buffer.result_buffer.Text = strShp.ToString();
            mr_buffer.show_buffer.Text = mr_result.ToString();
            ts_buffer.show_buffer.Text = ts_result.ToString();
            //RS_Buffer.Show();
            pc_buffer.Show();
        }

        private void sign_button_Click(object sender, RoutedEventArgs e)
        {
            Int64 content;
            if (Int64.TryParse(sign_content.Text, out content) == false)
            {
                this.ShowMessageAsync("Error~", "没有输入，或者输入不是符合要求的数字,请输入范围在2^64以内");
                //MessageBox.Show("输入不是符合要求的数字请输入范围在2^64以内");
            }
            else
            {
                System.Int64 cipertext = encode(content, 17, n);
                this.signature_value.Text = cipertext.ToString();
            }
        }

        private void generate_key_button_Click(object sender, RoutedEventArgs e)
        {
            Int64 content;
            if (Int64.TryParse(sign_content.Text, out content) == false)
            {
                this.ShowMessageAsync("Error~", "没有输入，或者输入不是符合要求的数字,请输入范围在2^64以内");
                //MessageBox.Show("输入不是符合要求的数字请输入范围在2^64以内");
            }
            else
            {
                initialize(content);
            }
        }

        private void send_to_verify_Click(object sender, RoutedEventArgs e)
        {
            Int64 signature, content;
            if (Int64.TryParse(signature_value.Text, out signature) == false || Int64.TryParse(sign_content.Text, out content) == false)
            {
                this.ShowMessageAsync("Error~", "请在完成签名后使用此功能~");
                //MessageBox.Show("你输入的值，非符合要求的数值，数值范围2^64");
            }
            else
            {
                signature_verify.Text = signature_value.Text;
                content_verify.Text = sign_content.Text;
            }
            
        }

        private void Main_windows_close(object sender, EventArgs e)
        {
            System.Environment.Exit(0);

        }

        private void verify_button_Click(object sender, RoutedEventArgs e)
        {
            Int64 signature, content;
            if (Int64.TryParse(signature_verify.Text, out signature) == false || Int64.TryParse(content_verify.Text, out content) == false)
            {
                this.ShowMessageAsync("Error~", "没有输入或者，输入不是符合要求的数字,请输入范围在2^64以内");
                //MessageBox.Show("你输入的值，非符合要求的数值，数值范围2^64");
            }
            else
            {
                System.Int64 cleartext = decode(signature, d, n);
                if (cleartext == content)
                    verify_result.Text = "成功";
                else
                    verify_result.Text = "错误的签名";
            }
            
        }
    }

    public static class ConsoleManager
    {
        private const string Kernel32_DllName = "kernel32.dll";
        [DllImport(Kernel32_DllName)]
        private static extern bool AllocConsole();
        [DllImport(Kernel32_DllName)]
        private static extern bool FreeConsole();
        [DllImport(Kernel32_DllName)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport(Kernel32_DllName)]
        private static extern int GetConsoleOutputCP();
        public static bool HasConsole
        {
            get { return GetConsoleWindow() != IntPtr.Zero; }
        }
        /// Creates a new console instance if the process is not attached to a console already.    
        public static void Show()
        {
#if DEBUG
            if (!HasConsole)
            {
                AllocConsole();
                InvalidateOutAndError();
            }
#endif
        }
        /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.     
        public static void Hide()
        {
#if DEBUG
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }
#endif
        }
        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        static void InvalidateOutAndError()
        {
            Type type = typeof(System.Console);
            System.Reflection.FieldInfo _out = type.GetField("_out",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            System.Reflection.FieldInfo _error = type.GetField("_error",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            Debug.Assert(_out != null);
            Debug.Assert(_error != null);
            Debug.Assert(_InitializeStdOutError != null);
            _out.SetValue(null, null);
            _error.SetValue(null, null);
            _InitializeStdOutError.Invoke(null, new object[] { true });
        }
        static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}
