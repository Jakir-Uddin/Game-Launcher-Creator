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
    public partial class Form4 : Form
    {
        public static Form4 f4;
        public TextBox t1;
        public Form4()
        {
            InitializeComponent();
            f4 = this;
            t1 = textBox2;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Form1.f1.b2.Text = textBox1.Text;
        }
    }
}
