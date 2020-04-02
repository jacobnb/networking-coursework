using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    // TODO: Not updated to use current time. see coupled boid manager
    Boid boids;
    NetworkManager nm;
    // Start is called before the first frame update
    void Start()
    {
        nm = NetworkManager.Instance;
        boids = GetComponent<Boid>();
        boids.initBoidObjects();

        if (nm.isServer)
        {
            // server shouldn't render hack.
            //foreach (GameObject go in boid.gameObjects)
            //{
            //    go.SetActive(false);
            //}
        }

    }

    // Update is called once per frame
    void Update()
    {

        Network.readMessages();
        string message = nm.readMessage();
        while (message != "")
        {
            // see MessageParser.messageTypes
            if (message[0] == '0') //there's a boid position update
            {
                int length = int.Parse(message.Substring(1));
                if (length != boids.boids.Length)
                {
                    //boid.resize(length);
                }
                nm.readBoids(ref boids.boids);
            }
            else if (message[0] == '1')
            {
                // spawn boids message;
                int boidsToSpawn = MessageParser.getNumSpawnBoids(message);
                boids.resize(boidsToSpawn);
            }
            message = nm.readMessage();
        }
        if (nm.isServer)
        {
            boids.updateBoids(Network.getCurrentTime());
            nm.sendBoids(ref boids.boids);
        }
        else
        {
            boids.setPosition();
        }
    }
}
