using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class FrmMain : Form
    {
        public static int nX = 0;//轨迹x坐标
        public static int nY = 0;//轨迹y坐标
        public static string ch=null;
        public static bool FLjs = false;//示教结束标记
        public static string dltX, dltY;//记录轨迹数据0-3
        public static StringBuilder sb = new StringBuilder();//定义构建一个数组sb，保存轨迹（每一步的±X,±Y)
        //StringBuilder sb = new StringBuilder();//定义构建一个数组sb，保存轨迹（每一步的±X,±Y)
        public static double ydzl =150.0;//运动阻力
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
        
        public FrmMain()
        {
            InitializeComponent();
        }

        private void tsbStart_Click(object sender, EventArgs e)//系统测试
        {
            FrmDebug FrmDebug = new FrmDebug();
            FrmDebug.ShowDialog();
           
        }

       
        private void btnks_Click(object sender, EventArgs e)//示教按钮  走斜线，按力的大小成比例的进给，力大走的步数就多。
        {
            sj();
        }  

       
        private void toolStripButton3_Click(object sender, EventArgs e)//系统退出
        {
            this.Close();
        }


        private void tsbsj_Click(object sender, EventArgs e)//示教工具栏按钮  走斜线，按力的大小成比例的进给，力大走的步数就多。
        {
            sj();
        }

        private void tsbzx_Click(object sender, EventArgs e)//再现
        {
            label10.Show();//显示被再现的文件名
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();
            tbcy = 1;
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择要打开的文本文件";
            //ofd.InitialDirectory = @"C:\Users\SpringRain\Desktop";
            ofd.Multiselect = true;
            //ofd.Filter = "数据文件|*.dat|所有文件|*.*";

            ofd.ShowDialog();//显示文件对话框
           
            string path = ofd.FileName;//获得用户选中的文件的路径
            path1 = path;
          
            string fileName = Path.GetFileName(path1);  //获得了用户选中的文件名

            if (path1 == "") {  return;     }

            label10.Text = "系统正在再现:" + fileName;//显示被再现的文件名

           // zxht();//先把轨迹画一遍，只画不走。
           
            using (FileStream fsRead = new FileStream(path1, FileMode.OpenOrCreate, FileAccess.Read))//打开文件流
            {
                StreamReader sr = new StreamReader(fsRead);//定义文件流的读取变量sr（一个结构体）
                string s = sr.ReadLine();//（读一行）
               
                nX = int.Parse(s);//把字符串变成32位的整型数，读起点
                s = sr.ReadLine();
                nY= int.Parse(s);
                int GJsj;//轨迹数据
                int i = 0; // 控制显示用                  
                //int xhj1 = 0;
                //int xhj2 = 0;
                //int xhj3 = 0;
                //int xhj4 = 0;
               
              while ((s =  sr.ReadLine()) != null )
              {
                  string s0 = s.Substring(0, 1);
                  s0 = s0.Trim();

                  //if (s == "11")//走圆
                  //{   s = sr.ReadLine(); tbcy = int.Parse(s);  //再读一行，tbcy是图表成员（4个成员，4种颜色），换一种颜色来画。
                  //    s = sr.ReadLine(); int nxm = int.Parse(s);
                  //    xhj1++;
                  //    label6.Text = "走圆形";
                  //    textBox5.Text = "共循环" + nxm + "次";
                  //    textBox7.Text = (xhj1).ToString() + "次";
                      
                  //}
                  //if (s == "22")//走方
                  //{
                  //    s = sr.ReadLine(); tbcy = int.Parse(s);  //再读一行，tbcy是图表成员（4个成员，4种颜色），换一种颜色来画。
                  //    s = sr.ReadLine(); int nxm = int.Parse(s);
                  //    xhj2++;
                  //    label6.Text = "走方形";
                  //    textBox5.Text = "共循环" + nxm + "次";
                  //    textBox7.Text = (xhj2).ToString() + "次";

                  //}
                  //if (s == "33")//走三角
                  //{
                  //    s = sr.ReadLine(); tbcy = int.Parse(s);  //再读一行，tbcy是图表成员（4个成员，4种颜色），换一种颜色来画。
                  //    s = sr.ReadLine(); int nxm = int.Parse(s);
                  //    xhj3++;
                  //    label6.Text = "走三角";
                  //    textBox5.Text = "共循环" + nxm + "次";
                  //    textBox7.Text = (xhj3).ToString() + "次";
                     
                  //}
                  //if (s == "44")//往返走
                  //{
                  //    s = sr.ReadLine(); tbcy = int.Parse(s);  //再读一行，tbcy是图表成员（4个成员，4种颜色），换一种颜色来画。
                  //    s = sr.ReadLine(); int nxm = int.Parse(s);
                  //    xhj4++;
                  //    label6.Text = "往返走";
                  //    textBox5.Text = "共循环" + nxm + "次";
                  //    textBox7.Text = (xhj4).ToString() + "次";

                  //}
             
                    GJsj = int.Parse(s);
                    textBox1.Text = nX.ToString();
                    textBox2.Text = nY.ToString();
                   if (GJsj == 0)
                    {
                        HardwareOpt.XDirp();
                        HardwareOpt.XMotoForeward(ksped);
                        nX++;
                    }
                    if (GJsj == 1)
                    {
                        HardwareOpt.XDirn();
                        HardwareOpt.XMotoForeward(ksped);
                        nX--;
                    }

                    if (GJsj == 2)
                    {
                        HardwareOpt.YDirp();
                        HardwareOpt.YMotoForeward(ksped);
                        nY++;
                    }
                    if (GJsj == 3)
                    {
                        HardwareOpt.YDirn();
                        HardwareOpt.YMotoForeward(ksped);
                        nY--;
                    }
                  chart1.Series[tbcy].Points.AddXY(nX, nY);//画一点

                  i++;
                  if (i%1000==0)  Application.DoEvents();//200个点显示轨迹一次
                                 
                 }
                fsRead.Close();
                MessageBox.Show("再现完毕");
                label10.Hide();

            }

        }


        private void btnjs_Click(object sender, EventArgs e)//示教结束
        {
            timer1.Enabled = false;
            FLjs = true;
          
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }
     
      

        private void button3_Click(object sender, EventArgs e)//走圆形
        {
            double r=1500.0;
            if (checkBox1.Checked == true) { r = 4000.0; }// else r = 1500;
            if (checkBox2.Checked == true) { r = 3000.0; }
            if (checkBox3.Checked == true) { r = 2000.0; }
            nX = (int)((-1.0) * r);   // 1500;
            nY = 0;
            sb.AppendLine(nX.ToString());//保存起点
            sb.AppendLine(nY.ToString());//（nx.ny)
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
            yuan();
            gjsave();
        }

        private void button4_Click(object sender, EventArgs e)//走正方形
        {
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
            Int32 b = 0xFF;
            tbcy = 1;
            //nX = -1500;
            //nY = 0;
            //sb.AppendLine(nX.ToString());//保存起点
            //sb.AppendLine(nY.ToString());//（nx.ny)

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

            tbcy++; if (tbcy == 4) tbcy = 1;
            for (int i = 0; i < 4; i++)//走方
            {
                if (i == 0)
                {
                    bdzx(-1500.0, 0, 1500, 0.0);//绝对坐标

                }
                if (i == 1)
                {
                    bdzx(1500.0, 0.0, 1500.0, 3000.0);

                }
                if (i == 2)
                {
                    bdzx(1500, 3000.0, -1500, 3000);

                }
                if (i == 3)
                {
                    bdzx(-1500.0, 3000.0, -1500.0, 0.0);


                }
                HardwareOpt.ReadI(ref b);    //读限位开关
                if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环
            }
            ////nX = -1500;
            ////nY = 0;
            ////sb.AppendLine(nX.ToString());//保存起点
            ////sb.AppendLine(nY.ToString());//（nx.ny)
            //for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
            //fang();
            gjsave();
         
          
        }

        private void button5_Click(object sender, EventArgs e)//走三角形
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
                    bdzx(-1500.0, 0.0, 0.0, 3000.0);


                }
                if (i == 1)
                {
                    bdzx(0.0, 3000.0, -3000.0, 3000.0);


                }
                if (i == 2)
                {
                    bdzx(-3000.0, 3000.0, -1500.0, 0.0);


                }

            }
            gjsave();
            //nX = -1500;
            //nY = 0;
            //sb.AppendLine(nX.ToString());//保存起点
            //sb.AppendLine(nY.ToString());//（nx.ny)
            //for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
            //sanjiao();
            //gjsave();
            
        }

        private void button2_Click_1(object sender, EventArgs e)//被动往返走
        {
            tbcy = 1;
            label6.Text = "往返走";
            string nxm = "2";
            for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
            for (int i = 0; i < int.Parse(nxm); i++)
            {
                textBox5.Text = "共循环" + nxm + "次";
                textBox7.Text = (i + 1).ToString() + "次";
                bdwangf();
               
            }
          gjsave();
        }
         
        private void toolStripButton1_Click(object sender, EventArgs e)//系统回零
        {
            for (int j = 0; j < 4; j++) chart1.Series[j].Points.Clear();//清除每个成员
            Int32 b = 0xFF; 
            nX = 0;
            nY = 0;
            tbcy = 0;
            HardwareOpt.XDirn();
            do                                    //到x下限
           {
             Application.DoEvents();           
             HardwareOpt.XMotoForeward(ksped);// x向发一个脉冲，走一步
             nX--;
             textBox1.Text = nX.ToString();
             chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）
             HardwareOpt.ReadI(ref b);
          
           } while ((b & 0x01) ==1);// (true);

           int x = 0;
           tbcy++;
           if (tbcy == 4) tbcy = 1;
           HardwareOpt.XDirp();     //到x上限
         do
           {   
               Application.DoEvents();           
               HardwareOpt.XMotoForeward(ksped);// x向发一个脉冲，走一步
               nX++;
               x++;
               textBox1.Text = nX.ToString();
               chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）
               HardwareOpt.ReadI(ref b);
             
           } while ((b & 0x02) == 2);// (true);

           tbcy++;
           if (tbcy == 4) tbcy = 1;
           HardwareOpt.XDirn();      //到x零位
           for (int i=0;i<x/2;i++)
           {
               Application.DoEvents();
               HardwareOpt.XMotoForeward(ksped);// x向发一个脉冲，走一步
               nX--;
               textBox1.Text = nX.ToString();
               chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）    
             
           }
           tbcy++;
           if (tbcy == 4) tbcy = 1;
           HardwareOpt.YDirn();   //到y下限
        do
           {
               Application.DoEvents();           
               HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
               nY--;
               textBox2.Text = nY.ToString();
               chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）
               HardwareOpt.ReadI(ref b);
              
           } while ((b & 0x04) == 4);// (true);

           int y = 0;
           tbcy++;
           if (tbcy == 4) tbcy = 1;
           HardwareOpt.YDirp();      //到y上限
        do
           {
               Application.DoEvents();
               HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
               nY++;
               y++;
               textBox2.Text = nY.ToString();
               chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）
               HardwareOpt.ReadI(ref b);
             

           } while ((b & 0x08) == 8);
           tbcy++;
           if (tbcy == 4) tbcy = 1;
           HardwareOpt.XDirn();        //到y零位
           for (int i = 0; i < y / 2; i++)
           {
              // Application.DoEvents();
               HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
               nY--;
               textBox2.Text = nY.ToString();
               chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）

           }
           MessageBox.Show("已回零！");

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{   FLcsjs = true;
        //    timer1.Enabled = false;
           
        //}

        private void stbcs_Click(object sender, EventArgs e)
        {

        }


   
         private void stepline(double xe, double ye)   //直线插补，存轨迹
        {
            ksped = 4;
            if (checkBox19.Checked == true) { ksped = 1; }
            if (checkBox20.Checked == true) { ksped = 4; }
            if (checkBox21.Checked == true) { ksped = 8; }
             double F = 0.0;//F判别式
             double xeabs = Math.Abs(xe);
             double yeabs = Math.Abs(ye);
             double er = xeabs + yeabs;//终点判别
             int ndsp = 0;
             tbcy=2; 
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
                         sb.AppendLine(dltX);
                         nX++;

                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();
                        
                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;
                        
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             label8.Text = "                                    ";
                             bntjt = false;
                             bntjx = false;
                          }
                         
                     }
                     else
                     {
                         HardwareOpt.YMotoForeward(ksped);//  Y向发一个脉冲，走一步
                         dltY = "2";
                         F = F + xeabs;
                         sb.AppendLine(dltY);
                         nY++;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();
                        
                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）
                         er--;
                         if (bntjt == true) { label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             label8.Text = "                                    ";
                             bntjt = false; bntjx = false; }
                        
                     }

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
                         sb.AppendLine(dltX);
                         nX--;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();
                        
                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;

                         if (bntjt == true) { label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             bntjt = false; bntjx = false; }
                        
                     }
                     else
                     {
                         HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
                         dltY = "2";
                         F = F + xeabs;
                         sb.AppendLine(dltY);
                         nY++;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次

                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();
                        
                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;
                       
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             bntjt = false;
                             bntjx = false;
                         }
                       
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
                         sb.AppendLine(dltX);
                         nX--;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();
                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             bntjt = false;
                             bntjx = false;
                         }
                     }
                     else
                     {
                         HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
                      
                         dltY = "3"; //  y--;
                         F = F + xeabs;
                         sb.AppendLine(dltY);
                         nY--;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();

                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             bntjt = false;
                             bntjx = false;
                         }
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
                         sb.AppendLine(dltX);
                         nX++ ;
                         ndsp++;if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();
                  
                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             bntjt = false;
                             bntjx = false;
                         }
                     }
                     else
                     {
                         HardwareOpt.YMotoForeward(ksped);// x向发一个脉冲，走一步
                       
                         dltY = "3"; // y--;
                         F = F + xeabs;
                         sb.AppendLine(dltY);
                         nY--;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();
                      
                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下继续按钮
                             bntjt = false;
                             bntjx = false;
                         }
                     }

                 }
               

             }
         }


         private void stepline1(double xe, double ye)   //直线插补,不存轨迹
         {
             ksped = 4;
             if (checkBox19.Checked == true) { ksped = 1; }
             if (checkBox20.Checked == true) { ksped = 4; }
             if (checkBox21.Checked == true) { ksped = 8; }
             double F = 0.0;//F判别式
             double xeabs = Math.Abs(xe);
             double yeabs = Math.Abs(ye);
             double er = xeabs + yeabs;//终点判别
             int ndsp = 0;
             tbcy=2; 
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

                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false;
                             bntjx = false;
                         };

                     }
                     else
                     {
                         HardwareOpt.YMotoForeward(ksped);// Y向发一个脉;冲，走一步
                         dltY = "2";
                         F = F + xeabs;
                        // sb.AppendLine(dltY);
                         nY++;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();

                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）
                         er--;
                         if (bntjt == true) { label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false; bntjx = false; }

                     }

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

                         if (bntjt == true) { label8.Text = "系统暂停，按【继续】按钮，继续运行！"; 
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false; bntjx = false; }

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

                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false;
                             bntjx = false;
                         }

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
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false; bntjx = false;
                         }
                     }
                     else
                     {
                         HardwareOpt.YMotoForeward(ksped);// Y向发一个脉冲，走一步

                         dltY = "3"; //  y--;
                         F = F + xeabs;
                         //sb.AppendLine(dltY);
                         nY--;
                         ndsp++; if (ndsp % 200 == 0) Application.DoEvents();//200个点显示轨迹一次
                         textBox1.Text = nX.ToString();
                         textBox2.Text = nY.ToString();

                         chart1.Series[tbcy].Points.AddXY(nX, nY);//画轨迹（一点）  
                         er--;
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false; bntjx = false;
                         }
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
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false; bntjx = false;
                         }
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
                         if (bntjt == true)
                         {
                             label8.Text = "系统暂停，按【继续】按钮，继续运行！";
                             while (bntjx == false) { Application.DoEvents(); }//等待按下【继续】
                             bntjt = false; bntjx = false;
                         }
                     }

                 }


             }
         }






         private void sj()//走斜线，按力的大小成比例的进给，力大走的步数就多。
         {
             for (int j = 0; j <5; j++) chart1.Series[j].Points.Clear();//清除每个成员
             nX = -4000;//绘图起点坐标
             nY = -4000;
             sb.AppendLine(nX.ToString());//保存起点，再现时用
             sb.AppendLine(nY.ToString());//（nx.ny)
             FLjs = false;//示教结束标志
             Int32 b = 0xFF; //设置输入口初值全1（断开状态）

             while (FLjs == false)  //按【结束】退出
             {
                 Application.DoEvents();//允许多控件并发
                 HardwareOpt.SamplePointByMean(3, ref dForceX, ref dForceY);//4623采集3个力数据求平均
                
                 double xymin = (Math.Min(Math.Abs(dForceX), Math.Abs(dForceY))) / 10.0;
                /*!*/
                double k1 = Math.Max(Math.Abs(dForceX), Math.Abs(dForceY));
                double k2 = Math.Min(Math.Abs(dForceX), Math.Abs(dForceY));
                double k = (Math.Max(Math.Abs(dForceX), Math.Abs(dForceY))) / (Math.Min(Math.Abs(dForceX), Math.Abs(dForceY)));
                 if (k1/10 < 0.005) { dForceX = 0; dForceY = 0; }
                 else if(k2/10<0.005) { k1 = Math.Sign(dForceX) * 20.0; k2 = 0; }
                dForceX = dForceX / xymin;//求fx,fy的比值k，力小的走10步，力大的走10k步。
                 dForceY = dForceY / xymin;

                if (k> 20) { k1 = Math.Sign(k1) * 20.0; k2 = 0; }
                //if (Math.Abs(dForceX) / xymin > 20) { dForceX = Math.Sign(dForceX) * 20.0; dForceY = 0; }//比值k>20，力小的轴不走
                //if (Math.Abs(dForceY) / xymin > 20) { dForceY = Math.Sign(dForceY) * 20.0; dForceX = 0; }

                txtForceX.Text = dForceX.ToString("F1");//显示力数据，一位小数（折线段的终点坐标
                 txtForceY.Text = dForceY.ToString("F1");

                 stepline(dForceX, dForceY);//直线插补(进给，轨迹存储sb，坐标显示，绘图）

                 HardwareOpt.ReadI(ref b);    //读限位开关
                 if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环
             }
             string path = System.Environment.CurrentDirectory + string.Format("/data/{0:yyMMdd-HHmmss}.dat", DateTime.Now);//取得文件名

             if (path != "")
             {

                 using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))  //创建输出文件流
                 {
                     byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());//把sb数组中的字符编码形成字节序列，赋给缓冲区
                     fsWrite.Write(buffer, 0, buffer.Length);//把缓冲区的字节数据写入文件
                     fsWrite.Close();
                 }
                 sb = new StringBuilder();//全局数组sb清零，为下一次准备
                 MessageBox.Show("轨迹保存成功");
             }

         }

         private void yuan()//走圆
         {

             double r = 1500.0;
             if (checkBox1.Checked == true) { r = 4000.0; }// else r = 1500;
             if (checkBox2.Checked == true) { r = 3000.0; }
             if (checkBox3.Checked == true) { r = 2000.0; }
             int cjds = -180;
             Int32 b = 0xFF;
             //nX = -2500;
             //nY = 0;
             //sb.AppendLine(nX.ToString());//保存起点
             //sb.AppendLine(nY.ToString());//（nx.ny)
             
             for (int j = 180; j >= cjds; j = j - 10)//画圆（先画一个轨迹）
             {
                // Application.DoEvents();
                 //double x1 = 1500.0 * (Math.Cos(j * Math.PI / 180.0));//产生圆弧轨迹
                 //double y1 = 1500.0 * (Math.Sin(j* Math.PI / 180.0));
                 double x1 = r * (Math.Cos(j * Math.PI / 180.0));//产生圆弧轨迹
                 double y1 = r * (Math.Sin(j * Math.PI / 180.0));
                 chart1.Series[tbcy].Points.AddXY(x1, y1);//画小折线,红线
             }

                 double x0 = r * (Math.Cos(-180 * Math.PI / 180.0));//起点
                 double y0 = r * (Math.Sin(-180 * Math.PI / 180.0));

              for (int j = 180; j >= cjds; j = j - 10)//走整圆
              {
                 tbcy++;
                 if (tbcy == 4) tbcy = 1;
                 double x1 = r * (Math.Cos((j-10) * Math.PI / 180.0));//
                 double y1 = r * (Math.Sin((j-10) * Math.PI / 180.0));
                 chart1.Series[4].Points.AddXY(x1, y1);//画提示小圆点（提前10°）
                
                  x1 = r * (Math.Cos(j * Math.PI / 180.0));//终点 理论轨迹
                  y1 = r * (Math.Sin(j * Math.PI / 180.0));
                 //计算小折线            
                  dForceX = x1 - x0;
                  dForceY = y1 - y0;
                  x0 = x1;
                  y0 = y1;
               
                 txtForceX.Text = dForceX.ToString("F1");//显示小线段，一位小数（折线段的终点坐标
                 txtForceY.Text = dForceY.ToString("F1");

                  stepline(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                 HardwareOpt.ReadI(ref b);    //读限位开关
                 if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

             }
         }

  private void zdyuan()//zd走圆
  {           cjds = -180;
  double r = 1500.0;
  if (checkBox1.Checked == true) { r = 4000.0; }
  if (checkBox2.Checked == true) { r = 3000.0; }
  if (checkBox3.Checked == true) { r = 2000.0; }
  nX = (int)((-1) * r);
             
              nY = 0;

              //sb.AppendLine(nX.ToString());//保存起点
              //sb.AppendLine(nY.ToString());//（nx.ny)


              for (int j = 180; j >= cjds; j--)//画圆（先画一个轨迹）
              {
                  double x1 = r * (Math.Cos(j * Math.PI / 180.0));//产生圆弧轨迹
                  double y1 = r * (Math.Sin(j * Math.PI / 180.0));
                  chart1.Series[1].Points.AddXY(x1, y1);//画小折线,红线
              }

              double x0 = r * (Math.Cos(-180 * Math.PI / 180.0));//起点
              double y0 = r * (Math.Sin(-180 * Math.PI / 180.0));

              for (int j = 180; j >= cjds + 5; j = j - 5)//走5°弦整圆
              {
                  double x1 = r * (Math.Cos((j - 5) * Math.PI / 180.0));//终点
                  double y1 = r * (Math.Sin((j - 5) * Math.PI / 180.0));
                  chart1.Series[4].Points.AddXY(x1, y1);//画提示小圆点（提前5°）
    
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
                  HardwareOpt.SamplePointByMean(1, ref dForceX, ref dForceY);// 病人发力跟踪小圆点，4623采集3个力数据求平均

                  double fxy1 = Math.Sqrt(dForceX * dForceX + dForceY * dForceY);//病人主动跟踪小圆点，采集的小线段长度

                  if (fxy1 < ydzl) goto Loop01;//病人用力小于设定阻力，则转去重新测力，等待大于阻力。

                  double k12 = fxy / fxy1;//fxy是理论长度，fxy1是实际长度，实际长度要等于理论长度，
                  dForceX = Math.Sign(dForceX0) * k12 * Math.Abs(dForceX);//跟踪方向是力方向，跟踪线段长度要等于理论长度
                  dForceY = Math.Sign(dForceY0) * k12 * Math.Abs(dForceY);
                  sb.AppendLine(dForceX.ToString());//保存实际轨迹
                  sb.AppendLine(dForceY.ToString());//
                  txtForceX.Text = dForceX.ToString("F1");
                  txtForceY.Text = dForceY.ToString("F1");

                  chart1.Series[2].Points.AddXY(x0, y0);//此行为了看对应关系（实际与理论的差距）
                  x0 = x1;
                  y0 = y1;
                  stepline1(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                  //HardwareOpt.ReadI(ref b);    //读限位开关
                  //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

              }
              x0 = r * (Math.Cos(-180 * Math.PI / 180.0));//看最后一点
              y0 = r * (Math.Sin(-180 * Math.PI / 180.0));
              chart1.Series[2].Points.AddXY(x0, y0);//

             // gjsave();
         }
          
            
       
         private void fang()//走正方形
         {
            // for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
             Int32 b = 0xFF;
             tbcy = 1;
             //nX = -1500;
             //nY = 0;
             //sb.AppendLine(nX.ToString());//保存起点
             //sb.AppendLine(nY.ToString());//（nx.ny)

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

             tbcy++; if (tbcy == 4) tbcy = 1;
             for (int i = 0; i < 4; i++)//走方
             {
                 if (i == 0)
                 {
                     bdzx(-1500.0, 0, 1500, 0.0);//绝对坐标

                 }
                 if (i == 1)
                 {
                     bdzx(1500.0, 0.0, 1500.0, 3000.0);

                 }
                 if (i == 2)
                 {
                     bdzx(1500, 3000.0, -1500, 3000);

                 }
                 if (i == 3)
                 {
                     bdzx(-1500.0, 3000.0, -1500.0, 0.0);


                 }
                 HardwareOpt.ReadI(ref b);    //读限位开关
                 if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环
             }
            
            //gjsave();

        }

         private void zdfang()//zd走正方形
         {
             Int32 b = 0xFF;
             tbcy = 1;
             //nX = -1500;
             //nY = 0;
             //sb.AppendLine(nX.ToString());//保存起点
             //sb.AppendLine(nY.ToString());//（nx.ny)

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

             tbcy++; if (tbcy == 4) tbcy = 1;
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

         private void sanjiao()//走三角形
         {
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
                     bdzx(-1500.0, 0.0, 0.0, 3000.0);


                 }
                 if (i == 1)
                 {
                     bdzx(0.0, 3000.0, -3000.0, 3000.0);


                 }
                 if (i == 2)
                 {
                     bdzx(-3000.0, 3000.0, -1500.0, 0.0);


                 }

             }
           
            // gjsave();
         }

         private void zdsanjiao()//zd走三角形
         {
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


         private void wangf()//往返
         {
                // Application.DoEvents();
              
                 dForceX = 0;
                 dForceY = 3500;  //应参数化
                 stepline(dForceX, dForceY);
                 //nX = nX + 200;   //往返隔开画
                            
                 dForceX = 0;
                 dForceY = -3500;
                 tbcy++;
                 if( tbcy ==4) tbcy=1;
                 stepline(dForceX, dForceY);
               

         }

         private void zdwangf()//zd往返
         {
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
             double r = 1500.0;
             if (checkBox13.Checked == true) { r = 4000.0; }
             if (checkBox14.Checked == true) { r = 3000.0; }
             if (checkBox15.Checked == true) { r = 2000.0; }
             double sta = Math.PI/2.0;
             if (checkBox10.Checked == true) { sta = 0.0; }
             if (checkBox11.Checked == true) { sta = Math.PI / 4.0; }
             if (checkBox12.Checked == true) { sta = Math.PI / 2.0; }

             zdzx((-1) * r / 2.0 * Math.Cos(sta), (-1) * r / 2.0 * Math.Sin(sta), r / 2.0 * Math.Cos(sta), r / 2.0 * Math.Sin(sta));
             zdzx(r / 2.0 * Math.Cos(sta), r / 2.0 * Math.Sin(sta), (-1) * r / 2.0 * Math.Cos(sta), (-1) * r / 2.0 * Math.Sin(sta));
             

         }

         private void bdwangf()//往返
         {
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
             double r = 1500.0;
             if (checkBox13.Checked == true) { r = 4000.0; }
             if (checkBox14.Checked == true) { r = 3000.0; }
             if (checkBox15.Checked == true) { r = 2000.0; }
             double sta = Math.PI / 2.0;
             if (checkBox10.Checked == true) { sta = 0.0; }
             if (checkBox11.Checked == true) { sta = Math.PI / 4.0; }
             if (checkBox12.Checked == true) { sta = Math.PI / 2.0; }

             bdzx((-1) * r / 2.0 * Math.Cos(sta), (-1) * r / 2.0 * Math.Sin(sta), r / 2.0 * Math.Cos(sta), r / 2.0 * Math.Sin(sta));
             bdzx(r / 2.0 * Math.Cos(sta), r / 2.0 * Math.Sin(sta), (-1) * r / 2.0 * Math.Cos(sta), (-1) * r / 2.0 * Math.Sin(sta));


             //bdzx(-2000, -2000, 2000, 2000);
             //bdzx(2000, 2000, -2000, -2000);

         }


         private void 示教ToolStripMenuItem_Click(object sender, EventArgs e) //示教菜单  走斜线，按力的大小成比例的进给，力大走的步数就多。
        {       
             sj();
         }

         private void 再现ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             label10.Show();//显示被再现的文件名
             for (int j = 0; j < 4; j++) chart1.Series[j].Points.Clear();//清除每个成员
             tbcy = 1;

             OpenFileDialog ofd = new OpenFileDialog();
             ofd.Title = "请选择要打开的文本文件";
             //ofd.InitialDirectory = @"C:\Users\SpringRain\Desktop";
             ofd.Multiselect = true;
             //ofd.Filter = "数据文件|*.dat|所有文件|*.*";

             ofd.ShowDialog();//显示文件对话框

             string path = ofd.FileName;//获得用户选中的文件的路径
             path1 = path;

             string fileName = Path.GetFileName(path1);  //获得了用户选中的文件名

             if (path1 == "") { return; }

             label10.Text = "系统正在再现:" + fileName;//显示被再现的文件名

             zxht();//先把轨迹画一遍，只画不走。

             using (FileStream fsRead = new FileStream(path1, FileMode.OpenOrCreate, FileAccess.Read))//打开文件流
             {
                 StreamReader sr = new StreamReader(fsRead);//定义文件流的读取变量sr（一个结构体）
                 string s = sr.ReadLine();//（读一行）

                 nX = int.Parse(s);//把字符串变成32位的整型数，读起点
                 s = sr.ReadLine();
                 nY = int.Parse(s);
                 int GJsj;//轨迹数据
                 int i = 0; // 控制显示用                  

                 while ((s = sr.ReadLine()) != null)
                 {


                     if (s == "h") //这一行如果是“h”，表示循环次数已加一，要换一种颜色来画。
                     { s = sr.ReadLine(); tbcy = int.Parse(s); }//再读一行，tbcy是图表成员（4个成员，4种颜色），换一种颜色来画。

                     GJsj = int.Parse(s);
                     textBox1.Text = nX.ToString();
                     textBox2.Text = nY.ToString();
                     if (GJsj == 0)
                     {
                         HardwareOpt.XDirp();
                         HardwareOpt.XMotoForeward(ksped);
                         nX++;
                     }
                     if (GJsj == 1)
                     {
                         HardwareOpt.XDirn();
                         HardwareOpt.XMotoForeward(ksped);
                         nX--;
                     }

                     if (GJsj == 2)
                     {
                         HardwareOpt.YDirp();
                         HardwareOpt.YMotoForeward(ksped);
                         nY++;
                     }
                     if (GJsj == 3)
                     {
                         HardwareOpt.YDirn();
                         HardwareOpt.YMotoForeward(ksped);
                         nY--;
                     }
                     chart1.Series[tbcy + 1].Points.AddXY(nX, nY);//画一点

                     i++;
                     if (i % 1000 == 0) Application.DoEvents();//200个点显示轨迹一次

                 }
                 fsRead.Close();
                 MessageBox.Show("再现完毕");
                 label10.Hide();

             }
         }

         private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
         {

         }

         private void  zxcb(double X, double Y)
         {
                 txtForceX.Text = X.ToString("F1");//显示力数据，一位小数
                 txtForceY.Text = Y.ToString("F1");
                 double xymin = (Math.Min(Math.Abs(X), Math.Abs(Y)))/10.0;
                 if (xymin == 0) xymin = 1;
                 X = X / xymin;
                 Y = Y / xymin;
                 if (X / xymin > 10) { X = 10; Y = 0; }
                 if (Y / xymin > 10) { Y = 10; X = 0; }
              
                 stepline(X, Y);//直线插补(进给，坐标显示，绘图）
         }

         private void 编程ToolStripMenuItem_Click(object sender, EventArgs e)
         {

             Frmbc Frmbc = new Frmbc();
             Frmbc.ShowDialog();
         }

         private void button1_Click(object sender, EventArgs e)//编程
         {
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();

             double r = 1500.0;
             if (checkBox1.Checked == true) { r = 4000.0; }// else r = 1500;
             if (checkBox2.Checked == true) { r = 3000.0; }
             if (checkBox3.Checked == true) { r = 2000.0; }
             nX =(int)((-1)*r);
             nY = 0;
             tbcy = 0;
             sb.AppendLine(nX.ToString());//保存起点
             sb.AppendLine(nY.ToString());//（nx.ny)
             string strY = textBox3.Text;//读程序
             //分析和执行程序
             string xm = strY.Substring(0, 1);
             xm = xm.Trim();
             string nxm=strY.Substring(1, 2);
             textBox5.Text = "共循环" + nxm + "次";
             if (xm == "y")
             {
                 label6.Text = "走圆形";

                 for (int i = 0; i < int.Parse(nxm); i++)
                 { textBox7.Text = (i + 1).ToString()+"次";
                   for(int j=0;j<5;j++) chart1.Series[j].Points.Clear();
                   //sb.AppendLine("11");//
                   //sb.AppendLine(tbcy.ToString());//
                   //sb.AppendLine(nxm);//
                   yuan();
                   tbcy++;
                   if (tbcy == 4) tbcy = 1;
                  
                 } 
             }
             if (xm == "f") 
             {   label6.Text = "走方形";
                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();
                     //sb.AppendLine("22");//
                     //sb.AppendLine(tbcy.ToString());// 
                     //sb.AppendLine(nxm);//
                     fang();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;
                                      
                 }
             }
             if (xm == "s")
             {   label6.Text = "走三角";
                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();
                     //sb.AppendLine("33");//
                     //sb.AppendLine(tbcy.ToString());//
                     //sb.AppendLine(nxm);//
                     sanjiao();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;
                     
                 } 
             }
             if (xm == "w")
             { 
             tbcy = 1;
             label6.Text = "往返走";

             for (int i = 0; i < int.Parse(nxm); i++)
             {
                 textBox7.Text = (i + 1).ToString() + "次";
                 bdzx(-1500, 0, -1500, 3000);
                 bdzx(-1500, 3000, -1500, 0);
             }
             }

             if (strY.Length > 3)
             {
                 xm = strY.Substring(3, 1);
                 nxm = strY.Substring(4, 2);
                 textBox5.Text = "共循环" + nxm + "次";
                 if (xm == "y")
                 {
                     label6.Text = "走圆";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("11");//
                         //sb.AppendLine(tbcy.ToString());// 
                         //sb.AppendLine(nxm);//
                         yuan();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;
                         
                     }
                 }

                 if (xm == "f")
                 {
                     label6.Text = "走方形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("22");//
                         //sb.AppendLine(tbcy.ToString());//  
                         //sb.AppendLine(nxm);//
                         fang();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;
                         
                     }
                 }
                 if (xm == "s")
                 {
                     label6.Text = "走三角形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("33");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         sanjiao();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;
                        
                     }
                 }
                 if (xm == "w")
                 {
                     tbcy = 1;
                     label6.Text = "往返走";
               
                    for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         bdzx(-1500, 0,-1500, 3000);
                         bdzx( -1500, 3000,-1500, 0);
                     }
                   }
             }
             if (strY.Length > 6)
             {
                 xm = strY.Substring(6, 1);
                 nxm = strY.Substring(7, 2);
                 textBox5.Text = "共循环" + nxm + "次";
                 if (xm == "y")
                 {
                     label6.Text = "走圆";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("11");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         yuan();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;
                         
                     }
                 }

                 if (xm == "f")
                 {
                     label6.Text = "走方形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("22");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         fang();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;
                         
                     }
                 }
                 if (xm == "s")
                 {
                     label6.Text = "走三角形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("33");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         sanjiao();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;
                        
                     }
                 }
                 if (xm == "w")
                 {
                     tbcy = 1;
                     label6.Text = "往返走";

                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         bdzx(-1500, 0, -1500, 3000);
                         bdzx(-1500, 3000, -1500, 0);
                     }
                 }
             }
             if (strY.Length > 9)
             {
                 xm= strY.Substring(9, 1);
                 nxm = strY.Substring(10, 2);
                 textBox5.Text = "共循环" + nxm + "次";
             if (xm == "y")
             {
                 label6.Text = "走圆";
                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     //sb.AppendLine("11");//
                     //sb.AppendLine(tbcy.ToString());//
                     //sb.AppendLine(nxm);//
                     yuan();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;
                    
                 } 
             }
            
             if (xm == "f") 
             {   label6.Text = "走方形";
                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     //sb.AppendLine("22");//
                     //sb.AppendLine(tbcy.ToString());//
                     //sb.AppendLine(nxm);//
                     fang();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;
                    
                 }
             }
             if (xm == "s")
             {   label6.Text = "走三角形";
                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     //sb.AppendLine("33");//
                     //sb.AppendLine(tbcy.ToString());//
                     //sb.AppendLine(nxm);//
                     sanjiao();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;
                    
                 } 
             }
             if (xm == "w")
             {
                 tbcy = 1;
                 label6.Text = "往返走";

                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     bdzx(-1500, 0, -1500, 3000);
                     bdzx(-1500, 3000, -1500, 0);
                 }
             }
             }
           //  MessageBox.Show("规定轨迹已走完！");
             gjsave();
         }

         private void textBox4_TextChanged(object sender, EventArgs e)
         {

         }

         private void gjsave()
         {
             string path = System.Environment.CurrentDirectory + string.Format("/data/{0:yyMMdd-HHmmss}.dat", DateTime.Now);//取得文件名

             if (path != "")
             {

                 using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))  //创建输出文件流
                 {
                     byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());//把sb数组中的字符编码形成字节序列，赋给缓冲区
                     fsWrite.Write(buffer, 0, buffer.Length);//把缓冲区的字节数据写入文件
                     fsWrite.Close();
                 }
                 sb = new StringBuilder();//全局数组sb清零，为下一次准备
                 MessageBox.Show("轨迹保存成功");
             }
         }

         private void zxht()//再现
         {
             label10.Show();//显示被再现的文件名
           
             string fileName = Path.GetFileName(path1);  //获得了用户选中的文件名

             label10.Text = "系统正在再现:" + fileName;//显示被再现的文件名

             using (FileStream fsRead = new FileStream(path1, FileMode.OpenOrCreate, FileAccess.Read))//打开文件流
             {
                 StreamReader sr = new StreamReader(fsRead);//定义文件流的读取变量sr（一个结构体）
                 string s = sr.ReadLine();//（读一行）
                 nX = int.Parse(s);//把字符串变成32位的整型数，读起点
                 s = sr.ReadLine();
                 nY = int.Parse(s);
                 int GJsj;//轨迹数据
                 int i = 0; // 控制显示用                  

                 while ((s = sr.ReadLine()) != null)
                 {
                     if (s == "y") //这一行如果是“h”，表示循环次数已加一，要换一种颜色来画。
                     { s = sr.ReadLine(); tbcy = int.Parse(s); }//再读一行，tbcy是图表成员（4个成员，4种颜色），换一种颜色来画。
                     
                     GJsj = int.Parse(s);
                     textBox1.Text = nX.ToString();
                     textBox2.Text = nY.ToString();
                     if (GJsj == 0)
                     {
                         //HardwareOpt.XDirp();
                         //HardwareOpt.XMotoForeward(ksped);
                         nX++;
                     }
                     if (GJsj == 1)
                     {
                         //HardwareOpt.XDirn();
                         //HardwareOpt.XMotoForeward(ksped);
                         nX--;
                     }

                     if (GJsj == 2)
                     {
                         //HardwareOpt.YDirp();
                         //HardwareOpt.YMotoForeward(ksped)();
                         nY++;
                     }
                     if (GJsj == 3)
                     {
                         //HardwareOpt.YDirn();
                         //HardwareOpt.YMotoForeward(ksped)();
                         nY--;
                     }
                     chart1.Series[tbcy].Points.AddXY(nX, nY);//画一点

                     i++;
                     if (i % 1000 == 0) Application.DoEvents();//200个点显示轨迹一次

                 }
                 fsRead.Close();
                 //MessageBox.Show("再现完毕");
                 //label10.Hide();

             }

         }

         private void button6_Click(object sender, EventArgs e)
         {
            timer1.Enabled = true;
         }
         private void timer1_Tick(object sender, EventArgs e)//系统测试
         {

             System.DateTime currentTime = new System.DateTime();//定义时间型变量（结构体）
             currentTime = System.DateTime.Now;//更新时间年月日时分秒

             string strY = currentTime.ToString(); //取当前年月日时分秒

             textBox6.Text = strY;
          
         }

         private void button7_Click(object sender, EventArgs e)
         {

         }

         private void textBox6_TextChanged(object sender, EventArgs e)
         {

         }

         private void button7_Click_1(object sender, EventArgs e)
         {
             bntjt = true;
         }

         private void button8_Click(object sender, EventArgs e)
         {
             bntjx = true;
         }

         private void button9_Click(object sender, EventArgs e)
         {
             bntks = true;
         }

         private void label8_Click(object sender, EventArgs e)
         {

         }

         private void button10_Click(object sender, EventArgs e)//主动走圆
            /*主动训练，走整圆弧： 1.先画理论轨迹（整圆弧） 2.在理论轨迹上画提示小圆点 3.病人用力跟踪小圆点 
             4.系统测力 5.如果力大于设定阻力就按力矢运动，边走边画，边保存理论轨迹和实际轨迹想线段，用于评价训练效果 */
         {   for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
             double r = 1500.0;
             if (checkBox1.Checked == true) { r = 4000.0; }
             if (checkBox2.Checked == true) { r = 3000.0; }
             if (checkBox3.Checked == true) { r = 2000.0; }
             ydzl = 200.0;
             if (checkBox16.Checked == true) { ydzl= 200.0; }
             if (checkBox17.Checked == true) { ydzl= 100.0; }
             if (checkBox18.Checked == true) { ydzl= 50.0; }
             cjds = -180;
             nX = (int)((-1.0) * r);  
             nY =0;

             //sb.AppendLine(nX.ToString());//保存起点
             //sb.AppendLine(nY.ToString());//（nx.ny)

                                
             for (int j = 180; j >=cjds; j --)//画圆（先画一个轨迹）
             {
                 double x1 = r * (Math.Cos(j * Math.PI / 180.0));//产生圆弧轨迹
                 double y1 = r * (Math.Sin(j * Math.PI / 180.0));
                 chart1.Series[1].Points.AddXY(x1, y1);//画小折线,红线
             }
            
             double x0 = r * (Math.Cos(-180 * Math.PI / 180.0));//起点
             double y0 = r * (Math.Sin(-180 * Math.PI / 180.0));

             for (int j = 180; j >= cjds+5; j = j - 5)//走5°弦整圆
             {
                 double x1 = r * (Math.Cos((j - 5) * Math.PI / 180.0));//终点
                 double y1 = r * (Math.Sin((j - 5) * Math.PI / 180.0));
                 chart1.Series[4].Points.AddXY(x1, y1);//画提示小圆点（提前5°）
        
                 dForceX = x1 - x0;
                 dForceY = y1 - y0;
               
                 sb.AppendLine(dForceX.ToString());//保存理论轨迹
                 sb.AppendLine(dForceY.ToString());//
                double dForceX0 = dForceX;
                double dForceY0 = dForceY;

                double fxy = Math.Sqrt(dForceX * dForceX + dForceY * dForceY);//理论小线段长度

             Loop01:
                 HardwareOpt.SamplePointByMean(1, ref dForceX, ref dForceY);// 病人发力跟踪小圆点，4623采集3个力数据求平均

                 double fxy1 = Math.Sqrt(dForceX * dForceX + dForceY * dForceY);//病人主动跟踪小圆点，采集的小线段长度
             
                 if (fxy1 < ydzl) goto Loop01;//病人用力小于设定阻力，则转去重新测力，等待大于阻力。
                
                 double k12 = fxy / fxy1;//fxy是理论长度，fxy1是实际长度，实际长度要等于理论长度，
                 dForceX = Math.Sign(dForceX0) * k12 * Math.Abs(dForceX);//跟踪方向是力方向，跟踪线段长度要等于理论长度
                 dForceY = Math.Sign(dForceY0) * k12 * Math.Abs(dForceY);
                 sb.AppendLine(dForceX.ToString());//保存实际轨迹
                 sb.AppendLine(dForceY.ToString());//
                 txtForceX.Text = dForceX.ToString("F1");
                 txtForceY.Text = dForceY.ToString("F1");

                 chart1.Series[2].Points.AddXY(x0, y0);//此行为了看对应关系（实际与理论的差距）


                 x0 = x1;
                 y0 = y1;
                 stepline1(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                 //HardwareOpt.ReadI(ref b);    //读限位开关
                 //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

             }
             x0 = r * (Math.Cos(-180 * Math.PI / 180.0));//看最后一点
             y0 = r * (Math.Sin(-180 * Math.PI / 180.0));
             chart1.Series[2].Points.AddXY(x0, y0);//
           
             gjsave();
            
         }


         private void button11_Click(object sender, EventArgs e)//被动走圆
         {
            /*被动训练，走90°圆弧： 1.先画理论轨迹（90°圆弧） 2.在理论轨迹上画提示小圆点 3.在理论轨迹上走5°弦. */
         
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
             cjds = 90;
             nX = -4000;
             nY = -4000;
             sb.AppendLine(nX.ToString());//保存起点
             sb.AppendLine(nY.ToString());//（nx.ny)

             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
                        
             for (int j = 180; j >=cjds; j --)//画圆（先画一个轨迹）
             {
                 double x1 = 4000.0 * (Math.Cos(j * Math.PI / 180.0));//产生圆弧轨迹
                 double y1 = 4000.0 * (Math.Sin(j * Math.PI / 180.0));
                 chart1.Series[1].Points.AddXY(x1, y1-4000);//画小折线,红线
             }
            
             double x0 = 4000.0 * (Math.Cos(-180 * Math.PI / 180.0));//起点
             double y0 = 4000.0 * (Math.Sin(-180 * Math.PI / 180.0));

             for (int j = 180; j >= cjds+5; j = j - 5)//走整圆5°弦
             {
                 double x1 = 4000.0 * (Math.Cos((j - 5) * Math.PI / 180.0));//终点
                 double y1 = 4000.0 * (Math.Sin((j - 5) * Math.PI / 180.0));
                 chart1.Series[4].Points.AddXY(x1, y1-4000);//画提示小圆点（提前5°）

                 x1 = 4000.0 * (Math.Cos(j * Math.PI / 180.0));//终点 理论轨迹
                 y1 = 4000.0 * (Math.Sin(j * Math.PI / 180.0));
                 //计算小折线            
                 dForceX = x1 - x0;
                 dForceY = y1 - y0;
                 x0 = x1;
                 y0 = y1;
               
                 txtForceX.Text = dForceX.ToString("F1");
                 txtForceY.Text = dForceY.ToString("F1");

                 stepline(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                 //HardwareOpt.ReadI(ref b);    //读限位开关
                 //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

             }
           
                gjsave();
            
         }

         private void button12_Click(object sender, EventArgs e)//主动走线
         {            
            /*主动训练，走斜线段： 1.先画理论轨迹 2.在理论轨迹上画提示小圆点 3.病人用力跟踪小圆点 
              4.系统测力 5.如果力大于设定阻力就按力矢运动，边走边画，边保存理论轨迹和实际轨迹想线段，用于评价训练效果 */
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
           
             zdzx(-4000,-4000,0,0);
             gjsave();

             }

         private void button13_Click(object sender, EventArgs e)//急停
         {
             bntjt = true;
         }

         private void button14_Click(object sender, EventArgs e)//被动走线
         {
             
             /*被动训练，走斜线段： 1.先画理论轨迹 2.在理论轨迹上画提示小圆点 3.走小线段，边走边画*/

             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
           
             nX = -4000;//起点
             nY = -4000;
             int nxe = 0;//终点
             int nye = 0;

             int x = nxe - nX;
             int y = nye - nY;
             double r = Math.Sqrt(x * x + y * y);//线段理论长度
             double sta = Math.Atan(y / x);//线段斜率
             double n = 10.0;//把线段n等分
             //sb.AppendLine(nX.ToString());//保存起点
             //sb.AppendLine(nY.ToString());//（nx.ny)

             for (int j = 0; j <= n; j++)//画轨迹
             {
                 //Application.DoEvents();
                 chart1.Series[1].Points.AddXY(j*r/n*Math.Cos(sta)-4000,j*r/n*Math.Sin(sta) - 4000);//画轨迹
             }

              for (int j = 0; j < n; j++)//被动走轨迹
             {
                 chart1.Series[4].Points.AddXY((j+1)*r/n*Math.Cos(sta)-4000,(j+1)*r/n*Math.Sin(sta) - 4000);//画提示圆点            
                 //计算小线段            
                 dForceX = x/n;
                 dForceY = y/n;

                 txtForceX.Text = dForceX.ToString("F1");
                 txtForceY.Text = dForceY.ToString("F1");

                 stepline(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                 //HardwareOpt.ReadI(ref b);    //读限位开关
                 //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

             }

                gjsave();

             }

         private void checkBox3_CheckedChanged(object sender, EventArgs e)
         {

         }


         private void zdzx(double x0, double y0, double xe, double ye)   //主动走线
         {
             nX = (int)x0;//起点
             nY = (int)y0;
             double x = xe - x0;
             double y = ye - y0;
             double r = Math.Sqrt(x * x + y * y);//线段理论长度
             double sta = Math.Atan(y / x);//线段斜率
             if (x<0.0) sta = sta + Math.PI;//线段斜率
             double n = 10.0;//把线段n等分
             ydzl = 200.0;
             if (checkBox16.Checked == true) { ydzl = 200.0; }// else r = 1500;
             if (checkBox17.Checked == true) { ydzl = 100.0; }
             if (checkBox18.Checked == true) { ydzl = 50.0; }
            
             for (int j = 0; j <= n; j++)//画轨迹
             {
                
               chart1.Series[1].Points.AddXY(j * r / n * Math.Cos(sta) +x0, j * r / n * Math.Sin(sta) +y0);//画轨迹
             
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
                 dForceX = Math.Sign(dForceX0+1) * k12 * Math.Abs(dForceX);//跟踪方向是力方向，跟踪线段长度要等于理论长度
                 dForceY = Math.Sign(dForceY0+1) * k12 * Math.Abs(dForceY);
                 sb.AppendLine(dForceX.ToString());//保存实际轨迹
                 sb.AppendLine(dForceY.ToString());//
                 txtForceX.Text = dForceX.ToString("F1");
                 txtForceY.Text = dForceY.ToString("F1");
               
                 stepline1(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                 chart1.Series[2].Points.AddXY((j+1) * r / n * Math.Cos(sta) + x0, (j+1) * r / n * Math.Sin(sta) + y0);//画连线  

                 //HardwareOpt.ReadI(ref b);    //读限位开关
                 //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

             }
            
            chart1.Series[4].Points.AddXY(r * Math.Cos(sta) + x0, r * Math.Sin(sta) + y0);//画提示圆点 
          

         }

         private void bdzx(double x0, double y0, double xe, double ye)   ////被动走线
         {
             nX = (int)x0;//起点
             nY = (int)y0;
             double x = xe - x0;
             double y = ye - y0;
             double r = Math.Sqrt(x * x + y * y);//线段理论长度
             double sta = Math.Atan(y / x);//线段斜率
             if (x < 0.0) sta = sta + Math.PI;//线段斜率
             double n = 6.0;//把线段n等分
             tbcy++; if (tbcy == 4) tbcy = 1;
             chart1.Series[tbcy].BorderWidth = 1;
             for (int j = 0; j <= n; j++)//画轨迹
             {
                
                 chart1.Series[tbcy].Points.AddXY(j * r / n * Math.Cos(sta) + x0, j * r / n * Math.Sin(sta) + y0);//画轨迹

             }

             for (int j = 0; j < n; j++)//被动走轨迹
             {

                 chart1.Series[4].Points.AddXY(j * r / n * Math.Cos(sta) + x0, j * r / n * Math.Sin(sta) + y0);//画提示圆点  

                 dForceX = x / n;
                 dForceY = y / n;
                 //double dForceX0 = dForceX;
                 //double dForceY0 = dForceY;

                 sb.AppendLine(dForceX.ToString());//保存小线段理论轨迹
                 sb.AppendLine(dForceY.ToString());//

                 txtForceX.Text = dForceX.ToString("F1");
                 txtForceY.Text = dForceY.ToString("F1");

                 stepline1(dForceX, dForceY);//逐点比较法直线插补，起点坐标(0,0)，终点坐标(dForceX, dForceY)

                 ////chart1.Series[2].Points.AddXY((j + 1) * r / n * Math.Cos(sta) + x0, (j + 1) * r / n * Math.Sin(sta) + y0);//画连线  

                 //HardwareOpt.ReadI(ref b);    //读限位开关
                 //if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环

             }

             //chart1.Series[4].Points.AddXY(r * Math.Cos(sta) + x0, r * Math.Sin(sta) + y0);//画提示圆点 


         }

         private void button15_Click(object sender, EventArgs e)//主动走方
         {
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个图表成员
             Int32 b = 0xFF;
             tbcy = 1;
             //nX = -1500;
             //nY = 0;
             //sb.AppendLine(nX.ToString());//保存起点
             //sb.AppendLine(nY.ToString());//（nx.ny)

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

             tbcy++;  if (tbcy == 4) tbcy = 1;
             for (int i = 0; i < 4; i++)//走方
             {
                 if (i == 0)
                 {
                     zdzx(-1500.0, 0, 1500, 0.0);//绝对坐标
                    
                 }
                 if (i == 1)
                 {
                     zdzx(1500.0,0.0,1500.0,3000.0);
                    
                 }
                 if (i == 2)
                 {
                     zdzx(1500,3000.0,-1500,3000);
                    
                 }
                 if (i == 3)
                 {
                     zdzx(-1500.0,3000.0, -1500.0,0.0);
                  

                 }
                 HardwareOpt.ReadI(ref b);    //读限位开关
                 if ((b & 0x0F) != 0x0F) break;// 任意一个出界都退出循环
             }
            
         }

         private void button16_Click(object sender, EventArgs e)//主动走三角
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
                     zdzx( 0.0, 3000.0,-3000.0, 3000.0);
                    

                 }
                 if (i == 2)
                 {
                     zdzx(-3000.0, 3000.0,-1500.0,0.0 );
                   

                 }

             }
              gjsave();
         }

         private void button17_Click(object sender, EventArgs e)//主动往返走
         {
             //double r = 1500.0;
             //if (checkBox13.Checked == true) { r = 4000.0; }
             //if (checkBox14.Checked == true) { r = 3000.0; }
             //if (checkBox15.Checked == true) { r = 2000.0; }
             tbcy = 1;
             label6.Text = "往返走";
             string nxm = "2";
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();//清除每个成员
             for (int i = 0; i < int.Parse(nxm); i++)
             {
                 textBox5.Text = "共循环" + nxm + "次";
                 textBox7.Text = (i + 1).ToString() + "次";
                 zdwangf();
             }
             gjsave();
         }

         private void toolStripButton2_Click(object sender, EventArgs e)
         {
             this.Hide();
             Frmbc Frmbc = new Frmbc();
             Frmbc.ShowDialog();
         }

         private void button11_Click_1(object sender, EventArgs e)
         {
             for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();
             double r = 1500.0;
             if (checkBox1.Checked == true) { r = 4000.0; }// else r = 1500;
             if (checkBox2.Checked == true) { r = 3000.0; }
             if (checkBox3.Checked == true) { r = 2000.0; }
             nX = (int)((-1) * r);
             
             nY = 0;
             tbcy = 0;
             sb.AppendLine(nX.ToString());//保存起点
             sb.AppendLine(nY.ToString());//（nx.ny)
             string strY = textBox3.Text;//读程序
             //分析和执行程序
             string xm = strY.Substring(0, 1);
             xm = xm.Trim();
             string nxm = strY.Substring(1, 2);
             textBox5.Text = "共循环" + nxm + "次";
             if (xm == "y")
             {
                 label6.Text = "走圆形";

                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();
                     //sb.AppendLine("11");//
                     //sb.AppendLine(tbcy.ToString());//
                     //sb.AppendLine(nxm);//
                     zdyuan();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;

                 }
             }
             if (xm == "f")
             {
                 label6.Text = "走方形";
                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();
                     //sb.AppendLine("22");//
                     //sb.AppendLine(tbcy.ToString());// 
                     //sb.AppendLine(nxm);//
                     zdfang();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;

                 }
             }
             if (xm == "s")
             {
                 label6.Text = "走三角";
                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     for (int j = 0; j < 5; j++) chart1.Series[j].Points.Clear();
                     //sb.AppendLine("33");//
                     //sb.AppendLine(tbcy.ToString());//
                     //sb.AppendLine(nxm);//
                     zdsanjiao();
                     tbcy++;
                     if (tbcy == 4) tbcy = 1;

                 }
             }
             if (xm == "w")
             {
                 tbcy = 1;
                 label6.Text = "往返走";

                 for (int i = 0; i < int.Parse(nxm); i++)
                 {
                     textBox7.Text = (i + 1).ToString() + "次";
                     zdzx(-1500, 0, -1500, 3000);
                     zdzx(-1500, 3000, -1500, 0);
                 }
             }

             if (strY.Length > 3)
             {
                 xm = strY.Substring(3, 1);
                 nxm = strY.Substring(4, 2);
                 textBox5.Text = "共循环" + nxm + "次";
                 if (xm == "y")
                 {
                     label6.Text = "走圆";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("11");//
                         //sb.AppendLine(tbcy.ToString());// 
                         //sb.AppendLine(nxm);//
                         zdyuan();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }

                 if (xm == "f")
                 {
                     label6.Text = "走方形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("22");//
                         //sb.AppendLine(tbcy.ToString());//  
                         //sb.AppendLine(nxm);//
                         zdfang();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }
                 if (xm == "s")
                 {
                     label6.Text = "走三角形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("33");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         zdsanjiao();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }
                 if (xm == "w")
                 {
                     tbcy = 1;
                     label6.Text = "往返走";

                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         zdzx(-1500, 0, -1500, 3000);
                         zdzx(-1500, 3000, -1500, 0);
                     }
                 }
             }
             if (strY.Length > 6)
             {
                 xm = strY.Substring(6, 1);
                 nxm = strY.Substring(7, 2);
                 textBox5.Text = "共循环" + nxm + "次";
                 if (xm == "y")
                 {
                     label6.Text = "走圆";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("11");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         zdyuan();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }

                 if (xm == "f")
                 {
                     label6.Text = "走方形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("22");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         zdfang();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }
                 if (xm == "s")
                 {
                     label6.Text = "走三角形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("33");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         zdsanjiao();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }
                 if (xm == "w")
                 {
                     tbcy = 1;
                     label6.Text = "往返走";

                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         zdzx(-1500, 0, -1500, 3000);
                         zdzx(-1500, 3000, -1500, 0);
                     }
                 }
             }
             if (strY.Length > 9)
             {
                 xm = strY.Substring(9, 1);
                 nxm = strY.Substring(10, 2);
                 textBox5.Text = "共循环" + nxm + "次";
                 if (xm == "y")
                 {
                     label6.Text = "走圆";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("11");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         zdyuan();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }

                 if (xm == "f")
                 {
                     label6.Text = "走方形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("22");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         zdfang();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }
                 if (xm == "s")
                 {
                     label6.Text = "走三角形";
                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         //sb.AppendLine("33");//
                         //sb.AppendLine(tbcy.ToString());//
                         //sb.AppendLine(nxm);//
                         zdsanjiao();
                         tbcy++;
                         if (tbcy == 4) tbcy = 1;

                     }
                 }
                 if (xm == "w")
                 {
                     tbcy = 1;
                     label6.Text = "往返走";

                     for (int i = 0; i < int.Parse(nxm); i++)
                     {
                         textBox7.Text = (i + 1).ToString() + "次";
                         zdzx(-1500, 0, -1500, 3000);
                         zdzx(-1500, 3000, -1500, 0);
                     }
                 }
             }
             //  MessageBox.Show("规定轨迹已走完！");
             gjsave();
         }

         private void checkBox15_CheckedChanged(object sender, EventArgs e)
         {

         }

         private void checkBox10_CheckedChanged(object sender, EventArgs e)
         {

         }

         private void label7_Click(object sender, EventArgs e)
         {

         }

         private void label11_Click(object sender, EventArgs e)
         {

         }

         private void groupBox4_Enter(object sender, EventArgs e)
         {

         }

         private void checkBox11_CheckedChanged(object sender, EventArgs e)
         {

         }

         private void checkBox12_CheckedChanged(object sender, EventArgs e)
         {

         }

         private void checkBox17_CheckedChanged(object sender, EventArgs e)
         {

         }

         private void checkBox21_CheckedChanged(object sender, EventArgs e)
         {

         }

         private void checkBox19_CheckedChanged(object sender, EventArgs e)
         {

         }
         
       




         

      
                


     }   //class FrmMain : For

 }      //namespace Test
 