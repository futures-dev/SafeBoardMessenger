 // This is the main DLL file.

#pragma comment(lib, "libmessenger.lib")
#pragma comment(lib, "expat.lib")
#pragma comment(lib, "libstrophe.lib")
#pragma comment(lib, "advapi32")

#include "stdafx.h"
#include <msclr\marshal_cppstd.h>
#include "MessengerCLR.h"

namespace MessengerCLR
{

	std::string CLRStringToNative(String^ stringCLR)
	{
		char st[50] = { 0 };
		sprintf(st, "%s", stringCLR);
		return std::string(st);
	}


	MessageContentCLR::MessageContentCLR(const MessageContentCLR% other)
	{
		type = other.type;
		encrypted = other.encrypted;
		data = other.data;
	}

	MessageContent MessageContentCLR::ToNative(MessageContentCLR^ messageContentCLR)
	{
		MessageContent messageContent;
		std::vector<unsigned char> vec;
		for (int i = 0; i < messageContentCLR->data->Count; i++)
		{
			vec.push_back(messageContentCLR->data[i]);
		}
		messageContent.data = vec;
		messageContent.encrypted = messageContentCLR->encrypted;
		messageContent.type = (message_content_type::Type)messageContentCLR->type;
		return messageContent;
	}

	MessageContentCLR ^ MessageContentCLR::FromNative(MessageContent messageContent)
	{
		MessageContentCLR^ new_messageContent = gcnew MessageContentCLR();
		new_messageContent->encrypted = messageContent.encrypted;
		new_messageContent->type = messageContent.type;
		for (unsigned int i = 0; i < messageContent.data.size(); i++) {
			new_messageContent->data->Add(messageContent.data[i]);
		}
		return new_messageContent;
	}


	MessageCLR::MessageCLR(const MessageCLR% other) : identifier(other.identifier), time(other.time), content(other.content) { }


	Message MessageCLR::ToNative(MessageCLR^ messageCLR)
	{
		Message message;
		message.content = MessageContentCLR::ToNative(messageCLR->content);
		message.identifier = CLRStringToNative(messageCLR->identifier);
		message.time = messageCLR->time;
		return message;
	}

	MessageCLR ^ MessageCLR::FromNative(Message message)
	{
		MessageCLR^ new_message = gcnew MessageCLR();
		new_message->identifier = gcnew String(message.identifier.c_str());
		new_message->time = message.time;
		new_message->content = MessageContentCLR::FromNative(message.content);
		return new_message;
	}

	Messenger::Messenger(MessengerSettingsCLR^ messengerSettings)
	{
		messengerImpl = new std::shared_ptr<IMessenger>(
			GetMessengerInstance(
				MessengerSettingsCLR::ToNative(messengerSettings)));
	}

	void Messenger::Login(UserIdCLR userId, String^ password, SecurityPolicyCLR^ securityPolicy, Callbacks::LoginCallbackHandler^ LoginCallbackHandler)
	{
		std::string _userId =
			CLRStringToNative(userId);
		std::string _password =
			CLRStringToNative(password);
		SecurityPolicy _securityPolicy =
			SecurityPolicyCLR::ToNative(securityPolicy);
		//(*messengerImpl)->Disconnect();
		(*messengerImpl)->Login(
			_userId,
			_password,
			_securityPolicy,
			new Callbacks::Native::LoginCallback(LoginCallbackHandler));
	}

	void Messenger::Disconnect()
	{
		(*messengerImpl)->Disconnect();
		delete messengerImpl;
	}

	void Messenger::RequestActiveUsers(Callbacks::RequestUsersCallbackHandler^ handler)
	{
		(*messengerImpl)->RequestActiveUsers(
			new Callbacks::Native::RequestUsersCallback(handler));
	}

	MessageCLR^ Messenger::SendMessage(UserIdCLR userId, MessageContentCLR^ messageContent)
	{
		return MessageCLR::FromNative((*messengerImpl)->SendMessage(CLRStringToNative(userId), MessageContent(MessageContentCLR::ToNative(messageContent))));
	}

	void Messenger::SendMessageSeen(UserIdCLR userId, MessageIdCLR messageId)
	{
		(*messengerImpl)->SendMessageSeen(CLRStringToNative(userId), CLRStringToNative(messageId));
	}

