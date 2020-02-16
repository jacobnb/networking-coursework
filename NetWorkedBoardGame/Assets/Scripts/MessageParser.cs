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
        CHAT,
        INVALID_TYPE
    }
    public static string encodeForceMessage(RocketLeague.ForceDirection forceDirection)
    {
        string message = ((int)MessageType.FORCE).ToString() + ((int)forceDirection).ToString();
        return message;
    }
    public static string encodeColorMessage(Vector3 data)
    {
        string message = ((int)MessageType.COLOR).ToString() + data.x.ToString() + "," + data.y.ToString() + "," + data.z.ToString();
        return message;
    }
    public static string encodeForceChangeMessage(int data)
    {
        string message = ((int)MessageType.FORCE_CHANGE).ToString() + data.ToString();
        return message;
    }
    public static string encodeChatMessage(string data)
    {
        string message = ((int)MessageType.CHAT).ToString() + data;
        return message;
    }
    public static MessageType getMessageType(string message)
    {
        if (message.Length < 2)
        {
            return MessageType.INVALID_TYPE;
        }
        return (MessageType)int.Parse(message.Substring(0,1));
    }
    public static RocketLeague.ForceDirection getForce(string message)
    {
        return (RocketLeague.ForceDirection)int.Parse(message.Substring(1));
    }
    public static Vector4 getColor(string message)
    {
        Vector4 vec;
        string [] values = message.Substring(1).Split(',');
        vec.x = int.Parse(values[0]);
        vec.y = int.Parse(values[1]);
        vec.z = int.Parse(values[2]);
        vec.w = 1f;
        return vec;
    }
    public static int getForceChange(string message)
    {
        return int.Parse(message.Substring(1));
    }
}
