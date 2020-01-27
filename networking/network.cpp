#include "network.h"


void Network::dispCurrentMessage()
{
	for (int i = 0; i <= cursor; i++) {
		printf("%c", curMsg[i]);
	}
	// lets you print over the line. space to cover prev chars after backspace
	printf(" ");
	printf("\r");
}

void Network::clearAsyncKeyBuffers()
{
	// TODO: make this only clear the keys we check
	for (int i = 0x1; i <= 0xFE; i++) {
		GetAsyncKeyState(i);
	}
}

void Network::checkAndCreateMessage()
{
	SHORT keyStatus;
	keyStatus = GetAsyncKeyState(VK_RSHIFT);
	if (keyStatus & 0x1) { // check least significant bit only

		char usrName[USERNAME_LENGTH];
		char message[MESSAGE_LENGTH];
		printf("What's your message: \n");
		fgets(message, MESSAGE_LENGTH, stdin); // read in the message
		printf("\n-----------------------------------------------\n");
		printf("if this is a private message please enter the username. If not, hit enter: \n");
		fgets(usrName, USERNAME_LENGTH, stdin);

		if (isServer)
		{
			if (usrName[1] != 0) {
				// private message.
				messageData messagePackage = messageData(ID_SEND_MESSAGE, message, true, usrName);
				clientData cd = getClient(usrName);
				if (cd.userName[0] != -1) {
					peer->Send(reinterpret_cast<char*>(&messagePackage), sizeof(messagePackage), HIGH_PRIORITY, RELIABLE_ORDERED, 0, cd.clientAddress, false);
				}
			}
			else {
				sendPublicServerMessage(message);
			}
		}
		else
		{
			if (usrName[1] != 0) {
				// private message.
				messageData messagePackage = messageData(CLIENT_SEND_MESSAGE, message, true, usrName);
				peer->Send(reinterpret_cast<char*>(&messagePackage), sizeof(messagePackage), HIGH_PRIORITY, RELIABLE_ORDERED, 0, serverAddress, false);
			}
			else {
				// public message
				messageData messagePackage = messageData(CLIENT_SEND_MESSAGE, message, false);
				peer->Send(reinterpret_cast<char*>(&messagePackage), sizeof(messagePackage), HIGH_PRIORITY, RELIABLE_ORDERED, 0, serverAddress, false);
			}
		}
	}
}

Network::Network()
{
	peer = RakNet::RakPeerInterface::GetInstance();

}

Network::~Network()
{
}

int Network::init()
{
	// for Client
	MAX_CLIENTS = 1;
	cursor = 0;
	printf("Set SERVER_PORT (default 60000)\n");
	fgets(str, 510, stdin);
	int numInput = std::atoi(str);
	numInput == 0 ? 1 : SERVER_PORT = numInput;

	printf("(C)lient or (S)erver?\n");
	fgets(str, 510, stdin);
	if ((str[0] == 'c') || (str[0] == 'C'))
	{
		RakNet::SocketDescriptor sd;
		peer->Startup(1, &sd, 1);
		isServer = false;
	}
	else {
		printf("Set MAX_CLIENTS (default 10)\n");
		fgets(str, 510, stdin);
		numInput = std::atoi(str);
		numInput == 0 ? 1 : MAX_CLIENTS = numInput;

		RakNet::SocketDescriptor sd(SERVER_PORT, 0);
		peer->Startup(MAX_CLIENTS, &sd, 1);
		isServer = true;
	}

	if (isServer)
	{
		printf("Enter your username\n");
		fgets(str, USERNAME_LENGTH, stdin);
		// clear newline.
		int index = 0;
		while (str[index] != '\n' && str[index] != '\0' && index >= USERNAME_LENGTH) {
			index++;
		}
		str[index] = '\0';
		strcpy(serverName, str);

		printf("Starting the server.\n");
		// We need to let the server accept incoming connections from the clients
		peer->SetMaximumIncomingConnections(MAX_CLIENTS);
	}
	else {
		printf("Enter server IP or hit enter for 127.0.0.1\n");
		// str[1] = 0;
		fgets(str, 510, stdin);
		if (str[1] == 0) {
			strcpy(str, "127.0.0.1:60000");
		}
		printf("Starting the client.\n");
		peer->Connect(str, SERVER_PORT, 0, 0);

	}

	// attempt to clear buffers
	fflush(stdin);
	clearAsyncKeyBuffers();

	return 0;
}

int Network::cleanup()
{
	RakNet::RakPeerInterface::DestroyInstance(peer);
	return 0;
}

