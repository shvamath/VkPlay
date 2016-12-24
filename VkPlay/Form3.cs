using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkPlay
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //Пытаемся разлогиниться
            WebBrowser webBrowser1 = new WebBrowser();
            webBrowser1.Navigate("https://login.vk.com/?act=logout&hash=14466908cac58bbe4b&_origin=http://vk.com");
            //Ждем,пока все операции завершатся
            while (webBrowser1.IsBusy == true)
            {
                Application.DoEvents();
            }
            //Переходим на страничку входа
        }

        private void Form3_Load(object sender, EventArgs e)
        {
 
          
        }
    }
}
