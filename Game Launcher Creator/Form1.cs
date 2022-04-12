using System.Diagnostics;
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace Game_Launcher_Creator
{
    public partial class Form1 : Form
    {
        public static Form1 f1;
        public Button b1;
        public Button b2;
        private bool mode = false;
        private Form2 f2 = new Form2();
        private Form4 f4 = new Form4();
        private Form5 f5 = new Form5();
        private OpenFileDialog pic = new OpenFileDialog();
        private FolderBrowserDialog open = new FolderBrowserDialog();
        private FolderBrowserDialog delete = new FolderBrowserDialog();

        //int mov;
        //int movX;
        //int movY;
        string Version;
        string Version3;
        int Version1;
        int Version2 = 704;
       // string news;

        Stopwatch stopwatch = new Stopwatch();
        WebClient DownloadManager;
        Boolean UpdateReady = false;
        Boolean reInstallReady = false;

        public Form1()
        {
            InitializeComponent();
            f1 = this;
            b1 = button2;
            b2 = button1;
            pictureBox1.SendToBack();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (mode == false)
            {
                ControlExtension.Draggable(pictureBox1, true);
                ControlExtension.Draggable(label2, true);
                ControlExtension.Draggable(label5, true);
                ControlExtension.Draggable(button3, true);
                ControlExtension.Draggable(progressBar1, true);
                ControlExtension.Draggable(button4, true);
                ControlExtension.Draggable(button5, true);
                ControlExtension.Draggable(button1, true);
            }
            else
            {
                ControlExtension.Draggable(pictureBox1, false);
                ControlExtension.Draggable(label2, false);
                ControlExtension.Draggable(label5, false);
                ControlExtension.Draggable(button3, false);
                ControlExtension.Draggable(progressBar1, false);
                ControlExtension.Draggable(button4, false);
                ControlExtension.Draggable(button5, false);
                ControlExtension.Draggable(button1, false);
            }

            this.Location = Screen.AllScreens[0].WorkingArea.Location;

            label2.Text = "0 MB/S";
            label5.Text = "Total size";

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

                try
                {
                    DownloadManager.DownloadFile(VersionLink, @"C:\Evolve\Version.txt");
                }
                catch (Exception Error)
                {
                    MessageBox.Show(Error.Message);
                }
            }
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
                button3.Text = "Re-Install";
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
                    button3.Text = "Update";
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

                Uri GameLink = new Uri(f5.t1.Text);

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
            button3.Text = "Re-Install";
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
            ControlExtension.Draggable(pictureBox1, true);
            ControlExtension.Draggable(label2, true);
            ControlExtension.Draggable(label5, true);
            ControlExtension.Draggable(button3, true);
            ControlExtension.Draggable(progressBar1, true);
            ControlExtension.Draggable(button4, true);
            ControlExtension.Draggable(button5, true);
            ControlExtension.Draggable(button1, true);
        }

        private void runModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = true;
            ControlExtension.Draggable(pictureBox1, false);
            ControlExtension.Draggable(label2, false);
            ControlExtension.Draggable(label5, false);
            ControlExtension.Draggable(button3, false);
            ControlExtension.Draggable(progressBar1, false);
            ControlExtension.Draggable(button4, false);
            ControlExtension.Draggable(button5, false);
            ControlExtension.Draggable(button1, false);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pic.ShowDialog();
            pictureBox1.Visible = true;
            pictureBox1.Image = Image.FromFile(pic.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mode == true)
            {
                if (File.Exists(@"C:\Evolve\Evolve\Evolve.exe"))
                {
                    if (UpdateReady == true)
                    {
                        MessageBox.Show("There is an update downlaoding");
                        DownloadGame();
                    }
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
            else
            {
                f5.Show();
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
            label2.Show();
            label5.Show();
            button3.Show();
        }

        private void webSiteNewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1.Show();
        }

        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (mode == false)
            {
                f4.Show();
            }
            else
            {
                try
                {
                    Process.Start(new ProcessStartInfo(f4.t1.Text) { UseShellExecute = true });
                }
                catch (Win32Exception w)
                {

                    MessageBox.Show(w.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (mode == false)
            {
                open.ShowDialog();

            }
            else
            {
                Process.Start("explorer.exe", open.SelectedPath);
            }
        }

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button5.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(mode == false)
            {
                delete.ShowDialog();
            }
            else
            {
                try
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to uninstall?", "uninstall", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Directory.Delete(delete.SelectedPath);
                    }
                }
                catch (Exception t)
                {
                    MessageBox.Show("The process failed: {0}", t.Message);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PlayButtonVisable = button2.Visible;
            Properties.Settings.Default.PlayButtonName = button2.Text;
            Properties.Settings.Default.PlayButtonFile = f2.ofd.FileName;
            Properties.Settings.Default.PlayButtonLocationX = button2.Location.X.ToString();
            Properties.Settings.Default.PlayButtonLocationY = button2.Location.Y.ToString();
            Properties.Settings.Default.PlayButtonImage = f2.pic1.FileName;
            Properties.Settings.Default.BackgroundPicture = pic.FileName;
            Properties.Settings.Default.BackgroundPictureVisable = pictureBox1.Visible;
            Properties.Settings.Default.UninstallLocationX = button5.Location.X.ToString();
            Properties.Settings.Default.UninstallLocationY = button5.Location.Y.ToString();
            Properties.Settings.Default.UninstallVisable = button5.Visible;
            Properties.Settings.Default.UninstallFolder = delete.SelectedPath;
            Properties.Settings.Default.NewsLocationX = button1.Location.X.ToString();
            Properties.Settings.Default.NewsLocationY = button1.Location.Y.ToString();
            Properties.Settings.Default.NewsVisable = button1.Visible;
            Properties.Settings.Default.NewsName = button1.Text;
            Properties.Settings.Default.NewsLink = f4.t1.Text;
            Properties.Settings.Default.OpenFileVisable = button4.Visible;
            Properties.Settings.Default.OpenFileLocationX = button4.Location.X.ToString();
            Properties.Settings.Default.OpenFileLocationY = button4.Location.Y.ToString();
            Properties.Settings.Default.OpenFileFolder = open.SelectedPath;
            Properties.Settings.Default.Label2Visable = label2.Visible;
            Properties.Settings.Default.Label2LocationX = label2.Location.X.ToString();
            Properties.Settings.Default.Label2LocationY = label2.Location.Y.ToString();
            Properties.Settings.Default.Label5Visable = label5.Visible;
            Properties.Settings.Default.Label5LocationX = label5.Location.X.ToString();
            Properties.Settings.Default.Label5LocationY = label5.Location.Y.ToString();
            Properties.Settings.Default.ProgressbarVisable = progressBar1.Visible;
            Properties.Settings.Default.ProgressbarLocationX = progressBar1.Location.X.ToString();
            Properties.Settings.Default.ProgressbarLocationY = progressBar1.Location.Y.ToString();
            Properties.Settings.Default.DownloadVisable = button3.Visible;
            Properties.Settings.Default.DownloadLocationX = button3.Location.X.ToString();
            Properties.Settings.Default.DownloadLocationY = button3.Location.Y.ToString();
            Properties.Settings.Default.DownloadLink = f5.t1.Text;
            Properties.Settings.Default.Save();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2.Visible = Properties.Settings.Default.PlayButtonVisable;
            button2.Text = Properties.Settings.Default.PlayButtonName;
            f2.ofd.FileName = Properties.Settings.Default.PlayButtonFile;
            button2.Location = new Point(Int32.Parse(Properties.Settings.Default.PlayButtonLocationX), Int32.Parse(Properties.Settings.Default.PlayButtonLocationY));
            if(Properties.Settings.Default.PlayButtonImage == string.Empty)
            {

            }
            else
            {
                button2.Image = Image.FromFile(Properties.Settings.Default.PlayButtonImage);
            }
            if (Properties.Settings.Default.BackgroundPicture == string.Empty)
            {

            }
            else
            {
                pictureBox1.Image = Image.FromFile(Properties.Settings.Default.BackgroundPicture);
            }
            pictureBox1.Visible = Properties.Settings.Default.BackgroundPictureVisable;
            button5.Visible = Properties.Settings.Default.UninstallVisable;
            button5.Location = new Point(Int32.Parse(Properties.Settings.Default.UninstallLocationX), Int32.Parse(Properties.Settings.Default.UninstallLocationY));
            open.SelectedPath = Properties.Settings.Default.UninstallFolder;
            button1.Visible = Properties.Settings.Default.NewsVisable;
            button1.Text = Properties.Settings.Default.NewsName;
            button1.Location = new Point(Int32.Parse(Properties.Settings.Default.NewsLocationX), Int32.Parse(Properties.Settings.Default.NewsLocationY));
            f4.t1.Text = Properties.Settings.Default.NewsLink;
            button4.Visible = Properties.Settings.Default.OpenFileVisable;
            button4.Location = new Point(Int32.Parse(Properties.Settings.Default.OpenFileLocationX), Int32.Parse(Properties.Settings.Default.OpenFileLocationY));
            open.SelectedPath = Properties.Settings.Default.OpenFileFolder;
            label2.Visible = Properties.Settings.Default.Label2Visable;
            label2.Location = new Point(Int32.Parse(Properties.Settings.Default.Label2LocationX), Int32.Parse(Properties.Settings.Default.Label2LocationY));
            label5.Visible = Properties.Settings.Default.Label5Visable;
            label5.Location = new Point(Int32.Parse(Properties.Settings.Default.Label5LocationX), Int32.Parse(Properties.Settings.Default.Label5LocationY));
            progressBar1.Visible = Properties.Settings.Default.ProgressbarVisable;
            progressBar1.Location = new Point(Int32.Parse(Properties.Settings.Default.ProgressbarLocationX), Int32.Parse(Properties.Settings.Default.ProgressbarLocationY));
            button3.Visible = Properties.Settings.Default.DownloadVisable;
            button3.Location = new Point(Int32.Parse(Properties.Settings.Default.DownloadLocationX), Int32.Parse(Properties.Settings.Default.DownloadLocationY));
            f5.t1.Text = Properties.Settings.Default.DownloadLink;
        }
    }
}