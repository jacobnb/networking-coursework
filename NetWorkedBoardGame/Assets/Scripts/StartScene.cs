using System.Collections;
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
        //Mode.GetComponent<Dropdown>().value = 1;

    }

	public Toggle isServerToggle;
	public InputField IP_input;
    public InputField PortInput;
    public InputField UserName;
    public GameObject Mode;
	bool networkLinked = false;
	public bool isServer = false;

    public void onServerChange()
    {
        //if (isServerToggle.isOn)
        //{
        //    Mode.SetActive(true);
        //}
        //else
        //{
        //    Mode.SetActive(false);
        //}
    }
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
            
            NetworkManager.Instance.initServer(int.Parse(PortInput.text), UserName.text, (NetworkManager.NetworkMode)Mode.GetComponent<Dropdown>().value);
        }
        else
        {
            NetworkManager.Instance.initClient(IP_input.text, int.Parse(PortInput.text), UserName.text, (NetworkManager.NetworkMode)Mode.GetComponent<Dropdown>().value);
        }
        SceneManager.LoadScene("game");
    }

}
