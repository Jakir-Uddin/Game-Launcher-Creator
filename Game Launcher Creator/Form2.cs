using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Launcher_Creator
{
    public partial class Form2 : Form
    {
        public static Form2 f2;
        public OpenFileDialog ofd = new OpenFileDialog();
        public Form2()
        {
            InitializeComponent();
            f2 = this;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Form1.f1.b1.Text = textBox1.Text;  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            Form1.f1.b1.Image = Image.FromFile(ofd.FileName);
        }
    }
}
