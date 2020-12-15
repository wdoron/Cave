using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Dweiss
{
    public class SimpleWebClient : MonoBehaviour
    {
        public enum Method
        {
            Put,
            Post,Get
        }

        public Method method;

        private UTF8Encoding utf8 = new UTF8Encoding();
        public void SendCommand(string url, string data, int maxTry = 1)
        {
            StartCoroutine(SendCoroutine(url, data, maxTry));
        }

        IEnumerator SendCoroutine(string url, string data, int maxTry)
        {

            //byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
            int tryCount = 0;
            while (tryCount++ < maxTry)
            {
                UnityWebRequest www = null;
                //Debug.Log("Sending to " + url + " " + data);

                //byte[] encodedBytes = utf8.GetBytes(getData);
                //Debug.Log("Sending " + data);
                switch (method)
                {
                    case Method.Put:
                        www = UnityWebRequest.Put(url, data);
                        break;
                    case Method.Post:
                        www = UnityWebRequest.Post(url, data);
                        break;
                    case Method.Get:
                        www = UnityWebRequest.Get(url);
                        break;
                }
                yield return www.SendWebRequest();

                //WWW www = new WWW(url + "?" + data);//, encodedBytes);
                //yield return www;
                if (string.IsNullOrEmpty(www.error) == false)
                    Debug.LogError("Client " + url + " " + data + " Rec: " + www.error);
                else
                    break;

                yield return new WaitForSecondsRealtime(.01f);
            }
        }



#if UNITY_EDITOR
        //[Header("SimpleTest in editor only")]
        //public string urlTest = "http://127.0.0.1:1212";
        //public string dataTest = "Hello";
        //public KeyCode key;

        //void Update()
        //{
        //    if (Input.GetKeyDown(key))
        //    {
        //        Debug.Log("Debug user Sending: " + urlTest + " >> " + dataTest);

        //        StartCoroutine(SendCoroutine(urlTest, dataTest));
        //    }
        //}
#endif

    }
}