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
    [DllImport("networking", CharSet=CharSet.Ansi)]
    public static extern int sendMessage(string message, int ID = 0);
    [DllImport("networking", CharSet=CharSet.Ansi)]
    public static extern int readMessage(StringBuilder message, int bufferSize, int ID = 0);
    [DllImport("networking")]
    public static extern int checkConnection(int ID = 0);
    [DllImport("networking")]
    public static extern int doEverything(bool isServer);
}