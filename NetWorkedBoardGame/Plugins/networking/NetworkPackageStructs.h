#pragma once
#include <string>
struct ConnectMessage
{
	std::string clientUserName;
};

struct GameMessage
{
	int playerID; //the player who moved piece id

	int pieceLocationX;
	int pieceLocationY;

	int newLocationX;
	int newLocationY;
};

struct ChatMessage
{
	int targetId; //if not to a target, it is -1 which is global
	std::string message;
};

struct startMessage
{
	int playerOneID;
	int playerTwoID;
};