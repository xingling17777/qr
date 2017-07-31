using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace qrtest
{
    

    // 调用  
   
    public partial class Form1 : Form
    {
        ArrayList customer = new ArrayList();
        ArrayList current = new ArrayList();
        ArrayList localScan = new ArrayList();
       static int n = 0;
        static int scancount = 0;
        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            //系统显示前加载客户数据
            
            if (File.Exists(Directory.GetCurrentDirectory()+"\\customer.txt"))
            {
                FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\customer.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string mod;
                while ((mod = sr.ReadLine()) != null)
                {
                    customer.Add(mod.Trim());
                }
                sr.Close();
                fs.Close();
                label1.Text = "已加载" + customer.Count + "条客户数据";
            }
            //加载上次用户数据
            if (File.Exists(Directory.GetCurrentDirectory() + "\\current.txt"))
            {
                FileStream fs = new FileStream("current.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string mod;
                while ((mod = sr.ReadLine()) != null)
                {
                    current.Add(mod.Trim());
                }
                scancount = current.Count;
                sr.Close();
                fs.Close();
                //label1.Text = "已加载" + customer.Count + "条数据";
            }
        }

       

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { 

            if (textBox1.Text.Trim().ToString() != "")
            {
                    string sy = textBox1.Text.Trim().ToString();


                    label4.Text = "";
                    label2.Text = "";
                    label3.Text = "";
                    textBox1.Text = "";
                    if (current.IndexOf(sy) != -1)
                    {
                        
                        if(localScan.IndexOf(sy)!=-1)
                        {
label4.Text = "当前重复的二维码为：" + sy+"";
                            BeepUp.Beep(500, 2000);
                        MessageBox.Show("当前箱内重复的软管，请继续！"); 
                        }
                        else
                        {
                            label4.Text = "当前重复的数据为：" + sy + "";
                            BeepUp.Beep(500, 2000);
                            MessageBox.Show("非当前箱内重复的软管，请放置等待处理！");
                        }
                                               
                        return;
                    }


                   
                    if (customer.IndexOf(sy) == -1)
                    {
                        label3.Text = "未在客户提供的数据内：" + sy;
                        BeepUp.Beep(500, 2000);
                        MessageBox.Show("非法数据，未在客户提供的数据内！");
                        return;
                    }
                    current.Add(sy);
                    localScan.Add(sy);
                    label2.Text = "当前已扫描" + (n+1) + "支软管！\n累计扫描"+((n+scancount+1)/447)+"箱"+ ((n + scancount + 1) % 447) + "软管!";                
                    n++;
                   if(current.Count % 447==0)
                    {
                        FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\current.txt", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        foreach (string s in current)
                        {
                            sw.WriteLine(s);
                        }
                        sw.Close();
                        fs.Close();

                        string nameby = DateTime.Now.ToString().Replace(":", "").Replace("/", "").Replace("-", "").Replace(" ", "");
                        FileStream fk = new FileStream(Directory.GetCurrentDirectory() + "\\" + nameby + ".txt", FileMode.Create);
                        StreamWriter sk = new StreamWriter(fk);
                        foreach (string s in localScan)
                        {
                            sk.WriteLine(s);
                        }
                        localScan.Clear();
                        sk.Close();
                        fk.Close();
                        localScan.Clear();
                    }
                
            }
        }
    }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\current.txt",FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach(string s in current)
            {
                sw.WriteLine(s);
            }
            sw.Close();
            fs.Close();
            if(localScan.Count!=0)
            {
                string nameby = DateTime.Now.ToString().Replace(":", "").Replace("/","").Replace("-","").Replace(" ","");
                FileStream fk = new FileStream(Directory.GetCurrentDirectory() + "\\" + nameby+ ".txt", FileMode.Create);
                StreamWriter sk = new StreamWriter(fk);
                foreach (string s in localScan)
                {
                    sk.WriteLine(s);
                }
                localScan.Clear();
                sk.Close();
                fk.Close();
            }
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList tempSum = new ArrayList();
            foreach(string s in customer)
            {
                if(current.IndexOf(s)==-1)
                {
                    tempSum.Add(s);
                }
            }
            FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\sum.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (string s in tempSum)
            {
                sw.WriteLine(s);
            }
            sw.Close();
            fs.Close();
        }
    }
    public class BeepUp  //新建一个类
    {
        /// <param name="iFrequency">声音频率（从37Hz到32767Hz）。在windows95中忽略</param>  
        /// <param name="iDuration">声音的持续时间，以毫秒为单位。</param>  
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;  
        public static extern bool Beep(int frequency, int duration);
    }
}
