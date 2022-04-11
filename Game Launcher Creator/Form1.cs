using System.Diagnostics;
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Game_Launcher_Creator
{
    public partial class Form1 : Form
    {
        public static Form1 f1;
        public Button b1;
        private bool mode = false;
        private Form2 f2 = new Form2();
        private OpenFileDialog pic = new OpenFileDialog();

        int mov;
        int movX;
        int movY;
        string Version;
        string Version3;
        int Version1;
        int Version2 = 704;
        string news;

        Stopwatch stopwatch = new Stopwatch();
        WebClient DownloadManager;
        Boolean UpdateReady = false;
        Boolean reInstallReady = false;

        public Form1()
        {
            InitializeComponent();
            f1 = this;
            b1 = button2;
            pictureBox1.SendToBack();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (mode == false)
            {
                ControlExtension.Draggable(button1, true);
                ControlExtension.Draggable(pictureBox1, true);
            }
            else
            {
                ControlExtension.Draggable(button1, false);
                ControlExtension.Draggable(pictureBox1, false);
            }

            this.Location = Screen.AllScreens[0].WorkingArea.Location;

            label2.Text = "";
            label5.Text = "";

            if (Directory.Exists(@"C:\Evolve"))
            {

            }
            else
            {
                Directory.CreateDirectory(@"C:\Evolve");
            }
            if (File.Exists(@"C:\Evolve\Version.txt"))
            {
                File.Delete(@"C:\Evolve\Version.txt");
            }
            if (File.Exists(@"C:\Evolve\CurrentVersion.txt"))
            {

            }
            else
            {
                File.Create(@"C:\Evolve\CurrentVersion.txt");
            }
            using (DownloadManager = new WebClient())
            {
                Uri VersionLink = new Uri("https://dl.dropbox.com/s/sqh72wguyq2la7w/Version.txt?dl=1");
                Uri newsLink = new Uri("https://dl.dropbox.com/s/y2mwznidhu6ehme/News.txt?dl=1");

                try
                {
                    DownloadManager.DownloadFile(VersionLink, @"C:\Evolve\Version.txt");
                    DownloadManager.DownloadFile(newsLink, @"C:\Evolve\News.txt");
                }
                catch (Exception Error)
                {
                    MessageBox.Show(Error.Message);
                }
            }
            news = File.ReadAllText(@"C:\Evolve\News.txt");
            label4.Text = news;
            Version = File.ReadAllText(@"C:\Evolve\Version.txt");
            try
            {
                Version3 = File.ReadAllText(@"C:\Evolve\CurrentVersion.txt");
            }
            catch (Exception)
            {

            }
            Int32.TryParse(Version, out Version1);
            Int32.TryParse(Version3, out Version2);
            if (Version1 == Version2)
            {
                Properties.Settings.Default.CurrentVersion = File.ReadAllText(@"C:\Evolve\Version.txt");
                File.Delete(@"C:\Evolve\Version.txt");
                button2.Text = "Re-Install";
                reInstallReady = true;
            }
            else
            {
                if (Properties.Settings.Default.CurrentVersion == File.ReadAllText(@"C:\Evolve\Version.txt"))
                {

                }
                else
                {
                    UpdateReady = true;
                }
            }
        }

        void DownloadGame()
        {
            using (DownloadManager = new WebClient())
            {
                if (File.Exists(@"C:\Evolve\Evolve\Evolve.exe"))
                {
                    File.Delete(@"C:\Evolve\Evolve\Evolve.exe");
                }
                if (Directory.Exists(@"C:\Evolve\Evolve\"))
                {
                    Directory.Delete(@"C:\Evolve\Evolve\", true);
                }
                DownloadManager.DownloadProgressChanged += new DownloadProgressChangedEventHandler(UpdateValues);
                DownloadManager.DownloadFileCompleted += new AsyncCompletedEventHandler(Done);

                Uri GameLink = new Uri("https://dl.dropbox.com/s/qbxd0ezvnkq6n6z/Evolve.zip?dl=1");

                stopwatch.Start();

                try
                {
                    DownloadManager.DownloadFileAsync(GameLink, @"C:\Evolve\Game.zip");
                }
                catch (Exception Error)
                {
                    MessageBox.Show(Error.Message);
                }
            }
        }

        void UpdateValues(object sender, DownloadProgressChangedEventArgs DPCEA)
        {
            progressBar1.Value = DPCEA.ProgressPercentage;
            label2.Text = string.Format("{0} MB/s", (DPCEA.BytesReceived / 1048576d / stopwatch.Elapsed.TotalSeconds).ToString("0"));
            label5.Text = string.Format("{0} MB Total", (DPCEA.TotalBytesToReceive / 1048576d).ToString("0"));
        }

        void Done(object sender, AsyncCompletedEventArgs ACEA)
        {
            stopwatch.Reset();

            if (ACEA.Cancelled == false)
            {
                label2.Text = "Extracting";
                ZipFile.ExtractToDirectory(@"C:\Evolve\Game.zip", @"C:\Evolve");
                File.Delete(@"C:\Evolve\Game.zip");
                ZipFile.ExtractToDirectory(@"C:\Evolve\Evolve.zip", @"C:\Evolve");
                File.Delete(@"C:\Evolve\Evolve.zip");
                Version2 = Version1;
                string path = @"C:\Evolve\CurrentVersion.txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                    try
                    {
                        TextWriter tw = new StreamWriter(path);
                        tw.WriteLine("704");
                        tw.Close();
                    }
                    catch (Exception)
                    {

                    }
                }
                else if (File.Exists(path))
                {
                    File.WriteAllText(@"C:\Evolve\CurrentVersion.txt", Version2.ToString());
                }
                File.Delete(@"C:\Evolve\Version.txt");
                MessageBox.Show("Download Completed!");
            }
            else
            {
                MessageBox.Show("Download has failed");
            }
            button2.Text = "Re-Install";
            reInstallReady = true;
            UpdateReady = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(mode == false)
            {
                f2.Show();
                ControlExtension.Draggable(button2, true);
            }
            else
            {
                ControlExtension.Draggable(button2, false);
                try
                {
                    Process.Start(f2.ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void assetsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Button b1 = new Button();
            //b1.Size = new Size(100,100);
            //b1.Show();
            button2.Show();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void publishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void editModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = false;
        }

        private void runModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pic.ShowDialog();
            pictureBox1.Image = Image.FromFile(pic.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\Evolve\Evolve\Evolve.exe"))
            {
                if (UpdateReady == true)
                {
                    MessageBox.Show("There is an update downlaoding");
                    DownloadGame();
                }
                //else
                //{
                //Process.Start(@"C:\Evolve\Evolve\Evolve.exe");
                //}
            }
            else
            {
                DownloadGame();
            }

            if (reInstallReady == true)
            {
                DownloadGame();
            }
        }
    }
}