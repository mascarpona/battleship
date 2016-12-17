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
        /*
         0 - пустая клетка
        -1 - клетка около корабля
         1 - однопалубный корабль
         2 - двухпалубный корабль
         3 - трехпалубный корабль
         4 - четырехпалубный корабль
        */
        Label[,] computer; //игровое поле компьютера
        Label[,] user; //игровое поле пользователя
        Label[] resp1, numb1, resp2, numb2; //массивы букв и цифр
        char[] c = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

        public Form1()
        {
            computer = new Label[10, 10];
            user = new Label[10, 10];
            resp1 = new Label[10];
            numb1 = new Label[10];
            resp2 = new Label[10];
            numb2 = new Label[10];
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "vertical";
            for (int j = 0; j < 10; ++j)
            {
                for (int i = 0; i < 10; ++i)
                {
                    user[i, j] = new Label();
                    computer[i, j] = new Label();
                    computer[i, j].Cursor = Cursors.Hand;
                    user[i, j].Tag = 0;
                    user[i, j].AutoSize = false;
                    user[i, j].AllowDrop = true;
                    user[i, j].Size = new Size(16, 16);
                    user[i, j].Location = new Point(i * 17 + 330, j * 17 + 280);
                    user[i, j].BackColor = Color.LightBlue;
                    computer[i, j].BackColor = Color.LightBlue;
                    this.Controls.Add(user[i, j]);
                    computer[i, j].Tag = 0;
                    computer[i, j].AutoSize = false;
                    computer[i, j].AllowDrop = true;
                    computer[i, j].Size = new Size(16, 16);
                    computer[i, j].Location = new Point(i * 17 + 330, j * 17 + 50);
                    this.Controls.Add(computer[i, j]);
                }
            }
            for (int i = 0; i < 10; ++i)
            {
                resp1[i] = new Label();
                resp2[i] = new Label();
                resp1[i].AutoSize = true;
                resp2[i].AutoSize = true;
                resp1[i].Size = new Size(10, 10);
                resp2[i].Size = new Size(10, 10);
                resp2[i].Location = new Point(i * 17 + 330, 260);
                resp1[i].Location = new Point(i * 17 + 330, 30);
                numb1[i] = new Label();
                numb2[i] = new Label();
                numb1[i].AutoSize = true;
                numb2[i].AutoSize = true;
                numb1[i].Size = new Size(10, 10);
                numb2[i].Size = new Size(10, 10);
                numb1[i].Location = new Point(310, i * 17 + 52);
                numb2[i].Location = new Point(310, i * 17 + 282);
                numb1[i].Text = numb2[i].Text = Convert.ToString(i + 1);
                resp1[i].Visible = resp2[i].Visible = numb1[i].Visible = numb2[i].Visible = true;
                resp2[i].Text = resp1[i].Text = c[i].ToString();
                this.Controls.AddRange(new Control[] { resp1[i], resp2[i], numb1[i], numb2[i] });

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)// разворачиваем корабли, грузим изображения
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label1.Size = new Size(20, 80);
                label1.Location = new Point(comboBox1.Location.X + 1, 495);
                label1.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "4_.png");

                label2.Size = new Size(20, 60);
                label2.Location = new Point(comboBox1.Location.X + 70, 495);
                label2.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "3_.png");

                label3.Size = new Size(20, 40);
                label3.Location = new Point(comboBox1.Location.X + 142, 495);
                label3.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "2_.png");

                label4.Size = new Size(20, 20);
                label4.Location = new Point(comboBox1.Location.X + 142, 545);
                label4.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "1_.png");
            }
            else
            {
                label1.Size = new Size(80, 20);
                label1.Location = new Point(comboBox1.Location.X + 1, 496);
                label1.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "4.png");

                label2.Size = new Size(60, 20);
                label2.Location = new Point(comboBox1.Location.X + 1, 543);
                label2.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "3.png");

                label3.Size = new Size(40, 20);
                label3.Location = new Point(comboBox1.Location.X + 140, 503);
                label3.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "2.png");

                label4.Size = new Size(20, 20);
                label4.Location = new Point(comboBox1.Location.X + 143, 543);
                label4.Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "1.png");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;
            button7.Enabled = true;
            button6.Enabled = false;
            button5.Enabled = false;
            comboBox1.Enabled = true;
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            label4.Enabled = true;
            button3.Visible = true;
            label6.Text = "Ready";
        }

    }
}