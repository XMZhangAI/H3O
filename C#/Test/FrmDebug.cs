using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class FrmDebug : Form
    {
        public static double dForceX = 0;//力fx
        public static double dForceY = 0;//力fy
        public static bool FLcsjs = false;//系统测试结束标志
    
        public FrmDebug()
        {
            InitializeComponent();
        }

                 
        private void timer1_Tick(object sender, EventArgs e)//系统测试
        {
            while (FLcsjs == false)//测试结束
            {
                Application.DoEvents();
                HardwareOpt.SamplePointByMean(10, ref dForceX, ref dForceY);//采集10个力数据求平均
                dForceX = dForceX - 2048;
                dForceY = dForceY - 2048;
                txtForceX.Text = dForceX.ToString("F1");
                txtForceY.Text = dForceY.ToString("F1");
                Int32 b = 0xFF;
                HardwareOpt.ReadI(ref b);
                textBox3.Text = (b & 0x01).ToString();
                textBox4.Text = (b & 0x02).ToString();
                textBox5.Text = (b & 0x04).ToString();
                textBox6.Text = (b & 0x08).ToString();
            }          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FLcsjs = false;
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FLcsjs = true;
            timer1.Enabled = false;
        }

      }
}

