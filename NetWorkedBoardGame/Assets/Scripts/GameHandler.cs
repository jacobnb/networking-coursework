using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum messageID
{
	GAMESTART,
	MOVE_TURN,
	END_TURN,
	GAME_END,
	GAME_CHAT
}

public enum modes
{
LOBBY,
TIC_TAC,
CHECKERS
}

public class GameHandler : MonoBehaviour
{
	modes currentGameMode = modes.LOBBY;
	bool isServer = false;

	bool mouseclicked = false;
	Vector2 mousePosition = Vector2.zero;

	GameObject[,] board;

	int[,] ttBoard;
	GameObject[,] checkerBoard;

	public GameObject tileWhite;
	public GameObject tileBlack;
	public GameObject ticTacPiece1;
	public GameObject ticTacPiece2;
	public GameObject checkerPiece;

	public Dropdown modeDropDown;
	public Dropdown player1Drop;
	public Dropdown player2Drop;
	public Button endStartButton;
	public InputField chatInput;

	List<GameObject> player1Pieces;
	List<GameObject> player2Pieces;

	int thisPlayerID; // 0 is player1, 1 is player2, -1 is spectator
	int playerTurnCount; //0 is firstplayer, 1 is second player turn

	bool checkerPieceHighlighted;
	GameObject highlightedPiece;
	private void Start()
	{
        isServer = NetworkManager.Instance.isServer;
		player1Pieces = new List<GameObject>();
		player2Pieces = new List<GameObject>();
        initTicTac();
		initLobby();
	}

	private void Update()
	{
		//get user input
		getInput();
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
		if(Input.GetMouseButtonDown(0))
		{
			mouseclicked = true;
			mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}

		if(Input.GetKeyDown(KeyCode.Return))
		{
			string chatMessage = chatInput.text;
			chatInput.text = "";
			//TO DO SEND MESSAGE
		}
	}

	void buttonClicked()
	{
		switch (currentGameMode)
		{
			case modes.LOBBY:
				if(isServer)
				{
					//check for drop down if filled
					int player1ID = player1Drop.value -1;
					int player2ID = player2Drop.value -1;

					
					if(player1ID != player2ID)
					{
						currentGameMode = (modes)modeDropDown.value + 1;
						//if server is a player set itself as player if not then set as spec
						//send these two non server players a message that they are the players and start game
						if (player1ID == -1)
						{
							thisPlayerID = 0;
						}
						else
						{
							//send message to playerID to start with as player1(0)
						}

						if (player2ID == -1)
						{
							thisPlayerID = 1;
						}
						else
						{
							//send message to playerID to start with as player2(1)
						}

						//send the rest as specators start
						for (int i = 0; i < 8; i++) //TO DO: need to grab player list
						{
							if (i != player1ID && i != player2ID)
							{
								//TODO: send player i that game is starting and they are specator
							}
						}

						switch (currentGameMode)
						{
							case modes.TIC_TAC:
								initTicTac();
								break;
							case modes.CHECKERS:
								initCheckers();
								break;
							default:
								break;
						}
					}
				}
				break;
			case modes.TIC_TAC:
				//nothing
				break;
			case modes.CHECKERS:
				//if user's turn send server message unless if server then send all users
				break;
			default:
				break;
		}
	}

	void handleLobby()
	{
		if (isServer)
		{
			player1Drop.ClearOptions();
			player2Drop.ClearOptions();

			player1Drop.options.Add(new Dropdown.OptionData("server"));
			player2Drop.options.Add(new Dropdown.OptionData("server"));
			//update list of all players into dropdown
			for (int i = 0; i < 8; i++) //TODO: need to grab playerlist
			{
				string playerName = "name"; //TODO: need to grab player name
				player1Drop.options.Add(new Dropdown.OptionData(playerName));
				player2Drop.options.Add(new Dropdown.OptionData(playerName));
			}
		}
		else
		{
			//check if gamestart if so generate correct init for game
			//set player data so this client's player either -1 , 0 , 1
			//TODO: recieve message parse 
			string message = "";
			List<string> parsedMess = parseData(message);

			switch ((messageID)int.Parse(parsedMess[0]))
			{
				case messageID.GAMESTART:
					thisPlayerID = int.Parse(parsedMess[1]);
					break;

				case messageID.GAME_CHAT:
					Debug.Log(parsedMess[1]);
					break;
			}
			
		}
	}

	void handleTicTac()
	{
		int x, y;

		//check for piece movement
		//TODO: recieve message 		
		string message = "";
		//TODO if(readMessage(message) == 1)
		List<string> parsedMess = parseData(message);
		switch ((messageID)int.Parse(parsedMess[0]))
		{
			case messageID.GAME_CHAT:
				Debug.Log(parsedMess[1]);
				break;
			case messageID.GAME_END:
				if(int.Parse(parsedMess[1]) == thisPlayerID)
				{
					Debug.Log("Player One won");
				}
				else
				{
					Debug.Log("Player Two won");
				}
				initLobby();
				break;
			case messageID.MOVE_TURN:
				x = int.Parse(parsedMess[1]);
				y = int.Parse(parsedMess[2]);
				int playerID = int.Parse(parsedMess[5]);
				if (playerTurnCount == 0)
				{
					//player1 has moved
					GameObject piece = Instantiate(ticTacPiece1, new Vector2(x, y), ticTacPiece1.transform.rotation);
					player1Pieces.Add(piece);
					ttBoard[x, y] = 0;
					playerTurnCount = 1;
				}
				else
				{
					//player2 has moved
					GameObject piece = Instantiate(ticTacPiece2, new Vector2(x, y), ticTacPiece2.transform.rotation);
					player2Pieces.Add(piece);
					ttBoard[x, y] = 1;
					playerTurnCount = 0;
				}

				if (isServer)
				{
					//TODO: send data to all other users using playerID to not send back the message to the player who moved
					//if is server send movedpiece info to specators
				}
				break;
		}

		//if player is not spectator and it is player's turn and input is pressed move/generate piece send message to server if possible
		if (thisPlayerID != -1 && playerTurnCount == thisPlayerID && mouseclicked)
		{
			if(ttBoard[Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y)] == -1)		
			{
				if(isServer)
				{
					//TODO: send all users move piece message
				}
				else
				{
					//TODO: send move piece message to server 
				}

			}
		}

