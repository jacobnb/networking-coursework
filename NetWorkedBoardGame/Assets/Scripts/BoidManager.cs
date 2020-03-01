using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    Boid boid;
    NetworkManager nm;
    // Start is called before the first frame update
    void Start()
    {
        nm = NetworkManager.Instance;
        boid = GetComponent<Boid>();
        boid.initBoidObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if(nm.mode == NetworkManager.NetworkMode.DATA_PUSH)
        {
            if (nm.isServer)
            {
                boid.updateBoids(Time.deltaTime);
                nm.sendBoids(ref boid.boids);
            }
            else
            {
                nm.readBoids(ref boid.boids);
                boid.setPosition();
            }
        }

    }
}
