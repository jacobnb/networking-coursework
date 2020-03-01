using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Network.blah[] arr = new Network.blah[5];
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<5; i++)
        {
            arr[i].data = new Network.vec3(1f, 2f, 3f);
            arr[i].dataz = new Network.vec3(0f,0f,0f);
        }
        sendArr();
    }

    void sendArr()
    {
        Network.testArray(arr, 5);
        for(int i=0; i<5; i++)
        {
            Debug.Log(arr[i].data.x);
            Debug.Log(arr[i].dataz.x);
        }
    }
}
