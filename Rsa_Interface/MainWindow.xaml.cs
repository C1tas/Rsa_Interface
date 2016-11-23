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
        extern static int return_string(StringBuilder tmp_str);

        StringBuilder strShp = new StringBuilder(20480);
        Result_Buffer RS_Buffer = new Result_Buffer();
        public void initialize()
        {
            while (true)
            {
                strShp.Clear();
                System.Int64 prime_1 = random_prime(1024, 2048);
                System.Int64 prime_2 = random_prime(65536, 131072);

                //Console.WriteLine("The random_prime_1 is " + prime_1.ToString());
                //Console.WriteLine("The random_prime_2 is " + prime_2.ToString());
                /*
                System.Int64 prime_1 = 1259;
                System.Int64 prime_2 = 80897;
                */
                System.Int64 n = prime_1 * prime_2;

                System.Int64 fain = (prime_1 - 1) * (prime_2 - 1);
                //Console.WriteLine("The result of Euler function is " + fain.ToString());
                if (GCD(17, fain) == 1)
                {
                    //Console.WriteLine("The result of GCD(fain, 17) == 1.");
                    System.Int64 d = ext_gcd(17, fain);
                    //this.public_key.Text = "(" + fain.ToString() + "," + "17" + ")";
                    //this.private_key.Text = "(" + fain.ToString() + "," + d.ToString() + ")";
                    this.public_key.Text = fain.ToString();
                    this.private_key.Text = d.ToString();
                    //this.private_key.Text = "123";
                    //Console.WriteLine("The num n is " + n.ToString());

                    //Console.WriteLine("The key d is " + d.ToString());
                    System.Int64 cipertext = encode(32655, 17, n);
                    this.signature_value.Text = cipertext.ToString();
                    //Console.WriteLine("The cleartext is 32655, and the corresponding cipertext is " + cipertext.ToString());
                    System.Int64 cleartext = decode(cipertext, d, n);
                    //Console.WriteLine("The cipertext is "+ cipertext.ToString() + " and the corresponding cleartext is " + cleartext.ToString() + ".");
                    break;
                }

            }

            //StringBuilder strShp = new StringBuilder(20480);

            return_string(strShp);

            //ConsoleManager.Show();
            //Console.Write(strShp);

        }
        public MainWindow()
        {
            
            InitializeComponent();
            
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void show_RS_Buffer_button_Click(object sender, RoutedEventArgs e)
        {
            RS_Buffer.result_buffer.Text = strShp.ToString();
            RS_Buffer.Show();
        }

        private void sign_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void generate_key_button_Click(object sender, RoutedEventArgs e)
        {
            initialize();
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
