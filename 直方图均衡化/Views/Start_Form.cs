using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 直方图均衡化.Views
{
    public partial class Start_Form : Form
    {
        public Start_Form()
        {
            InitializeComponent();
        }

        private void Start_Form_Load(object sender, EventArgs e)
        {
            timer1.Start();//启动定时器
            timer1.Interval = 1000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //定时时间到了处理事件
            this.Hide();//隐藏本窗体
            Form1 MainForm = new Form1();//实例化一个MainForm对象
            MainForm.Show();//显示窗体
            timer1.Stop();//定制定时器
        }
    }
}
