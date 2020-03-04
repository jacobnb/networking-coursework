using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class Server : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!NetworkManager.Instance.isServer)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder sb = new StringBuilder(256);
        int isMessage = Network.readMessage(sb, sb.Capacity);
        string message = sb.ToString();
        while (isMessage == 1)
        {
            // TODO: Add in the queue stuff here.
            Network.sendMessage(sb.ToString());
        }
    }
}