	void Messenger::RegisterObserver(Observers::MessagesObserverHandler_Received^ received, Observers::MessagesObserverHandler_StatusChanged^ statusChanged)
	{
		(*messengerImpl)->RegisterObserver(new Observers::Native::MessagesObserver(received, statusChanged));
	}



	MessengerSettingsCLR::MessengerSettingsCLR(const MessengerSettingsCLR% other)
	{
		serverURL = other.serverURL;
		serverPort = other.serverPort;
	}

	MessengerSettings MessengerSettingsCLR::ToNative(MessengerSettingsCLR^ messengerSettingsCLR)
	{
		MessengerSettings messengerSettings;
		messengerSettings.serverPort = messengerSettingsCLR->serverPort;
		messengerSettings.serverUrl = CLRStringToNative(messengerSettingsCLR->serverURL);
		return messengerSettings;
	}

	MessengerSettingsCLR ^ MessengerSettingsCLR::FromNative(MessengerSettings messengerSettings)
	{
		MessengerSettingsCLR^ new_messengerSettings = gcnew MessengerSettingsCLR();
		new_messengerSettings->serverPort = messengerSettings.serverPort;
		new_messengerSettings->serverURL = gcnew String(messengerSettings.serverUrl.c_str());
		return new_messengerSettings;
	}


	SecurityPolicyCLR::SecurityPolicyCLR(const SecurityPolicyCLR %other):SecurityPolicyCLR()
	{
		encryptionAlgo = other.encryptionAlgo;
		encryptionPubKey->AddRange(other.encryptionPubKey);
	}

	SecurityPolicyCLR::SecurityPolicyCLR(int algo, const SecPublicKeyCLR% pubKey):SecurityPolicyCLR() {
		encryptionAlgo = algo;
		encryptionPubKey->AddRange(pubKey);
	}


	SecurityPolicy SecurityPolicyCLR::ToNative(SecurityPolicyCLR^ securityPolicyCLR)
	{
		SecurityPolicy securityPolicy;
		securityPolicy.encryptionAlgo = (messenger::encryption_algorithm::Type)(securityPolicyCLR->encryptionAlgo);
		for (int i = 0; i < securityPolicyCLR->encryptionPubKey->Count; i++)
		{
			securityPolicy.encryptionPubKey.push_back(securityPolicyCLR->encryptionPubKey[i]);
		}
		return securityPolicy;
	}

	SecurityPolicyCLR ^ SecurityPolicyCLR::FromNative(SecurityPolicy securityPolicy)
	{
		SecurityPolicyCLR^ new_securityPolicy = gcnew SecurityPolicyCLR();
		new_securityPolicy->encryptionAlgo = securityPolicy.encryptionAlgo;
		for (unsigned int i = 0; i < securityPolicy.encryptionPubKey.size(); i++) {
			new_securityPolicy->encryptionPubKey->Add(securityPolicy.encryptionPubKey[i]);
		}
		return new_securityPolicy;
	}

	User UserCLR::ToNative(UserCLR^)
	{
		return User();
	}

	UserCLR ^ UserCLR::FromNative(User user)
	{
		UserCLR^ new_userCLR = gcnew UserCLR();
		new_userCLR->identifier = gcnew String(user.identifier.c_str());
		new_userCLR->securityPolicy = SecurityPolicyCLR::FromNative(user.securityPolicy);
		return new_userCLR;
	}


	void Callbacks::CLR::LoginCallbackRegistrator::Reg(Native::LoginCallback *loginCallback, LoginCallbackHandler ^LoginCallbackHandler)
	{
		dictionary.Add((size_t)loginCallback, LoginCallbackHandler);
	}

	void Callbacks::CLR::LoginCallbackRegistrator::Trigger(Native::LoginCallback *callback, int arg1)
	{
		if (dictionary.ContainsKey((size_t)callback))
			dictionary[(size_t)callback]((Enums::operation_result)arg1);
		Cancel(callback);
		delete callback;
	}

	inline void MessengerCLR::Callbacks::CLR::LoginCallbackRegistrator::Cancel(Native::LoginCallback * loginCallback) {
		dictionary.Remove((size_t)loginCallback);
	}

