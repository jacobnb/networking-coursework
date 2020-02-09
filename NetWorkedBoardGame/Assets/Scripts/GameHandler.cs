using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum modes
{
LOBBY,
TIC_TAC,
CHECKERS
}

public class GameHandler : MonoBehaviour
{
	modes currentGameMode = modes.LOBBY;

	bool mouseclicked = false;
	Vector2 mousePosition = Vector2.zero;

	private void Start()
	{
		initLobby();
	}
	private void Update()
	{
		//get user input
		switch (currentGameMode)
		{
			case modes.LOBBY:
				break;
			case modes.TIC_TAC:
				break;
			case modes.CHECKERS:
				break;
			default:
				break;
		}
	}

	void getInput()
	{

	}

	void handleLobby()
	{

	}

	void handleTicTac()
	{

	}

	void handleCheckers()
	{

	}

	void initTicTac()
	{

	}

	void initLobby()
	{

	}

	void initCheckers()
	{

	}
}
