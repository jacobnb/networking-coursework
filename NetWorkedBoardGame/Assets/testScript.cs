using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(initNetwork(5));
        int idx = getNetworkInstance();
        Debug.Log(initClient(idx, "0.0.0.0", 90000, "howdy"));
    }

    [DllImport("networking")]
    public static extern int initNetwork(int numNetworkInstances);
    [DllImport("networking")]
    public static extern int getNetworkInstance();

    [DllImport("networking")]
    public static extern int initClient(int ID, string IP, int port, string username);
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]
    //[DllImport("networking")]

}
