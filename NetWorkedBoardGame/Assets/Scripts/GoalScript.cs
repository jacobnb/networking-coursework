using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public int teamNum;
    public RocketLeague gameManager;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            gameManager.addScore(teamNum);
            gameManager.resetBall();
        }
    }
}
