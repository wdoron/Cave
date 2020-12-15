using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using UnityEditor;
using System.Net.NetworkInformation;
using System.Linq;

#if UNITY_EDITOR
using System.Net;
using System.Net.Sockets;
using System.Globalization;
#endif
using System.Threading;

public class RemoteLogWIndow : EditorWindow
{

    private static UdpRecieverClass udpReciever;
    private static List<string> siteIds = new List<string>();
    public int messageCount = 5;
    public int serverPort = 55555;// new System.Random().Next(1000, short.MaxValue);
    public string msgFormat = "{0}:{1} {2} >> {3}";


    // Add menu named "My Window" to the Window menu
    [MenuItem("Dweiss/Remote Log WIndow")]
    static void Init()
    {

        // Get existing open window or if none, make a new one:
        var window = (RemoteLogWIndow)EditorWindow.GetWindow(typeof(RemoteLogWIndow));
        window.Show();
        setupIds();

       
        window.InitWindow();
    }

    private void InitWindow()
    {
        //udpReciever = GameObject.FindObjectOfType<UdpRecMon>();
        CloseServer();
#if UNITY_5
        EditorApplication.playmodeStateChanged += EditorApplication_playModeStateChangedOld;

#else
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
#endif
    }
#if UNITY_5
    private void EditorApplication_playModeStateChangedOld()
    {
        CloseServer();
    }
#else
    private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
    {

        CloseServer();
    }
#endif
    void OnDestroy()
    {
        CloseServer();
    }
    private void CloseServer() {
        if (udpReciever != null)
        {
            Debug.Log("Stop listenning to log");
            //udpReciever.Stop();
            //DestroyImmediate(udpReciever.gameObject);
            udpReciever.OnDestroy();
            udpReciever = null;
        }
    }

    private static void setupIds()
    {
        var connections = System.Enum.GetValues(typeof(NetworkInterfaceType)).
                    Cast<NetworkInterfaceType>().Where(a => string.IsNullOrEmpty(GetLocalIPv4(a)) == false).ToList();
        siteIds = connections.Select(a => GetLocalIPv4(a)).ToList();
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

    private void Start()
    {
        Debug.Log("Start listenning to log");

        CloseServer();

        //var go = new GameObject("RemoteLogInfo");
       // udpReciever = go.AddComponent<UdpRecMon>();
        udpReciever = new UdpRecieverClass();
        udpReciever.port = serverPort;

        udpReciever.onData += OnMessage;
        //_start = DateTime.Now;
        udpReciever.Start();
    }

    public bool netInfo = true;
    public string timeFormat = "yy.MM.dd HH:mm:ss";

    private void OnMessage(string arg0, string arg1, byte[] arg2)
    {

        var str = netInfo ? string.Format(msgFormat, arg0, arg1, System.DateTime.Now.ToString(timeFormat), Encoding.UTF8.GetString(arg2))
            : string.Format("{0} - {1} ",System.DateTime.Now.ToString(timeFormat), Encoding.UTF8.GetString(arg2));

        Debug.Log(str);
    }


    void OnGUI()
    {
        GUILayout.Label("Log settings", EditorStyles.boldLabel);
        serverPort = EditorGUILayout.IntField("Port ", serverPort);
        netInfo = EditorGUILayout.Toggle("Show Net Info ", netInfo);
        timeFormat = EditorGUILayout.TextField("Time format ", timeFormat);

        GUILayout.Label("This pc ids:\n" + string.Join("\n", siteIds.ToArray()));

        EditorGUI.BeginDisabledGroup(udpReciever != null && udpReciever.IsListening);
        if (GUILayout.Button("Start"))
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                Debug.LogError("RemoteLogWIndow: " + e);
            }
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(udpReciever == null || !udpReciever.IsListening);
        if (GUILayout.Button("Stop"))
        {
            try
            {
                CloseServer();
            }
            catch (Exception e)
            {
                Debug.LogError("RemoteLogWIndow: " + e);
            }
        }
        EditorGUI.EndDisabledGroup();   

    }
}


//[ExecuteInEditMode]
//public class UdpRecMon: MonoBehaviour
//{
//    private UdpRecieverClass udpRecv;
//    public int port;

//    public bool IsListening
//    {
//        get { return udpRecv != null && udpRecv.IsListening; }
//    }

//    public System.Action<string, string, byte[]> onData;

//    private void Awake()
//    {
//        udpRecv = new UdpRecieverClass();
//    }
//    private void Start()
//    {
//        udpRecv.port = port;

//        udpRecv.onData += onData;

//        udpRecv.Start();
//    }
//    private void OnDestroy()
//    {
//        Stop();
//    }
//    public void Stop()
//    {
//        if(udpRecv != null)
//            udpRecv.OnDestroy();
//    }
//}
public class UdpRecieverClass
{
    //public bool debug;
    private bool active = true;
    public int port;

    public System.Action<string, string, byte[]> onData;

    ~UdpRecieverClass() { OnDestroy(); }

    // infos
    private byte[] input;
    //private string lastReceivedUDPPacket = "";
    private string infoData;

#if UNITY_EDITOR
    // receiving Thread
    private Thread receiveThread;
    private UdpClient client;

    private bool _listening;
    public bool IsListening
    {
        get { return _listening; }
    }

    public void Start()
    {
        init();
    }

    private void init()
    {

        _listening = true;
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        //receiveThread.IsBackground = true;
        
        receiveThread.Start();

    }

    public void OnDestroy()
    {
        EndClient();
    }

    private void EndClient()
    {
        _listening = false;
        active = false;
        if (receiveThread != null) { receiveThread.Abort(); receiveThread = null; }
        if (client != null) { client.Close(); client = null; }
       // Debug.Log("EndRecieveClient");

    }
    // receive thread

    // Handles IPv4 and IPv6 notation.
    private static IPEndPoint CreateIPEndPoint(string endPoint)
    {
        string[] ep = endPoint.Split(':');
        if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
        IPAddress ip;
        if (ep.Length > 2)
        {
            if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
        }
        else
        {
            if (!IPAddress.TryParse(ep[0], out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
        }
        int port;
        if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
        {
            throw new FormatException("Invalid port");
        }
        return new IPEndPoint(ip, port);
    }

    private void ReceiveData()
    {
        //remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient(port);
        //client = new UdpClient(port);
        //IPEndPoint ip = CreateIPEndPoint(string.Format("{0}:{1}",IP,  port));
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
        while (active)
        {

            try
            {
                // Bytes empfangen.
                //if (debug) infoData = DateTime.Now + " Start " + ip;

                input = client.Receive(ref ip);

                //if (debug)
                //{
                //    Debug.Log(">>iP: " + ip.Address);
                //    string text = Encoding.UTF8.GetString(input);
                //    Debug.Log(">> " + text);
                //    lastReceivedUDPPacket = text;
                //    infoData = DateTime.Now + " End";
                //}

                if (onData != null) onData(ip.Address.ToString(), ip.Port.ToString(), input);
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
        client.Close();
    }

#else
    void Start(){}
    void OnDestroy(){}
#endif
}
