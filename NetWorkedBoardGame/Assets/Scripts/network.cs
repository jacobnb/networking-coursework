using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
public class Network
{
    [StructLayout(LayoutKind.Sequential)]
    public struct vec3
    {
        public float x;
        public float y;
        public float z;
        public vec3(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public vec3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }
        public Vector3 toVector3()
        {
            return new Vector3(x, y, z);
        }
        public static vec3 operator *(vec3 vec, float scalar)
        {
            vec.x *= scalar;
            vec.y *= scalar;
            vec.z *= scalar;
            return vec;
        }
        public static vec3 operator +(vec3 vec, vec3 rhs)
        {
            vec.x += rhs.x;
            vec.y += rhs.y;
            vec.z += rhs.z;
            return vec;
        }
    }

    [DllImport("networking")]
    public static extern int initNetwork(int numNetworkInstances);
    [DllImport("networking")]
    public static extern int getNetworkInstance();
    [DllImport("networking", CharSet = CharSet.Ansi)]
    public static extern int initClient(string IP, int port, string username, int ID = 0);
    [DllImport("networking")]
    public static extern int initServer(int port, string username, int maxClients = 10, int ID = 0);
    [DllImport("networking")]
    public static extern int cleanup(int ID = 0);

    //send gameplay message
    [DllImport("networking", CharSet = CharSet.Ansi)]
    public static extern int sendMessage(string message, int ID = 0);
    //@return 1 = message, 0 = no message, -1 = error
    [DllImport("networking", CharSet = CharSet.Ansi)]
    public static extern int readMessage(StringBuilder message, int bufferSize, int ID = 0);
    [DllImport("networking")]
    public static extern int checkConnection(int ID = 0);
    [DllImport("networking")]
    public static extern int doEverything(bool isServer);
    [DllImport("networking")]
    public static extern int sendBoidMessage(Boid.data[] boids, int length, int ID = 0);
    [DllImport("networking")]
    public static extern int readBoidMessage(Boid.data[] boids, int length, int ID = 0);
    [DllImport("networking")]
    public static extern int readMessages(int ID = 0);
    [DllImport("networking")]
    public static extern int serverMessages(int ID=0);
}