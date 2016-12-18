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
        int a_ = 0, b_ = 0; //координаты прострела 
        bool exists = true; //задавать новые координаты для прострела, или стрелять по старым т.к. там подбит корабль 
        bool vert = true, horizont = true; //размещение корабля при прострелке компьютера 
        //для трехпалубного корабля проверяем возможно ли корабль был подбит до этого 
        bool horiz_3_left = true;
        bool horiz_3_rigth = true;
        bool vertic_3_vverh = true;
        bool vertic_3_vniz = true;
        bool horizont_4_2_left = true; //стреляем влево 
        bool hotizont_4_1_left = true; //стреляем влево 
        bool horizont_4_2_rigth = true; //стреляем вправо 
        bool hotizont_4_1_rigth = true; //стреляем вправо 
        bool vert_4_2_up = true; //стреляем вверх 
        bool vert_4_1_up = true; //стреляем вверх 
        bool vert_4_2_down = true; //стреляем вниз 
        bool vert_4_1_down = true; //стреляем вниз 
        bool ranen = true; //выдача сообщения о ранении 
        int time = 30; //задержка стрельбы компа 

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
                    computer[i, j].MouseClick += new MouseEventHandler(click_komp);
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
            Clear(); // чистим поля 
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
                            mas[x0, j].Image
                            = Image.FromFile(Application.StartupPath + @"\..\..\Resources\2_\" + count_ship.ToString() + "_" + im.ToString() + ".png");
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
                visible_ship(); // кол-во кораблей и их доступность 
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

        private Point location(object
        sender, Label[,] mas) // возращем место, где пользователь кликнул 
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

        void Clear() // очитска полей, задание начальных значений 
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
            ranen = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = true;
            shoot_user = true; // игрок может стрелять 
            label6.Text = "Ready";
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
                label6.Text = "Enemy's zone";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!label1.Enabled &&
            !label2.Enabled && !label3.Enabled && !label4.Enabled)
            {
                button3.Enabled = false;
                button4.Enabled = false;
                shoot_user = true; // игрок может стрелять 
                label6.Text = "Enemy's zone";
                label7.Visible = true;
                label1.Enabled = false;
                label2.Enabled = false;
                label3.Enabled = false;
                label4.Enabled = false; ;
                comboBox1.Enabled = false;
                button7.Text = "Finish";
                checkBox1.Enabled = true;
            }
            else
                MessageBox.Show("Not all the ships are placed!", "Warning");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label6.Text = "Enemy's zone";
            checkBox1.Enabled = false;
            button7.Enabled = false;
            timer1.Stop();
            label7.Text = "Your turn";
            shoot_user = false;
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            Clear(); // чистим поля, задаем значения 
            label7.Visible = false;
            comboBox1.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)// пользователь чистит поле, корабли снова доступны 
        {
            for (int j = 0; j < 10; ++j)
            {
                for (int i = 0; i < 10; ++i)
                {
                    user[i, j].Image = null;
                    user[i, j].Tag = 0;
                }
            }
            ship_3 = 2;
            ship_2 = 3;
            ship_1 = 4;
            ship_4 = 1;
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            label4.Enabled = true;
        }

        void hit_bloomer(Label[,] mas, int a, int b, bool fl) // загружаем картинку, попал/промах 
        {
            if (fl)
                mas[a, b].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "попал.png");
            else
                mas[a, b].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "промах.png");
        }

        bool check(Label[,] mas, int tg)// убит или ранен корабль:4,3,-3 
        {
            bool bl = false;
            for (int j = 0; j < 10; ++j)
            {
                for (int i = 0; i < 10; ++i)
                {
                    if ((int)computer[i, j].Tag == tg)
                    {
                        bl = true;
                        break;
                    }
                }
                if (bl) break;
            }
            return bl;
        }

        private void click_komp(object sender, MouseEventArgs e)
        {
            Point click = location(sender, computer);// координаты клика пользовтеля 
            if (checkBox1.Checked)
            {
                if ((int)computer[click.X, click.Y].Tag != 5)
                    hit_bloomer(computer, click.X, click.Y, false);
            }
            else
                if (shoot_user)
                {
                    shoot_user = false;
                    switch ((int)computer[click.X, click.Y].Tag)
                    {
                        case 1:
                            label7.Text = "Killed";
                            computer[click.X, click.Y].Tag = 5;
                            label9.Text = "Killed: " + Convert.ToString(++count_k);
                            computer[click.X, click.Y].Tag = 5;
                            hit_bloomer(computer, click.X, click.Y, true);
                            shoot_user = true;
                            if (count_k == 10)
                            {
                                MessageBox.Show("You win!");
                                {
                                    shoot_user = false;
                                    label7.Text = "Victory";
                                }
                            }
                            break;
                        case 2:
                            int k = 0;
                            computer[click.X, click.Y].Tag = 5;
                            hit_bloomer(computer, click.X, click.Y, true);
                            for (int j = click.Y - 1; j <= click.Y + 1; ++j)
                            {
                                for (int i = click.X - 1; i <= click.X + 1; ++i)
                                {
                                    try
                                    {
                                        if ((int)computer[i, j].Tag == 5)
                                            k++;
                                    }
                                    catch (IndexOutOfRangeException) { }
                                }
                            }

                            if (k == 2)
                            {
                                label7.Text = "Killed";
                                hit_bloomer(computer, click.X, click.Y, true);
                                label9.Text = "Killed: " + Convert.ToString(++count_k);
                                shoot_user = true;
                                if (count_k == 10)
                                {
                                    MessageBox.Show("You win!");
                                    {
                                        shoot_user = false;
                                        label7.Text = "Victory";
                                    }
                                }
                            }
                            else
                            {

                                label7.Text = "Wounded";
                                hit_bloomer(computer, click.X, click.Y, true);
                                shoot_user = true;
                            }
                            if (count_k == 10)
                            {
                                MessageBox.Show("You win!");
                                {
                                    shoot_user = false;
                                    label7.Text = "Victory";
                                }
                            }
                            break;
                        case 3:
                            computer[click.X, click.Y].Tag = 5;
                            if (check(computer, 3))
                            {
                                label7.Text = "Wounded";
                                hit_bloomer(computer, click.X, click.Y, true);
                                computer[click.X, click.Y].Tag = 5;
                                shoot_user = true;
                            }
                            else
                            {
                                label7.Text = "Killed";
                                hit_bloomer(computer, click.X, click.Y, true);
                                computer[click.X, click.Y].Tag = 5;
                                shoot_user = true;
                                label9.Text =
                                "Killed: " + Convert.ToString(++count_k);
                            }
                            if (count_k == 10)
                            {
                                MessageBox.Show("You win!");
                                {
                                    shoot_user = false;
                                    label7.Text = "Victory";
                                }
                            }
                            break;
                        case -3:
                            computer[click.X, click.Y].Tag = 5;
                            if (check(computer, -3))
                            {
                                label7.Text = "Wounded";
                                hit_bloomer(computer, click.X, click.Y, true);
                                computer[click.X, click.Y].Tag = 5;
                                shoot_user = true;
                            }
                            else
                            {
                                label7.Text = "Killed";
                                hit_bloomer(computer, click.X, click.Y, true);
                                computer[click.X, click.Y].Tag = 5;
                                shoot_user = true;
                                label9.Text = "Killed: " + Convert.ToString(++count_k);
                            }
                            if (count_k == 10)
                            {
                                MessageBox.Show("You win!");
                                {
                                    shoot_user = false;
                                    label7.Text = "Victory";
                                }
                            }
                            break;
                        case 4:
                            computer[click.X, click.Y].Tag = 5;
                            if (check(computer, 4))
                            {
                                label7.Text = "Wounded";
                                hit_bloomer(computer, click.X, click.Y, true);
                                computer[click.X, click.Y].Tag = 5;
                                shoot_user = true;
                            }
                            else
                            {
                                label7.Text = "Killed";
                                hit_bloomer(computer, click.X, click.Y, true);
                                computer[click.X, click.Y].Tag = 5;
                                shoot_user = true;
                                label9.Text = "Killed: " + Convert.ToString(++count_k);
                            }
                            if (count_k == 10)
                            {
                                MessageBox.Show("You win!");
                                {
                                    shoot_user = false;
                                    label7.Text = "Victory";
                                }
                            }
                            break;
                        case -1:
                            computer[click.X, click.Y].Tag = -2;
                            shoot_user = false;
                            hit_bloomer(computer, click.X, click.Y, false);
                            label7.Text = "Missed";
                            System.Threading.Thread.Sleep(300);
                            label7.Text = "Enemy's turn";
                            timer1.Start(); // старт таймера, стреляет комп 
                            break;
                        case 0: goto case -1;
                        case 5:
                            MessageBox.Show("Wrong shot");
                            shoot_user = true;
                            break;
                        case -2: goto case 5;
                        case -5: goto case 5;
                    }
                }
        }

        int shoot(int n, int m)
        {
            int num = -1;
            // 0 есть корабль 
            //-1 нету промах 
            // 1- заново стреляем 
            switch ((int)user[n, m].Tag)
            {
                ////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                case 1:
                    hit_bloomer(user, n, m, true);
                    buffer_zone(user, n - 1, n + 1, m - 1, m + 1, -2);
                    user[n, m].Tag = 5;
                    return num = 1; // стреляем заново 
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                case 2:
                    hit_bloomer(user, n, m, true);
                    if (ranen)
                    {
                        label7.Text = "Wounded";
                        ranen = false;
                    }
                    System.Threading.Thread.Sleep(250);
                    if (n >= 1)
                    {
                        if ((int)user[n - 1, m].Tag != -2)
                        {
                            if ((int)user[n - 1, m].Tag == 2)
                            {
                                hit_bloomer(user, n - 1, m, true);
                                buffer_zone(user, n - 2, n + 1, m - 1, m + 1, -2);
                                user[n - 1, m].Tag = 5;
                                user[n, m].Tag = 5;
                                return num = 1;
                            }
                            else
                            {
                                user[n - 1, m].Tag = -2;
                                hit_bloomer(user, n - 1, m, false);
                                return num = 0;
                            }
                        }
                    }
                    if (n <= 8)
                    {
                        if ((int)user[n + 1, m].Tag != -2)
                        {
                            if ((int)user[n + 1, m].Tag == 2)
                            {
                                hit_bloomer(user, n + 1, m, true);
                                buffer_zone(user, n - 1, n + 2, m - 1, m + 1, -2);
                                user[n + 1, m].Tag = 5;
                                user[n, m].Tag = 5;
                                return num = 1;
                            }
                            else
                            {
                                user[n + 1, m].Tag = -2;
                                hit_bloomer(user, n + 1, m, false);
                                return num = 0;
                            }
                        }
                    }
                    if (m >= 1)
                    {
                        if ((int)user[n, m - 1].Tag != -2)
                        {
                            if ((int)user[n, m - 1].Tag == 2)
                            {
                                hit_bloomer(user, n, m - 1, true);
                                buffer_zone(user, n - 1, n + 1, m - 2, m + 1, -2);
                                user[n, m].Tag = 5;
                                user[n, m - 1].Tag = 5;
                                return num = 1;
                            }
                            else
                            {
                                user[n, m - 1].Tag = -2;
                                hit_bloomer(user, n, m - 1, false);
                                return num = 0;
                            }
                        }
                    }

                    if (m <= 8)
                    {
                        if ((int)user[n, m + 1].Tag != -2)
                        {
                            if ((int)user[n, m + 1].Tag == 2)
                            {
                                hit_bloomer(user, n, m + 1, true);
                                buffer_zone(user, n - 1, n + 1, m - 1, m + 2, -2);
                                user[n, m + 1].Tag = 5;
                                user[n, m].Tag = 5;
                                return num = 1;
                            }
                            else
                            {
                                user[n, m + 1].Tag = -2;
                                hit_bloomer(user, n, m + 1, false);
                                return num = 0;
                            }
                        }
                    }
                    break;
                //////////////////////////////////////////////////////////////////////////////////////////////////// 
                case 3:
                    // стреляем по 3-х палубном в лево 
                    hit_bloomer(user, n, m, true);

                loop:
                    if (ranen)
                    {
                        label7.Text = "Wounded";
                        ranen = false;
                    }
                    if (horizont)
                    {
                        if (n >= 1)
                        {
                            if ((int)user[n - 1, m].Tag != -2 && (int)user[n - 1, m].Tag != 5)
                            {
                                if ((int)user[n - 1, m].Tag == 3)
                                {
                                    vert = false;
                                    hit_bloomer(user, n - 1, m, true);
                                    label7.Text = "Wounded";
                                    if (n <= 8 && horiz_3_left)
                                    {
                                        if ((int)user[n + 1, m].Tag == 5)
                                        {
                                            buffer_zone(user, n - 2, n + 2, m - 1, m + 1, -2);
                                            user[n, m].Tag = 5;
                                            user[n - 1, m].Tag = 5;
                                            user[n + 1, m].Tag = 5;
                                            vert = true;
                                            horiz_3_left = true;
                                            return num = 1;
                                        }
                                        else
                                        {
                                            horiz_3_left = false;
                                            goto loop;
                                        }
                                    }
                                    else
                                    {
                                        user[n - 1, m].Tag = 5;
                                        if (n > 1)
                                        {
                                            if ((int)user[n - 2, m].Tag == 3)
                                            {
                                                hit_bloomer(user, n - 2, m, true);
                                                buffer_zone(user, n - 3, n + 1, m - 1, m + 1, -2);
                                                user[n, m].Tag = 5;
                                                user[n - 1, m].Tag = 5;
                                                user[n - 2, m].Tag = 5;
                                                vert = true;
                                                horiz_3_left = true;
                                                return num = 1;
                                            }
                                            else
                                            {
                                                hit_bloomer(user, n - 2, m, false);
                                                user[n - 2, m].Tag = -2;

                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop; //если вверх низь стрельнуть, заново заходим 
                                    }
                                }
                                else
                                {
                                    user[n - 1, m].Tag = -2;
                                    hit_bloomer(user, n - 1, m, false);
                                    return num = 0;
                                }
                            }
                        }
                    }
                    ///////////////////////////// //// стреляем вправо 
                    if (horizont)
                    {
                        if (n <= 8)
                        {
                            if ((int)user[n + 1, m].Tag != -2 && (int)user[n + 1, m].Tag != 5)
                            {
                                if ((int)user[n + 1, m].Tag == 3)
                                {
                                    //igrok[n + 1, m].Tag = 5; 
                                    vert = false;
                                    hit_bloomer(user, n + 1, m, true);
                                    label7.Text = "Wounded";
                                    if (n >= 1 && horiz_3_rigth) // проверяем возможно уже две палубы подбиты 
                                    {
                                        if ((int)user[n - 1, m].Tag == 5)
                                        {
                                            buffer_zone(user, n - 2, n + 2, m - 1, m + 1, -2);
                                            user[n, m].Tag = 5;
                                            user[n - 1, m].Tag = 5;
                                            user[n + 1, m].Tag = 5;
                                            vert = true;
                                            horiz_3_rigth = true;
                                            return num = 1;
                                        }
                                        else
                                        {
                                            horiz_3_rigth = false;
                                            goto loop;
                                        }
                                    }
                                    else
                                    {
                                        user[n + 1, m].Tag = 5;
                                        if (n < 8)
                                        {
                                            if ((int)user[n + 2, m].Tag == 3)
                                            {
                                                hit_bloomer(user, n + 2, m, true);
                                                buffer_zone(user, n - 1, n + 3, m - 1, m + 1, -2);
                                                user[n, m].Tag = 5;
                                                user[n + 1, m].Tag = 5;
                                                user[n + 2, m].Tag = 5;
                                                vert = true;
                                                horiz_3_rigth = true;
                                                return num = 1;
                                            }
                                            else
                                            {
                                                hit_bloomer(user, n + 2, m, false);
                                                user[n + 2, m].Tag = -2;
                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop;
                                    }
                                }
                                else
                                {
                                    user[n + 1, m].Tag = -2;
                                    hit_bloomer(user, n + 1, m, false);
                                    return num = 0;
                                }
                            }
                        }
                    }
                    //////////////////////// стреляем вверх 
                    if (vert)
                    {
                        if (m >= 1)
                        {
                            if ((int)user[n, m - 1].Tag != -2 && (int)user[n, m - 1].Tag != 5)
                            {
                                if ((int)user[n, m - 1].Tag == 3)
                                {
                                    horizont = false;
                                    hit_bloomer(user, n, m - 1, true);
                                    label7.Text = "Wounded";
                                    if (m <= 8 && vertic_3_vverh)
                                    {
                                        if ((int)user[n, m + 1].Tag == 5)
                                        {
                                            buffer_zone(user, n - 1, n + 1, m - 2, m + 2, -2);
                                            user[n, m].Tag = 5;
                                            user[n, m - 1].Tag = 5;
                                            user[n, m - 2].Tag = 5;
                                            horizont = true;
                                            vertic_3_vverh = true;
                                            return num = 1;
                                        }
                                        else
                                        {
                                            vertic_3_vverh = false;
                                            goto loop;
                                        }
                                    }
                                    else
                                    {
                                        user[n, m - 1].Tag = 5;
                                        if (m > 1)
                                        {
                                            if ((int)user[n, m - 2].Tag == 3)
                                            {
                                                hit_bloomer(user, n, m - 2, true);
                                                buffer_zone(user, n - 1, n + 1, m - 3, m + 1, -2);
                                                user[n, m].Tag = 5;
                                                user[n, m - 1].Tag = 5;
                                                user[n, m - 2].Tag = 5;
                                                horizont = true;
                                                vertic_3_vverh = true;
                                                return num = 1;
                                            }
                                            else
                                            {
                                                hit_bloomer(user, n, m - 2, false);
                                                user[n, m - 2].Tag = -2;
                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop;
                                    }
                                }
                                else
                                {
                                    user[n, m - 1].Tag = -2;
                                    hit_bloomer(user, n, m - 1, false);
                                    return num = 0;
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////// 
                    // стреляем вниз 
                    if (vert)
                    {
                        if (m <= 8)
                        {
                            if ((int)user[n, m + 1].Tag != -2 && (int)user[n, m + 1].Tag != 5)
                            {
                                if ((int)user[n, m + 1].Tag == 3)
                                {
                                    horizont = false;
                                    hit_bloomer(user, n, m + 1, true);
                                    label7.Text = "Wounded";
                                    if (m >= 1 && vertic_3_vniz) // провереям возможно уже убили 3 х парусный 
                                    {
                                        if ((int)user[n, m - 1].Tag == 5)
                                        {
                                            buffer_zone(user, n - 1, n + 1, m - 2, m + 2, -2);
                                            user[n, m].Tag = 5;
                                            user[n, m - 1].Tag = 5;
                                            user[n, m + 1].Tag = 5;
                                            horizont = true;
                                            vertic_3_vniz = true;
                                            return num = 1;
                                        }
                                        else // если там пусто , идем дальше 
                                        {
                                            vertic_3_vniz = false;
                                            goto loop;
                                        }
                                    }
                                    else
                                    {
                                        user[n, m + 1].Tag = 5;
                                        if (m < 8)
                                        {
                                            if ((int)user[n, m + 2].Tag == 3)
                                            {
                                                hit_bloomer(user, n, m + 2, true);
                                                buffer_zone(user, n - 1, n + 1, m - 1, m + 3, -2);
                                                user[n, m].Tag = 5;
                                                user[n, m + 1].Tag = 5;
                                                user[n, m + 2].Tag = 5;
                                                horizont = true;
                                                vertic_3_vniz = true;
                                                return num = 1;
                                            }
                                            else
                                            {
                                                hit_bloomer(user, n, m + 2, false);
                                                user[n, m + 2].Tag = -2;
                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop;
                                    }
                                }
                                else
                                {
                                    user[n, m + 1].Tag = -2;
                                    hit_bloomer(user, n, m + 1, false);
                                    return num = 0;
                                }
                            }
                        }
                    }
                    break;
                case 4: // 4 палубы 
                    hit_bloomer(user, n, m, true);
                    if (ranen)
                    {
                        label7.Text = "Wounded";
                        ranen = false;
                    }
                loop4:
                    if (horizont) // горизонтльно стреляем влево от ранее попавшего 
                    {
                        if (n >= 1)
                        {
                            if ((int)user[n - 1, m].Tag != -2 && (int)user[n - 1, m].Tag != 5)
                            {
                                if ((int)user[n - 1, m].Tag == 4)
                                {
                                    vert = false;
                                    hit_bloomer(user, n - 1, m, true);
                                    label7.Text = "Wounded";
                                    // проверяем, возможно уже подбит корабль с другой стороны, если да, задаем таг, возращаем значение 
                                    if (n <= 7 && horizont_4_2_left)
                                    {
                                        if ((int)user[n + 1, m].Tag == 5 && (int)user[n + 2, m].Tag == 5)
                                        {
                                            buffer_zone(user, n - 2, n + 3, m - 1, m + 1, -2);
                                            user[n, m].Tag = 5;
                                            user[n - 1, m].Tag = 5;
                                            user[n + 1, m].Tag = 5;
                                            user[n + 2, m].Tag = 5;
                                            vert = true;
                                            horizont_4_2_left = true;
                                            hotizont_4_1_left = true;
                                            return num = 1;
                                        }
                                        else
                                        {
                                            horizont_4_2_left = false;
                                            goto loop4;
                                        }
                                    }
                                    ///////////////////////////// 
                                    else
                                    {
                                        user[n - 1, m].Tag = 5;
                                        if (n > 1)
                                        {
                                        loop4_1: if ((int)user[n - 2, m].Tag == 4)
                                            {
                                                hit_bloomer(user, n - 2, m, true);
                                                label7.Text = "Wounded";
                                                if (n <= 8 && hotizont_4_1_left)
                                                {
                                                    if ((int)user[n + 1, m].Tag == 5)
                                                    {
                                                        buffer_zone(user, n - 3, n + 2, m - 1, m + 1, -2);
                                                        user[n, m].Tag = 5;
                                                        user[n - 1, m].Tag = 5;
                                                        user[n - 2, m].Tag = 5;
                                                        user[n + 1, m].Tag = 5;
                                                        vert = true;
                                                        horizont_4_2_left = true;
                                                        hotizont_4_1_left = true;
                                                        return num = 1;
                                                    }
                                                    else
                                                    {
                                                        hotizont_4_1_left = false;
                                                        goto loop4_1;
                                                    }
                                                }
                                                else
                                                {
                                                    user[n - 2, m].Tag = 5;
                                                    if (n > 2)
                                                    {
                                                        if ((int)user[n - 3, m].Tag == 4)
                                                        {
                                                            user[n - 3, m].Tag = 5;
                                                            hit_bloomer(user, n - 3, m, true);
                                                            buffer_zone(user, n - 4, n + 1, m - 1, m + 1, -2);
                                                            user[n, m].Tag = 5;
                                                            user[n - 1, m].Tag = 5;
                                                            user[n - 2, m].Tag = 5;
                                                            user[n - 3, m].Tag = 5;
                                                            vert = true;
                                                            horizont_4_2_left = true;
                                                            hotizont_4_1_left = true;
                                                            return num = 1;
                                                        }
                                                        else
                                                        {
                                                            user[n - 3, m].Tag = -2;
                                                            hit_bloomer(user, n - 3, m, false);
                                                            return num = 0;
                                                        }
                                                    }
                                                    else
                                                        goto loop4;
                                                }
                                            }
                                            else
                                            {
                                                user[n - 2, m].Tag = -2;
                                                hit_bloomer(user, n - 2, m, false);
                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop4;

                                    }

                                }
                                else
                                {
                                    user[n - 1, m].Tag = -2;
                                    hit_bloomer(user, n - 1, m, false);
                                    return num = 0;
                                }
                            }
                        }
                    }

                    //////////////////////// стреляем вправо 
                    if (horizont) // горизонтльно стреляем влево от ранее попавшего 
                    {
                        if (n <= 8)
                        {
                            if ((int)user[n + 1, m].Tag != -2 && (int)user[n + 1, m].Tag != 5)
                            {
                                if ((int)user[n + 1, m].Tag == 4)
                                {
                                    vert = false;
                                    hit_bloomer(user, n + 1, m, true);
                                    label7.Text = "Wounded";
                                    // проверяем, возможно уже подбит корабль с другой стороны, если да, задаем таг, возращаем значение 
                                    if (n >= 2 && horizont_4_2_rigth)
                                    {
                                        if ((int)user[n - 1, m].Tag == 5 && (int)user[n - 2, m].Tag == 5)
                                        {
                                            buffer_zone(user, n - 3, n + 2, m - 1, m + 1, -1);
                                            user[n, m].Tag = 5;
                                            user[n + 1, m].Tag = 5;
                                            user[n - 1, m].Tag = 5;
                                            user[n - 2, m].Tag = 5;
                                            vert = true;
                                            horizont_4_2_rigth = true;
                                            hotizont_4_1_rigth = true;
                                            return num = 1;
                                        }
                                        else
                                        {
                                            horizont_4_2_rigth = false;
                                            goto loop4;
                                        }
                                    }
                                    ///////////////////////////// 
                                    else
                                    {
                                        user[n + 1, m].Tag = 5;
                                        if (n <= 7)
                                        {
                                        loop4_1: if ((int)user[n + 2, m].Tag == 4)
                                            {
                                                hit_bloomer(user, n + 2, m, true);
                                                label7.Text = "Wounded";
                                                if (n >= 1 && hotizont_4_1_rigth)
                                                {
                                                    if ((int)user[n - 1, m].Tag == 5)
                                                    {
                                                        buffer_zone(user, n - 2, n + 3, m - 1, m + 1, -2);
                                                        user[n, m].Tag = 5;
                                                        user[n - 1, m].Tag = 5;
                                                        user[n + 2, m].Tag = 5;
                                                        user[n + 1, m].Tag = 5;
                                                        vert = true;
                                                        horizont_4_2_rigth = true;
                                                        hotizont_4_1_rigth = true;
                                                        return num = 1;
                                                    }
                                                    else
                                                    {
                                                        hotizont_4_1_rigth = false;
                                                        goto loop4_1;
                                                    }
                                                }
                                                else
                                                {
                                                    user[n + 2, m].Tag = 5;
                                                    if (n <= 6)
                                                    {
                                                        if ((int)user[n + 3, m].Tag == 4)
                                                        {
                                                            user[n + 3, m].Tag = 5;
                                                            hit_bloomer(user, n + 3, m, true);
                                                            buffer_zone(user, n - 1, n + 4, m - 1, m + 1, -2);
                                                            user[n, m].Tag = 5;
                                                            user[n + 1, m].Tag = 5;
                                                            user[n + 2, m].Tag = 5;
                                                            user[n + 3, m].Tag = 5;
                                                            vert = true;
                                                            horizont_4_2_rigth = true;
                                                            hotizont_4_1_rigth = true;
                                                            return num = 1;
                                                        }
                                                        else
                                                        {
                                                            user[n + 3, m].Tag = -2;
                                                            hit_bloomer(user, n + 3, m, false);
                                                            return num = 0;
                                                        }
                                                    }
                                                    else
                                                        goto loop4;
                                                }
                                            }
                                            else
                                            {
                                                user[n + 2, m].Tag = -2;
                                                hit_bloomer(user, n + 2, m, false);
                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop4;

                                    }

                                }
                                else
                                {
                                    user[n + 1, m].Tag = -2;
                                    hit_bloomer(user, n + 1, m, false);
                                    return num = 0;
                                }
                            }
                        }
                    }

                    /////////////////// стреляем вврех 
                    if (vert) // горизонтльно стреляем влево от ранее попавшего 
                    {
                        if (m >= 1)
                        {
                            if ((int)user[n, m - 1].Tag != -2 && (int)user[n, m - 1].Tag != 5)
                            {
                                if ((int)user[n, m - 1].Tag == 4)
                                {
                                    horizont = false;
                                    hit_bloomer(user, n, m - 1, true);
                                    label7.Text = "Wounded";
                                    // проверяем, возможно уже подбит корабль с другой стороны, если да, задаем таг, возращаем значение 
                                    if (m <= 7 && vert_4_2_up)
                                    {
                                        if ((int)user[n, m + 1].Tag == 5 && (int)user[n, m + 2].Tag == 5)
                                        {
                                            buffer_zone(user, n - 1, n + 1, m - 2, m + 3, -2);
                                            user[n, m].Tag = 5;
                                            user[n, m - 1].Tag = 5;
                                            user[n, m + 1].Tag = 5;
                                            user[n, m + 2].Tag = 5;
                                            horizont = true;
                                            vert_4_2_up = true;
                                            vert_4_1_up = true;
                                            return num = 1;
                                        }
                                        else
                                        {
                                            vert_4_2_up = false;
                                            goto loop4;
                                        }
                                    }
                                    ///////////////////////////// 
                                    else
                                    {
                                        user[n, m - 1].Tag = 5;
                                        if (m > 1)
                                        {
                                        loop4_1: if ((int)user[n, m - 2].Tag == 4)
                                            {
                                                hit_bloomer(user, n, m - 2, true);
                                                label7.Text = "Wounded";
                                                if (m <= 8 && vert_4_1_up)
                                                {
                                                    if ((int)user[n, m + 1].Tag == 5)
                                                    {
                                                        buffer_zone(user, n - 1, n + 1, m - 3, m + 2, -2);
                                                        user[n, m].Tag = 5;
                                                        user[n, m - 1].Tag = 5;
                                                        user[n, m - 2].Tag = 5;
                                                        user[n, m + 1].Tag = 5;
                                                        horizont = true;
                                                        vert_4_2_up = true;
                                                        vert_4_1_up = true;
                                                        return num = 1;
                                                    }
                                                    else
                                                    {
                                                        vert_4_1_up = false;
                                                        goto loop4_1;
                                                    }
                                                }
                                                else
                                                {
                                                    user[n, m - 2].Tag = 5;
                                                    if (m > 2)
                                                    {
                                                        if ((int)user[n, m - 3].Tag == 4)
                                                        {
                                                            user[n, m - 3].Tag = 5;
                                                            hit_bloomer(user, n, m - 3, true);
                                                            buffer_zone(user, n - 1, n + 1, m - 4, m + 1, -2);
                                                            user[n, m].Tag = 5;
                                                            user[n, m - 1].Tag = 5;
                                                            user[n, m - 2].Tag = 5;
                                                            user[n, m - 3].Tag = 5;
                                                            horizont = true;
                                                            vert_4_2_up = true;
                                                            vert_4_1_up = true;
                                                            return num = 1;
                                                        }
                                                        else
                                                        {
                                                            user[n, m - 3].Tag = -2;
                                                            hit_bloomer(user, n, m - 3, false);
                                                            return num = 0;
                                                        }
                                                    }
                                                    else
                                                        goto loop4;
                                                }
                                            }
                                            else
                                            {
                                                user[n, m - 2].Tag = -2;
                                                hit_bloomer(user, n, m - 2, false);
                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop4;

                                    }

                                }
                                else
                                {
                                    user[n, m - 1].Tag = -2;
                                    hit_bloomer(user, n, m - 1, false);
                                    return num = 0;
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////стреляем вниз 
                    if (vert) // горизонтльно стреляем влево от ранее попавшего 
                    {
                        if (m <= 8)
                        {
                            if ((int)user[n, m + 1].Tag != -2 && (int)user[n, m + 1].Tag != 5)
                            {
                                if ((int)user[n, m +
                                1].Tag == 4)
                                {
                                    horizont = false;
                                    hit_bloomer(user, n, m + 1, true);
                                    label7.Text = "Wounded";
                                    // проверяем, возможно уже подбит корабль с другой стороны, если да, задаем таг, возращаем значение 
                                    if (m >= 2 && vert_4_2_down)
                                    {
                                        if ((int)user[n, m - 1].Tag == 5 && (int)user[n, m - 2].Tag == 5)
                                        {
                                            buffer_zone(user, n - 1, n + 1, m - 3, m + 2, -2);
                                            user[n, m].Tag = 5;
                                            user[n, m + 1].Tag = 5;
                                            user[n, m - 1].Tag = 5;
                                            user[n, m - 2].Tag = 5;
                                            horizont = true;
                                            vert_4_2_down = true;
                                            vert_4_1_down = true;
                                            return num = 1;
                                        }
                                        else
                                        {
                                            vert_4_2_down = false;
                                            goto loop4;
                                        }
                                    }
                                    ///////////////////////////// 
                                    else
                                    {
                                        user[n, m + 1].Tag = 5;
                                        if (m <= 7)
                                        {
                                        loop4_1: if ((int)user[n, m + 2].Tag == 4)
                                            {
                                                hit_bloomer(user, n, m + 2, true);
                                                label7.Text = "Wounded";
                                                if (m >= 1 && vert_4_1_down)
                                                {
                                                    if ((int)user[n, m - 1].Tag == 5)
                                                    {
                                                        buffer_zone(user, n - 1, n + 1, m - 2, m + 3, -2);
                                                        user[n, m].Tag = 5;
                                                        user[n, m - 1].Tag = 5;
                                                        user[n, m + 2].Tag = 5;
                                                        user[n, m + 1].Tag = 5;
                                                        horizont = true;
                                                        vert_4_2_down = true;
                                                        vert_4_1_down = true;
                                                        return num = 1;
                                                    }
                                                    else
                                                    {
                                                        vert_4_1_down = false;
                                                        goto loop4_1;
                                                    }
                                                }
                                                else
                                                {
                                                    user[n, m + 2].Tag = 5;
                                                    if (m <= 6)
                                                    {
                                                        if ((int)user[n, m + 3].Tag == 4)
                                                        {
                                                            user[n, m + 3].Tag = 5;
                                                            hit_bloomer(user, n, m + 3, true);
                                                            buffer_zone(user, n - 1, n + 1, m - 1, m + 4, -2);
                                                            user[n, m].Tag = 5;
                                                            user[n, m + 1].Tag = 5;
                                                            user[n, m + 2].Tag = 5;
                                                            user[n, m + 3].Tag = 5;
                                                            horizont = true;
                                                            vert_4_2_down = true;
                                                            vert_4_1_down = true;
                                                            return num = 1;
                                                        }
                                                        else
                                                        {
                                                            user[n, m + 3].Tag = -2;
                                                            hit_bloomer(user, n, m + 3, false);
                                                            return num = 0;
                                                        }
                                                    }
                                                    else
                                                        goto loop4;
                                                }
                                            }
                                            else
                                            {
                                                user[n, m + 2].Tag = -2;
                                                hit_bloomer(user, n, m + 2, false);
                                                return num = 0;
                                            }
                                        }
                                        else
                                            goto loop4;

                                    }

                                }
                                else
                                {
                                    user[n, m + 1].Tag = -2;
                                    hit_bloomer(user, n, m + 1, false);
                                    return num = 0;
                                }
                            }
                        }
                    }
                    break;
                case -1:
                    hit_bloomer(user, n, m, false);
                    user[n, m].Tag = -2;
                    return num = -1;
                case 0: goto case -1;
                default:
                    break;
            }
            return num;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time--;
            if (time == 0)
            {
                time = 30;
                timer1.Stop();
            стреляем_заново: if (exists)
                {
                    while (true)
                    {
                        a_ = r.Next(0, 10); b_ = r.Next(0, 10);
                        if ((int)user[a_, b_].Tag != -2 && (int)user[a_, b_].Tag != 5)
                            break;
                    }
                    switch (shoot(a_, b_))
                    {
                        case 1:
                            exists = true;
                            ranen = true;
                            label8.Text = "Killed: " + Convert.ToString(++count_i);
                            label7.Text = "Killed";
                            if (count_i == 10)
                            {
                                MessageBox.Show("You lose!");

                                timer1.Stop();
                                shoot_user = false;
                                for (int j = 0; j < 10; ++j)
                                {
                                    for (int i = 0; i < 10; ++i)
                                    {
                                        if ((int)computer[i, j].Tag > 0 && (int)computer[i, j].Tag != 5)
                                        {
                                            computer[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "кораблик.png");
                                        }
                                    }
                                }
                                label7.Text = "Defeat";
                                return;
                            }
                            System.Threading.Thread.Sleep(600);
                            label7.Text = "Enemy's turn";

                            goto стреляем_заново;
                        case 0:
                            exists = false;
                            ranen = false;
                            System.Threading.Thread.Sleep(250);
                            label7.Text = "Missed";
                            System.Threading.Thread.Sleep(250);
                            label7.Text = "Your turn";
                            System.Threading.Thread.Sleep(250);
                            shoot_user = true;
                            break;
                        case -1:
                            exists = true;
                            label7.Text = "Missed";
                            System.Threading.Thread.Sleep(250);
                            label7.Text = "Your turn";
                            System.Threading.Thread.Sleep(250);
                            shoot_user = true;
                            break;
                    }
                }
                else
                {
                    switch (shoot(a_, b_))
                    {
                        case 1:
                            exists = true;
                            ranen = true;
                            label7.Text = "Killed";
                            label8.Text = "Killed: " + Convert.ToString(++count_i);
                            if (count_i == 10)
                            {
                                MessageBox.Show("You lose!");

                                timer1.Stop();
                                shoot_user = false;
                                for (int j = 0; j < 10; ++j)
                                {
                                    for (int i = 0; i < 10; ++i)
                                    {
                                        if ((int)computer[i, j].Tag > 0 && (int)computer[i, j].Tag != 5)
                                        {
                                            computer[i, j].Image =
                                            Image.FromFile(Application.StartupPath + @"\..\..\Resources\" + "кораблик.png");
                                        }
                                    }
                                }
                                label7.Text = "Defeat";
                                return;
                            }
                            System.Threading.Thread.Sleep(600);
                            label7.Text = "Enemy's turn";
                            goto стреляем_заново;
                        case 0:
                            exists = false;
                            ranen = false;
                            System.Threading.Thread.Sleep(250);
                            label7.Text = "Missed";
                            System.Threading.Thread.Sleep(250);
                            label7.Text = "Your turn";
                            System.Threading.Thread.Sleep(250);
                            shoot_user = true;
                            break;
                        case -1:
                            exists = true;
                            label7.Text = "Missed";
                            System.Threading.Thread.Sleep(250);
                            label7.Text = "Your turn";
                            shoot_user = true;

                            break;
                    }
                }
            }
        }

        void func(int i_, int j_) // задание тага -5, место около корабля, используется в checkBox1_CheckedChanged( 
        {
            try
            {
                computer[i_, j_].Tag = -5;
            }
            catch (IndexOutOfRangeException) { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // ищем корабли и окло кораблей пользоватедб расставляет метки 
        {
            if (!checkBox1.Checked) // разрешаем доступность поля компьютера 
            {
                label7.Text = "Your turn";
                for (int j = 0; j < 10; ++j)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        computer[i, j].Enabled = true;
                    }
                }
            }
            else
            {
                // проверяем, возможно ли есть еще раненный корабль 
                int k_2 = 0;
                int k_3 = 0;
                int _k3 = 0;
                int k_4 = 0;
                int k = 0;
                for (int j = 0; j < 10; ++j)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        if ((int)computer[i, j].Tag == 2)
                            k_2++;
                        if ((int)computer[i, j].Tag == 3)
                            k_3++;
                        if ((int)computer[i, j].Tag == -3)
                            _k3++;
                        if ((int)computer[i, j].Tag == 4)
                            k_4++;
                    }
                }
                if (k_2 % 2 == 0) k++;
                if (k_3 == 0 || k_3 == 3) k++;
                if (_k3 == 0 || _k3 == 3) k++;
                if (k_4 == 0 || k_4 == 4) k++;
                if (k != 4)
                {
                    MessageBox.Show("There is a wounded ship");
                    checkBox1.Checked = false;
                    return;
                }

                // если нету, отмечаем зону 
                bool h = false;
                bool v = false;
                for (int j = 0; j < 10; ++j)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        if ((int)computer[i, j].Tag == 5)
                        {
                            if (i < 9)
                                if ((int)computer[i + 1, j].Tag == 5)
                                {
                                    v = true;
                                    h = false;
                                }
                            if (j < 9)
                                if ((int)computer[i, j + 1].Tag == 5)
                                {
                                    h = true;
                                    v = false;
                                }
                            if (v == false && h == false)
                            {
                                func(i - 1, j - 1);
                                func(i - 1, j + 1);
                                func(i + 1, j - 1);
                                func(i + 1, j + 1);
                                func(i, j - 1);
                                func(i, j + 1);
                                func(i - 1, j);
                                func(i + 1, j);
                            }
                            if (v)
                            {
                                func(i - 1, j - 1);
                                func(i - 1, j + 1);
                                func(i + 1, j - 1);
                                func(i + 1, j + 1);
                                func(i, j - 1);
                                func(i, j + 1);
                                if (i > 0)
                                    if ((int)computer[i - 1, j].Tag != 5)
                                    {
                                        computer[i - 1, j].Tag = -5;
                                    }
                                if (i < 9)
                                    if ((int)computer[i + 1, j].Tag != 5)
                                    {
                                        computer[i + 1, j].Tag = -5;
                                        v = false;
                                    }
                            }
                            if (h)
                            {
                                func(i - 1, j - 1);
                                func(i + 1, j - 1);
                                func(i - 1, j + 1);
                                func(i + 1, j + 1);
                                func(i - 1, j);
                                func(i + 1, j);
                                if (j > 0)
                                    if ((int)computer[i, j - 1].Tag != 5)
                                    {
                                        computer[i, j - 1].Tag = -5;
                                    }
                                if (j < 9)
                                    if ((int)computer[i, j + 1].Tag != 5)
                                    {
                                        computer[i, j + 1].Tag = -5;
                                        h = false;
                                    }
                            }
                        }
                    }
                }
                for (int j = 0; j < 10; ++j)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        if ((int)computer[i, j].Tag == -5 || (int)computer[i, j].Tag == 5 || (int)computer[i, j].Tag == -2)
                            computer[i, j].Enabled = true;
                        else
                            computer[i, j].Enabled = false;
                    }
                }
            }
        }
    }
}
