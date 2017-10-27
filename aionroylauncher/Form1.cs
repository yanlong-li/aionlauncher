using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;

namespace aionroylauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //禁用账号密码输入
            this.username.ReadOnly = true;
            this.password.ReadOnly = true;
            //Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //string username = config.AppSettings.Settings["username"].Value;
            //string password = config.AppSettings.Settings["password"].Value;
            //string serverAddress = config.AppSettings.Settings["serverAddress"].Value;
            //string autoLogin = config.AppSettings.Settings["autoLogin"].Value;
            string username = Properties.Settings.Default.username;
            string password = Properties.Settings.Default.password;
            string serverAddress = Properties.Settings.Default.serverAddress;
            bool autoLogin = Properties.Settings.Default.autoLogin;
            if (username.Length > 0) this.username.Text = username;
            if (password.Length > 0) this.password.Text = password;
            if (serverAddress.Length > 0) this.serverAddress.Text = serverAddress;
            if (autoLogin) { this.checkBox1.Checked = true; } else { this.checkBox1.Checked = false; }



        }

        private void button1_Click(object sender, EventArgs e)
        {


            string serverAddress = "sdjk.f3322.net";
            string port = "2106";
            string username = "";
            string password = "";
            string userlogin = "";
            if (this.checkBox1.Checked)
            {
                username = this.username.Text;
                password = this.password.Text;
                if (username.Length < 1 || password.Length < 1) { MessageBox.Show("请输入账号或密码"); return; }
                userlogin = " -account:" + username + " -password:" + password + " ";
                Properties.Settings.Default.username = username;
                Properties.Settings.Default.password = password;
            }
            if (this.port.Text.Length >= 1)
            {
                port = this.port.Text;
            }
            if (this.serverAddress.Text.Length >= 3)
            {
                if (ValidateIPAddress(this.serverAddress.Text))//是不是IP地址
                {
                    serverAddress = this.serverAddress.Text;//是直接写入
                }
                else
                {
                    //否，获取url的IP地址
                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(this.serverAddress.Text);
                    System.Net.IPAddress[] addr = ipEntry.AddressList;
                    serverAddress = addr[0].ToString();
                }

                
            }
            string sd = "/" + System.Environment.SystemDirectory.Substring(0, 1) + " ";
            var startInfo = new ProcessStartInfo
            {

                FileName = "cmd.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = sd+" start bin32\\aion.bin -ip:"+serverAddress+" -port:"+port+" -cc:5 -noauthgg -noweb -nb -ingameshop -nowebshop "+ userlogin
                //Arguments = sd + "start bin32\\aion.bin -ip:192.168.1.5 -port:2106 -cc:5 -noauthgg -noweb -nb -ingameshop -nowebshop"
            };
            Process.Start(startInfo);
            MessageBox.Show("正在启动");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBox1.Checked)
            {
                this.username.ReadOnly = true;
                this.password.ReadOnly = true;
                Properties.Settings.Default.autoLogin = false;
            }
            else
            {
                this.username.ReadOnly = false;
                this.password.ReadOnly = false;
                Properties.Settings.Default.autoLogin = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static bool ValidateIPAddress(string ipAddress)
        {
            Regex validipregex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            return (ipAddress != "" && validipregex.IsMatch(ipAddress.Trim())) ? true : false;
        }

        /// <summary>
        /// 拖动窗体
        /// </summary>
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
    }
}
