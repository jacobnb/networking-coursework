using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Network.initNetwork(5));
        //    int idx = getNetworkInstance();
        Debug.Log("test int");
        Debug.Log(Network.testInt(5));
        // Debug.Log("test int (string)");
        // Debug.Log(testString("howdy"));
        // Debug.Log("test string (string)");
        // Debug.Log(testString2("howdy"));
        // Debug.Log("test string ()");
        // Debug.Log(testString3());
    }
}