		//if is server check for win state
		if (isServer)
		{
			string winner = "player";

			for(int i = 0; i < 3; i++)
			{
				if(ttBoard[i,0] == ttBoard[i,1] && ttBoard[i,0] == ttBoard[i,2] && ttBoard[i,0] != -1)
				{
					if(ttBoard[i,0] == 0)
					{
						winner = "player1";
					}
					else
					{
						winner = "player2";
					}
				}

				if (ttBoard[0, i] == ttBoard[1, i] && ttBoard[0, i] == ttBoard[2, i] && ttBoard[0, i] != -1)
				{
					if (ttBoard[0, i] == 0)
					{
						winner = "player1";
					}
					else
					{
						winner = "player2";
					}
				}
			}

			if(ttBoard[0,0] == ttBoard[1,1] && ttBoard[0,0] == ttBoard[2,2] && ttBoard[0,0] != -1)
			{
				if (ttBoard[0, 0] == 0)
				{
					winner = "player1";
				}
				else
				{
					winner = "player2";
				}
			}

			if(ttBoard[2,0] == ttBoard[1,1] && ttBoard[1,1] == ttBoard[0,2] && ttBoard[2,0] != -1)
			{
				if(ttBoard[2,0] == 0)
				{
					winner = "player1";
				}
				else
				{
					winner = "player2";
				}
			}

			if(winner == "player1" || winner == "player2")
			{
				//TODO: SEND MESSAGE GAME END to all
				initLobby();
			}
		}

	}

	void handleCheckers()
	{
		//if client check for piece movement
		//if client and piece had moved, move that piece
		//if client check for end of turn if so alterturncount

		//if is server send movedpiece info to specators
		//if is server check for win state
		//if server check for end of turn if so alterturncount send message to everyone

		//if player is not spectator and it is player's turn and input is pressed highlight checker piece
		if (thisPlayerID != -1)
		{
			//same thing as comment above but with if piece had been highlighted, move piece if possible
			
			//check if that piece can move again if so move again if wished.

			//if end turn button us pressed turn over turn order and send server data
		}
	}

	void initTicTac()
	{
		ttBoard = new int[3, 3];
		board = new GameObject[3, 3];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				ttBoard[j, i] = -1;

				if((i + j) % 2 == 0)
				{
					board[j, i] = Instantiate(tileWhite, new Vector2(j, i), tileBlack.transform.rotation);
				}
				else
				{
					board[j, i] = Instantiate(tileBlack, new Vector2(j, i), tileBlack.transform.rotation);
				}
			}
		}
	}

	void initLobby()
	{
		//clear all data
		for(int i = 0; i < board.Length; i++)
		{
			for(int j = 0; j < board.Length; j++)
			{
				Destroy(board[i, j]); 
			}
		}

		for(int i = 0; i < player1Pieces.Count; i++)
		{
			Destroy(player1Pieces[i]);
		}

		player1Pieces.Clear();

		for(int i = 0; i < player2Pieces.Count; i++)
		{
			Destroy(player2Pieces[i]);
		}

		player2Pieces.Clear();

		playerTurnCount = 0;
		currentGameMode = modes.LOBBY;
		
		if(isServer)
		{
			//is server activate ui stuff
			player1Drop.gameObject.SetActive(true);
			player2Drop.gameObject.SetActive(true);
			modeDropDown.gameObject.SetActive(true);
		}
		else
		{
			//is client
			player1Drop.gameObject.SetActive(false);
			player2Drop.gameObject.SetActive(false);
			modeDropDown.gameObject.SetActive(false);
			//show text sayign wiating on server
		}
	}

	
	List<string> parseData(string message)
	{
		List<string> dataList = new List<string>();

		string data = "";
		for(int i = 0; i < message.Length; i++)
		{
			if(message[i] == ',')
			{
				dataList.Add(data);
			}
			else
			{
				data += message[i];
			}
		}

		return dataList;
	}

	void initCheckers()
	{
		board = new GameObject[8, 8];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if ((i + j) % 2 == 0)
				{
					board[j, i] = Instantiate(tileWhite, new Vector2(j, i), tileBlack.transform.rotation);
				}
				else
				{
					board[j, i] = Instantiate(tileBlack, new Vector2(j, i), tileBlack.transform.rotation);

					//spawn checker pieces
					if(i < 3 ) //playerone pieces
					{
						GameObject gamePiece = Instantiate(checkerPiece, new Vector2(j, i), tileBlack.transform.rotation);
						player1Pieces.Add(gamePiece);

						//set game piece color blue
		
						
					}
					else if(i > 4) //platertwopieces
					{
						GameObject gamePiece = Instantiate(checkerPiece, new Vector2(j, i), tileBlack.transform.rotation);
						player2Pieces.Add(gamePiece);

						//set game piece color red
					}
				}
			}
		}
	}
}
