using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using MathNet.Numerics.Statistics;

namespace Test
{
    public static class HardwareOpt
    {
        public static IntPtr hDevice;
        public static Int32 Int32_DO;
       // public static Int32 Int32_DI;

        public static int nSampleFrequency = 20000;//采样频率20K
        public static int nChannelCount = 2;//通道数
        public static int nCycle = 20;//采集周期数
        public static double dn = 0.0;

        public static void InitCard()
        {
            HardwareOpt.hDevice = MP4623.MP4623_OpenDevice(0);//加载驱动，获取驱动句柄
            MP4623.MP4623_CAL(HardwareOpt.hDevice);
        }

        public static void SamplePointByMean(int nMeanPointCount,ref double dChannel0,ref double dChannel1)
        {
            dChannel0 = 0;
            dChannel1 = 0;

            if (hDevice != (IntPtr)(-1) && hDevice != IntPtr.Zero)
            {
                int gain = 0;//-5_+5对应gain=0
                int nSampleCount = nMeanPointCount * HardwareOpt.nChannelCount;
                Int32[] addata = new Int32[nSampleCount];

                MP4623.MP4623_CAL(hDevice);//AD校准
                MP4623.MP4623_FAD(hDevice, 0, nChannelCount - 1, gain, 0, 0, 0, 0, 0, 20000 / HardwareOpt.nSampleFrequency * 1000, nSampleCount);//采样频率20K

                int nCount = 0;
                do
                {
                    //Thread.Sleep(1);
                    for (int i = 0; i < 1000; i++) ;
                    nCount = MP4623.MP4623_FRead(hDevice, addata, nSampleCount);
                } while (nCount != 0);

                double[] dPoints0 = new double[nMeanPointCount];
                double[] dPoints1 = new double[nMeanPointCount];
                for (int i = 0; i < nMeanPointCount; i++)
                {
                    dPoints0[i] = MP4623.MP4623_ADV(gain, addata[i * HardwareOpt.nChannelCount] >> 4);
                    dPoints1[i] = MP4623.MP4623_ADV(gain, addata[i * HardwareOpt.nChannelCount + 1] >> 4);
                    //dPoints0[i] = dPoints0[i] * Program.dSensorK + Program.dSensorB;
                    //dPoints1[i] = dPoints1[i] * Program.dSensorK + Program.dSensorB;
                }

                dChannel0 = Statistics.Mean(dPoints0);
                dChannel1 = Statistics.Mean(dPoints1);
            }
            else //没有采集卡 ，随机产生数据
            {
                Random rd = new Random();
                double r = rd.NextDouble() * 500;
                dChannel0 = r;  // *Program.dSensorK + Program.dSensorB;
                r = rd.NextDouble() * 500;
                dChannel1 = r;  // *Program.dSensorK + Program.dSensorB;
            }

            return;
        }

        public static void ReadI(ref Int32 a)//
        {
            if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero) return;
            a = MP4623.MP4623_DI(hDevice);
            return;
        }
       
        public static void XMotoForeward(int sped)//x电机走一步
        {
       
            if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                return;

            Int32_DO &= 0xFE;  //1111-1111-1111-1110
            MP4623.MP4623_DO(hDevice, Int32_DO);
            //Thread.Sleep(1);
           for (int i = 0; i < 50000; i++) ;

            Int32_DO |= 0x1;  //0000-0000-0000-0001
            MP4623.MP4623_DO(hDevice, Int32_DO);
         
            Thread.Sleep(sped);
        //  for (int i = 0; i <sped*50000; i++) ;
        }

      
        public static void YMotoForeward(int sped) //y电机走一步

        {
            if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                return;

            Int32_DO &= 0xFB;  //1111-1111-1111-1011 DO2
            MP4623.MP4623_DO(hDevice, Int32_DO);
        
            for (int i = 0; i < 50000; i++) ;

            Int32_DO |= 0x4;  //0000-0000-0000-0100
            MP4623.MP4623_DO(hDevice, Int32_DO);
            Thread.Sleep(sped);
          // for (int i = 0; i <sped*50000; i++) ;
                  
        }

    

         public static void XDirp()
         {
             if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                 return;

             Int32_DO &= 0xFD;  //1111-1101  DO1
             MP4623.MP4623_DO(hDevice, Int32_DO);
             for (int i = 0; i <50000; i++) ;
         }
         public static void XDirn()
         {
             if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                 return;
             Int32_DO |= 0x2;  //0000-0000-0000-0010 DO1
             MP4623.MP4623_DO(hDevice, Int32_DO);
             for (int i = 0; i < 50000; i++) ;
         }

         public static void YDirp()
         {
             if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                 return;

             Int32_DO &= 0xF7;  //1111-1111-1111-0111 DO3
             MP4623.MP4623_DO(hDevice, Int32_DO);
         }

         public static void YDirn()
         {
             if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                 return;
             Int32_DO |= 0x8;  //0000-0000-0000-1000
             MP4623.MP4623_DO(hDevice, Int32_DO);
         }
        
        public static void StratMoto(int nSpeed, bool bForeward)
        {
            if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                return;

            //开伺服
            //Int32_DO &= 0xFE;  //1111-1111-1111-1110

            //速度
            Int32 pdata0 = 60000 / nSpeed; // = (60 * 10000000) / (10000 * nSpeed) （每圈10000个脉冲）


            //方向
            if (!bForeward)
                Int32_DO |= 0x1; //0000-0000-0000-0001
            else
                Int32_DO &= 0xFE; //1111-1111-1111-1110

            MP4623.MP4623_DO(hDevice, Int32_DO);

            MP4623.MP4623_PRun(hDevice, 0, 0, pdata0, pdata0 / 2);
        }

        public static void StopMoto()
        {
            if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                return;

            ////关伺服
            //Int32_DO |= 0x1;  //0000-0000-0000-0001

            MP4623.MP4623_DO(hDevice, Int32_DO);
            MP4623.MP4623_PEnd(hDevice, 0);
        }

        public static void MotoServo(bool bEnalbe)
        {
            if (hDevice == (IntPtr)(-1) || hDevice == IntPtr.Zero)
                return;

            //if (!bEnalbe)
            //    Int32_DO |= 0x1;  //0000-0000-0000-0001关伺服
            //else
            //    Int32_DO &= 0xFE;  //1111-1111-1111-1110开伺服
            //MP4623_DO(hDevice, Int32_DO);
        }

       
    }
}
