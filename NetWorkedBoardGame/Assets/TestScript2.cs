using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TestScript2 : MonoBehaviour
{
    private IEnumerator coroutine;
    int id;
    int status = -1;
    // Start is called before the first frame update
    //void Start()
    //{
    //    id = Network.getNetworkInstance();
    //    Network.initClient("69.5.121.143", 60000, "jacob", id);
    //    coroutine = checkStatus();
    //    StartCoroutine(coroutine);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (status == 2)
    //    {
    //        Network.sendMessage("Howdy Worldy", id);
    //    }
    //}
    IEnumerator checkStatus()
    {
        while (status != 2)
        {
            yield return new WaitForSeconds(3);
            //status = Network.checkConnection(id);
            Debug.Log(Network.checkConnection(id));
        }
    }
}
