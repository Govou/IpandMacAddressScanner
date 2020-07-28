using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using WebApplication24.Models;

namespace WebApplication24.Helper
{
    public static class GetIps
    {

        public static string GetMyIpAddress()
        {
     
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;

        }

        public static async Task<List<string>> GetNetworkIPandMAC(int minRange, int maxRange, string subnet)
        {
            var Ips = new List<string>();

            Ping p = new Ping();

            if (!string.IsNullOrEmpty(subnet))
            {

                var hostSubIp = GetIPSubnetWithClassC();

                for (int i = minRange; i <= maxRange; i++)
                {
                    PingReply rep = p.Send($"{hostSubIp}.{subnet}.{i}");
                    if (rep.Status == IPStatus.Success)
                    {
                        //host is active
                        Ips.Add("IP Address " + rep.Address.ToString() + "  -   MAC Address " + MACHelper.GetMacAddress(rep.Address.ToString()));
                    }
                }
            }

            else
            {

                var hostSubIp = GetIPSubnet();

                for (int i = minRange; i <= maxRange; i++)
                {
                    PingReply rep = p.Send($"{hostSubIp}.{i}");
                    if (rep.Status == IPStatus.Success)
                    {
                        //host is active
                        Ips.Add("IP Address = " + rep.Address.ToString() + "  -   MAC Address = " + MACHelper.GetMacAddress(rep.Address.ToString()));
                    }
                }
            }

           
            return Ips;
        }

        public async static Task<List<string>> GetAllIpsandMAC(string subnet = null)
        {


            List<string> Ips = new List<string>();

            var task1 =  Task.Run(() => GetNetworkIPandMAC(0, 50, subnet));
            var task2 =  Task.Run(() => GetNetworkIPandMAC(51, 101, subnet));
            var task3 =  Task.Run(() => GetNetworkIPandMAC(102, 153, subnet));
            var task4 =  Task.Run(() => GetNetworkIPandMAC(154, 205, subnet));
            var task5 =  Task.Run(() => GetNetworkIPandMAC(206, 255, subnet));


            var results = await Task.WhenAll(task1, task2, task3, task4, task5);

            return results.SelectMany(result => result).ToList();


        }

        private static string GetIPSubnet()
        {
            var ip = GetMyIpAddress();
            var ipSubnet = string.Empty;
            var ipArr = ip.Split('.');
            ipSubnet = ipArr[0];
            for (int i = 1; i < 3; i++)
            {
                ipSubnet += "." + ipArr[i];   
            }

            return ipSubnet;
        }

        private static string GetIPSubnetWithClassC()
        {
            var ip = GetMyIpAddress();
            var ipSubnet = string.Empty;
            var ipArr = ip.Split('.');
            ipSubnet = ipArr[0];
            for (int i = 1; i < 2; i++)
            {
                ipSubnet += "." + ipArr[i];
            }

            return ipSubnet;
        }
    }
}