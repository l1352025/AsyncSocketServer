using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AsyncSocketServer
{
    class Packet
    {
        public stLoginReq loginReq;
        public stLoginRsp loginRsp;
        public stLoginRsp2 loginRsp2;
        public stDevInfo devInfo;
        public stChangeDesk deskChange;
        public stPlayerData playerData;
        public stCmSetNtf cmSet;
        public stGameState gameState;
        public stGameOddsNtf gameOdds;
        public stOpenRstNtf openResult;
        //public stOpenListReq openListReq;
        public stOpenListRsp openListRsp;
        public stBetTotalNtf betTotal;
        public stBetInfo betInfo;
        public stBetCancel betCancel;
        public stBetQryReq betQryReq;
        public stBetQryRsp betQryRsp;
        public stVideoRegReq videoRegReq;
        public stVideoRegRsp videoRegRsp;


        public Packet()
        {
            loginReq.cmd = 0x222A;
            loginReq.len = 48;
            loginRsp.cmd = 0x222A;
            loginRsp.len = 16;
            loginRsp2.cmd = 0x222B;
            loginRsp2.len = 48;

            videoRegReq.cmd = 0xAB01;
            videoRegReq.len = 24;
            videoRegRsp.cmd = 0xAB01;
            videoRegRsp.len = 12;

            playerData.cmd = 0x2712;
            playerData.len = 16;
            playerData.remain = 10000;
            playerData.profit = 0;

            deskChange.cmd = 0x2715;
            deskChange.len = 16;

            openResult.cmd = 0x271A;
            openResult.len = 36;

            betInfo.cmd = 0x271C;
            betInfo.len = 36;

            betCancel.cmd = 0x271D;
            betCancel.len = 16;

            cmSet.cmd = 0x2713;
            cmSet.len = 40;

            gameOdds.cmd = 0x2720;
            gameOdds.len = 32;

            betTotal.cmd = 0x2726;
            betTotal.len = 32;

            openListRsp.cmd = 0x2731;   // len = 20 + N
            openListRsp.list = new byte[180];

            betQryRsp.cmd = 0x8000;     // len = 12 + 60 * N
        }


        #region 结构体 与 byte数组 互转
        /// 结构体转byte数组
        /// </summary>
        /// <param name="structObj">要转换的结构体</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] StructToBytes<T>(T structObj) where T : struct
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);

            //创建byte数组
            byte[] bytes = new byte[size];

            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);

            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);

            //释放内存空间
            Marshal.FreeHGlobal(structPtr);

            //返回byte数组
            return bytes;
        }

        public static void StructToBytes<T>(T structObj, byte[] bytes, int index, int len) where T : struct
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, index, len);
            Marshal.FreeHGlobal(structPtr);
        }

        /// <summary>
        /// byte数组转结构体
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <param name="type">结构体类型</param>
        /// <returns>转换后的结构体</returns>
        public static T BytesToStruct<T>(byte[] bytes, int startIndex) where T : struct
        {
            //T stru = new T();

            //得到结构体的大小
            int size = Marshal.SizeOf(typeof(T));

            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {
                //返回空
                return default(T);
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, startIndex, structPtr, size);

            //将内存空间转换为目标结构体
            //Marshal.PtrToStructure(structPtr, stru);
            object obj = Marshal.PtrToStructure(structPtr, typeof(T));

            //释放内存空间
            Marshal.FreeHGlobal(structPtr);

            //返回结构体
            //return stru;
            return (T)obj;
        }

        public static T BytesToStruct<T>(byte[] bytes, int index, int len)
        {
            //T stru = default;

            int size = Marshal.SizeOf(typeof(T));
            if (len > bytes.Length - index)
            {
                return default(T);
            }

            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, index, structPtr, len);
            object obj = Marshal.PtrToStructure(structPtr, typeof(T));
            Marshal.FreeHGlobal(structPtr);

            return (T)obj;
        }

        public static T StructClone<T>(T structObj)
        {
            int size = Marshal.SizeOf(typeof(T));

            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            object obj = Marshal.PtrToStructure(structPtr, typeof(T));
            Marshal.FreeHGlobal(structPtr);

            return (T)obj;
        }

        #endregion
    }

    #region 结构体定义

    // 字节数组转结构体：struct_xxx obj = ( struct_xxx )Marshal.PtrToStructure(intPtr, typeof(struct_xxx));

    // 用户数据
    class UserData
    {
        public string user;         // 用户名
        public string pswd;         // 密码
        public string userBind;     // 绑定的用户名
        public string pswdBind;     // 绑定的密码
        public string createTime;   // 创建时间

        public int remain;             
        public int profit;
        public int remainBind;
        public int profitBind;
        public int desk;
        public bool isLogin;

        public byte ctrl;           // 控制状态：0 - 未控制，1 - 已控制
        public byte logRlt;         // 登录结果：0 - 成功，1 - 卡号无效或锁定，2 - 密码不正确，3 - 群组锁定或账号不存在，7 - 限制登录2分钟
        public int ip;
        public byte[] mac;
        public string loginTime;


        public byte autoOperate;    // 自动操作标志 0 - 未设置，1 - 输，2 - 赢，3 - 和
        public byte videoRegFlag;   // 视频注册标志 0 - 未注册，1 - 已注册
        public int commuKey;        // 通信密钥 - 登录响应的最后4字节 （服务端生成）
        public int loginKey;        // 登录密钥 - 登录请求的最后4字节的反码 （即最后4字节与0xFFFFFFF异或）

        public float BjlXian;
        public float BjlHe;
        public float BjlZhuang;
        public float BjlXianDui;
        public float BjlZhuangDui;
        public float LhHu;
        public float LhHe;
        public float LhLong;


        public BetRecordState[] betStates;
        public BetRecordState[] betStatesGw;
        public List<OpenResultInfo> openResultList;
        public List<OpenResultInfo> openResultListGw;
        public List<stBetRecord> betRecordList;
        public List<stBetRecord> betRecordListGw;

        public UserData()
        {
            createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            mac = new byte[6];
            betStates = new BetRecordState[6];
            betStatesGw = new BetRecordState[6];
            for (int i = 0; i < 6; i++)
            {
                betStates[i] = new BetRecordState() { desk = i + 1 };
                betStatesGw[i] = new BetRecordState() { desk = i + 1 };
            }
            openResultList = new List<OpenResultInfo>();
            openResultListGw = new List<OpenResultInfo>();
            betRecordList = new List<stBetRecord>();
            betRecordListGw = new List<stBetRecord>();
        }
    }

    // 当前 1~6 号桌状态
    class BetRecordState
    {
        public string time;     // 投注时间
        public int desk;        // 桌号 1~6
        public int round;       // 靴次 (第几轮)
        public int roundTime;   // 口次 (第几局)
        public int xianHu;      // 闲/虎
        public int he;          // 和
        public int zhuangLong;  // 庄/龙
        public int xianDui;     // 闲对
        public int zhuangDui;   // 庄对
        public int openRst;     // 开奖结果
        public int profitTotal; // 总输赢


        public string date;         // 投注日期
        public byte openRstGw;      // 开答结果(官网)：0 - 未开， 1~15 - 结果
        public byte openRstOpr;     // 开答结果(操作)： 0 - 未开， 1~15 - 结果
        public byte operateFlag;    // 操作标志： 0 - 未设置，1 - 输，2 - 赢，3 - 和
        public byte resumeFlag;     // 恢复标志： 0 - 不恢复， 1 - 切台后恢复当前桌路子

        public int betTotal;        // 总注额
        public int winTotal;        // 总赢额

        public int roundOld;        // 之前的轮次
        public int roundTimeOld;    // 之前的局数

        public int state;      // 状态 1/2 - 开始/停止
        public stBetInfo betInfo;
    }

    // 当前 1~6 号桌状态
    class OpenResultInfo
    {
        public int desk;        // 桌号 1~6
        public int round;       // 靴次 (第几轮)
        public int roundTime;   // 口次 (第几局)
        public byte openRst;    // 开奖结果
        public byte openRstLd;  // 开奖结果-路单
        public byte openRstGw;  // 开奖结果-官网
        public string time;     // 投注时间
        public string date;     // 投注时间
    }

    // 开奖结果
    public class Result
    {
        public const int NotOpen = 0;
        public const int Xian = 1;
        public const int XianXd = 2;
        public const int XianZd = 3;
        public const int XianXdZd = 4;
        public const int ZxHe = 5;
        public const int ZxHeXd = 6;
        public const int ZxHeZd = 7;
        public const int ZxHeXdZd = 8;
        public const int Zhuang = 9;
        public const int ZhuangXd = 10;
        public const int ZhuangZd = 11;
        public const int ZhuangXdZd = 12;
        public const int Hu = 13;
        public const int LhHe = 14;
        public const int Long = 15;
    }

    // 0x222A   登录 请求
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stLoginReq
    {
        public int cmd;
        public int len;        // 48
        public int type;       // 登录类型：0/1/2 - 电信/网通/其他
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string user;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string pswd;
        public int dwFixed;    // 固定 0x 3F80 0000
    }

    // 0x222A   登录 响应
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stLoginRsp
    {
        public int cmd;
        public int len;        // 16
        public int dwFixed1;   // 固定 0
        public int dwFixed2;   // 固定 0
    }

    // 0x222B   登录 响应2（平台响应）
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stLoginRsp2
    {
        public int cmd;
        public int len;        // 48
        public int commuKey;   // 通信密钥
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string user;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string pswd;
        public char ctrl;       // 控制状态：0 - 未控制，1 - 已控制
        public char logRlt;     // 登录结果：0 - 成功，1 - 卡号无效或锁定，2 - 密码不正确，3 - 限制登录2分钟
        public short wFixed;    // 固定 0
    }

    // 0x222C   设备信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stDevInfo
    {
        public int ip;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] mac;
    }

    // 0xAB01   视频注册 请求
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stVideoRegReq
    {
        public int cmd;
        public int len;        // 24
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string user;
    }

    // 0xAB01   视频注册 响应
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stVideoRegRsp
    {
        public int cmd;
        public int len;         // 12
        public int result;      // 注册结果：0 - 成功，1 - 失败
    }

    // 0xAB02   视频上传：   命令字4、长度4、桌号4、场次4、局数4、视频数据N

    // 0xAB03   视频下发：   命令字4、长度4、视频数据N


    // 0x8000 注单查询 请求
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stBetQryReq
    {
        public int cmd;
        public int len;         // 16
        public int lastDays;    // 前几天 
        public int pageNo;      // 页序号 
    }

    // 0x8000 注单查询 响应
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stBetQryRsp
    {
        public int cmd;
        public int len;         // 12 + 60 x N
        public int pageNo;      // 页序号 
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        //public stBetRecord[] records;   // 注单记录：每页 0~20 条
    }

    // 注单记录 每条60字节
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stBetRecord
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string time;    // 投注时间 ascii码，yyyy-MM-dd HH:mm:ss
        public int desk;       // 桌号 1~6
        public int round;      // 靴次 (第几轮)
        public int roundTime;  // 口次 (第几局)
        public int xianHu;     // 闲/虎
        public int he;         // 和
        public int zhuangLong; // 庄/龙
        public int xianDui;    // 闲对
        public int zhuangDui;  // 庄对
        public int openRst;    // 开奖结果
        public int profit;     // 输赢
    }

    // 0x2712   玩家数据 通知
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stPlayerData
    {
        public int cmd;
        public int len;        // 16
        public int remain;     // 余额
        public int profit;     // 输赢
    }

    // 0x2715 切换台桌 请求/响应
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stChangeDesk
    {
        public int cmd;
        public int len;        // 16
        public int deskLast;   // 上一桌号 0~6  ：0 - 大厅
        public int deskCurr;   // 当前桌号 0~6  ：0 - 大厅
    }

    // 00x2714   台桌设置
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stDeskSetNtf
    {
        public int cmd;
        public int len;        // 28
        public int desk;       // 桌号 1~6
        public int dw1;        // 未知：0x23 (35) - 百家乐， 0x1E (30)- 龙虎
        public int dw2;        // 未知：0x1E (30) - 百家乐， 0x19 (25)- 龙虎
        public int dwFixed1;   // 固定 5
        public int dwFixed2;   // 固定 5
    }

    // 0x2713   筹码设置
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stCmSetNtf
    {
        public int cmd;
        public int len;        // 40
        public int desk;       // 桌号 1~6
        public int cmType;     // 筹码类型：0x0B60 - 庄闲，0x02D8 - 龙虎
        public int min;        // 庄闲/龙虎 最小/最大
        public int max;
        public int heMin;      // 和 最小/最大
        public int heMax;
        public int dzMin;      // 对子 最小/最大
        public int dzMax;
    }

    // 0x2719   游戏状态
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stGameState
    {
        public int cmd;
        public int len;        // 20 + N
        public int desk;       // 桌号 1~6
        public int round;      // 靴次 (第几轮)
        public int roundTime;  // 口次 (第几局)
        public int state;      // 状态 1/2 - 开始/停止
        public int dwFixd1;    // 固定 0
        public int time;       // 时间
        public int dwFixed2;   // 固定 0x08DA86E3
    }

    // 0x271A   开奖通知
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stOpenRstNtf
    {
        public int cmd;
        public int len;
        public int desk;       // 桌号 1~6
        public int openRst;    // 开奖结果
        public int remain;     // 余额
        public int profit;     // 输赢
        public int win;        // 赢额
        public int round;      // 靴次 (第几轮)
        public int roundTime;  // 口次 (第几局)
    }

    // 0x2720   游戏赔率
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stGameOddsNtf
    {
        public int cmd;
        public int len;
        public int desk;       // 桌号 1~6
        public float xianHu;        // 闲/虎
        public float he;            // 和
        public float zhuangLong;    // 庄/龙
        public float xianDui;       // 闲对
        public float zhuangDui;     // 庄对 
    }

    // 0x271C   投注信息 请求/响应/通知
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stBetInfo
    {
        public int cmd;
        public int len;        // 36
        public int desk;       // 桌号 1~6
        public int seqNo;      // 当前桌投注序号
        public int xianHu;     // 闲/虎
        public int he;         // 和
        public int zhuangLong; // 庄/龙
        public int xianDui;    // 闲对
        public int zhuangDui;  // 庄对
    }
    // 0x271D 撤销投注 请求/响应
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stBetCancel
    {
        public int cmd;
        public int len;         // 16
        public int desk;        // 当前桌号 1~6 
        public int seqNo;       // 当前桌投注序号 
    }
    // 0x2726   投注总额 通知
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stBetTotalNtf
    {
        public int cmd;
        public int len;         // 36
        public int desk;        // 桌号 1~6
        public int xianHu;      // 闲/虎
        public int he;          // 和
        public int zhuangLong;  // 庄/龙
        public int xianDui;     // 闲对
        public int zhuangDui;   // 庄对
    }

    // 0x2730   台桌路单 请求 
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stOpenListReq
    {
        public int cmd;
        public int len;         // 12
        public int desk;        // 桌号 1~6
    }

    // 0x2731 / 0x2718  台桌路单 响应/通知
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct stOpenListRsp
    {
        public int cmd;
        public int len;         // 20 + N
        public int desk;        // 桌号 1~6
        public int round;       // 靴次 (第几轮)
        public int roundTime;   // 口次 (第几局)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
        public byte[] list;     // 每一局结果占一字节： 1 ~ 15
    }



    #endregion
}
