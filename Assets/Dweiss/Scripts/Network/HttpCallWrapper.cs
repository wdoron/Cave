using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;
using CI.HttpClient;


namespace Dweiss.Network
{
    public class HttpCallWrapper
    {

        public struct HttpReturn
        {
            public System.Net.HttpStatusCode statusCode;
            public Exception exception;

            public string url;
            public string origin;

            public string data;
            

            public Dictionary<string, object> args;
            public Dictionary<string, object> headers;

            public Dictionary<string, object> json;
            public Dictionary<string, object> files;
            public Dictionary<string, object> form;

            public bool IsValid() {
                    return ((int)statusCode >= 200) && ((int)statusCode <= 299); 
            }

            public T GetData<T>()
            {
                return JsonConvert.DeserializeObject<T>(data);
            }

            public T GetOrigin<T>()
            {
                return JsonConvert.DeserializeObject<T>(origin);
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            //         {
            //  "args": {}, 
            //  "data": "{\"str\":\"\",\"integer\":0}", 
            //  "files": {}, 
            //  "form": {}, 
            //  "headers": {
            //    "Connection": "close", 
            //    "Content-Length": "22", 
            //    "Content-Type": "charset=utf-8", 
            //    "Expect": "100-continue", 
            //    "Host": "httpbin.org"
            //  }, 
            //  "json": {
            //    "integer": 0, 
            //    "str": ""
            //  }, 
            //  "origin": "79.182.153.100", 
            //  "url": "http://httpbin.org/post"
            //}
        }

        public delegate void OnCallBack(int id, string url, HttpReturn retVal);

        private int counter;

        private HttpClient httpClient = new HttpClient();

        private void OnReturnValue(OnCallBack callback, int id, string url, HttpResponseMessage<string> input)
        {
            HttpReturn ret = new HttpReturn();
            if (input.IsSuccessStatusCode == false)
            {
                ret = new HttpReturn() { statusCode = input.StatusCode, exception = input.Exception };
            }
            else
            {
                var str = "";
                try
                {
                    str = input.Data;
                    ret = JsonConvert.DeserializeObject<HttpReturn>(str);
                    ret.statusCode = System.Net.HttpStatusCode.OK;
                }
                catch (Exception)
                {
                    Debug.LogError(str);
                }
            }

            if (callback == null)
            {
                Debug.Log("return " + JsonConvert.SerializeObject(ret));
            }
            else
            {
                callback(id, url, ret);
            }
        }

        public int Get(OnCallBack callback, string url, object obj = null)
        {
            var id = counter++;

            HttpClient client = new HttpClient();
            var urlWithParas = obj == null ? url : url + obj.ToUrlParams();
            client.GetString(new Uri(urlWithParas), (r) =>
            {
                OnReturnValue(callback, id, url, r);
            });
            return id;
        }

        public int Post<T>(OnCallBack callback, string url, T obj)
        {
            //Debug.Log("Start " + Time.time);

            var id = counter++;

            var json = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(json);
            httpClient.Post(new Uri(url), new ByteArrayContent(data, "charset=utf-8"), (HttpResponseMessage<string> r) =>
            {
                OnReturnValue(callback, id, url, r);
            });
            return id;
        }

        public int Put(OnCallBack callback, string url, object input)
        {
            var id = counter++;
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(input));
            httpClient.Put(new Uri(url), new ByteArrayContent(data, "charset=utf-8"), (HttpResponseMessage<string> r) =>
            {
                OnReturnValue(callback, id, url, r);
            });
            return id;
        }
    }
}