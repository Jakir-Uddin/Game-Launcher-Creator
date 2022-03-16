using System.Diagnostics;

namespace Game_Launcher_Creator
{
    public partial class Form1 : Form
    {
        public static Form1 f1;
        public Button b1;
        private bool mode = false;
        private Form2 f2 = new Form2();
        private OpenFileDialog pic = new OpenFileDialog();
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
    }
}