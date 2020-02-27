using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class NetworkManager : MonoBehaviour
{
    public enum NetworkMode
    {
        DATA_PUSH,
        DATA_SHARING,
        DATA_COUPLED
    }
    private static NetworkManager instance;
    public static NetworkManager Instance
    {
        get{ return instance; }
    }
    public int networkInstances = 5;
    public bool isServer = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        Network.initNetwork(networkInstances);
        DontDestroyOnLoad(this.gameObject);
    }
    public void initServer(int port, string username)
    {
        isServer = true;
        Network.getNetworkInstance();
        Network.initServer(port, username);
    }
    public void initServer(int port, string username, NetworkMode netMode)
    {
        isServer = true;
        Network.getNetworkInstance();
        Network.initServer(port, username);
    }
    public void initClient(string IP, int port, string username)
    {
        isServer = false;
        Network.getNetworkInstance();
        Network.initClient(IP, port, username);
    }
    public string readMessage()
    {
        Debug.Log("Reading Message");
        StringBuilder sb = new StringBuilder(256);
        if(Network.readMessage(sb, sb.Capacity)>0)
            return sb.ToString();
        return "";
    }
    public void sendMessage(string message)
    {
        Network.sendMessage(message);
        Debug.Log("Seding Message");
    }
}
