using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;

namespace WebApplication24.Helper
{
   
        public sealed class MACHelper
        {
            [DllImport("iphlpapi.dll", ExactSpelling = true)]
            private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

            private MACHelper() { }

            public static string GetMacAddress(string ipAddress)
            {
                if (string.IsNullOrEmpty(ipAddress))
                {
                    throw new ArgumentNullException("ipAddress");
                }

                IPAddress IP = IPAddress.Parse(ipAddress);
                byte[] macAddr = new byte[6];
                uint macAddrLen = (uint)macAddr.Length;

                if (SendARP((int)IP.Address, 0, macAddr, ref macAddrLen) != 0)
                {
                    throw new Exception("ARP command failed");
                }

                string[] str = new string[(int)macAddrLen];

                for (int i = 0; i < macAddrLen; i++)
                {
                    str[i] = macAddr[i].ToString("x2");
                }

                return string.Join(":", str).ToUpper();
            }
        }
    
}