using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace socket通信
{
    public partial class socket练习 : Form
    {
        public socket练习()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //服务器创建一个socket用于监听
            Socket socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //设置ip和端口
            IPAddress ip = IPAddress.Any;
            IPEndPoint endpoint = new IPEndPoint(ip, Convert.ToInt32(textBox2.Text));
            //绑定，即监听
            socketwatch.Bind(endpoint);
            //设置监听数量
            socketwatch.Listen(10);
            Msger("监听成功！" + "\n\r");
            //创建线程预防卡死
            Thread th = new Thread(Listen);
            th.IsBackground = true;
            th.Start(socketwatch);// 传入监听进程

        }

        //监听socket循环创建多个通信socket线程，其中每个通信socket再创建一个线程处理信息，预防卡死

        // 全局服务器端通信socket
        Socket socketsend;
        Dictionary<string, Socket> sdic = new Dictionary<string, Socket>();

        private void Listen(object s)
        {
            //类型转换，as转换成功返回值否则NULL
            Socket socketwatch = s as Socket;
            //等待客户端连接，并创建一个负责通信的socket

            while (true)
            {
                socketsend = socketwatch.Accept();
                //将socket与ip信息放入字典集合
                sdic.Add(socketsend.RemoteEndPoint.ToString(), socketsend);
                //加入combox列表
                comboBox1.Items.Add(socketsend.RemoteEndPoint.ToString());
                //让combox显示当前连接的socket
                comboBox1.SelectedItem= socketsend.RemoteEndPoint.ToString();

                Msger(socketsend.RemoteEndPoint.ToString() + "  连接进来了" + "\t\r\n");

                //创建一个新的线程处理客户端发送过来的信息，否则一个socket就堵塞
                Thread th = new Thread(Operater);
                th.IsBackground = true;
                th.Start(socketsend);

            }

        }
        /// <summary>
        /// 信息处理线程
        /// </summary>
        /// <param name="s">socket返回客户端信息</param>
        public void Operater(object s)
        {
            Socket socketsend = s as Socket;
            //必须循环接收消息
            while (true)
            {
                //创建字节数组并接受传过来的数据，返回int为实际使用值
                byte[] receive = new byte[1024 * 1024 * 10];
                int realr = socketsend.Receive(receive);
                //判断客户端发送的内容，如果为0则停止循环
                if (realr == 0)
                {
                    break;
                }
                //用文件头判断传过来的是文字信息或文件的各种类型，0-文字信息 1-文件
                if (receive[0]==0) 
                {
                //解码
                string text = Encoding.UTF8.GetString(receive, 1, realr-1);
                //写入textbox
                Msger(text);

                }
                else if(receive[0]==1) //文件则保存
                {
                    SaveFileDialog sf = new SaveFileDialog();
                    sf.Filter = "所有文件|*.*";
                    sf.ShowDialog(this);         //注意this用法
                    using (FileStream fs=new FileStream(sf.FileName,FileMode.Create,FileAccess.Write))
                    {
                        fs.Write(receive, 1, realr-1); //-1去掉标志头

                    }


                }

                
            }
        }

        public void Msger(string ms)
        {
            textBox3.AppendText(ms + "\n"); //追加方式

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }



        //客户端socket
        Socket socketcustomsend;
        private void button4_Click(object sender, EventArgs e)
        {
            //创建socket
            socketcustomsend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //设置ip和port
            IPAddress ip = IPAddress.Parse(textBox5.Text);
            IPEndPoint ipendpoint = new IPEndPoint(ip, Convert.ToInt32(textBox6.Text));
            //连接
            socketcustomsend.Connect(ipendpoint);
            textBox4.Text = "连接成功！";


            //创建进程接受服务器传来的信息
            Thread th = new Thread(CustomReceive);
            th.IsBackground = true;
            th.Start(socketcustomsend);

        }
        /// <summary>
        /// 循环接受服务器传来的信息
        /// </summary>
        /// <param name="s">通信的socket</param>
        public void CustomReceive(object s)
        {
            while (true) //循环接收信息
            {
                Socket socketcustomsend = s as Socket;
                byte[] b = new byte[1024 * 1024 * 5];
                int r = socketcustomsend.Receive(b);
                if (r==0) //0表示服务器端关闭
                {
                    break;
                }
                textBox4.Text = Encoding.UTF8.GetString(b, 0, r);
                }
                

            }


        private void button2_Click(object sender, EventArgs e)
        {
            //准备数据
            byte[] b = Encoding.UTF8.GetBytes(textBox4.Text);
            //加入识别头0
            List<byte> nb = new List<byte>();
            nb.Add(0);
            nb.AddRange(b);
           byte[] sb= nb.ToArray();

            //发送
            socketcustomsend.Send(sb);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            of.Filter = "所有文件|*.*";
            of.ShowDialog();
            textBox7.Text = of.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(textBox7.Text, FileMode.Open, FileAccess.Read))
            {
                byte[] b = new byte[1024 * 1024 * 10];
                //返回真实使用数
                int r = fs.Read(b, 0, b.Length);

                //添加文件类型识别头,list与array互相转换
                List<byte> newbyte = new List<byte>();
                newbyte.Add(1);//1代表文件
                newbyte.AddRange(b);//将b中元素添加，add（）为整个添加
                byte[] nb=newbyte.ToArray();

                //发送真实大小数据, 为r+1
                socketcustomsend.Send(nb,0,r+1,SocketFlags.None);

            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
           sdic[comboBox1.SelectedItem.ToString()].Send(Encoding.UTF8.GetBytes(textBox3.Text));

        }
    }
}
