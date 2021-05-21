using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDI_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //生成bitmap画布
            Bitmap bmp = new Bitmap(150, 50);
            Graphics g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;


            //生成随机验证码
            Random r = new Random();
            string str = null;
            for (int i = 0; i < 5; i++)
            {
                str += r.Next(0, 10);

                label1.Text = str;
             }

            //对验证码进行单个调整,字体和颜色
            String[] fontnames = new string[] { "宋体", "楷体","黑体","微软雅黑" };
            Color[] colorcollection = new Color[] { Color.Black, Color.Yellow, Color.YellowGreen, Color.Silver, Color.SlateBlue };
            FontStyle[] fs = new FontStyle[] { FontStyle.Italic, FontStyle.Regular, FontStyle.Underline, FontStyle.Strikeout };
            for (int i = 0; i <5; i++)
            {
                //随机生成不同颜色的笔刷和字体
                SolidBrush sbrush = new SolidBrush(colorcollection[r.Next(0, 5)]);
                Font fontc = new Font (fontnames[r.Next(0, 3)], 20,fs[r.Next(0,4)]);

                //每个字的间距
                Point ypt = new Point(0 + i * 20, 0 );

                g.DrawString(str[i].ToString(),fontc, sbrush, ypt);
                //旋转，待加强
                g.RotateTransform(r.Next(-5,5));

            }

            //干扰像素点
            for (int i = 0; i < 1000; i++)
            {
                bmp.SetPixel(r.Next(0, 150), r.Next(0, 50), Color.Green);
            }

            //干扰线
            for (int i = 0; i < 10; i++)
            {
                Pen mpen = new Pen(colorcollection[r.Next(0,5)]);
                g.DrawLine(mpen, new Point(0, 0 + r.Next(0, 50)), new Point(150, 0 + r.Next(0, 50)));

            }

        }

            private void Form1_Load(object sender, EventArgs e)
            {

            }

            private void Form1_Paint(object sender, PaintEventArgs e)
           {
           


            }

        private void button2_Click(object sender, EventArgs e)
        {
            //创建画布，注意this
            Graphics g = this.CreateGraphics();
            //创建画笔，3种颜色方式
            Pen mp = new Pen(Color.Red, 10f); //10f宽度控制
            Pen np = new Pen(Color.FromArgb(255, 255, 255));
            Pen my = new Pen(Brushes.Black);
            //设置绘图开始和结束坐标
            Point pt1 = new Point(200, 200);
            Point pt2 = new Point(800, 800);
            //绘制直线
            g.DrawLine(mp, pt1, pt2);
            //绘制矩形
            Rectangle rec = new Rectangle(pt1, new Size(200, 500));//pt1为左上角坐标
            g.DrawRectangle(mp, rec);
            //绘制扇形
            g.DrawPie(mp, rec, 60, 60);
            //绘制字符串
            g.DrawString("c#是世界上最好的语言", new Font("宋体", 20, FontStyle.Underline), Brushes.Black, pt2);
            //定义单色画笔,画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径,此类不能被继承
            Brush solid = new SolidBrush(Color.Black);

        }
    }
    }