	Callbacks::Native::LoginCallback::LoginCallback(LoginCallbackHandler ^LoginCallbackHandler)
	{
		CLR::LoginCallbackRegistrator::Reg(this, LoginCallbackHandler);
	}

	void Callbacks::Native::LoginCallback::OnOperationResult(operation_result::Type result)
	{
		CLR::LoginCallbackRegistrator::Trigger(this, result);
	}

	MessengerCLR::Callbacks::Native::RequestUsersCallback::RequestUsersCallback(RequestUsersCallbackHandler ^requestUsersCallbackHandler)
	{
		CLR::RequestUsersCallbackRegistrator::Reg(this, requestUsersCallbackHandler);
	}
	void Callbacks::Native::RequestUsersCallback::OnOperationResult(operation_result::Type result, const UserList & userList)
	{
		UserListCLR userListCLR = gcnew Generics::List<UserCLR^>();
		for (unsigned int i = 0; i < userList.size(); i++) {
			userListCLR->Add(UserCLR::FromNative(userList[i]));
		}
		CLR::RequestUsersCallbackRegistrator::Trigger(this, result, userListCLR);
	}
}

void MessengerCLR::Callbacks::CLR::RequestUsersCallbackRegistrator::Reg(Native::RequestUsersCallback *callback, RequestUsersCallbackHandler ^handler)
{
	dictionary.Add((size_t)callback, handler);
}

void MessengerCLR::Callbacks::CLR::RequestUsersCallbackRegistrator::Trigger(Native::RequestUsersCallback *callback, int arg1, UserListCLR arg2)
{
	if (dictionary.ContainsKey((size_t)callback))
		dictionary[(size_t)callback]((Enums::operation_result)arg1,arg2);
	Cancel(callback);
	delete callback;
}

void MessengerCLR::Callbacks::CLR::RequestUsersCallbackRegistrator::Cancel(Native::RequestUsersCallback *callback)
{
	dictionary.Remove((size_t)callback);
}

void MessengerCLR::Observers::CLR::MessagesObserverRegistrator::Reg(Native::MessagesObserver *observer, MessagesObserverHandler_Received ^received, MessagesObserverHandler_StatusChanged ^statusChanged)
{	
	dictionary.Add((size_t)observer, 
		gcnew Tuple<MessagesObserverHandler_Received^, MessagesObserverHandler_StatusChanged^>(
			received, statusChanged));
}

void MessengerCLR::Observers::CLR::MessagesObserverRegistrator::Trigger_Received(Native::MessagesObserver *observer, UserIdCLR arg1, MessageCLR ^arg2)
{
	if (dictionary.ContainsKey((size_t)observer)) 
		dictionary[(size_t)observer]->Item1(arg1, arg2);
}

void MessengerCLR::Observers::CLR::MessagesObserverRegistrator::Trigger_StatusChanged(Native::MessagesObserver *observer, ::MessengerCLR::MessageIdCLR arg3, int arg4) {
	if (dictionary.ContainsKey((size_t)observer))
		dictionary[(size_t)observer]->Item2(arg3, (Enums::message_status) arg4);
}

void MessengerCLR::Observers::CLR::MessagesObserverRegistrator::Cancel(Native::MessagesObserver *observer)
{
	dictionary.Remove((size_t)observer);	
}

MessengerCLR::Observers::Native::MessagesObserver::MessagesObserver(MessagesObserverHandler_Received ^received, MessagesObserverHandler_StatusChanged ^statusChanged)
{
	CLR::MessagesObserverRegistrator::Reg(this,received, statusChanged);
}

void MessengerCLR::Observers::Native::MessagesObserver::OnMessageStatusChanged(const MessageId & msgId, message_status::Type status)
{
	CLR::MessagesObserverRegistrator::Trigger_StatusChanged(this,gcnew String(msgId.c_str()),status);
}

void MessengerCLR::Observers::Native::MessagesObserver::OnMessageReceived(const UserId & senderId, const Message & msg)
{
	CLR::MessagesObserverRegistrator::Trigger_Received(this, gcnew String(senderId.c_str()), MessageCLR::FromNative(msg));
}
