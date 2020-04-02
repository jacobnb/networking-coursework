﻿using UnityEngine;

public class CoupledBoidManager : MonoBehaviour
{
    bool shouldUpdate = true;
    Boid boids;
    Boid otherBoids;
    public GameObject foreignBoidFab;
    NetworkManager nm;
    ulong gameTime;
    // Start is called before the first frame update
    void Start()
    {
        nm = NetworkManager.Instance;
        boids = GetComponent<Boid>();
        otherBoids = gameObject.AddComponent<Boid>();
        otherBoids.boidFab = foreignBoidFab;
        if (!nm.isServer)
        {
            boids.initBoidObjects();
            otherBoids.initBoidObjects();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldUpdate = !shouldUpdate;
        }
        
        gameTime = Network.getCurrentTime();
        if (nm.isServer)
        {
            //Debug.Log("Routing Only");
            // should just route messages to all other clients.
            if (Network.serverMessages() == 0)
            {
                Debug.Log("Failed to send messages");
            }
            return;
        }
        // TODO: to enable server to interact with boids, 
        // need to link GUID to Unity side. OR handle interaction plugin side.
        Network.readMessages();
        string message = "";
        if(shouldUpdate)
            message = nm.readMessage();
        while (message != "")
        {
            // see MessageParser.messageTypes
            if (message[0] == '0') //there's a boid position update
            {
                Debug.Log("Reading Other Boids");
                int length = int.Parse(message.Substring(1));
                if (length != otherBoids.boids.Length)
                {
                    //boid.resize(length);
                }
                otherBoids.time = nm.readBoids(ref otherBoids.boids);
            }
            else
            {
                if (message[0] == '1')
                {
                    // spawn boids message;
                    int boidsToSpawn = MessageParser.getNumSpawnBoids(message);
                    otherBoids.resize(boidsToSpawn);
                }
            }
            message = nm.readMessage();
        }
        if (!nm.isServer)
        {
            boids.updateBoids(gameTime, otherBoids);
            if (shouldUpdate)
                nm.sendBoids(ref boids.boids);
            otherBoids.updateBoids(gameTime, boids);
        }
    }
}