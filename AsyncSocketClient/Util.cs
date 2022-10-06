using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace ElectricPowerLib.Common
{
    /// <summary>
    /// 常用工具类：bytes/strHex转换，Bcd/Dec转换，Crc/累加和计算，bit位统计，byte数组中查找，
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Hex字符串 -> byte数组
        /// </summary>
        /// <param name="strSource">源字符串</param>
        /// <param name="DataByte">目的byte数组</param>
        /// <param name="iStart">写入byte数组的位置</param>
        /// <param name="bReverse">是否倒置</param>
        /// <returns>byte数组的长度</returns>
		public static int GetBytesFromStringHex(string strSource, byte[] DataByte, int iStart, bool bReverse = false)
        {
            byte tmp;
            int iLoop, iPos = iStart;

            strSource = strSource.Trim().Replace(" ", "").Replace(",", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
            try
            {
                if (strSource.Length % 2 != 0) throw new Exception();

                for (iLoop = 0; iLoop < strSource.Length; )
                {
                    DataByte[iPos++] = Convert.ToByte(strSource.Substring(iLoop, 2), 16);
                    iLoop += 2;
                }
                if (true == bReverse)
                {
                    for (iLoop = 0; iLoop < (iPos - iStart) / 2; iLoop++)
                    {
                        tmp = DataByte[iLoop + iStart];
                        DataByte[iLoop + iStart] = DataByte[iPos - 1 - iLoop];
                        DataByte[iPos - 1 - iLoop] = tmp;
                    }
                }
            }
            catch(Exception)
            {
                return 0;
            }

            return (iPos - iStart);
        }

        /// <summary>
        /// Hex字符串 -> byte数组
        /// </summary>
        /// <param name="strSource">源字符串</param>
        /// <param name="strSeparate">源字符串中每byte间的分隔符，如 "" 或 " " 或 ","</param>
        /// <param name="bReverse">是否倒置</param>
        /// <returns>byte数组</returns>
        public static byte[] GetBytesFromStringHex(string strSource, string strSeparate = "", bool bReverse = false)
        {
            byte tmp;
            int iLoop;
            byte[] bytes;

            if (strSeparate == "")
            {
                strSource = strSource.Trim();
            }
            else
            {
                strSource = strSource.Trim().Replace(strSeparate, "");
            }
            bytes = new byte[strSource.Length / 2];

            try
            {
                if (strSource.Length % 2 != 0) throw new Exception();

                for (iLoop = 0; iLoop < bytes.Length; iLoop++)
                {
                    bytes[iLoop] = Convert.ToByte(strSource.Substring(iLoop * 2, 2), 16);
                }
                if (true == bReverse)
                {
                    for (iLoop = 0; iLoop < bytes.Length; iLoop++)
                    {
                        tmp = bytes[iLoop];
                        bytes[iLoop] = bytes[bytes.Length - 1 - iLoop];
                        bytes[bytes.Length - 1 - iLoop] = tmp;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return bytes;
        }

        /// <summary>
        /// byte数组 -> Hex字符串
        /// </summary>
        /// <param name="DataByte">源byte数组</param>
        /// <param name="iStart">开始索引</param>
        /// <param name="iLength">长度</param>
        /// <param name="strSeparate">保存字符串时每byte间插入的分隔符，如 "" 或 " " 或 ","</param>
        /// <param name="Reverse">是否倒置</param>
        /// <returns>Hex字符串</returns>
        public static string GetStringHexFromBytes(byte[] DataByte, int iStart, int iLength, string strSeparate = "", bool Reverse = false)
        {
            string strResult = "";
            
            if(DataByte == null || iStart + iLength > DataByte.Length || iStart < 0)
            {
                return strResult;
            }

            for (int iLoop = 0; iLoop < iLength; iLoop++)
            {
                if (Reverse == true)
                {
                    strResult += DataByte[iStart + iLength - 1 - iLoop].ToString("X2") + strSeparate;
                }
                else
                {
                    strResult += DataByte[iStart + iLoop].ToString("X2") + strSeparate;
                }
            }
            strResult.Trim();

            return strResult;
        }

        /// <summary>
        /// BCD码数字 转换为 十进制数字
        /// </summary>
        /// <param name="bcd"></param>
        /// <returns></returns>
        public static byte BcdToDec(byte bcd)
        {
            return (byte)(bcd - (bcd >> 4) * 6);
        }

        /// <summary>
        /// 十进制数字 转换为 BCD码数字
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static byte DecToBcd(byte dec)
        {
            return (byte)(dec + (dec / 10) * 6);
        }

        /// <summary>
        /// byte数组中查找 byte值
        /// </summary>
        /// <param name="srcArray">源数组</param>
        /// <param name="b">目的byte值</param>
        /// <param name="startIndex">源数组开始查找的索引</param>
        /// <param name="offset">查找的长度</param>
        /// <returns>若找到返回第一次出现的位置，否则返回 -1</returns>
        public static int IndexOf(byte[] srcArray, byte b, int startIndex, int offset)
        {
            return IndexOf(srcArray, new byte[] { b }, startIndex, offset);
        }

        /// <summary>
        /// byte数组中查找 字符串
        /// </summary>
        /// <param name="srcArray">源数组</param>
        /// <param name="str">目的字符串</param>
        /// <param name="startIndex">源数组开始查找的索引</param>
        /// <param name="offset">查找的长度</param>
        /// <returns>若找到返回第一次出现的位置，否则返回 -1</returns>
        public static int IndexOf(byte[] srcArray, string str, int startIndex, int offset)
        {
            byte[] bytes = (str != "" ? Encoding.UTF8.GetBytes(str) : new byte[] { 0x00 });
            return IndexOf(srcArray, bytes, startIndex, offset);
        }

        /// <summary>
        /// byte数组中查找 byte数组
        /// </summary>
        /// <param name="srcArray">源数组</param>
        /// <param name="bytes">目的数组</param>
        /// <param name="startIndex">源数组开始查找的索引</param>
        /// <param name="offset">查找的长度</param>
        /// <returns>若找到返回第一次出现的位置，否则返回 -1</returns>
        public static int IndexOf(byte[] srcArray, byte[] bytes, int startIndex, int offset)
        {
            int index = -1, i, j;

            if (bytes == null || bytes.Length == 0) return index;

            if(offset > (srcArray.Length - startIndex))
            {
                offset = (srcArray.Length - startIndex);
            }

            for (i = startIndex; i <= (startIndex + offset - bytes.Length); i++)
            {
                if (srcArray[i] == bytes[0])
                {
                    for (j = 0; j < bytes.Length; j++)
                    {
                        if (srcArray[i + j] != bytes[j])
                        {
                            break;
                        }
                    }

                    if (j == bytes.Length)
                    {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }

        /// <summary>
        /// 获取 0或1 bit位标志数量
        /// </summary>
        /// <param name="bitFlags">bit位数据缓存</param>
        /// <param name="index">起始索引</param>
        /// <param name="maxCnt">最大bit位数量</param>
        /// <param name="bitFlag">0 - 统计bit '0'， 1 - 统计bit '1'</param>
        /// <returns>bit '0' 或 bit '1'的数量</returns>
        public static int GetBitFlagCount(byte[] bitFlags, int index, int maxCnt, byte bitFlag = 0)
        {
            byte aByte;
            int bitIdx = 0, cnt = 0;

            for (int i = index; i < bitFlags.Length; i++)
            {
                aByte = bitFlags[i];

                if (aByte == 0xFF && bitIdx <= maxCnt - 8)
                {
                    bitIdx += 8;
                    cnt += (bitFlag == 1 ? 8 : 0);
                }
                else if (aByte == 0x00 && bitIdx <= maxCnt - 8)
                {
                    bitIdx += 8;
                    cnt += (bitFlag == 0 ? 8 : 0);
                }
                else
                {
                    for (int j = 0; j < 8 && bitIdx < maxCnt; j++)
                    {
                        if ((aByte & 0x01) == bitFlag)
                        {
                            cnt++;
                        }
                        aByte >>= 1;
                        bitIdx++;
                    }
                }

                if (bitIdx >= maxCnt)
                {
                    cnt = (cnt > maxCnt ? maxCnt : cnt);
                    break;
                }
            }

            return cnt;
        }

        /// <summary>
        /// 获取CRC8值，6009协议
        /// </summary>
        /// <param name="buffer">bytes数据缓冲区</param>
        /// <param name="index">起始索引</param>
        /// <param name="length">计算的长度</param>
        /// <returns></returns>
        public static byte GetCRC8(byte[] buffer, int index, int length)
        {
            byte crc8 = 0;

            if (buffer.Length < index + length)
            {
                throw new Exception("beyound buffer length");
            }

            for (int iLoop = index; iLoop < (index + length); iLoop++)
            {
                crc8 ^= buffer[iLoop];
                for (int i = 0; i < 8; i++)
                {
                    if ((crc8 & 0x01) > 0)
                    {
                        crc8 >>= 1;
                        crc8 ^= 0x8C;
                    }
                    else
                    {
                        crc8 >>= 1;
                    }
                }
            }
            return crc8;
        }

        /// <summary>
        /// 获取CRC16值，国网通用算法
        /// </summary>
        /// <param name="str">bytes的Hex字符串</param>
        /// <returns>CRC16值</returns>
        public static UInt16 GetCRC16(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return GetCRC16(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 获取CRC16值，国网通用算法
        /// </summary>
        /// <param name="buffer">bytes数据缓冲区</param>
        /// <param name="index">起始索引</param>
        /// <param name="length">计算的长度</param>
        /// <param name="seed">处理因子，缺省值0x8408</param>
        /// <returns>CRC16值</returns>
        public static UInt16 GetCRC16(byte[] buffer, int index, int length, UInt16 seed = 0x8408)
        {
            UInt16 uiCRC = 0xffff;

            if (buffer.Length < index + length) throw new Exception("索引+长度 超出了buffer的大小");

            for (int iLoop = 0; iLoop < length; iLoop++)
            {
                uiCRC ^= (UInt16)buffer[index + iLoop];
                for (int iLoop1 = 0; iLoop1 < 8; iLoop1++)
                {
                    if ((uiCRC & 1) == 1)
                    {
                        uiCRC >>= 1;
                        uiCRC ^= seed;
                    }
                    else
                    {
                        uiCRC >>= 1;
                    }
                }
            }
            uiCRC ^= 0xffff;
            return uiCRC;
        }

        /// <summary>
        /// 获取8位累加和
        /// </summary>
        /// <param name="str">bytes的Hex字符串</param>
        /// <returns>8位累加和</returns>
        public static byte GetChecksum(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return GetChecksum(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 获取8位累加和
        /// </summary>
        /// <param name="buffer">bytes数据缓冲区</param>
        /// <param name="index">起始索引</param>
        /// <param name="length">计算的长度</param>
        /// <returns>8位累加和</returns>
        public static byte GetChecksum(byte[] buffer, int index, int length)
        {
            byte sum = 0;

            if (buffer.Length < index + length) throw new Exception("索引+长度 超出了buffer的大小");

            for (int i = 0; i < length; i++)
            {
                sum += buffer[i + index];
            }

            return sum;
        }

        /// <summary>
        /// 获取16位累加和
        /// </summary>
        /// <param name="str">bytes的Hex字符串</param>
        /// <returns>16位累加和</returns>
        public static ushort GetChecksum16(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return GetChecksum16(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 获取16位累加和
        /// </summary>
        /// <param name="buffer">bytes数据缓冲区</param>
        /// <param name="index">起始索引</param>
        /// <param name="length">计算的长度</param>
        /// <returns>16位累加和</returns>
        public static ushort GetChecksum16(byte[] buffer, int index, int length)
        {
            ushort sum = 0;

            if (buffer.Length < index + length) throw new Exception("索引+长度 超出了buffer的大小");

            for (int i = 0; i < length; i++)
            {
                sum += buffer[i + index];
            }

            return sum;
        }

#if true
        //将一个字节数组序列化为结构
        private IFormatter formatter = new BinaryFormatter();
        private ValueType deserializeByteArrayToInfoObj(byte[] bytes)
        {
            ValueType vt;
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            try
            {
                MemoryStream stream = new MemoryStream(bytes);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                vt = (ValueType)formatter.Deserialize(stream);
                stream.Close();
                return vt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        //将一个结构序列化为字节数组
        private byte[] serializeInfoObjToByteArray(ValueType infoStruct)
        {
            if (infoStruct == null)
            {
                return null;
            }

            try
            {
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, infoStruct);

                byte[] bytes = new byte[(int)stream.Length];
                stream.Position = 0;
                int count = stream.Read(bytes, 0, (int)stream.Length);
                stream.Close();
                return bytes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将字节数组转换为结构体
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="type">结构体类型</param>
        /// <returns></returns>
        public object BytesToStruct(byte[] bytes, Type type)
        {
            //得到结构体大小
            int size = Marshal.SizeOf(type);
            Math.Log(size, 1);

            if (size > bytes.Length)
                return null;
            //分配结构大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将BYTE数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内容空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
        

        /// <summary>
        /// 将结构转换为字节数组
        /// </summary>
        /// <param name="obj">结构体对象</param>
        /// <returns>字节数组</returns>
        public byte[] StructTOBytes(object obj)
        {
            int size = Marshal.SizeOf(obj);
            //创建byte数组
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷贝到分配好的内存空间
            Marshal.StructureToPtr(obj, structPtr, false);
            //从内存空间拷贝到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }
#endif

        /// <summary>
        /// 执行dos命令
        /// </summary>
        /// <param name="executablePath">批处理/exe文件路径</param>
        /// <param name="args">命令字符串/.bat文件/.cmd文件</param>
        /// <param name="workingFolder">命令执行的位置</param>
        /// <param name="ignoreErrorCode">忽略错误码</param>
        /// <param name="ignoreOutput">忽略输出</param>
        /// <returns></returns>
        public static string ExecuteWindowsCmd(string executablePath, string args, string workingFolder, bool ignoreErrorCode = true, bool ignoreOutput = true)
        {
            if (!Path.IsPathRooted(executablePath))
            {
                string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                executablePath = Path.Combine(executingDirectory, executablePath);
            }

            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(executablePath, args);
                process.StartInfo.WorkingDirectory = (workingFolder != null ? workingFolder : "");
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                StringBuilder stdOutput = new StringBuilder();
                StringBuilder stdError = new StringBuilder();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null && !ignoreOutput)
                        {
                            stdOutput.AppendLine(e.Data);
                        }
                        else
                        {
                            outputWaitHandle.Set();
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null && !ignoreOutput)
                        {
                            stdError.AppendLine(e.Data);
                        }
                        else
                        {
                            errorWaitHandle.Set();
                        }
                    };

                    string processOutput = string.Empty;
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (process.WaitForExit(10000)
                        && outputWaitHandle.WaitOne(10000)
                        && errorWaitHandle.WaitOne(10000))
                    {
                        // Process is completed. 
                        processOutput = stdOutput.ToString() + stdError.ToString();
                        if (!ignoreErrorCode && process.ExitCode != 0)
                        {
                            throw new Exception(string.Format("{0} {1}, ExitCode {2}, Args {3}.", executablePath, args, process.ExitCode, processOutput));
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("Process running is time out in {0} s ", 10));
                    }

                    return processOutput;

                }
            }
        }

    }
}
