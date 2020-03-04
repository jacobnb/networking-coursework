using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    Boid boids;
    Boid otherBoids;
    public GameObject foreignBoidFab;
    NetworkManager nm;
    // Start is called before the first frame update
    void Start()
    {
        nm = NetworkManager.Instance;
        boids = GetComponent<Boid>();
        otherBoids = gameObject.AddComponent<Boid>();
        otherBoids.boidFab = foreignBoidFab;
        if (nm.isServer)
        {
            // server shouldn't render hack.
            //foreach (GameObject go in boid.gameObjects)
            //{
            //    go.SetActive(false);
            //}
            if (nm.mode == NetworkManager.NetworkMode.DATA_PUSH)
            {
                boids.initBoidObjects();

            }
            else if (nm.mode == NetworkManager.NetworkMode.DATA_SHARING)
            {
            //    boids.initBoidObjects();
            //    otherBoids.initBoidObjects();
            }
        }
        else
        {
            if (nm.mode == NetworkManager.NetworkMode.DATA_PUSH)
            {
                otherBoids.initBoidObjects();
                //nm.sendMessage(MessageParser.spawnBoidsMessage(boid.getNumBoids()));
            }
            else if (nm.mode == NetworkManager.NetworkMode.DATA_SHARING)
            {
                boids.initBoidObjects();
                otherBoids.initBoidObjects();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nm.mode == NetworkManager.NetworkMode.DATA_SHARING && nm.isServer)
        {
            //Debug.Log("Routing Only");
            // should just route messages to all other clients.
            if (Network.serverMessages() == 0)
            {
                Debug.Log("Failed to send messages");
            }
            return;
        }
        else
        {


            string message = nm.readMessage();
            while (message != "")
            {
                Debug.Log("Reading Messages");
                Debug.Log(message);
                // see MessageParser.messageTypes
                if (message[0] == '0') //there's a boid position update
                {
                    Debug.Log("Reading Other Boids");
                    int length = int.Parse(message.Substring(1));
                    if (length != otherBoids.boids.Length)
                    {
                        //boid.resize(length);
                    }
                    nm.readBoids(ref otherBoids.boids);
                }
                else if (message[0] == '1')
                {
                    // spawn boids message;
                    int boidsToSpawn = MessageParser.getNumSpawnBoids(message);
                    otherBoids.resize(boidsToSpawn);
                }
                message = nm.readMessage();
            }
            if (nm.mode == NetworkManager.NetworkMode.DATA_PUSH)
            {
                if (nm.isServer)
                {
                    // TODO: replace w/ timestamp
                    boids.updateBoids(Time.deltaTime);
                    nm.sendBoids(ref boids.boids);
                }
                else
                {
                    otherBoids.setPosition();
                }
            }
            else if (nm.mode == NetworkManager.NetworkMode.DATA_SHARING)
            {
                if (!nm.isServer)
                {
                    // TODO: replace w/ timestamp
                    boids.updateBoids(Time.deltaTime);
                    nm.sendBoids(ref boids.boids);
                    otherBoids.setPosition();
                }
            }
            else if (nm.mode == NetworkManager.NetworkMode.DATA_COUPLED)
            {
                boids.updateBoids(Time.deltaTime, otherBoids);
                nm.sendBoids(ref boids.boids);
                otherBoids.setPosition();
            }
        }
    }
}
