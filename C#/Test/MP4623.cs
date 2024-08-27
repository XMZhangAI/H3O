using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Test //名称空间
{
    static class MP4623 //4623的所有函数都在mp4623这个类
    {
        //打开一个 MP4623 设备 
        //函数：HANDLE MP4623_OpenDevice(__int32 dev_num) 
        //dev_num：入口参数，MP4623 设备号，=0、1、2….，表示第一个、第二个 MP4623 模块。设备号的定义 参考驱动安装部分。  
        //函数返回值：卡的操作句柄。
        [DllImport("MP4623.DLL")]
        public static extern IntPtr MP4623_OpenDevice(Int32 dev_num);

        //关闭一个 MP4623 设备 
        //函数：__int32 MP4623_CloseDevice(HANDLE hDevice) 
        //功能：关闭以 hDevice 打开的 MP4623 卡。
        //hDevice：入口参数，卡的操作句柄。  
        //函数返回数值：0：成功 / -1：失败。
        [DllImport("MP4623.DLL")]
        public static extern Int32 MP4623_CloseDevice(IntPtr hDevice);


        //开关量输入 功能：读入 16 位开关量输入。 
        //函数：__int32 MP4623_DI(HANDLE hDevice)  
        //hDevice：入口参数，卡的操作句柄。  
        //函数返回：出口参数，返回读入的数据。低 16 位数据（D15-D0）对应输入端口 15-0 号。
        [DllImport("MP4623.DLL")]
        public static extern Int32 MP4623_DI(IntPtr hDevice);

        //开关量输出 功能：设置 16 位输出数据。 
        //函数：__int32 MP4623_DO(HANDLE hDevice,__int32 DO_Data)  
        //hDevice：入口参数，卡的操作句柄。  
        //DO_Data：入口函数，输出的数据。数据的低 16 位有效。16 位数据（D15-D0）分别对应端口的 16 个 IO 输出口 15-0 号  
        //函数返回：出口参数，=0 操作成功，其他失败
        [DllImport("MP4623.DLL")]
        public static extern Int32 MP4623_DO(IntPtr hDevice, Int32 data);

        //转换电压计算： 
        //注：用户可以用函数 double MP4623_ADV(__int32 adg,__int32 addata)计算电压 
        //1. 返回电压，单位 mV 
        //2. adg：用户采样时设置的 gain 的数值 
        //3. addata：需要计算的 12 位 AD 数据。（addata=读入数据/16）
        [DllImport("MP4623.DLL")]
        public static extern double MP4623_ADV(Int32 adg, Int32 addata);


        //MP4623_CAL() AD 校正操作。 
        //功能：启动 AD 自动校正操作。在开机时，至少要进行一次此操作。 
        //函数：__int32 MP4623_CAL(HANDLE hDevice) 
        //hDevice：入口参数，卡的操作句柄。  
        //函数返回：出口参数：=0 操作成功/其他失败。
        [DllImport("MP4623.DLL")]
        public static extern Int32 MP4623_CAL(IntPtr hDevice);

        //MP4623_FAD()设置采样参数并启动 AD 采样 
        //功能：设置所有与采样相关的参数并启动采样过程。 
        //函数：__int32 MP4623_FAD(HANDLE hDevice, __int32 stch,__int32 endch,__int32 gain,__int32 sidi, 
        //__int32 sammode,__int32 trsl,__int32 trpol, __int32 clksl,__int32 clkpol,__int32 tdata,__int32 saml)  
        //hDevice：入口参数，卡的操作句柄。  
        //stch：入口参数，=0-14 设置采样的起始通道号码。  
        //endch：入口参数，=0-14 设置采样的停止通道号码。  
        //gain：设置 AD 的输入量程 G。=0-1 对应选择所有的输入范围见下面表格。0：-5V-+5V，1：-10V-+10V  
        //sidi：=0 ，MP4623 不用这个参数，必须设置为 0。  
        //trsl：设置触发模式。=0 设置软件启动一次采样过程/=1：设置外部触发启动一次采样过程。  
        //trpol：设置触发输入极性。=0 设置外部触发上升边沿有效/=1 设置外部触发下降边沿有效。  
        //clksl：设置时钟模式。=0 设置 AD 启动利用内部时钟/=1：外部时钟。  
        //clkpol：设置时钟输入极性。=0 设置上升边沿有效/=1 设置下降边沿有效。  
        //tdata：设置采样频率（40～65535）。总采样速度=20000/tdata(KHz)，AD 启动周期=0.05 * tdata (uS)。 详细见第二章说明。  
        //saml：设置采样长度（1～2，000，000）。
        //函数返回：出口参数：=0 操作成功/其他失败。
        [DllImport("MP4623.DLL")]
        public static extern Int32 MP4623_FAD(IntPtr hDevice, Int32 stch, Int32 endch, Int32 gain, Int32 sidi, Int32 trsl, Int32 trpol, Int32 clksl, Int32 clkpol, Int32 tdata, Int32 saml);

        //MP4623_FRead() 回读采样数据 
        //功能：判断采样是否结束，如果结束读入用户设置长度的采样数据。函数返回长度“大于或等于 1”表示采样
        //正在进行，返回等于“0”表示结束并将用户数据回读入用户定义的数组中。注意：为了区别状态，即使 AD 
        //没有启动采样，返回数值也=1，用户可以利用 FREAD 在慢速采集状态下查询已经采集到多少数据。
        //采样数据的排列按用户设置的起始与停止通道顺序循环排列，例如：起始通道=0，结束通道=2，
        //读出数据排 列按如下顺序： ch0 ch1 ch2 ch0 ch1 ch2 …….ch0 ch1 ch2
        //函数：__int32 MP4623_FRead(HANDLE hDevice，__int32 *rdata,__int32 readlen)  
        //hDevice：入口参数，卡的操作句柄。  
        //*rdata：指向存储回读数据数组(32 位有符号数)的指针，要求数组容量大于 FAD 中设置的采样长度 saml。
        //readlen：用户设置的回读数据长度。 
        //函数返回：如果小于 0 表示 MP4623 的硬件或软件缓冲溢出错误（此时以后的采样点均无效）。
        //其他表示采样忙,返回数值为 AD 已经存储器中已经采集数据的个数（个数为 0 或 1 时，返回数据=1）；
        //=0 表示采样结束并已经将数据放入 rdata 中，长度=saml。
        //MP4623 读入的 32 位 AD 数据的低 16 位为的高 12 位 AD 数据；最低 4 位对应开关量输入状态 DI0-DI3
        [DllImport("MP4623.DLL")]
        /*unsafe*/ public static extern Int32 MP4623_FRead(IntPtr hDevice, Int32[] addata, Int32 readlen);



        //设置脉冲输出模式、数据、输出端口允许。 
        //功能：设置并启动输出。 
        //函数：int32 MP4623_PRun(HANDLE hDevice,int32 pch, int32 pmode, int32 pdata0, int32 pdata1)  
        //hDevice：入口参数，卡的操作句柄。
        //pch: =0-1：选择通道 0-1 号。  
        //pmode：工作模式，=0-2.  
        //Pdata0: =0 – FFFFFH， 设置 0 号数据。对应 PWM 模式的周期、SP 模式的宽度、PLP 模式的周期。  
        //Pdata1: =0 – FFFFFH， 设置 1 号数据。对应 PWM 模式的正脉冲宽度、PLP 模式的脉冲个数。  
        //函数返回：0 正常/其它失败
        [DllImport("MP4623.DLL")]
        public static extern Int32 MP4623_PRun(IntPtr hDevice, Int32 pch, Int32 pmode, Int32 pdata0, Int32 pdata1);


        //结束脉冲输出并将对应输出端口恢复到开关量输出。
        //函数：int32 MP4623_PEnd(HANDLE hDevice,int32 pch)  
        //hDevice：入口参数，卡的操作句柄。 
        //pch: =0,1：选择通道 0、1 号。  
        //函数返回：-1:失败 。 =0 正常。
        [DllImport("MP4623.DLL")]
        public static extern Int32 MP4623_PEnd(IntPtr hDevice, Int32 pch);
    }

    
}
