using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace Dweiss
{
    public class NetworkUtils : MonoBehaviour
    {
        public bool debug;

        #region Time Fields
        [Header("Time configuration")]
        private DateTime time;
        private bool pulledTime = false;
        public float comTimeout = 3;

        public UnityEngine.Events.UnityEvent onTimeReady, onIpReady;

        public static DateTime StartTime
        {
            get
            {
                if (inst != null && inst.pulledTime)
                {
                    return inst.time;
                }
                return DateTime.Now;
            }
        }

        public static DateTime StartTimeUtc { get { return StartTime.ToUniversalTime(); } }

        #endregion

        #region IP fields

        [Header("Network IP")]
        private string myNetIp;
        public static string MyNetIp
        {
            get
            {
                if (inst != null)
                {
                    return inst.myNetIp;
                }
                return "";
            }
        }


        #endregion


        #region Singleton and Start
        private static NetworkUtils inst;
        public static NetworkUtils Instance { get { return inst; } }
        void Awake()
        {
            if (inst != null)
                Debug.LogError("Duplicate singeltons for time");
            else
                inst = this;

            GetNetTime(a => { if (debug) Debug.Log("NetTime: " + a); });
            GetNetIp(a => { if (debug) Debug.Log("NetIp: " + a); });

        }
        private void OnDestroy()
        {
            if (inst == this) inst = null;
        }
        #endregion

        #region Email
        public static void SendEmail(string emailSender, string senderPass, string emailReciever, string subject, string body)
        {

            System.Threading.ThreadPool.QueueUserWorkItem((a) =>
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(emailSender);// "Fromaddress@gmail.com");
                    mail.To.Add(emailReciever);// "Toaddress@gmail.com");
                    mail.Subject = subject;// "Test Smtp Mail";
                    mail.Body = body;// "Testing SMTP mail from GMAIL";


                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                    smtpServer.Port = 587;
                    smtpServer.Credentials = new System.Net.NetworkCredential(emailSender, senderPass) as ICredentialsByHost;
                    smtpServer.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    smtpServer.Send(mail);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                    Debug.LogError("Error with sending email. If you use gmail please change your security here https://myaccount.google.com/lesssecureapps?pli=1");
                    throw;
                }
                Debug.Log("Success! sending email");
            });
        }
        #endregion

        #region NetworkTime 

        [ContextMenu("GetNetTime")]
        public void GetNetTime()
        {
            GetNetTime((a) => Debug.Log(a));
        }

        public void GetNetTime(Action<DateTime> ret)
        {
            if (pulledTime)
                ret.Invoke(time);
            else
            {
                StartCoroutine(this.WaitForFirst<DateTime?>(
                    (t) => {
                        time = t == null || t.Value == DateTime.MinValue ? DateTime.Now : t.Value;
                        pulledTime = true;
                        ret(time);
                        onTimeReady.Invoke();
                    },
                    comTimeout,
                    () => new DateTime?(GetNetTime1()), () => new DateTime?(GetNetTime2()), () => new DateTime?(GetGoogleTime())));
            }
        }

        private DateTime GetNetTime1()
        {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
            var response = myHttpWebRequest.GetResponse();
            string todaysDates = response.Headers["date"];
            var ret = DateTime.ParseExact(todaysDates,
                                       "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                       CultureInfo.InvariantCulture.DateTimeFormat,
                                       DateTimeStyles.AssumeUniversal);

            //if (debug) Debug.Log("GetNetTime1 " + ret);

            return ret;
        }

        private DateTime GetNetTime2()
        {
            try
            {
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {

                    var response = streamReader.ReadToEnd();

                    var utcDateTimeString = response.Substring(7, 17);
                    var ret = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                    //Debug.Log(ret.ToString("yy-MM-dd HH:mm:ss zzz"));

                    //if (debug) Debug.Log("GetNetTime2 " + ret);
                    return ret;
                }
            }
            catch (System.Exception)
            {
                return DateTime.MinValue;

            }
        }

        private DateTime GetGoogleTime()
        {
            try
            {
                using (var response =
                  WebRequest.Create("http://www.google.com").GetResponse())
                {

                    var ret = DateTime.ParseExact(response.Headers["date"],
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AssumeUniversal);
                    //if (debug) Debug.Log("GetGoogleTime " + ret);
                    return ret;
                }
            }
            catch (WebException)
            {
                return DateTime.MinValue; //In case something goes wrong. 
            }
        }
        #endregion

        #region RemoteIp

        public void GetNetIp(Action<string> ret = null)
        {
            if (string.IsNullOrEmpty(myNetIp) == false)
                if(ret != null)ret.Invoke(myNetIp);
            else
            {
                //StartCoroutine(WaitForAnyTime((t) => { ret(t); onFinished.Invoke(); }));
                StartCoroutine(this.WaitForFirst<string>(
                    (v) => { myNetIp = v; if (ret != null) ret.Invoke(v); onIpReady.Invoke(); },
                    comTimeout, GetIp1, GetIp2));
            }
        }

        private readonly static string[] UrlList = new string[] { "https://api.ipify.org", "http://icanhazip.com" };
        private static string GetIp1()
        {
            var url = UrlList[new System.Random().Next(0, UrlList.Length)];
            string externalip = new WebClient().DownloadString(url);
            return externalip;
        }
        private static string GetIp2()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }
        #endregion

        #region Local IP

        public static List<string> GetLocalMac()
        {
            List<string> ret = new List<string>();

            var netInterfaces = System.Enum.GetValues(typeof(NetworkInterfaceType)).Cast<NetworkInterfaceType>().Where(a => string.IsNullOrEmpty(NetworkUtils.GetLocalIPv4(a)) == false);
            foreach (var netType in netInterfaces)
            {
                foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.NetworkInterfaceType == netType && item.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ret.Add(item.GetPhysicalAddress().ToString());
                            }
                        }
                    }
                }
            }
            return ret;
        }


        public static NetworkInterfaceType[] GetLocalIps()
        {
            var myNetwork = System.Enum.GetValues(typeof(NetworkInterfaceType)).Cast<NetworkInterfaceType>().Where(a => string.IsNullOrEmpty(NetworkUtils.GetLocalIPv4(a)) == false).ToArray();
            return myNetwork;
        }
        public static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        public static string[] GetHttpGETParams(System.Collections.Specialized.NameValueCollection q)
        {
            var ret = new string[q.Count];
            for (int i = 0; i < q.Count; ++i)
            {
                ret[i] = q.GetKey(i);
            }
            return ret;
        }


        #endregion




    }

    public static class ExtendMono
    {
        public static Coroutine CoroutineWaitForFirst<T>(this MonoBehaviour that, Action<T> ret, float timeout, params Func<T>[] calls)
        {
            return that.StartCoroutine(that.WaitForFirst(ret, timeout, calls));
        }
        public static IEnumerator WaitForFirst<T>(this MonoBehaviour that, Action<T> ret, float timeout, params Func<T>[] calls)
        {
            float startTime = Time.realtimeSinceStartup;

            List<T> results = new List<T>();
            int callCount = 0;
            for (int i = 0; i < calls.Length; i++)
            {
                that.StartCoroutine(that.WaitFor(calls[i], (res) => { ++callCount; if (res != null) results.Add(res); }));
            }

            yield return new WaitWhile(() => results.Count == 0 && callCount != calls.Length && startTime + timeout > Time.realtimeSinceStartup);
            //Debug.LogFormat("{3} - sucess:{0} returned:{1} timeout?{2}", results.Count, callCount, Time.time - startTime, typeof(T).Name);
            ret.Invoke(results.Count == 0 ? default(T) : results[0]);

        }
        public static IEnumerator WaitFor<T>(this MonoBehaviour that, Func<T> onRemoteAction, Action<T> onFinished)
        {
            T obj = default(T);
            bool finished = false;
            System.Threading.ThreadPool.QueueUserWorkItem((a) =>
            {
                obj = onRemoteAction.Invoke();
                finished = true;
            });
            yield return new WaitUntil(() => finished);
            onFinished.Invoke(obj);
        }
    }
}