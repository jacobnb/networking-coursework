using UnityEngine;

public class SharingBoidManager : MonoBehaviour
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

            //    boids.initBoidObjects();
            //    otherBoids.initBoidObjects();
        }
        else
        {

            boids.initBoidObjects();
            otherBoids.initBoidObjects();

        }
    }

    // Update is called once per frame
    void Update()
    {
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

        Network.readMessages();
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
        if (!nm.isServer)
        {
            // TODO: replace w/ timestamp
            boids.updateBoids(Time.deltaTime);
            nm.sendBoids(ref boids.boids);
            otherBoids.setPosition();
        }
    }
}
