using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)// разворачиваем корабли, грузим изображения
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label1.Size = new Size(20, 80);
                label1.Location = new Point(comboBox1.Location.X + 1, 495);
                label1.Image = Image.FromFile(Application.StartupPath + @"\" + "4_.png");

                label2.Size = new Size(20, 60);
                label2.Location = new Point(comboBox1.Location.X + 70, 495);
                label2.Image = Image.FromFile(Application.StartupPath + @"\" + "3_.png");

                label3.Size = new Size(20, 40);
                label3.Location = new Point(comboBox1.Location.X + 142, 495);
                label3.Image = Image.FromFile(Application.StartupPath + @"\" + "2_.png");

                label4.Size = new Size(20, 20);
                label4.Location = new Point(comboBox1.Location.X + 142, 545);
                label4.Image = Image.FromFile(Application.StartupPath + @"\" + "1_.png");
            }
            else
            {
                label1.Size = new Size(80, 20);
                label1.Location = new Point(comboBox1.Location.X + 1, 496);
                label1.Image = Image.FromFile(Application.StartupPath + @"\" + "4.png");

                label2.Size = new Size(60, 20);
                label2.Location = new Point(comboBox1.Location.X + 1, 543);
                label2.Image = Image.FromFile(Application.StartupPath + @"\" + "3.png");

                label3.Size = new Size(40, 20);
                label3.Location = new Point(comboBox1.Location.X + 140, 503);
                label3.Image = Image.FromFile(Application.StartupPath + @"\" + "2.png");

                label4.Size = new Size(20, 20);
                label4.Location = new Point(comboBox1.Location.X + 143, 543);
                label4.Image = Image.FromFile(Application.StartupPath + @"\" + "1.png");
            }
        }

        
    }
}
