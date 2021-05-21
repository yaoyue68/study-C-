using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 线程抽奖
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
       
        private void uiButton1_Click(object sender, EventArgs e)
        {

            Thread th = new Thread(PlayGame);
            if (judge==false)
            {
                judge = true;
                
                th.IsBackground = true;
                th.Start();
                uiButton1.Text = "停止";
            }
            else
            {
                judge = false;
                th.Abort();
                uiButton1.Text = "开始";

            }

        }

        bool judge=false;
          public void PlayGame()
        {
            while (judge)
            {
                Random r = new Random();
                uiLabel1.Text=  r.Next(0, 10).ToString();
                uiLabel2.Text = r.Next(0, 10).ToString();
                uiLabel3.Text = r.Next(0, 10).ToString();
                
            }

        }

       


        private void Form1_Load(object sender, EventArgs e)
        {
           



        }
    }
}
