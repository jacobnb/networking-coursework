﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    public static NetworkManager Instance
    {
        get{ return instance; }
    }
    public int networkInstances = 5;

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
        Network.getNetworkInstance();
        Network.initServer(port, username);
    }
    public void initClient(string IP, int port, string username)
    {
        Network.getNetworkInstance();
        Network.initClient(IP, port, username);
    }
}
