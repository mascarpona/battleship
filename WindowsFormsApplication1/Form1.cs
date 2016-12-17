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
        bool flag = false; //проверка на зажатие кнопки мыши
        static int ship; //размерность корабля
        int ship_4 = 1; //количество четырехпалубных кораблей
        int ship_3 = 2; //количество трехпалубных кораблей
        int ship_2 = 3; //количество двухпалубных кораблей
        int ship_1 = 4; //количество однопалубных кораблей
        int x0, y0; //координаты места, куда навели мышью на поле пользователя
        static int x, y; //координаты предыдущего места корабля, чтобы его можно было стереть
        static bool vertikal = true;
        int count_k = 0; //количество убитых кораблей пользователем
        int count_i = 0; //количество убитых кораблей компьютером
        bool shoot_user = false; //доступность поля компьютера
        Random r = new Random();

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
                    user[i, j].DragOver += new DragEventHandler(my_DragOver);
                    user[i, j].Size = new Size(16, 16);
                    user[i, j].Location = new Point(i * 17 + 330, j * 17 + 280);
                    user[i, j].DragDrop += new DragEventHandler(label_drag);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //разворачиваем корабли, грузим изображения
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
            Clear(); //чистим поля
            generate(computer, false);
            comboBox1.Enabled = true;
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            label4.Enabled = true;
            button3.Visible = true;
            label6.Text = "Ready";
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                flag = true;
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            Label my = (Label)sender;
            if (flag)
            {
                ship = Convert.ToInt32(my.Tag);
                my.DoDragDrop(my, DragDropEffects.Copy);
                flag = false;
                // если курсор вышел за границы поля, чистим клетки
                for (int j = 0; j < 10; ++j)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        if ((int)user[i, j].Tag == 0) user[i, j].Image = null;
                    }
                }
            }

        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                flag = false;
        }

        void visible_ship() // досутпность и корл-во кораблей
        {
            if (ship == 3)// кол-во кораблей 3 - х парсных
                ship_3--;
            if (ship == 2)
                ship_2--;
            if (ship == 1)
                ship_1--;
            if (ship == 4)
                ship_4--;
            if (ship_1 == 0)
                label4.Enabled = false;
            if (ship_2 == 0)
                label3.Enabled = false;
            if (ship_3 == 0)
                label2.Enabled = false;
            if (ship_4 == 0)
                label1.Enabled = false;
        }

        void download_image(Label[,] mas, int count_ship, bool fl) // загрузка картинок вертикальное/горизрнтальное отображение
        {
            int im = count_ship; //размерность корабля
            if (fl)// вертикально
            {
                for (int j = y0; j < y0 + count_ship; ++j)
                {
                    switch (count_ship)
                    {
                        case 4:
                            mas[x0, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\4_\" + count_ship.ToString() + "_" + im.ToString() + ".png");
                            im--; break;
                        case 3:
                            mas[x0, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\3_\" + count_ship.ToString() + "_" + im.ToString() + ".png");
                            im--; break;
                        case 2:
                            mas[x0, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\2_\" + count_ship.ToString() + "_" + im.ToString() + ".png");
                            im--; break;
                        case 1:
                            mas[x0, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "1_.png");
                            break;
                        default: break;
                    }
                }
            }
            else // горизонтально
            {
                for (int i = x0; i < x0 + count_ship; ++i)
                {
                    switch (count_ship)
                    {
                        case 4:
                            mas[i, y0].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\4\" + count_ship.ToString() + "_" + im.ToString() + ".png");
                            im--; break;
                        case 3:
                            mas[i, y0].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\3\" + count_ship.ToString() + "_" + im.ToString() + ".png");
                            im--; break;
                        case 2:
                            mas[i, y0].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\2\" + count_ship.ToString() + "_" + im.ToString() + ".png");
                            im--; break;
                        case 1:
                            mas[i, y0].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "1.png");
                            break;
                    }
                }
            }
        }

        private void label_drag(object sender, DragEventArgs e) // загрузка картинок, задание значения в поле tag
        {
            if (comboBox1.SelectedIndex == 0)// вертикально
            {
                download_image(user, ship, true);// загрузка картинок
                buffer_zone(user, x0 - 1, x0 + 1, y0 - 1, y0 + ship, -1);
                for (int j = y0; j < y0 + ship; ++j)
                {
                    user[x0, j].Tag = ship;
                }
                visible_ship(); //  кол-во кораблей и их доступность
            }
            else // если горизонт.
            {
                download_image(user, ship, false);
                buffer_zone(user, x0 - 1, x0 + ship, y0 - 1, y0 + 1, -1);
                for (int i = x0; i < x0 + ship; ++i)
                {
                    user[i, y0].Tag = ship;
                }
                visible_ship();
            }
        }

        private void my_DragOver(object sender, DragEventArgs e)
        {
            // поиск места , над какой клеткой находится указатель мыши
            Point loc = location(sender, user);
            x0 = loc.X; y0 = loc.Y;
            if (comboBox1.SelectedIndex == 0)
            {
                if (vertikal)
                {
                    x = x0; y = y0;
                    vertikal = false;
                }
                int k = 0; // кол-во клеток не задействованных 
                if (y0 + ship <= 10 && (int)user[x0, y0].Tag == 0)
                {
                    for (int j = y0; j < y0 + ship; ++j)
                    {
                        if (Convert.ToInt32(user[x0, j].Tag) == 0) k++;
                    }
                    if (k == ship) // если место поустое
                    {
                        if (x == x0 && y == y0) // если в фокусе другая клетка, чистим предыдущий корабль
                        {
                            download_image(user, ship, true);
                            e.Effect = DragDropEffects.Copy;
                        }
                        else
                        {
                            try
                            {
                                vertikal = true;
                                e.Effect = DragDropEffects.None;
                                for (int j = y; j < y + ship; ++j)
                                {
                                    if (Convert.ToInt32(user[x, j].Tag) == 0)
                                        user[x, j].Image = null;
                                }
                            }
                            catch (IndexOutOfRangeException) { }
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                        try
                        {
                            for (int j = y; j < y + ship; ++j)
                            {
                                if (Convert.ToInt32(user[x, j].Tag) == 0)
                                    user[x, j].Image = null;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            // горизоньально
            ////////////////////////////////////////////////
            else
            {
                if (vertikal)
                {
                    x = x0; y = y0;
                    vertikal = false;
                }
                int k = 0; // кол-во клеток не задействованных 
                if (x0 + ship <= 10 && (int)user[x0, y0].Tag == 0)
                {
                    for (int i = x0; i < x0 + ship; ++i)
                    {
                        if (Convert.ToInt32(user[i, y0].Tag) == 0) k++;
                    }
                    if (k == ship) // если место поустое
                    {
                        if (x == x0 && y == y0) // если в фокусе другая клетка, чистим предыдущий корабль
                        {
                            download_image(user, ship, false);
                            e.Effect = DragDropEffects.Copy;
                        }
                        else
                        {
                            try
                            {
                                vertikal = true;
                                e.Effect = DragDropEffects.None;
                                for (int i = x; i < x + ship; ++i)
                                {
                                    if (Convert.ToInt32(user[i, y].Tag) == 0)
                                        user[i, y].Image = null;
                                }

                            }
                            catch (IndexOutOfRangeException) { }

                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                        try
                        {
                            for (int i = x; i < x + ship; ++i)
                            {
                                if (Convert.ToInt32(user[i, y].Tag) == 0)
                                    user[i, y].Image = null;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        private Point location(object sender, Label[,] mas) // возращем место, где пользователь кликнул
        {
            Point place = new Point();
            bool click = false; // клик на клекте , поиск места где кликнули
            for (int j = 0; j < 10; ++j)
            {
                for (int i = 0; i < 10; ++i)
                {
                    if (mas[i, j] == sender)
                    {
                        place.X = i; place.Y = j;
                        click = true;
                        break;
                    }
                }
                if (click) break;
            }
            return place;
        }

        void buffer_zone(Label[,] mas, int n0, int n1, int m0, int m1, int value) // задаем значение около кораблей 
        {
            for (int j = m0; j <= m1; ++j)
            {
                for (int i = n0; i <= n1; ++i)
                {
                    try
                    {
                        mas[i, j].Tag = value;//значение в клекте, -1 и -2 может быть
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }
        }

        private void generate(Label[,] mas, bool visible) // генерирование расположения кораблей/ bool visible грузить ли картинки т.к. для поля противника мне не грузим кораблики
        {
            int position = 0;
            int[] count_ship = new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            Random r = new Random();
            while (true)
            {
                int random; // вертикально или горизрнтально
            loop: x0 = r.Next(0, 10);
                y0 = r.Next(0, 10);
                random = r.Next(0, 2);
                int k = 0; // кол-во не занятых клеток должно равняться размерности кораблика
                if (random == 1)
                {
                    if (y0 + count_ship[position] <= 10 && (int)mas[x0, y0].Tag == 0)
                    {
                        for (int j = y0; j < y0 + count_ship[position]; ++j)
                        {
                            if ((int)mas[x0, j].Tag == 0) k++;
                        }
                        if (k == count_ship[position])
                        {
                            if (visible)
                                download_image(mas, count_ship[position], true);
                            buffer_zone(mas, x0 - 1, x0 + 1, y0 - 1, y0 + count_ship[position], -1);
                            for (int j = y0; j < y0 + count_ship[position]; ++j)
                            {
                                if (visible)
                                {
                                    mas[x0, j].Tag = count_ship[position];
                                }
                                else
                                {
                                    if (position == 1)
                                        mas[x0, j].Tag = -3;
                                    else
                                        mas[x0, j].Tag = count_ship[position];
                                }

                            }
                            position++;
                            if (position == 10) break;
                        }
                        else
                            goto loop;
                    }
                    else
                        goto loop;
                }
                ///// горизонтально размещаем корабли
                else
                {
                    if (x0 + count_ship[position] <= 10 && (int)mas[x0, y0].Tag == 0)
                    {

                        for (int i = x0; i < x0 + count_ship[position]; ++i)
                        {
                            if ((int)mas[i, y0].Tag == 0) k++;
                        }
                        if (k == count_ship[position])
                        {
                            if (visible)
                                download_image(mas, count_ship[position], false);
                            buffer_zone(mas, x0 - 1, x0 + count_ship[position], y0 - 1, y0 + 1, -1);
                            for (int i = x0; i < x0 + count_ship[position]; ++i)
                            {
                                if (visible)
                                    mas[i, y0].Tag = count_ship[position];
                                else
                                {
                                    if (position == 1)
                                        mas[i, y0].Tag = -3;
                                    else
                                        mas[i, y0].Tag = count_ship[position];
                                }
                            }
                            position++;
                            if (position == 10) break;
                        }
                        else
                            goto loop;
                    }
                    else
                        goto loop;
                }
            }
        }

        void Clear() //очитска полей, задание начальных значений
        {
            for (int j = 0; j < 10; ++j)
            {
                for (int i = 0; i < 10; ++i)
                {
                    user[i, j].Tag = 0;
                    computer[i, j].Tag = 0;
                    user[i, j].Image = null;
                    computer[i, j].Image = null;
                    computer[i, j].Enabled = true;
                }
            }
            count_i = 0;
            count_k = 0;
            label9.Text = "Killed: " + count_k.ToString();
            label8.Text = "Killed: " + count_i.ToString();
            ship_1 = 4;
            ship_2 = 3;
            ship_3 = 2;
            ship_4 = 1;
            checkBox1.Checked = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = true;
            shoot_user = true; // игрок может стрелять
            label6.Text = "READY";
            //label6.Location = new Point(computer[4, 0].Location.X + 20 - label6.Size.Width / 2, 60);
            Clear(); // чистим поля
            comboBox1.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            button3.Enabled = false;
            generate(user, true);
            generate(computer, false);

            DialogResult dr = MessageBox.Show("Generate again?", "Placement of the ships", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
                button5_Click(sender, e);
            else
            {
                label7.Visible = true;
                //label6.Text = "Поле противника";
                //label6.Location = new Point(computer[4, 0].Location.X + 20 - label6.Size.Width / 2, 60);
            }
        }

    }
}
