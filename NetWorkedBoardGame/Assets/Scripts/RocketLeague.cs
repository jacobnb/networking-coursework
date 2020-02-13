using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLeague : MonoBehaviour
{
    // TODO: Colors, speed changes.
    public Rigidbody ball;
    [SerializeField]
    float force = 10f;
    enum ForceDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        VERT_UP,
        VERT_DOWN
    }
    Queue<ForceDirection> forces = new Queue<ForceDirection>();
    Queue<Vector4> colors = new Queue<Vector4>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Remove following
        getKeyboardInput();

        getInput();
        getAndApplyForces();

        // TODO: Do we need to sync clients?
    }
    void getKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            forces.Enqueue(ForceDirection.UP);
        if (Input.GetKeyDown(KeyCode.S))
            forces.Enqueue(ForceDirection.DOWN);
        if (Input.GetKeyDown(KeyCode.A))
            forces.Enqueue(ForceDirection.LEFT);
        if (Input.GetKeyDown(KeyCode.D))
            forces.Enqueue(ForceDirection.RIGHT);
        if (Input.GetKeyDown(KeyCode.E))
            forces.Enqueue(ForceDirection.VERT_UP);
        if (Input.GetKeyDown(KeyCode.Q))
            forces.Enqueue(ForceDirection.VERT_DOWN);
    }
    void getInput()
    {
        // TODO: for each message, queue force in forces.
        // TODO: for each message queue color in colors
        // TODO: for each message queue speed change in speeds.

    }
    void getAndApplyForces()
    {
        for(int i = forces.Count; i >0; i--)
        {
            applyForce(forces.Dequeue());
        }
    }
    void applyForce(ForceDirection forceDirection)
    {
    switch(forceDirection)
            {
        case ForceDirection.DOWN:
                ball.AddForce(new Vector3(0,0,-1) * force, ForceMode.Impulse);
            break;
        case ForceDirection.UP:
                ball.AddForce(new Vector3(0, 0, 1) * force, ForceMode.Impulse);
            break;
            case ForceDirection.RIGHT:
                ball.AddForce(Vector3.right * force, ForceMode.Impulse);
            break;
            case ForceDirection.LEFT:
                ball.AddForce(-Vector3.right * force, ForceMode.Impulse);
            break;
            case ForceDirection.VERT_DOWN:
                ball.AddForce(-Vector3.up * force, ForceMode.Impulse);
            break;
            case ForceDirection.VERT_UP:
                ball.AddForce(Vector3.up * force, ForceMode.Impulse);
            break;
        }
}
}
