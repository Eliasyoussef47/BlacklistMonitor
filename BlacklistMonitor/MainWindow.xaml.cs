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
using BlacklistMonitor.Interop;
using Forms = System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;

namespace BlacklistMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherFrame _frame;
        BlockCursor CursorBlocker;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ManualResetEvent mre = new ManualResetEvent(false))
            {
                Thread thread = new Thread(() =>
                {
                    // Initiate the hook and the frame on this thread.
                    _frame = new DispatcherFrame(true);

                    CursorBlocker = new BlockCursor();
                    CursorBlocker.Start();

                    // Proceed with the caller method.
                    mre.Set();

                    // Start the message loop.
                    Dispatcher.PushFrame(_frame);
                })
                {
                    IsBackground = false,

                    // This thread should be more prioritized over UI thread and all others.
                    Priority = ThreadPriority.Highest
                };

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                mre.WaitOne();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CursorBlocker.Stop();
            _frame.Continue = false;
        }
    }
}
