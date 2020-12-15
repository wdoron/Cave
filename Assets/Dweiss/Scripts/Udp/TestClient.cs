//using UnityEngine;
//using System.Collections.Generic;
//using System;
//using System.IO;
//using System.Net.Sockets;

//using System.Linq;
//using System.Text;
//using System.Net;
//using System.Threading;

//public class TestClient : MonoBehaviour
//{
//    internal Boolean socketReady = false;
//    TcpClient mySocket;
//    NetworkStream theStream;
//    StreamWriter theWriter;
//    BinaryReader theReader;

//    public int floatCount = 3;
//    private int byteCount;
//    public bool active = true;
//    public const int SizeOfSingle = 4;
//    // receiving Thread
//    Thread receiveThread;

//    public TextMesh txtMsh;

//    public String Host = "localhost";
//    public Int32 Port = 5111;

//    public string inputFromServer, debugServer;

//    void Awake()
//    {
//        byteCount = floatCount * SizeOfSingle;
//    }
//    void Start()
//    {
//        receiveThread = new Thread(
//            new ThreadStart(setupSocket));
//        receiveThread.IsBackground = true;
//        receiveThread.Start();
        
//    }

//    private void Update()
//    {
//        txtMsh.text = string.Format("{0}\n{1}", inputFromServer, debugServer);
//    }

//    // **********************************************
//    public void setupSocket()
//    {
//        try
//        {
//            mySocket = new TcpClient(Host, Port);
//            theStream = mySocket.GetStream();
//            theWriter = new StreamWriter(theStream);
//            theReader = new BinaryReader(theStream);
//            socketReady = true;

//            while (active)
//            {
//                debugServer = "RS: " + DateTime.Now;
//                var data = readSocket();
//                debugServer = "D: " + DateTime.Now;
//                //var bytes = System.Text.Encoding.UTF8.GetBytes(myString);
//                //And to get the string back:
//                //System.Text.Encoding.UTF8.GetString(bytes);
//                if (data != null) {
//                    var floatArray = new List<float>();
//                    for (int i = 0; i < floatCount; ++i)
//                    {
//                        var fl = BitConverter.ToSingle(data, i * SizeOfSingle);
//                        floatArray.Add(fl);
//                    }

//                    inputFromServer = string.Join(",", floatArray.Select(a=>a.ToString()).ToArray());// System.Text.Encoding.UTF8.GetString(data);
//                }
//            }
//            closeSocket();
//        }
//        catch (Exception e)
//        {
//            Debug.Log("Socket error: " + e);
//        }
//    }
//    //public void writeSocket(string theLine)
//    //{
//    //    if (!socketReady)
//    //        return;
//    //    String foo = theLine + "\r\n";
//    //    theWriter.Write(foo);
//    //    theWriter.Flush();
//    //}
//    public byte[] readSocket()
//    {
//        if (!socketReady)
//            return null;
//        if (theStream.CanRead && theStream.DataAvailable) {
//            var buffer = new byte[byteCount];
//            theReader.Read(buffer, 0, byteCount);
//            return buffer;
//        }
//                //theReader.ReadLine();
//        return null;
//    }
//    private void OnDestroy()
//    {
        
//        active = false;
//        if (receiveThread != null) { receiveThread.Abort(); receiveThread = null; }
//        try
//        {
//            closeSocket();
//        }
//        catch (System.Exception) { }
//        //if (client != null) { client.Close(); client = null; }
//        Debug.Log("EndClient");

//    }
//    public void closeSocket()
//    {
//        if (!socketReady)
//            return;
//        theWriter.Close();
//        theReader.Close();
//        mySocket.Close();
//        socketReady = false;
//    }
//} // end class s_TCP