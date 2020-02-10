using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartScene : MonoBehaviour
{
	private static StartScene instance;

	public static StartScene Instance
	{
		get{ return instance; }
	}

	private void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		instance = this;
		
	}

	public Toggle isServerToggle;
	public InputField IPinput;

	bool networkLinked = false;
	public bool isServer = false;
    // Update is called once per frame
    void Update()
    {
        if(networkLinked == false && Input.GetKeyDown(KeyCode.Return))
		{
			isServer = isServerToggle.isOn;
			if(isServer)
			{
				//move to next scene
			}
	

			string ipaddress = IPinput.text;
			//TODO: INIT NETWORK	
			/*
			if(connect to network and successful)
			{
				networkLinked = true;
				//move to next scene
			}
			*/
		}
    }
}
