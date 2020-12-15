using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;

namespace Dweiss
{
    public class PingLocalNetwork : MonoBehaviour
    {
        private const int IpCountToPing = 256;

        public int ttlInMiliSec = 3000;

        public class IntWrapper { public int data; }// to be used as reference


        public string GetIp()
        {
            return System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
        public static System.Net.IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties() != null ? n.GetIPProperties().GatewayAddresses : null)
        .Select(g => g != null? g.Address : null)
        .Where(a => a != null)
         // .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
         // .Where(a => Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
        .FirstOrDefault();
}

        [ContextMenu("Try ping all")]
        private void TestPing()
        {
            FindListOfActiveIps((success) => {
                Debug.Log("Done all: " + string.Join("\n", success.ToArray()));
            });
        }

        [ContextMenu("Try ping first")]
        private void TestPingFirst()
        {
            FindFirstActiveIps((success) => {
                Debug.Log("Done first: " + string.Join("\n", success.ToArray()));
            });
        }

        public void FindListOfActiveIps(System.Action<List<string>> pingSuccessList) { 
            var ip = GetIp();// NetworkManager.singleton.networkAddress;
           // Debug.Log("myip " + ip);
            var prefixIp = ip.Substring(0, ip.LastIndexOf(".") + 1);
            var suffix = int.Parse(ip.Substring(ip.LastIndexOf(".") + 1));
            var gateway = GetDefaultGateway().ToString();
            Debug.Log("gateway " + gateway);
            var gatewaySuffix = int.Parse(gateway.Substring(gateway.LastIndexOf(".") + 1));
            StartCoroutine(PingAllIps(prefixIp, suffix, gatewaySuffix, pingSuccessList, false));
        }

        public void FindFirstActiveIps(System.Action<List<string>> pingSuccessList)
        {
            var ip = GetIp();// NetworkManager.singleton.networkAddress;
                             // Debug.Log("myip " + ip);
            var prefixIp = ip.Substring(0, ip.LastIndexOf(".") + 1);
            var suffix = int.Parse(ip.Substring(ip.LastIndexOf(".") + 1));
            var gateway = GetDefaultGateway().ToString();
            Debug.Log("gateway " + gateway);
            var gatewaySuffix = int.Parse(gateway.Substring(gateway.LastIndexOf(".") + 1));
            StartCoroutine(PingAllIps(prefixIp, suffix, gatewaySuffix, pingSuccessList, true));
        }


        private IEnumerator PingAllIps(string prefixIp, int suffix, int gatewaySuffix, System.Action<List<string>> pingSuccessList, 
            bool returnFirst) {
            
            bool[] status = new bool[IpCountToPing];
            IntWrapper checkCount = new IntWrapper();
            IntWrapper successId = new IntWrapper(); successId.data = -1;
            for (int i = 0; i < IpCountToPing; i++)
            {
                if (i != suffix)
                    PingThis(prefixIp + i, i, status, checkCount, successId);
            }
            //Wait for ping to finish
            while ( checkCount.data < status.Length- 1 && 
                (returnFirst == false || (successId.data == -1 || successId.data == gatewaySuffix)))
            {
                yield return 0;
            }

            //Save only success list
            List<string> success = new List<string>();
            for (int i = 0; i < status.Length; i++)
            {
                if(i != suffix && i != gatewaySuffix)//Dont return my ip
                    if (status[i]) success.Add(prefixIp + i);
            }
            pingSuccessList.Invoke(success);
        }

        private void PingThis(string ip, int index, bool[] status, IntWrapper checkCount, IntWrapper successId)
        {
           // Debug.Log("check ip  " + ip);
           
            System.Threading.ThreadPool.QueueUserWorkItem((a) =>
            {
                var ping = new System.Net.NetworkInformation.Ping();
              
                var input = ping.Send(ip, ttlInMiliSec);
                status[index] = input.Status == IPStatus.Success ? true : false;// no need for look

                lock (status)
                {
                    checkCount.data++;
                }
                if (input.Status == IPStatus.Success)
                {
                    successId.data = index;
                    Debug.LogFormat("<color=blue>{0} : {1} ({2})</color>", ip, input.Status, input.RoundtripTime);
                }
            });
            //yield return 0;
            
        }
    }
}