void Network::update()
{
	checkAndCreateMessage(); // non async way to type message
	// get packet; if packet; get packet;
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		switch (packet->data[0])
		{
		case ID_CLIENT_TO_SERVER:
		{
			/* TODO?
			//recieve and print message
			printf("Message recieved from client \n");
			messageData message = *(messageData*)packet->data;
			printf("%s\n", message.mes);

			//request message and send to client with server to client id
			printf("What's your message?\n");
			fgets(str, MESSAGE_LENGTH, stdin);
			messageData msOut(CLIENT_SEND_MESSAGE, str, false);
			// https://www.leadwerks.com/community/topic/4222-raknet-send-and-receive-struct/
			peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);			*/

		}
		break;
		case CLIENT_SEND_MESSAGE:
		{
			//recieve and print message
			printf("Message recieved from client \n");
			messageData message = *(messageData*)packet->data;

			if (message.userName == MD_NO_DESTINATION || !message.privateMessage)
			{
				sendPublicMessage(message, packet->systemAddress);

				char* userName = getClient(packet->systemAddress).userName;
				if (userName[0] == -1) {
					printf("User Name Error\n");
				}
				else {
					printf("%s: %s\n", userName, message.mes);
				}
				delete userName;
				//printf(getClient(packet->systemAddress).userName + (char)": %s\n", message.mes);
			}
			else
			{

				clientData targetClient = getClient(message.userName);
				if (targetClient.userName[0] == -1) {
					printf("Client name error\n");
					return;
				}
				//send private message
				messageData msOut(CLIENT_SEND_MESSAGE, str, false);
				peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);

				char* userName = getClient(packet->systemAddress).userName;
				if (userName[0] == -1) {
					printf("User name error\n");
				}
				else {
					printf("%s: %s\n", userName , message.mes);
				}
				//printf(getClient(packet->systemAddress).userName + (char)"->" + (char)targetClient.userName + (char)": %s\n", message.mes);
			}


		}
		break;
		case ID_REMOTE_DISCONNECTION_NOTIFICATION:
			printf("Another client has disconnected.\n");
			break;
		case ID_REMOTE_CONNECTION_LOST:
			printf("Another client has lost the connection.\n");
			break;
		case ID_REMOTE_NEW_INCOMING_CONNECTION:
			printf("Another client has connected.\n");
			break;
		case ID_CONNECTION_REQUEST_ACCEPTED:
		{
			printf("Our connection request has been accepted.\n");
		}
		// request for message, then it will send message to server using client to server id
		// http://www.raknet.net/raknet/manual/creatingpackets.html
		if (!isServer) {

			serverAddress = packet->systemAddress;
			printf("Enter your username\n");
			fgets(str, USERNAME_LENGTH, stdin);
			// clear newline.
			int index = 0;
			while (str[index] != '\n' && str[index] != '\0') {
				index++;
			}
			str[index] = '\0';
			messageData msOut(ID_SEND_USERNAME, str, false);
			peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
		}
		break;
		case ID_SEND_USERNAME:
		{
			printf("Message recieved \n");
			messageData message = *(messageData*)packet->data;

			printf("Client Username: %s\n", message.mes);

			char serverMsg[MESSAGE_LENGTH];
			strcpy(serverMsg, message.mes + (char)" has joined the chat");
			sendPublicServerMessage(serverMsg);

			//TODOne: Add client to client list with username and ip address
			clientList.push_back(clientData(message.mes, packet->systemAddress));

			//send aknowledgement, sending private message
			strcpy(str, (char)"Welcome " + message.mes);
			messageData msOut(ID_SEND_MESSAGE, str, true, serverName);
			peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);

		}
		break;
		case ID_NEW_INCOMING_CONNECTION:
			printf("A connection is incoming.\n");
			break;
		case ID_NO_FREE_INCOMING_CONNECTIONS:
			printf("The server is full.\n");
			break;
		case ID_DISCONNECTION_NOTIFICATION:
			if (isServer) {
				printf("A client has disconnected.\n");
			}
			else {
				printf("We have been disconnected.\n");
			}
			break;
		case ID_CONNECTION_LOST:
			if (isServer) {
				printf("A client lost the connection.\n");
			}
			else {
				printf("Connection lost.\n");
			}
			break;
		case ID_SEND_MESSAGE:
		{
			// TODO: this prints out garbage rn
			messageData message = *(messageData*)packet->data;

			//if the message recieved is a private message
			if (message.privateMessage)
			{
				//printf(message.userName + (char)"(private): %s\n", message.mes);
				printf("%s: %s", message.userName, message.mes);
				
			}
			else
			{
				//printf(message.userName + (char)": %s\n", message.mes);
				printf("%s: %s", message.userName, message.mes);
			}

		}
		break;
		default:
			printf("Message with identifier %i has arrived.\n", packet->data[0]);
			break;
		}
	}
}

