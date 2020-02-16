using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
public class Network
{
    [DllImport("networking")]
    public static extern int initNetwork(int numNetworkInstances);
    [DllImport("networking")]
    public static extern int getNetworkInstance();
    [DllImport("networking", CharSet = CharSet.Ansi)]
    public static extern int initClient(string IP, int port, string username, int ID = 0);
    [DllImport("networking")]
    public static extern int initServer(int port, string username, int maxClients = 10, int ID = 0);
    [DllImport("networking")]
    public static extern int cleanup(int ID = 0);

    //send gameplay message
    [DllImport("networking", CharSet=CharSet.Ansi)]
    public static extern int sendMessage(string message, int ID = 0);
    //@return 1 = message, 0 = no message, -1 = error
    [DllImport("networking", CharSet=CharSet.Ansi)]
    public static extern int readMessage(StringBuilder message, int bufferSize, int ID = 0);
    [DllImport("networking")]
    public static extern int checkConnection(int ID = 0);
    [DllImport("networking")]
    public static extern int doEverything(bool isServer);

    [DllImport("networking")]
    public static extern int sendColorEvent(float r, float g, float b, int ID = 0);

    [DllImport("networking")]
    public static extern int sendDirectionEvent(int direction, int ID = 0);

    [DllImport("networking")]
    public static extern int sendMessageEvent(string message, int ID = 0);

    [DllImport("networking")]
    public static extern int sendSpeedEvent(float speed, int ID = 0);

    [DllImport("networking")]
    public static extern int getEventList(int ID = 0);

    [DllImport("networking")]
    public static extern int executeEvent(StringBuilder message, int ID = 0);
}