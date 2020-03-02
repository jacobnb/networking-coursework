using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    Boid boid;
    Boid otherBoids;
    //public GameObject foreignBoidFab;
    NetworkManager nm;
    // Start is called before the first frame update
    void Start()
    {
        nm = NetworkManager.Instance;
        boid = GetComponent<Boid>();
        //otherBoids = gameObject.AddComponent<Boid>();
        //otherBoids.boidFab = foreignBoidFab;
        boid.initBoidObjects();
        //if (nm.isServer)
        //{
        //    // server shouldn't render hack.
        //    //foreach (GameObject go in boid.gameObjects)
        //    //{
        //    //    go.SetActive(false);
        //    //}
        //    if (nm.mode == NetworkManager.NetworkMode.DATA_PUSH)
        //    {
               
        //    }
        //}
        //else
        //{
        //    if (nm.mode == NetworkManager.NetworkMode.DATA_PUSH)
        //    {
                
        //       // nm.sendMessage(MessageParser.spawnBoidsMessage(boid.NUM_BOIDS));
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        string message = nm.readMessage();
        while(message != "")
        {
            Debug.Log(message);
            // see MessageParser.messageTypes
            if(message[0] == '0') //there's a boid position update
            {
                int length = int.Parse(message.Substring(1));
                if(length != boid.boids.Length)
                {
                    boid.resize(length);
                }
                nm.readBoids(ref boid.boids);
            }
            else if (message[0] == '1')
            {
                // spawn boids message;
                int boidsToSpawn = MessageParser.getNumSpawnBoids(message);
                otherBoids.resize(boidsToSpawn);
            }
            message = nm.readMessage();
        }
        if(nm.mode == NetworkManager.NetworkMode.DATA_PUSH)
        {
            if (nm.isServer)
            {
                boid.updateBoids(Time.deltaTime);
                nm.sendBoids(ref boid.boids);
            }
            else
            {
                boid.setPosition();
            }
        }
        else if(nm.mode == NetworkManager.NetworkMode.DATA_SHARING)
        {

        }
        else if(nm.mode == NetworkManager.NetworkMode.DATA_COUPLED)
        {

        }


    }
}
