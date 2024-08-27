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
    public partial class Frmbc : Form
    {
        public static int nX = 0;//轨迹x坐标
        public static int nY = 0;//轨迹y坐标
        public static string ch = null;
        public static bool FLjs = false;//示教结束标记
        public static string dltX, dltY;//记录轨迹数据0-3
        public static StringBuilder sb = new StringBuilder();//定义构建一个数组sb，保存轨迹（每一步的±X,±Y)
        //StringBuilder sb = new StringBuilder();//定义构建一个数组sb，保存轨迹（每一步的±X,±Y)
        public static double ydzl = 0.0;//运动阻力
        public static int ksped = 1;//速度
        public static double dForceX = 0;//力fx
        public static double dForceY = 0;//力fy
        public static int cjds = 5000;//采集点数
        public static Int32 Int32_DI;//开关量输入
        public static bool FLcsjs = false;//系统测试结束标志
        public static int tbcy = 1;//选择图表成员
        public static string path1 = "";//获得用户选中的文件的路径
        public static bool bntjt = false;//急停
        public static bool bntjx = false;//继续
        public static bool bntks = false;//开始
        public Frmbc()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.DateTime currentTime = new System.DateTime();//定义时间型变量（结构体）
            currentTime = System.DateTime.Now;//更新时间年月日时分秒
            int nian = currentTime.Year;// 取年成员
            int yue = currentTime.Month;//取月成员
            string strY = currentTime.ToString(); //取当前年月日时分秒

            string ymd = strY.Substring(0, 4);

            textBox1.Text = nian.ToString();
            textBox2.Text = yue.ToString();
            textBox3.Text = strY;
            textBox4.Text = ymd;
            ymd = strY.Substring(5, 1);
            textBox5.Text = ymd;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            System.DateTime currentTime = new System.DateTime();//定义时间型变量（结构体）
            currentTime = System.DateTime.Now;//更新时间年月日时分秒

            string strY = currentTime.ToString(); //取当前年月日时分秒

            textBox6.Text = strY;


        }

        private void Frmbc_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            System.DateTime currentTime = new System.DateTime();//定义时间型变量（结构体）
            currentTime = System.DateTime.Now;//更新时间年月日时分秒

            string strY = currentTime.ToString(); //取当前年月日时分秒

            textBox6.Text = strY;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
            cjds = -180;
            nX = -4000;
            nY = 0;

            //sb.AppendLine(nX.ToString());//保存起点
            //sb.AppendLine(nY.ToString());//（nx.ny)


            for (int j = 180; j >= cjds; j--)//画圆（先画一个轨迹）
            {
                double x1 = 4000.0 * (Math.Cos(j * Math.PI / 180.0));//产生圆弧轨迹
                double y1 = 4000.0 * (Math.Sin(j * Math.PI / 180.0));
                chart1.Series[1].Points.AddXY(x1, y1);//画弦（小折线）,红线
            }

            double x0 = 4000.0 * (Math.Cos(-180 * Math.PI / 180.0));//起点
            double y0 = 4000.0 * (Math.Sin(-180 * Math.PI / 180.0));

            for (int j = 180; j >= cjds + 5; j = j - 5)//走整圆（5°弦）
            {
                double x1 = 4000.0 * (Math.Cos((j - 5) * Math.PI / 180.0));//终点
                double y1 = 4000.0 * (Math.Sin((j - 5) * Math.PI / 180.0));
                chart1.Series[4].Points.AddXY(x1, y1);//画提示小圆点（提前5°）

                //x1 = 4000.0 * (Math.Cos(j * Math.PI / 180.0));//终点 理论轨迹
                //y1 = 4000.0 * (Math.Sin(j * Math.PI / 180.0));
                //计算小折线            
                dForceX = x1 - x0;
                dForceY = y1 - y0;
                //x0 = x1;
                //y0 = y1;
                sb.AppendLine(dForceX.ToString());//保存理论轨迹
                sb.AppendLine(dForceY.ToString());//
                double dForceX0 = dForceX;
                double dForceY0 = dForceY;

                double fxy = Math.Sqrt(dForceX * dForceX + dForceY * dForceY);//理论小线段长度

            Loop01:
                HardwareOpt.SamplePointByMean(3, ref dForceX, ref dForceY);// 病人发力跟踪小圆点，4623采集3个力数据求平均

                double fxy1 = Math.Sqrt(dForceX * dForceX + dForceY * dForceY);//病人主动跟踪小圆点，采集的小线段长度

                if (fxy1 < ydzl) goto Loop01;//病人用力小于设定阻力，则转去重新测力，等待大于阻力。

                double k12 = fxy / fxy1;//fxy是理论长度，fxy1是实际长度，实际长度要等于理论长度，
                dForceX = Math.Sign(dForceX0) * k12 * Math.Abs(dForceX);//跟踪方向是力方向，跟踪线段长度要等于理论长度
                dForceY = Math.Sign(dForceY0) * k12 * Math.Abs(dForceY);
                sb.AppendLine(dForceX.ToString());//保存实际轨迹
                sb.AppendLine(dForceY.ToString());//
                //txtForceX.Text = dForceX.ToString("F1");
                //txtForceY.Text = dForceY.ToString("F1");

                chart1.Series[2].Points.AddXY(x0, y0);//此行为了看对应关系（实际与理论的差距）
                x0 = x1;
                y0 = y1;
                stepline1(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                //HardwareOpt.ReadI(ref b);    //读限位开关
                //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

            }
            x0 = 4000.0 * (Math.Cos(-180 * Math.PI / 180.0));//看最后一点
            y0 = 4000.0 * (Math.Sin(-180 * Math.PI / 180.0));
            chart1.Series[2].Points.AddXY(x0, y0);
           
        }

        private void stepline1(double xe, double ye)   //直线插补,不存轨迹
        {
            double F = 0.0;//F判别式
            double xeabs = Math.Abs(xe);
            double yeabs = Math.Abs(ye);
            double er = xeabs + yeabs;//终点判别
            int ndsp = 0;
            tbcy = 2;
            if (xe > 0 && ye >= 0)//第一象限
            {

                HardwareOpt.XDirp();
                HardwareOpt.YDirp();

                while (er >= 0)
                {
                    //Application.DoEvents();
                    if (F >= 0.0)//F判别式
                    {
                        HardwareOpt.XMotoForeward(ksped);// x向发一个脉冲，走一步
                        dltX = "0";
                        F = F - yeabs;
                        // sb.AppendLine(dltX);
                        nX++;

                        ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                        textBox1.Text = nX.ToString();
                        textBox2.Text = nY.ToString();

                        chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                        er--;


                    }
                    else
                    {
                        HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
                        dltY = "2";
                        F = F + xeabs;
                        // sb.AppendLine(dltY);
                        nY++;
                        ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                        textBox1.Text = nX.ToString();
                        textBox2.Text = nY.ToString();

                        chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）
                        er--;

                    }



                }
                if (xe <= 0 && ye > 0)//第二象限
                {
                    HardwareOpt.XDirn();
                    HardwareOpt.YDirp();
                    while (er >= 0)
                    {
                        // Application.DoEvents();
                        if (F >= 0)
                        {
                            HardwareOpt.XMotoForeward(ksped);// x向发一个脉冲，走一步
                            dltX = "1";
                            F = F - yeabs;
                            // sb.AppendLine(dltX);
                            nX--;
                            ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                            textBox1.Text = nX.ToString();
                            textBox2.Text = nY.ToString();

                            chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                            er--;


                        }
                        else
                        {
                            HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
                            dltY = "2";
                            F = F + xeabs;
                            //sb.AppendLine(dltY);
                            nY++;
                            ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次

                            textBox1.Text = nX.ToString();
                            textBox2.Text = nY.ToString();

                            chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                            er--;



                        }

                    }

                }

                if (xe < 0 && ye <= 0)//第三象限
                {
                    HardwareOpt.XDirn();
                    HardwareOpt.YDirn();
                    while (er >= 0)
                    {
                        //Application.DoEvents();
                        if (F >= 0)
                        {
                            HardwareOpt.XMotoForeward(ksped);// x向发一个脉冲，走一步

                            dltX = "1";   // x--;
                            F = F - yeabs;
                            // sb.AppendLine(dltX);
                            nX--;
                            ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                            textBox1.Text = nX.ToString();
                            textBox2.Text = nY.ToString();
                            chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                            er--;

                        }
                        else
                        {
                            HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步

                            dltY = "3"; //  y--;
                            F = F + xeabs;
                            //sb.AppendLine(dltY);
                            nY--;
                            ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                            textBox1.Text = nX.ToString();
                            textBox2.Text = nY.ToString();

                            chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                            er--;

                        }

                    }

                }

                if (xe >= 0 && ye < 0)//第四象限
                {
                    HardwareOpt.XDirp();
                    HardwareOpt.YDirn();
                    while (er >= 0)
                    {
                        // Application.DoEvents();
                        if (F >= 0)
                        {
                            HardwareOpt.XMotoForeward(ksped);// x向发一个脉冲，走一步

                            dltX = "0";  // x++;
                            F = F - yeabs;
                            //sb.AppendLine(dltX);
                            nX++;
                            ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                            textBox1.Text = nX.ToString();
                            textBox2.Text = nY.ToString();

                            chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                            er--;

                        }
                        else
                        {
                            HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步

                            dltY = "3"; // y--;
                            F = F + xeabs;
                            // sb.AppendLine(dltY);
                            nY--;
                            ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                            textBox1.Text = nX.ToString();
                            textBox2.Text = nY.ToString();

                            chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                            er--;

                        }

                    }


                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
            Int32 b = 0xFF;
            //nX = -1500;
            //nY = 0;
            //sb.AppendLine(nX.ToString());//保存起点
            //sb.AppendLine(nY.ToString());//（nx.ny)
            tbcy = 1;
            for (int i = 0; i < 4; i++)//画方
            {

                if (i == 0)
                {
                    nX = 1500;
                    nY = 0;


                }
                if (i == 1)
                {
                    nX = 1500;
                    nY = 3000;

                }
                if (i == 2)
                {
                    nX = -1500;
                    nY = 3000;


                }
                if (i == 3)
                {
                    nX = -1500;
                    nY = 0;


                }
                chart1.Series[tbcy].Points.AddXY(nX, nY);//画小折线,红线(绝对坐标）

            }

            tbcy++;
            if (tbcy == 4) tbcy = 1;
            for (int i = 0; i < 4; i++)//走方
            {
                if (i == 0)
                {
                   zdzx(-1500.0, 0, 1500, 0.0);//绝对坐标

                }
                if (i == 1)
                {
                    zdzx(1500.0, 0.0, 1500.0, 3000.0);

                }
                if (i == 2)
                {
                    zdzx(1500, 3000.0, -1500, 3000);

                }
                if (i == 3)
                {
                    zdzx(-1500.0, 3000.0, -1500.0, 0.0);


                }
                HardwareOpt.ReadI(ref b);    //读限位开关
                if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员

            nX = -1500;
            nY = 0;
            tbcy = 1;
            // chart1.Series[tbcy].Points.AddXY(nX, nY);//画小折线,红线
            // sb.AppendLine(nX.ToString());//保存起点
            // sb.AppendLine(nY.ToString());//（nx.ny)

            for (int i = 0; i < 3; i++)//先画理论轨迹
            {

                if (i == 0)
                {
                    nX = 0;
                    nY = 3000;


                }
                if (i == 1)
                {
                    nX = -3000;
                    nY = 3000;

                }
                if (i == 2)
                {
                    nX = -1500;
                    nY = 0;


                }

                chart1.Series[tbcy].Points.AddXY(nX, nY);//画小折线,红线

            }
            tbcy++;
            if (tbcy == 4) tbcy = 1;
            for (int i = 0; i < 3; i++)
            {

                if (i == 0)
                {
                    zdzx(-1500.0, 0.0, 0.0, 3000.0);


                }
                if (i == 1)
                {
                    zdzx(0.0, 3000.0, -3000.0, 3000.0);


                }
                if (i == 2)
                {
                    zdzx(-3000.0, 3000.0, -1500.0, 0.0);


                }

            }
        }

        private void zdzx(double x0, double y0, double xe, double ye)   //
        {
            nX = (int)x0;//
            nY = (int)y0;
            double x = xe - x0;
            double y = ye - y0;
            double r = Math.Sqrt(x * x + y * y);//线段理论长度
            double sta = Math.Atan(y / x);//线段斜率
            if (x < 0.0) sta = sta + Math.PI;//线段斜率
            double n = 5.0;//把线段n等分

            for (int j = 0; j <= n; j++)//画理论轨迹
            {

                chart1.Series[1].Points.AddXY(j * r / n * Math.Cos(sta) + x0, j * r / n * Math.Sin(sta) + y0);//画轨迹

            }

            for (int j = 0; j < n; j++)//主动走轨迹
            {

                chart1.Series[4].Points.AddXY(j * r / n * Math.Cos(sta) + x0, j * r / n * Math.Sin(sta) + y0);//画提示圆点  

                dForceX = x / n;
                dForceY = y / n;
                double dForceX0 = dForceX;
                double dForceY0 = dForceY;

                sb.AppendLine(dForceX.ToString());//保存小线段理论轨迹
                sb.AppendLine(dForceY.ToString());//

                double fxy = r / n;//理论小线段长度

                Loop01:
                HardwareOpt.SamplePointByMean(3, ref dForceX, ref dForceY);// 病人发力跟踪小圆点，4623采集3个力数据求平均

                double fxy1 = Math.Sqrt(dForceX * dForceX + dForceY * dForceY);//病人主动跟踪小圆点，系统采集的力矢长度

                if (fxy1 < ydzl) goto Loop01;//病人用力小于设定阻力，则转去重新测力，等待施力大于设定的阻力。

                double k12 = fxy / fxy1;//fxy是理论长度，fxy1是实际长度，要求实际长度要等于理论长度，
                //dForceX = k12 * dForceX;//跟踪运动的方向是力方向，跟踪线段长度要等于理论长度
                //dForceY = k12 * dForceY;
                dForceX = Math.Sign(dForceX0 + 1) * k12 * Math.Abs(dForceX);//跟踪方向是力方向，跟踪线段长度要等于理论长度
                dForceY = Math.Sign(dForceY0 + 1) * k12 * Math.Abs(dForceY);
              
                sb.AppendLine(dForceX.ToString());//保存实际轨迹
                sb.AppendLine(dForceY.ToString());//
                //txtForceX.Text = dForceX.ToString("F1");
                //txtForceY.Text = dForceY.ToString("F1");

                stepline1(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                chart1.Series[2].Points.AddXY((j+1) * r / n * Math.Cos(sta) + x0, (j +1)* r / n * Math.Sin(sta) + y0);//画连线  

                //HardwareOpt.ReadI(ref b);    //读限位开关
                //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

            }

            chart1.Series[4].Points.AddXY(r * Math.Cos(sta) + x0, r * Math.Sin(sta) + y0);//画提示圆点 最后一点 


        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员

            nX = -1500;
            nY = 0;
            tbcy = 1;
          
            for (int i = 0; i < 3; i++)//先画理论轨迹
            {

                if (i == 0)
                {
                    nX = 0;
                    nY = 3000;

                }
                if (i == 1)
                {
                    nX = -3000;
                    nY = 3000;

                }
                if (i == 2)
                {
                    nX = -1500;
                    nY = 0;

                }

                chart1.Series[tbcy].Points.AddXY(nX, nY);//画小折线,红线

            }

            tbcy++; if (tbcy == 4) tbcy = 1;
            for (int i = 0; i < 3; i++)
            {

                if (i == 0)
                {
                    zdzx(-1500.0, 0.0, 0.0, 3000.0);


                }
                if (i == 1)
                {
                    zdzx(0.0, 3000.0, -3000.0, 3000.0);

                }
                if (i == 2)
                {
                    zdzx(-3000.0, 3000.0, -1500.0, 0.0);

                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
            zdzx(-4000, -4000, 0, 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
