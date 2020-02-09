using UnityEngine;
using System.Runtime.InteropServices;
public class Network
{
    [DllImport("networking")]
    public static extern int initNetwork(int numNetworkInstances);
    //[DllImport("networking")]
    //public static extern int getNetworkInstance();

    //[DllImport("networking", CharSet=CharSet.Auto)]
    //public static extern int initClient(int ID, string IP, int port, string username);
    [DllImport("networking")]
    public static extern int testInt(int num);
    //[DllImport("networking", CharSet = CharSet.Auto)]
    // public static extern int testString(string str);
    //[DllImport("networking", CharSet = CharSet.Auto)]
    //public static extern string testString2(string str);
    //[DllImport("networking", CharSet = CharSet.Auto)]
    //public static extern string testString3();
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]
}