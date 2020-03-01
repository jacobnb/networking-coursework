using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct data
{
    public Network.vec3 position;
    public Network.vec3 velocity;
}
public struct behavior
{
    public float radius;
    public float separation;
    public float alignment;
    public float cohesion;
    public float forward;
    public float maxForce;
    public behavior(float Radius, float Separation, float Alignment, float Cohesion, float Forward, float MaxForce)
    {
        radius = Radius;
        separation = Separation;
        alignment = Alignment;
        cohesion = Cohesion;
        forward = Forward;
        maxForce = MaxForce;
    }
}
public class Boid: MonoBehaviour
{
    // TODO: Make boids collide with each other.
    public GameObject boidFab;
    const int NUM_BOIDS = 20;
    const int MIN_Z = 5;
    const int MAX_Z = 20;
    data[] boids = new data[NUM_BOIDS];
    behavior[] behave = new behavior[NUM_BOIDS];
    GameObject[] gameObjects = new GameObject[NUM_BOIDS];
    private void Start()
    {
        initBoidObjects();
    }
    private void Update()
    {
        updateBoids(Time.deltaTime);
    }
    void initBoidObjects()
    {
        Network.vec3 position = new Network.vec3(0,0,0);
        position.x = -10;
        Network.vec3 velocity = new Network.vec3(2f, 2f, 2f);
        for(int i=0; i < NUM_BOIDS; i++)
        {
            gameObjects[i] = Instantiate(boidFab);
            boids[i].position = position;
            position.x++;
            boids[i].velocity = velocity;
            //behave[i] = new behavior(5f, 5f, 5f, 5f, 5f, 10f);
        }
    }
    void updateBoids(float dt)
    {
        screenWrap();
        //updateBoidVelocity();
        applyVelocityAndPosition(dt);
    }
    void screenWrap()
    {
        for(int i = 0; i<NUM_BOIDS; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(boids[i].position.toVector3());
            if(screenPos.x > Screen.width)
            {
                //screenPos.x = 0f;
                boids[i].velocity.x = -boids[i].velocity.x;
            }
            else if (screenPos.x < 0f) {
                //screenPos.x = Screen.width;
                boids[i].velocity.x = -boids[i].velocity.x;
            }
            if(screenPos.y > Screen.height)
            {
                //screenPos.y = 0f;
                boids[i].velocity.y = -boids[i].velocity.y;
            }
            else if (screenPos.y < 0f)
            {
                //screenPos.y = Screen.height;
                boids[i].velocity.y = -boids[i].velocity.y;
            }
            if(screenPos.z > MAX_Z)
            {
                //screenPos.z = MIN_Z;
                boids[i].velocity.z = -boids[i].velocity.z;
            }
            if(screenPos.z < MIN_Z)
            {
                //screenPos.z = MAX_Z;
                boids[i].velocity.z = -boids[i].velocity.z;
            }
            boids[i].position = new Network.vec3(Camera.main.ScreenToWorldPoint(screenPos));
        }
    }
    void applyVelocityAndPosition(float dt)
    {
        for(int i =0; i<NUM_BOIDS; i++)
        {
            boids[i].position += boids[i].velocity * dt;
            gameObjects[i].transform.position = boids[i].position.toVector3();
        }
    }
    //void updateBoidVelocity()
    //{
    //    data currentBoid;
    //    Vector3 newVelocity = Vector3.zero;
    //    for (int i = 0; i < NUM_BOIDS; i++)
    //    {
    //        currentBoid = boids[i];
    //        int count = 0;
    //        Vector3 sepForce = Vector3.zero;
    //        Vector3 alignment = Vector3.zero;
    //        Vector3 cohesion = Vector3.zero;
    //        // get all other boids data
    //        for (int c = 0; /*see below*/; c++)
    //        {

