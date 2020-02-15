using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageParser : MonoBehaviour
{
    public enum MessageType
    {
        FORCE,
        FORCE_CHANGE,
        COLOR,
        CHAT
    }
    public static string encodeForceMessage(RocketLeague.ForceDirection forceDirection)
    {
        string message = MessageType.FORCE.ToString() + "," + forceDirection.ToString();
        return message;
    }
    public static string encodeColorMessage(Vector3 data)
    {
        string message = MessageType.COLOR.ToString() + "," + data.x.ToString() + "," + data.y.ToString() + "," + data.z.ToString();
        return message;
    }
    public static string encodeForceChangeMessage(int data)
    {
        string message = MessageType.FORCE_CHANGE.ToString() + "," + data.ToString();
        return message;
    }
    public static string encodeChatMessage(string data)
    {
        string message = MessageType.CHAT.ToString() + "," + data;
        return message;
    }


}
