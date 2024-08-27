//opem MP4623
extern "C" HANDLE __declspec(dllimport)  __stdcall MP4623_OpenDevice(__int32 dev_num);
//close device
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_CloseDevice(HANDLE hDevice);


//********************************************
//get board info
//model or type in *bStr
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_Info(HANDLE hDevice,char *modle);



//----------------------------EEPROM------------------------------------------
//read  32byte , buffer must great 256
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_EEPROM_Read(HANDLE hDevice,unsigned char *rbuf);
//write length=32
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_EEPROM_Write(HANDLE hDevice, unsigned char *wbuf);





//---------------------------------------------------------------------------------------------
//AD
//sammode=0 normal scan AD mode/ =1 SH -burst mode
//trsl: =0 soft /=1 trig
//trpol:=0 rising edge / =1 falling edge
//clksl=0 inner adclk / =1 out clk
//clkpol=0 rsining edge / =1 falling edge 
//tdata:=50-65535 , AD cycle=0.1uS * tdata
//gain: 0-3 via 10/5/2.5/1.25V, 4-7 via B10/B5/B2.5/B1.25V 
//sidi=1 di input
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_AD(HANDLE hDevice,
														   __int32 stch,__int32 endch,__int32 gain,__int32 sidi,
														   __int32 trsl,__int32 trpol,__int32 clksl,__int32 clkpol,
														   __int32 tdata);

extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_CAL(HANDLE hDevice);

//------------------------------------------------------------------------------------------
//polling if end of ad
// if fifo full return -1, else return read length, max length=512K word or =524288 words
// *fdata's length must >=524288
// return -1, fifo error of full, >=0 real read data length
//rdlen: user set data length to readm addata's size must > rdlen
//       rdlen>= MP4623_Poll's return value

extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_ADRead(HANDLE hDevice,__int32 rdlen, __int32 *addata);

//polling state 
//<0 fifo over error
//>=0 sam data length for user read
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_ADPoll(HANDLE hDevice);

//stop
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_ADStop(HANDLE hDevice);


//Fixed length sam
//run AD
//saml <=2000000
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_FAD(HANDLE hDevice,__int32 stch,__int32 endch,__int32 gain,__int32 sidi,__int32 trsl,__int32 trpol,__int32 clksl,__int32 clkpol,__int32 tdata, __int32 saml);
//read data
//if Return =1 AD busy, =0  read data ok and AD ok
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_FRead(HANDLE hDevice,__int32 *addata, __int32 readlen);




//convert data to voltage at mV
extern "C" double __declspec(dllimport) __stdcall MP4623_ADV(__int32 adg,__int32 addata);

//AD at POLL mode
//extern "C" __int32 __declspec(dllexport)  __stdcall MP4623_PAD(HANDLE hDevice, __int32 ch, __int32 gain, __int32 sidi,__int32 naver);

//DI
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_DI(HANDLE hDevice);
//DO
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_DO(HANDLE hDevice,__int32 data);

//-----------------------------------------------------------------------------------------------------
//DA
//set da out range
//DA
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_DA(HANDLE hDevice,__int32 dach,__int32 dag,__int32 dadata);


//--------------------------------------------------------------------------------------------------------------------
//stop temp cnt and restart cnt  and  setting cnt data
//cntmode: is using only by ch0-3
//cdata: is 24bit data from 0-FFFFFFH
extern "C" __int32 __declspec(dllexport) __stdcall MP4623_CRun(HANDLE hDevice,__int32 cntch, __int32 cntmode);


//read cnt
//cnt channel: cnntch
//*cdata: cnt 24bit data
//*tdata: timer's 24bit data , 0.1uS /LSB
//return: 0 ok / =1 cnt is over <=0
extern "C" __int32 __declspec(dllimport) __stdcall MP4623_CRead(HANDLE hDevice,__int32 cntch, __int32 *cdata);
//end cnt
//---------------------------------------------------------------------------------------------------------------------



///start pout and set out eb
//pch =0,1
//pmode: 0 PWM mode / 1 single pulse mode
//PWM mode: pdata0 cycle / padat1 high wideth
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_PRun(HANDLE hDevice,__int32 pch, __int32 pmode,__int32 pdata0, __int32 pdata1);
//return out state, =1 out =1 / =0 out =0
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_PState(HANDLE hDevice,__int32 pch);
//stop pch and enable DO ch recover
extern "C" __int32 __declspec(dllimport)  __stdcall MP4623_PEnd(HANDLE hDevice,__int32 pch);
//set pulse out  data
extern "C" __int32 __declspec(dllexport)  __stdcall MP4623_PSetData(HANDLE hDevice,__int32 pch,__int32 pdata0, __int32 pdata1);





