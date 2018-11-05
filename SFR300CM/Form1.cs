using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace NearField
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //禁用最大化和最小化按钮(保留关闭按钮)
            this.MinimizeBox = false;
            this.MaximizeBox = false;

        }

        //TestManager项目文件路径
        string prjfilepath = @"C:\TestManager\Projects\SFR300\SFR_300CM.prj";

        //测试模式文件和文件内容
        string testerconfigpath = @"C:\TestManager\TM\TesterConfig\";
        string testmodefilepath = @"C:\TestManager\TM\TesterConfig\testmode.json";
        string notes1 = "{\"TestMode\":\"Online\"}";
        string notes2 = "{\"TestMode\":\"Debug\"}";
        //校验程序路径和指令
        string str1 = "C:";
        string str2 = "cd " + @"C:\TestManager";
        string str3 = "LuffyUpdate-Diags.exe /md5d -s Update_Server.xml -l Update_List.xml -rl";
        string str4 = "exit";


        //在线测试按钮
        private void button1_Click(object sender, EventArgs e)
        { 

            //开始校验,打开MD5程序校验
            DoDos(str1, str2, str3, str4);
            //Open(@"C:\TestManager\LuffyUpdate-Diags.exe","/md5d -s Update_Server.xml -l Update_List.xml -rl");

            WriteTestmodeFile(notes1);
            Thread.Sleep(300);
            IsExistsFileAndOpen();
        }

        //执行cmd命令
        public void DoDos(string comd1, string comd2, string comd3, string comd4)
        {
            string output = null;
            Process p = new Process();//创建进程对象 
            p.StartInfo.FileName = "cmd.exe";//设定需要执行的命令 
            // startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
            p.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动 
            p.StartInfo.RedirectStandardInput = true;//可以重定向输入  
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;//不创建窗口 
            p.Start();
            // string comStr = comd1 + "&" + comd2 + "&" + comd3;
            p.StandardInput.WriteLine(comd1);
            p.StandardInput.WriteLine(comd2);
            p.StandardInput.WriteLine(comd3);
            p.StandardInput.WriteLine(comd4);
            p.WaitForExit();
            output = p.StandardOutput.ReadToEnd();
            //textBox1.Text += output;
            //textBox1.Text += "\r\n";
            if (p != null)
            {
                p.Close();
            }
            // return output;
            Open(@"C:\TestManager\Waiting.exe");

        }

        /// <summary>
        /// 正常启动window应用程序（d:\*.exe）
        /// </summary>
        public static void Open(String exe)
        {
            try
            {
                //System.Diagnostics.Process.Start(exe);
                System.Diagnostics.Process.Start(exe).WaitForExit();
            }
            catch
            {
            }

        }

        /// <summary>
        /// 正常启动window应用程序（d:\*.exe）,并传递初始命令参数
        /// </summary>
        public static void Open(String exe, String args)
        {
            System.Diagnostics.Process.Start(exe, args).WaitForExit();
        }

        //离线测试按钮
        private void button2_Click(object sender, EventArgs e)
        {
            WriteTestmodeFile(notes2);
            Thread.Sleep(300);
            IsExistsFileAndOpen();
        }

        //判断文件是否存在,存在就打开
        public void IsExistsFileAndOpen()
        {
            //判断前清除提示
            label2.Text = "";

            if (File.Exists(prjfilepath))
            {
                //存在,打开文件
                System.Diagnostics.Process.Start(prjfilepath);
                //直接关闭当前窗口.以后可以再调用.
                this.Close();
                //关闭当前窗口,以后不可以调用.
                this.Dispose();
            }
            else
            {
                //不存在
                MessageBox.Show("Prj文件不存在，请检查C盘测试程序是否正常！","提示");
                label2.Text = prjfilepath;
                label2.Visible = true;
            } 
        }

        //为文件写入内容
        public void WriteTestmodeFile(string notes)
        {
            //如果文件夹存在就创建并写入文件
            //如果不存在就忽略,不做处理,后面打开程序时会进行处理
            if (Directory.Exists(testerconfigpath) == true)
            {
                //文件夹存在,写入文件
                System.IO.StreamWriter sw = new System.IO.StreamWriter(testmodefilepath); ;
                sw.Write(notes);
                sw.Flush();
                sw.Close();

            }
            else
            {
                //文件夹不存在
            }

        }

        private void SkipProgramCheck_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //跳过校验开启程序
            WriteTestmodeFile(notes1);
            Thread.Sleep(300);
            IsExistsFileAndOpen();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"C:\TestManager\LuffyUpdate-Diags.exe");
            }
            catch
            {

                MessageBox.Show("C:\\TestManager\\LuffyUpdate-Diags.exe打开异常!","提示");
            }
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.BackColor = System.Drawing.Color.DarkSeaGreen;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.BackColor = System.Drawing.SystemColors.Control;
        }


    }
}
