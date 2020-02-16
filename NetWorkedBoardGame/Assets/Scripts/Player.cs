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
            Debug.Log("Moving Up");
            //message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.UP);
            Network.sendDirectionEvent((int)RocketLeague.ForceDirection.UP);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.DOWN);
            Network.sendDirectionEvent((int)RocketLeague.ForceDirection.DOWN);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Network.sendDirectionEvent((int)RocketLeague.ForceDirection.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Network.sendDirectionEvent((int)RocketLeague.ForceDirection.RIGHT);  
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Network.sendDirectionEvent((int)RocketLeague.ForceDirection.VERT_UP);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
           // message = MessageParser.encodeForceMessage(RocketLeague.ForceDirection.VERT_DOWN);
            Network.sendDirectionEvent((int)RocketLeague.ForceDirection.VERT_DOWN);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //message = MessageParser.encodeColorMessage(new Vector3(1f, 0f, 0f));
            Network.sendColorEvent(1, 0, 0);
        }
            if (Input.GetKeyDown(KeyCode.G))
        {
            //message = MessageParser.encodeColorMessage(new Vector3(0f, 1f, 0f));
            Network.sendColorEvent(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // message = MessageParser.encodeColorMessage(new Vector3(0f, 1f, 0f));
            Network.sendColorEvent(0, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //message = MessageParser.encodeForceChangeMessage(1);
            Network.sendSpeedEvent(1.0f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //  message = MessageParser.encodeForceChangeMessage(-1);
            Network.sendSpeedEvent(-1.0f);
        }
        if (message != "")
        {
            Debug.Log(message);
        }
    }
}
