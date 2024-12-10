using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Security
{
    public class SegCrypt
    {
        [DllImport(@"C:\Windows\SysWOW64\SegCryptCentinela64.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EncryptDecrypt([MarshalAs(UnmanagedType.Bool)] bool fEncrypt, string lpszInBuffer, StringBuilder sOut, [MarshalAs(UnmanagedType.I4)] ref int dsize);
        
        public static string EncryptDecrypt(bool fEncrypt, string text)
        {
            string res = string.Empty;
            if (string.IsNullOrWhiteSpace(text))
                return res;
            try
            {
                int nSize = text.Length * 2 + 1;
                bool bRet;
                StringBuilder outString = new StringBuilder(nSize);
                bRet = EncryptDecrypt(fEncrypt, text, outString, ref nSize);
                res = outString.ToString();
            }

            catch (Exception ex)
            {
                throw new Exception("Error en EncryptDecrypt, " + ex.Message);
            }
            return res;
        }
    }
}
