using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //process中的静态方法，类型名.方法名

            //显示所有进程
            Process[] pross = Process.GetProcesses();
            foreach (var item in pross)
            {
                Console.WriteLine(item);
                //结束所有进程
                // item.Kill();
            }

            //打开程序
            Process.Start("calc"); //打开计算器
            Process.Start("explorer", "d:");//打开D盘
            Process.Start("iexplore", "http://www.baidu.com"); //打开浏览器

            //打开指定文件
            Process pro = new Process(); //1.创建进程
                    //2.指定启动参数
            ProcessStartInfo psinfo = new ProcessStartInfo("d:\\1.exe"); 
            pro.StartInfo = psinfo;
            pro.Start();


        }


    }

}
