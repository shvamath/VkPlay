using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace VkPlay
{
    public partial class Form1 : Form
    {
        public List<Audio> audioList;
        string Path="";
        WMPLib.IWMPPlaylist PlayList;
        WMPLib.IWMPMedia Media;
        public class Audio
        {
            public int aid { get; set; }
            public int owner_id { get; set; }
            public string artist { get; set; }
            public string title { get; set; }
            public int duration { get; set; }
            public string url { get; set; }
            public string lurics_id { get; set; }
            public int genre { get; set; }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Form2().Show();
            backgroundWorker1.RunWorkerAsync();
        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!Settings1.Default.auth) { Thread.Sleep(500); }

            WebRequest request = WebRequest.Create("https://api.vk.com/method/audio.get?owner_id=" + Settings1.Default.id + "&needed_user=0&access_token=" + Settings1.Default.token);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close(); response.Close();
            responseFromServer = HttpUtility.HtmlDecode(responseFromServer);

            JToken token = JToken.Parse(responseFromServer);
            audioList = token["response"].Children().Skip(1).Select(c => c.ToObject<Audio>()).ToList();

            this.Invoke((MethodInvoker)delegate
            {
                PlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("VkPlay");
                for (int i = 0; i < audioList.Count(); i++)
                {
                    Media = axWindowsMediaPlayer1.newMedia(audioList[i].url);
                    PlayList.appendItem(Media);
                    listBox1.Items.Add(audioList[i].artist + " - " + audioList[i].title);
                }
                axWindowsMediaPlayer1.currentPlaylist = PlayList;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            });
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path1;
            path1 = Path;
            if (listBox1.SelectedIndex == -1) { MessageBox.Show("Choose Audio file", "Oooooops..."); return; }
            if (path1 != "")
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    path1 = Path;
                    if (listBox1.GetSelected(i))
                    {
                        path1 += @"\" + audioList[i].title + ".mp3";
                        string Source = @"C:\Users\Matthew\Documents\Visual Studio 2015\Projects\VkPlay\VkPlay\bin\Debug\" + audioList[i].title + ".mp3";
                        if (File.Exists(path1)) { MessageBox.Show("It was already uploaded", "Ooooops..."); return; }
                        string link = audioList[i].url;
                        WebClient webClient = new WebClient();
                        webClient.DownloadFileAsync(new Uri(link), path1);
                    }
                }
            }
            else MessageBox.Show("Choose the Path", "Oooooooooops...");
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.Ctlcontrols.currentItem = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox1.SelectedIndex);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            Path = folderBrowserDialog1.SelectedPath;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Would you like top log out from your account", "Message", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            { 
                e.Cancel = false;
            }
            else e.Cancel = true;
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {

        }


    }
}