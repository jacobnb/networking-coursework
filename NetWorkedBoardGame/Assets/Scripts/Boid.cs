using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;



public class Boid: MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    public struct data
    {
        public Network.vec3 position;
        public Network.vec3 velocity;
        public Network.vec3 acceleration;
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
    /* time variables
    * When the boids array was updated
    * When the boids were last integrated
    * current game time - to get delta
    * 
    * Need to store previous data to lerp? or maybe lerp based on game object position
    */
    public float msToLerp = 2000f;
    public ulong time; // last message time
    ulong lastUpdateTime;
    // TODO: Make boids collide with each other.
    float radius = .5f;
    float maxSpeed = 5f;
    public GameObject boidFab;
    int NUM_BOIDS = 4;
    public int getNumBoids() { return NUM_BOIDS; }
    const int MIN_Z = 15;
    const int MAX_Z = 25;
    public data[] boids;
    behavior[] behave;
    public GameObject[] gameObjects;
    private void Start()
    {
        //initBoidObjects();
    }
    private void Update()
    {
        //updateBoids(Time.deltaTime);
    }
    public void resize(int newLength)
    {
        // TODO: make this more efficient
        foreach(GameObject go in gameObjects)
        {
            go.SetActive(false);
        }
        NUM_BOIDS = newLength;
        initBoidObjects();
    }

    public void initBoidObjects()
    {
        time = Network.getCurrentTime();
        boids = new data[NUM_BOIDS];
        behave = new behavior[NUM_BOIDS];
        gameObjects = new GameObject[NUM_BOIDS];
        Network.vec3 position = new Network.vec3(-2,0,0);
        Network.vec3 velocity = new Network.vec3(5f, 1,1);
        for(int i=0; i < NUM_BOIDS; i++)
        {
            gameObjects[i] = Instantiate(boidFab);
            boids[i].position = position;
            position.x+=2;
            boids[i].velocity = velocity;
            boids[i].acceleration = new Network.vec3(0f,0f,0f);
            //behave[i] = new behavior(5f, 5f, 5f, 5f, 5f, 10f);
        }
    }
    public void updateBoids(ulong gameTime)
    {
        doCollision();
        screenWrap();
        //updateBoidVelocity();
        // applyVelocityAndPosition((gameTime-time)/1000f);
        eulerIntegrate((gameTime-time)/1000f);
        lastUpdateTime = time;
        time = gameTime;
        setPosition();

    }
    public void deadReckonBoids(ulong gameTime, Boid with)
    {
        float dt = (gameTime - time) / 1000f;

        doCollision(with);
        // applyVelocityAndPosition((gameTime-time)/1000f);
        eulerIntegrate(dt);
        if (gameTime- lastUpdateTime > 100) // > .1 s
        {
            lerpPosition(Mathf.Min(dt / msToLerp, 1f));
        }
        else
        {
            setPosition();
        }
        lastUpdateTime = time;
        time = gameTime;
    }
    public void updateBoids(ulong gameTime, Boid with)
    {
        float dt = (gameTime - time) / 1000f;
        doCollision(with);
        screenWrap();
        //updateBoidVelocity();
        //applyVelocityAndPosition((gameTime - time) / 1000f);
        eulerIntegrate(dt);
        lastUpdateTime = time;
        time = gameTime;
        setPosition();
    }
    public void doCollision()
    {
        for (int i = 0; i < NUM_BOIDS; i++)
        {
            data currentBoid = boids[i];
            Vector3 position = boids[i].position.toVector3();
            // get all other boids data
            for (int c = 0; /*see below*/; c++)
            {
                if (i == c) //skip self
                    c++;
                if (c >= NUM_BOIDS)
                    break;
                Vector3 oPosition = boids[c].position.toVector3();
                Vector3 diff = (position-oPosition);
                if(diff.sqrMagnitude <= 4 * radius*radius)
                {
                    Vector3 newVel =Vector3.Reflect(boids[i].velocity.toVector3(), diff);
                    boids[i].velocity = new Network.vec3(newVel.normalized*maxSpeed);
                }
            }
        }
    }
    public void doCollision(Boid with)
    {
        for (int i = 0; i < NUM_BOIDS; i++)
        {
            data currentBoid = boids[i];
            Vector3 position = boids[i].position.toVector3();
            // get all other boids data
            for (int c = 0; /*see below*/; c++)
            {
                if (i == c) //skip self
                    c++;
                if (c >= NUM_BOIDS)
                    break;
                Vector3 oPosition = boids[c].position.toVector3();
                Vector3 diff = (position - oPosition);
                if (diff.sqrMagnitude <= 4 * radius * radius)
                {
                    Vector3 newVel = Vector3.Reflect(boids[i].velocity.toVector3(), diff);
                    boids[i].velocity = new Network.vec3(newVel.normalized * maxSpeed);
                }
            }
            // Handle our collision with other boids.
            for (int c = 0; /*see below*/; c++)
            {
                if (c >= with.getNumBoids())
                    break;
                Vector3 oPosition = with.boids[c].position.toVector3();
                Vector3 diff = (position - oPosition);
                if (diff.sqrMagnitude <= 4 * radius * radius)
                {
                    Vector3 newVel = Vector3.Reflect(boids[i].velocity.toVector3(), diff);
                    boids[i].velocity = new Network.vec3(newVel.normalized * maxSpeed);
                }
            }
        }
    }
    public void screenWrap()
    {
        float border = 50f;
        for(int i = 0; i<NUM_BOIDS; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(boids[i].position.toVector3());
            if(screenPos.x > Screen.width-border)
            {
                //screenPos.x = 0f;
                boids[i].velocity.x = -boids[i].velocity.x;
            }
            else if (screenPos.x < 0f+border) {
                //screenPos.x = Screen.width;
                boids[i].velocity.x = -boids[i].velocity.x;
            }
            if(screenPos.y > Screen.height-border)
            {
                //screenPos.y = 0f;
                boids[i].velocity.y = -boids[i].velocity.y;
            }
            else if (screenPos.y < 0f+border)
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
         //   gameObjects[i].transform.position = boids[i].position.toVector3();
        }
    }
    public void applyVelocityAndPosition(float dt)
    {
        for(int i =0; i<NUM_BOIDS; i++)
        {
            boids[i].position += boids[i].velocity * dt;
            gameObjects[i].transform.position = boids[i].position.toVector3();
        }
    }
    public void eulerIntegrate(float dt)
    {
        // step 2: Motion Integration
        for (int i = 0; i < NUM_BOIDS; i++)
        {
            boids[i].position += boids[i].velocity * dt + boids[i].acceleration *.5f * dt * dt;
            boids[i].velocity += boids[i].acceleration * dt;
        }
    }
    public void setPosition()
    {
        for (int i = 0; i < NUM_BOIDS; i++)
        {
            gameObjects[i].transform.position = boids[i].position.toVector3();
        }
    }
    public void lerpPosition(float t)
    {
        for (int i = 0; i < NUM_BOIDS; i++)
        {
            gameObjects[i].transform.position = Vector3.Lerp(gameObjects[i].transform.position, boids[i].position.toVector3(), t);
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
