using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int teamNum;
    private void Start()
    {
        if (NetworkManager.Instance.isServer)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    private void sendMessage(string message)
    {
        Network.sendMessage(message);
    }

    private void getInput()
    {
        string message="";
        if (Input.GetKeyDown(KeyCode.W))
        {
            message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.UP);
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.DOWN);
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.LEFT);
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.RIGHT);
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.VERT_UP);
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.VERT_DOWN);
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            message = MessageParser.encodeColorMessage(new Vector3(1f, 0f, 0f));
            sendMessage(message);
        }
            if (Input.GetKeyDown(KeyCode.G))
        {
            message = MessageParser.encodeColorMessage(new Vector3(0f, 1f, 0f));
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            message = MessageParser.encodeColorMessage(new Vector3(0f, 0f, 1f));
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            message = MessageParser.encodeForceChangeMessage(1);
            sendMessage(message);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            message = MessageParser.encodeForceChangeMessage(-1);
            sendMessage(message);
        }
        if (message != "")
        {
            Debug.Log(message);
        }
    }
}
