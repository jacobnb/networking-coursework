#pragma once
#include <string>
struct vec3 {
	float x;
	float y;
	float z;
};
struct data
{
	vec3 position;
	vec3 velocity;
};

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