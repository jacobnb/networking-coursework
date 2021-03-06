﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	public InputField IP_input;
    public InputField PortInput;
    public InputField UserName;
	bool networkLinked = false;
	public bool isServer = false;

    public void Connect()
    {
        isServer = isServerToggle.isOn;
        if (IP_input.text == "" && !isServer)
        {
            Debug.Log("Enter IP");
            return;
        }
        if(PortInput.text == "")
        {
            Debug.Log("Enter Port");
            return;
        }

        if (isServer)
        {
            NetworkManager.Instance.initServer(int.Parse(PortInput.text), UserName.text);
        }
        else
        {
            NetworkManager.Instance.initClient(IP_input.text, int.Parse(PortInput.text), UserName.text);
        }
        SceneManager.LoadScene("waitingForPlayers");
    }

}
