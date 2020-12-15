using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Threading;


namespace Dweiss
{
    public class SimpleHttpListener
    {
        public int sleepInMili = 200;
        private HttpListener listener;
        private Action<HttpListenerContext> onRequest;
        public SimpleHttpListener(int port, string suffix, Action<HttpListenerContext> onRequest)
        {
            this.onRequest = onRequest;
            listener = new HttpListener();
            var formatedUrls = GetAllSitesUrl(new List<int>() { port });
            //listener.Prefixes.Add("http://localhost:" + port + "/"+ suffix);
            System.String str = "";
            foreach (var url in formatedUrls)
            {
                var fullUrl = string.Format("{0}{1}", url, suffix);
                str += fullUrl + "\n";
                //Debug.Log("Listening " + fullUrl);
                listener.Prefixes.Add(fullUrl);
            }
            Debug.Log("Listening " + str);
        }

        public void Stop()
        {
            listener.Stop();
        }
        public void StartListening()
        {
            if (!listener.IsListening)
            {
                Debug.Log("Start listenning");
                listener.Start();
            }
            listener.BeginGetContext(ListenerCallback, listener);
        }

        public void ListenerCallback(IAsyncResult result)
        {
            //Debug.Log("ListenerCallback");

            HttpListener listener = (HttpListener)result.AsyncState;

            // Call EndGetContext to complete the asynchronous operation.
            HttpListenerContext context = listener.EndGetContext(result);
            // HttpListenerRequest request = context.Request;
            //do your response-handling logic here
            Dweiss.MainThread.Instance.RunInMainThread(() =>
           {
               onRequest(context);
           });
            Thread.Sleep(sleepInMili);
            listener.BeginGetContext(ListenerCallback, listener);
            // listener.EndGetContext(ListenerCallback);


            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            System.IO.Stream output = response.OutputStream;
            //output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
        }

        private List<string> GetAllSitesNames()
        {
            List<string> siteNames = new List<string>();

            try
            {
                var connections = System.Enum.GetValues(typeof(NetworkInterfaceType)).
                    Cast<NetworkInterfaceType>().Where(a => string.IsNullOrEmpty(NetworkUtils.GetLocalIPv4(a)) == false).ToList();
                //				Debug.Log("GetAllSitesNames " +  string.Join("\n", connections.Select( a => a.ToString() + " : " + NetworkUtils.GetLocalIPv4(a)).ToArray()));
                siteNames = connections.Select(a => NetworkUtils.GetLocalIPv4(a)).ToList();
            }
            catch (System.Exception) { }

            try
            {
                siteNames.Add(Dns.GetHostName());
            }
            catch (System.Exception) { }

            siteNames.Add("localhost");
            siteNames.Add("127.0.0.1");

            return siteNames;
        }
        private void AddSites(List<string> siteNames, int port, HashSet<string> sites)
        {
            var siteFormat = string.Format("http://{0}:{1}/", "{0}", port);

            foreach (var sName in siteNames)
            {
                sites.Add(string.Format(siteFormat, sName));
            }
            sites.Add(string.Format("http://+:{0}/", port));
        }

        private List<string> GetAllSitesUrl(List<int> ports)
        {
            var siteNames = GetAllSitesNames();

            var sites = new HashSet<string>();

            for (int i = 0; i < ports.Count; ++i)
            {
                if (ports[i] > 0)
                {
                    AddSites(siteNames, ports[i], sites);
                }
                if (ports[i] == 80) sites.Add("http://+/");
            }
            return sites.ToList();
        }
    }
}
