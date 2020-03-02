#pragma once
#include <string>
struct vec3 {
	float x;
	float y;
	float z;
	vec3() {
		x = 0; y = 0; z = 0;
	};
	vec3(vec3 &cpy) {
		x = cpy.x;
		y = cpy.y;
		z = cpy.z;
	};
};
struct data
{
	vec3 position;
	vec3 velocity;
	data(data* cpy) {
		position = vec3(cpy->position);
		velocity = vec3(cpy->velocity);
	};
};
enum GameMessages
{
	BOID = ID_USER_PACKET_ENUM + 1, //135
	GAME_MESSAGE
};

struct GameMessage
{
	char* message;
	int length;
	GameMessage(char* mes, int len) {
		message = mes;
		length = len;
	}
};

struct clientData {
	std::string username;
	int ID;
};


struct ConnectMessage
{
	std::string clientUserName;
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