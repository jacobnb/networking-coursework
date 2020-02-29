using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct data
{
    public Vector3 position;
    public Vector3 velocity;
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
        updateBoids();
    }
    void initBoidObjects()
    {
        Vector3 position = Vector3.zero;
        Vector3 velocity = new Vector3(1f, 1f, 1f);
        for(int i=0; i < NUM_BOIDS; i++)
        {
            // TODO? are structs deep or shallow copied?
            gameObjects[i] = Instantiate(boidFab);
            boids[i].position = position;
            position.x++;
            boids[i].velocity = velocity;
            behave[i] = new behavior(5f, 5f, 5f, 5f, 5f, 10f);
        }
    }
    void updateBoids()
    {
        screenWrap();
        updateVelocity();
        applyVelocityAndPosition();
    }
    void screenWrap()
    {
        for(int i = 0; i<NUM_BOIDS; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(boids[i].position);
            if(screenPos.x > Screen.width)
            {
                screenPos.x = 0f;
            }
            else if (screenPos.x < 0f) {
                screenPos.x = Screen.width;
            }
            if(screenPos.y > Screen.height)
            {
                screenPos.y = 0f;
            }
            else if (screenPos.y < 0f)
            {
                screenPos.y = Screen.height;
            }
            if(screenPos.z > MAX_Z)
            {
                screenPos.z = MIN_Z;
            }
            if(screenPos.z < MIN_Z)
            {
                screenPos.z = MAX_Z;
            }
            boids[i].position = Camera.main.ScreenToWorldPoint(screenPos);
        }
    }
    void applyVelocityAndPosition()
    {
        for(int i =0; i<NUM_BOIDS; i++)
        {
            gameObjects[i].transform.position = boids[i].position;
            // TODO: cache this.
            gameObjects[i].GetComponent<Rigidbody>().velocity = boids[i].velocity;
        }
    }
    void updateVelocity()
    {
        data currentBoid;
        Vector3 newVelocity = Vector3.zero;
        for (int i = 0; i < NUM_BOIDS; i++)
        {
            currentBoid = boids[i];
            int count = 0;
            Vector3 sepForce = Vector3.zero;
            Vector3 alignment = Vector3.zero;
            Vector3 cohesion = Vector3.zero;
            // get all other boids data
            for (int c = 0; /*see below*/; c++)
            {

                if (i == c) //skip self
                    c++;
                if (c >= NUM_BOIDS)
                    break;
                if ((currentBoid.position - boids[c].position).sqrMagnitude
                    < (behave[i].radius * behave[i].radius))
                {
                    count++;
                    sepForce += calcSeparationForce(currentBoid, boids[c], behave[i].separation);
                    alignment += boids[c].velocity;
                    cohesion += boids[c].position;
                }

            }
            sepForce /= count;
            alignment /= count;
            cohesion /= count;
            newVelocity = currentBoid.velocity.normalized * behave[i].forward
                + sepForce
                + calcAlignmentForce(alignment, behave[i].alignment)
                + calcCohesionForce(currentBoid, cohesion, behave[i].cohesion);
            if (newVelocity.sqrMagnitude > (behave[i].maxForce * behave[i].maxForce))
            {
                newVelocity = newVelocity.normalized * behave[i].maxForce;
            }
            currentBoid.velocity = newVelocity;
            Debug.Log(newVelocity);
        }
    }
    //individual
    Vector3 calcSeparationForce(data self, data other, float separation)
    {
        Vector3 diff = self.position - other.position; //from o to s
        float dist = diff.magnitude;
        return diff.normalized * separation / (dist * dist);
    }

    //group average
    Vector3 calcAlignmentForce(Vector3 alignment, float alignmentMod)
    {
        return alignment.normalized * alignmentMod;
    }

    //group average
    Vector3 calcCohesionForce(data self, Vector3 cohesion, float cohesionMod)
    {
        return (cohesion - self.position).normalized * cohesionMod;
    }
}