    //            if (i == c) //skip self
    //                c++;
    //            if (c >= NUM_BOIDS)
    //                break;
    //            if ((currentBoid.position - boids[c].position).sqrMagnitude
    //                < (behave[i].radius * behave[i].radius))
    //            {
    //                count++;
    //                sepForce += calcSeparationForce(currentBoid, boids[c], behave[i].separation);
    //                alignment += boids[c].velocity;
    //                cohesion += boids[c].position;
    //            }

    //        }
    //        sepForce /= count;
    //        alignment /= count;
    //        cohesion /= count;
    //        newVelocity = currentBoid.velocity.normalized * behave[i].forward
    //            + sepForce
    //            + calcAlignmentForce(alignment, behave[i].alignment)
    //            + calcCohesionForce(currentBoid, cohesion, behave[i].cohesion);
    //        if (newVelocity.sqrMagnitude > (behave[i].maxForce * behave[i].maxForce))
    //        {
    //            newVelocity = newVelocity.normalized * behave[i].maxForce;
    //        }
    //        currentBoid.velocity = newVelocity;
    //        Debug.Log(newVelocity);
    //    }
    //}
    ////individual
    //Vector3 calcSeparationForce(data self, data other, float separation)
    //{
    //    Vector3 diff = self.position - other.position; //from o to s
    //    float dist = diff.magnitude;
    //    return diff.normalized * separation / (dist * dist);
    //}

    ////group average
    //Vector3 calcAlignmentForce(Vector3 alignment, float alignmentMod)
    //{
    //    return alignment.normalized * alignmentMod;
    //}

    ////group average
    //Vector3 calcCohesionForce(data self, Vector3 cohesion, float cohesionMod)
    //{
    //    return (cohesion - self.position).normalized * cohesionMod;
    //}    //void updateBoidVelocity()
    //{
    //    data currentBoid;
    //    Vector3 newVelocity = Vector3.zero;
    //    for (int i = 0; i < NUM_BOIDS; i++)
    //    {
    //        currentBoid = boids[i];
    //        int count = 0;
    //        Vector3 sepForce = Vector3.zero;
    //        Vector3 alignment = Vector3.zero;
    //        Vector3 cohesion = Vector3.zero;
    //        // get all other boids data
    //        for (int c = 0; /*see below*/; c++)
    //        {

    //            if (i == c) //skip self
    //                c++;
    //            if (c >= NUM_BOIDS)
    //                break;
    //            if ((currentBoid.position - boids[c].position).sqrMagnitude
    //                < (behave[i].radius * behave[i].radius))
    //            {
    //                count++;
    //                sepForce += calcSeparationForce(currentBoid, boids[c], behave[i].separation);
    //                alignment += boids[c].velocity;
    //                cohesion += boids[c].position;
    //            }

    //        }
    //        sepForce /= count;
    //        alignment /= count;
    //        cohesion /= count;
    //        newVelocity = currentBoid.velocity.normalized * behave[i].forward
    //            + sepForce
    //            + calcAlignmentForce(alignment, behave[i].alignment)
    //            + calcCohesionForce(currentBoid, cohesion, behave[i].cohesion);
    //        if (newVelocity.sqrMagnitude > (behave[i].maxForce * behave[i].maxForce))
    //        {
    //            newVelocity = newVelocity.normalized * behave[i].maxForce;
    //        }
    //        currentBoid.velocity = newVelocity;
    //        Debug.Log(newVelocity);
    //    }
    //}
    ////individual
    //Vector3 calcSeparationForce(data self, data other, float separation)
    //{
    //    Vector3 diff = self.position - other.position; //from o to s
    //    float dist = diff.magnitude;
    //    return diff.normalized * separation / (dist * dist);
    //}

    ////group average
    //Vector3 calcAlignmentForce(Vector3 alignment, float alignmentMod)
    //{
    //    return alignment.normalized * alignmentMod;
    //}

    ////group average
    //Vector3 calcCohesionForce(data self, Vector3 cohesion, float cohesionMod)
    //{
    //    return (cohesion - self.position).normalized * cohesionMod;
    //}
}
