using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Dweiss;
using System.Linq;

namespace Dweiss
{
    public class MonoServer : MonoBehaviour
    {
        private SimpleHttpListener listener;
        public int port = 1212;
        private byte[] buffer = new byte[0];

        public enum HttpMethodEnum
        {
            Get, Post, Put
        }

        [System.Serializable]
        public class SimpleHttpEvents : UnityEngine.Events.UnityEvent<HttpMethodEnum, string, string> { }

        [Tooltip("Event with http method, path, data ")]
        public SimpleHttpEvents onHttpEvent;

        void Start()
        {
            listener = new SimpleHttpListener(port, "", OnRequest);
            listener.StartListening();
        }

        private HttpMethodEnum GetMethod(string httpStr)
        {
            switch (httpStr)
            {
                case "GET": return HttpMethodEnum.Get;
                case "PUT": return HttpMethodEnum.Put;
                case "POST": return HttpMethodEnum.Post;
                default: throw new System.ArgumentException("HttpMethod not supported " + httpStr);
            }
        }

        private void OnRequest(HttpListenerContext context)
        {
            if (buffer.Length < context.Request.ContentLength64)
            {
                buffer = new byte[context.Request.ContentLength64];
            }
            var req = context.Request;
            var length = req.InputStream.Read(buffer, 0, (int)req.ContentLength64);
            //byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(s_unicode);
            var data = "";
            if (length == 0)
            {
                data = req.Url.Query.Substring(req.Url.Query.IndexOf("?") + 1);
            }
            else
            {
                data = System.Text.Encoding.UTF8.GetString(buffer, 0, length);
            }
            //Debug.LogFormat("Server: {0} >>> ({1}) [{2}] {3} | Len {4} | {5}",
            //    req.Url.AbsoluteUri
            //    , req.Url.Query
            //    , data
            //    , req.Url.AbsolutePath
            //    , length
            //    , req.HttpMethod);
            OnRequest(req.HttpMethod, req.Url.AbsolutePath, data);
        }

        private void OnRequest(string method, string url, string data) { 
            onHttpEvent.Invoke(GetMethod(method),url, data);
        }


        private void OnDestroy()
        {
            listener.Stop();
        }
    }
}