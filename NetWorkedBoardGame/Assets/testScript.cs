using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

public class testScript : MonoBehaviour
{
    private IEnumerator coroutine;
    int id;
    int status = -1;
    // Start is called before the first frame update
    //void Awake()
    //{
    //    Network.initNetwork(2);
    //    id = Network.getNetworkInstance();
    //    Network.initServer(60000, "server", 2, id);
    //    coroutine = checkStatus();
    //   // StartCoroutine(coroutine);
    //}

    //private void Update()
    //{
    //    //if (status == 2)
    //    {
    //        StringBuilder sb = new StringBuilder(256);
    //        if (Network.readMessage(sb, sb.Capacity, id) > 0)
    //        {
    //            Debug.Log(sb.ToString());
    //        }
    //    }
    //}

    IEnumerator checkStatus()
    {
        while(status != 2)
        {
            yield return new WaitForSeconds(3);
            status = Network.checkConnection(id);
            Debug.Log(status);
        }
        
    }
    public void serverTest()
    {
        Network.doEverything(true);
    }
    public void clientTest()
    {
        Network.doEverything(false);
    }
}
