using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace SysMatWPF
{
    class RemoteScope
    {
        //You'll need this pinvoke signature as it is not part of the .Net framework
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        public static bool ConnectWMIScope(string RemoteServer)
        {
            bool retval = true;

            try
            {                           
                string ConnectionString = string.Format("\\\\{0}\\root\\cimv2", RemoteServer);
                ManagementScope scope = new ManagementScope(ConnectionString);
                scope.Connect();
            }
            catch
            {
                retval = false;
                return retval;
            }

            return retval;
        }

        public static bool ConnectWMIScope(string RemoteServer, string UserName, String Password)
        {
            bool retval = true;
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                options.EnablePrivileges = true;
                options.Impersonation = ImpersonationLevel.Impersonate;
                options.Authentication = AuthenticationLevel.Packet;
                options.Username = UserName;
                options.Password = Password;
                options.Timeout = TimeSpan.FromSeconds(2);

                string ConnectionString = string.Format("\\\\{0}\\root\\cimv2", RemoteServer);
                ManagementScope scope = new ManagementScope(ConnectionString, options);
                scope.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                retval = false;
                return retval;               
            }
            
            return retval;
        }

        public static bool Valid_Ping(string RemoteServer)
        {
            bool PingReplied = false;
            try
            {
                Ping PingSender = new Ping();
                PingOptions Options = new PingOptions();
                //  Use default TTL of 128
                //  Change to not fragment
                Options.DontFragment = true;
                string PingData = "******Computer*****Details******";
                byte[] Pingbuffer = Encoding.ASCII.GetBytes(PingData);
                int PingTimeout = 70;
                PingReply PingReply = PingSender.Send(RemoteServer, PingTimeout, Pingbuffer);
                if ((PingReply.Status == IPStatus.Success))
                {
                    PingReplied = true;
                }
                else
                {
                    PingReplied = false;
                }
                return PingReplied;
            }
            catch (Exception)
            {
                return PingReplied;
            }
        }

        public static string GetMacAddress(string pcName)
        {
            string macAddress = string.Empty;

            if (!IsHostAccessible(pcName)) return null;
            try
            {
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\sysnative";
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                Process process = new Process();
                processStartInfo.FileName = filePath + "\\nbtstat.exe";
                processStartInfo.RedirectStandardInput = false;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.Arguments = "-a " + pcName;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                process = Process.Start(processStartInfo);

                int Counter = -1;

                while (Counter <= -1)
                {
                    Counter = macAddress.Trim().ToLower().IndexOf("mac address", 0);
                    if (Counter > -1)
                    {
                        break;
                    }
                    macAddress = process.StandardOutput.ReadLine();
                }
                process.WaitForExit();
                macAddress = macAddress.Trim();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed because:" + e.ToString());
            }

            return macAddress;
        }

        private static bool IsHostAccessible(string hostNameOrAddress)
        {
            PingReply reply = null;
            try
            {
                Ping ping = new Ping();
                reply = ping.Send(hostNameOrAddress, 100);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return reply.Status == IPStatus.TimedOut;
            }
            
        } 
    }
}
