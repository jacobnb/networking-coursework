using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int teamNum;
    
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
            //message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.UP);
            int num = Network.sendDirectionEvent((int)RocketLeague.ForceDirection.UP);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.DOWN);
            int num = Network.sendDirectionEvent((int)RocketLeague.ForceDirection.DOWN);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            int num = Network.sendDirectionEvent((int)RocketLeague.ForceDirection.LEFT);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int num = Network.sendDirectionEvent((int)RocketLeague.ForceDirection.RIGHT);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            int num = Network.sendDirectionEvent((int)RocketLeague.ForceDirection.VERT_UP);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.VERT_DOWN);
            int num = Network.sendDirectionEvent((int)RocketLeague.ForceDirection.VERT_DOWN);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //message = MessageParser.encodeColorMessage(new Vector3(1f, 0f, 0f));
            int num = Network.sendColorEvent(1, 0, 0);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            //message = MessageParser.encodeColorMessage(new Vector3(0f, 1f, 0f));
            int num = Network.sendColorEvent(0, 1, 0);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // message = MessageParser.encodeColorMessage(new Vector3(0f, 1f, 0f));
            int num = Network.sendColorEvent(0, 0, 1);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //message = MessageParser.encodeForceChangeMessage(1);
            int num = Network.sendSpeedEvent(1.0f);
            Debug.Log("Send Report: " + num);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //  message = MessageParser.encodeForceChangeMessage(-1);
            int num = Network.sendSpeedEvent(-1.0f);
            Debug.Log("Send Report: " + num);
        }
        if (message != "")
        {
            Debug.Log(message);
        }
    }
}
