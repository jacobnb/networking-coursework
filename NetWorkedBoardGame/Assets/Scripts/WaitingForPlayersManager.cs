using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaitingForPlayersManager : MonoBehaviour
{
    public Text chatBox;
    public InputField message;
    Queue<string> messages = new Queue<string>();
    public int numMessagesDisplayed = 5;
    // Update is called once per frame
    private void Start()
    {
        for(int i = 0; i < numMessagesDisplayed; i++)
            messages.Enqueue("");
    }
    void Update()
    {
        string newMessage = NetworkManager.Instance.readMessage();
        if(newMessage != "")
        {
            messages.Dequeue();
            messages.Enqueue(newMessage);
        }
        displayMessages();
    }
    public void sendMessage()
    {
        NetworkManager.Instance.sendMessage(message.text);
        message.text = "";
    }
    void displayMessages()
    {
        string mesDisplay = "";
        string curMessage;
        for(int i=0; i <numMessagesDisplayed; i++)
        {
            curMessage = messages.Dequeue();
            mesDisplay += curMessage + '\n';
            messages.Enqueue(curMessage);
        }
        chatBox.text = mesDisplay;
    }
}
