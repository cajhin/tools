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

using System.Net;


namespace dazMakeLib
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> productIds = new List<int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbLog.Text += "\r\nsearching HTML for IDs...";
            string html = tbHtml.Text;
            string marker = "\"product_id\":\"";
            int idx = 0;
            while((idx = html.IndexOf(marker, idx)) > 0)
            {
                idx += marker.Length;
                int idx2 = html.IndexOf('\"', idx);
                string strPid = "";
                try
                {
                    strPid=html.Substring(idx, idx2 - idx);
                    int pid = Int32.Parse(strPid);
                    productIds.Add(pid);
                }
                catch 
                {
                    tbLog.Text += "\r\nNaN: " + strPid;
                }
            }
            tbLog.Text += "\r\n\r\nFound: " + productIds.Count + " product IDs";
        }

        private void btDownload_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0;i<productIds.Count;i++)
            {
                //dbg limit
                if (i > 3)
                    return;


                using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                {

                    // Or you can get the file content without saving it
                    string htmlCode = client.DownloadString("https://www.daz3d.com/downloader/customer/files#prod_"+productIds[i]);
                    int idx = htmlCode.IndexOf("https://gcdn.daz3d.com");
                    int idx2 = htmlCode.IndexOf("\",idx");
                    string imgUrl = htmlCode.Substring(idx, idx2 - idx);

                    client.DownloadFile("imgUrl", @"C:\tmp\daz\"+productIds[i]+".jpg");

                }
            }
        }
    }
}
