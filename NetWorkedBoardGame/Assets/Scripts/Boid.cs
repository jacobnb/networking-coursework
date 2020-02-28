using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct data
{
    public Vector3 position;
    public Vector3 velocity;
}
struct behavior
{
    public float radius;
    public float separation;
    public float alignment;
    public float cohesion;
    public float forward;
    public float maxForce;
}
public class Boid
{
    const int NUM_BOIDS = 20;
    data[] boids = new data[NUM_BOIDS];
    behavior[] behave = new behavior[NUM_BOIDS];

    void updateBoids()
    {
        // TODO: Wrap screen.
        updateVelocity();
        // TODO: apply new velocity
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
            for (int c = 0; c < NUM_BOIDS; c++)
            {

                if (i == c) //skip self
                    c++;
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
