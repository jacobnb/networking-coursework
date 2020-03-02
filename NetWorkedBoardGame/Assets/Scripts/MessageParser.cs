using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageParser
{
    public enum messageTypes
    {
        BOID_POSITION,
        SPAWN_BOIDS,
    }
    public static string spawnBoidsMessage(int numBoids)
    {
        string mes;
        mes = ((int)messageTypes.SPAWN_BOIDS).ToString();
        mes += numBoids.ToString();
        return mes;
    }
    public static int getNumSpawnBoids(string message)
    {
        return int.Parse(message.Substring(1));
    }
}
