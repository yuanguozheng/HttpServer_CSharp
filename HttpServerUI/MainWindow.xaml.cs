using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace HttpServerUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpServer.HttpServer http;
        private StringBuilder builder = new StringBuilder();
        private Thread thread;
        public MainWindow()
        {
            InitializeComponent();
            HttpServer.Utils.UILog log = new HttpServer.Utils.UILog();
            log.Recevied += log_Recevied;
            BT_START.Click += BT_START_Click;
            BT_STOP.Click += BT_STOP_Click;
            this.Closed += MainWindow_Closed;
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (thread != null)
                {
                    thread.Abort();
                }
            }
            catch
            {
            }
        }

        void BT_STOP_Click(object sender, RoutedEventArgs e)
        {
            if (http != null)
            {
                http.Stop();
            }
        }

        void BT_START_Click(object sender, RoutedEventArgs e)
        {
            http = new HttpServer.HttpServer(int.Parse(TB_PORT.Text), TB_DIR.Text, TB_DEFAULT.Text);
            thread = new Thread(http.Start);
            thread.Start();
        }

        void log_Recevied(string msg)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                builder.AppendLine(msg);
                TB_LOG.Text = builder.ToString();
                TB_LOG.ScrollToEnd();
            }));
        }
    }
}