char Network::checkKeyboardState()
{
	SHORT keyStatus;
	keyStatus = GetAsyncKeyState(VK_RETURN);
	if (keyStatus & 0x1) { // check least significant bit only
		if (cursor <= 0) {
			while ((getchar()) != '\n');
		}
		else {
			char usrName[USERNAME_LENGTH];
			char message[MESSAGE_LENGTH];
			// TODOne: This doesn't seem to correctly read the username / lack of username.
			// Also for some reason the input to curMsg seems to print out here
			int index = 0;
			for (index = 0; index < cursor; index++) {
				message[index] = curMsg[index + 1]; //curMessage start with /0
			}
			message[index] = '\0';
			fgets(message, MESSAGE_LENGTH, stdin); // read in the message
			printf("\n-----------------------------------------------\n");
			printf("if this is a private message please enter the username. If not, hit enter: \n");
			fgets(usrName, USERNAME_LENGTH, stdin);

			if (isServer)
			{
				if (usrName[1] != 0) {
					// private message.
					messageData messagePackage = messageData(ID_SEND_MESSAGE, message, true, usrName);
					clientData cd = getClient(usrName);
					if (cd.userName[0] != -1) {
						peer->Send(reinterpret_cast<char*>(&messagePackage), sizeof(messagePackage), HIGH_PRIORITY, RELIABLE_ORDERED, 0, cd.clientAddress, false);
					}
				}
				else {
					sendPublicServerMessage(message);
				}
			}
			else
			{
				if (usrName[1] != 0) {
					// private message.
					messageData messagePackage = messageData(CLIENT_SEND_MESSAGE, message, true, usrName);
					peer->Send(reinterpret_cast<char*>(&messagePackage), sizeof(messagePackage), HIGH_PRIORITY, RELIABLE_ORDERED, 0, serverAddress, false);
				}
				else {
					// public message
					messageData messagePackage = messageData(CLIENT_SEND_MESSAGE, message, false);
					peer->Send(reinterpret_cast<char*>(&messagePackage), sizeof(messagePackage), HIGH_PRIORITY, RELIABLE_ORDERED, 0, serverAddress, false);
				}
			}
			//Note: see MessageDAta -> bool privateMessage determines if message is private, 
			// char userName is used when sending to server the destination, 
			// can be default as no destination but when client recieves it this char indicates the source of the message (mikesplaining srry)
			clearAsyncKeyBuffers();
			cursor = 0;
		}
	}
	keyStatus = GetAsyncKeyState(VK_BACK);
	if (keyStatus & 0x1) {
		cursor--;
		if (cursor < 0) {
			cursor = 0;
		}
		dispCurrentMessage();
	}
	keyStatus = GetAsyncKeyState(VK_SPACE);
	if (keyStatus & 0x1) {
		curMsg[++cursor] = ' ';
	}
	// punction are special cases see: 
	//https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
	//65 -> 90 - A-Z
	// TODO: Track punctuation and shift for upper/lower case.
	for (int vKey = 65; vKey < 90; vKey++) {

		keyStatus = GetAsyncKeyState(vKey);
		// msb = key is down
		// lsb = key pressed since previous call - but another app could read (take) this
		// doesn't actually matter - no bits set if not pressed, a bit set if pressed == not 0
		if (keyStatus & 0x1)
		{
			curMsg[++cursor] = vKey;
			char c = vKey;
			dispCurrentMessage();
		}
	}
	return 'a';
}

//given an ip address, find the client data
clientData Network::getClient(RakNet::SystemAddress userAddress)
{
	//oh boy i avoided auto here cool https://stackoverflow.com/questions/2395275/how-to-navigate-through-a-vector-using-iterators-c
	// for (auto it = clientList.begin(); it != clientList.end(); it++)
	for (std::vector<clientData>::iterator it = clientList.begin(); it != clientList.end(); it++)
	{
		if (it->clientAddress == userAddress)
		{
			return *it;
		}
	}
	return clientData();
}

//given an username, find the client data
clientData Network::getClient(char userName[16])
{
	// for (auto it = clientList.begin(); it != clientList.end(); it++)
	for (std::vector<clientData>::iterator it = clientList.begin(); it != clientList.end(); it++)
	{
		if (it->userName == userName)
		{
			return *it;
		}
	}

	return clientData();
}

//send a public message, avoid sending message back to user
void Network::sendPublicMessage(messageData msgData, RakNet::SystemAddress originClient)
{
	//set up new message
	char user[USERNAME_LENGTH];
	//strcpy(user, getClient(originClient).userName);
	clientData tempClient = getClient(originClient);
	if (tempClient.userName[0] == -1) {
		// couldn't access client
		printf("Couldn't access client\n");
		return;
	}
	strcpy(user, tempClient.userName);


	// for (auto it = clientList.begin(); it != clientList.end(); it++)
	for (std::vector<clientData>::iterator it = clientList.begin(); it != clientList.end(); it++)
	{
		if (it->clientAddress != originClient)
		{

			//send message
			messageData msOut(ID_SEND_MESSAGE, msgData.mes, false, user);
			peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, it->clientAddress, false);

		}
	}
}

//send a public message, avoid sending message back to user
void Network::sendPublicServerMessage(char msg[MESSAGE_LENGTH])
{
	// for (auto it = clientList.begin(); it != clientList.end(); it++)
	for (std::vector<clientData>::iterator it = clientList.begin(); it != clientList.end(); it++)
	{
		//send message
		messageData msOut(ID_SEND_MESSAGE, msg, false, serverName);
		peer->Send(reinterpret_cast<char*>(&msOut), sizeof(msOut), HIGH_PRIORITY, RELIABLE_ORDERED, 0, it->clientAddress, false);

	}
}
