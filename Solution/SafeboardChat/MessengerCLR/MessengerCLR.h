// MessengerCLR.h

#pragma once
#using <System.Core.dll>

using namespace System;
namespace MessengerCLR {


	typedef String ^UserIdCLR;
	typedef String ^MessageIdCLR;
	typedef Generics::List<Byte> ^ByteListCLR;
	typedef ByteListCLR DataCLR;
	typedef ByteListCLR SecPublicKeyCLR;
	int SecPublicKeyCLRLength;
	public ref class SecurityPolicyCLR {
	public:
		int encryptionAlgo;
		SecPublicKeyCLR encryptionPubKey = gcnew System::Collections::Generic::List<Byte>();
		SecurityPolicyCLR(int, const SecPublicKeyCLR%);
		SecurityPolicyCLR()
		{
			encryptionPubKey = gcnew System::Collections::Generic::List<Byte>();
		};
		SecurityPolicyCLR(const SecurityPolicyCLR%);
	internal:
		static SecurityPolicy ToNative(SecurityPolicyCLR^);
		static SecurityPolicyCLR^ FromNative(SecurityPolicy);
	};

	public ref class UserCLR {
	public:
		UserIdCLR identifier;
		SecurityPolicyCLR^ securityPolicy;
	internal:
		static User ToNative(UserCLR^);
		static UserCLR^ FromNative(User);
	};

	typedef Generics::List<UserCLR^> ^UserListCLR;

	public ref class MessengerSettingsCLR {
	public:
		String^ serverURL = "";
		UInt16 serverPort = 0;
		MessengerSettingsCLR() : serverURL(""), serverPort(0) {};
		MessengerSettingsCLR(String^ a_serverURL, UInt16 a_serverPort) :serverURL(a_serverURL), serverPort(a_serverPort) {};
		MessengerSettingsCLR(const MessengerSettingsCLR%);
	internal:
		static MessengerSettings ToNative(MessengerSettingsCLR^);
		static MessengerSettingsCLR^ FromNative(MessengerSettings);
	};

	public ref class MessageContentCLR {
	public:
		int type = 0;
		bool encrypted = false;
		DataCLR data = gcnew System::Collections::Generic::List<Byte>();
		MessageContentCLR() {};
		MessageContentCLR(const MessageContentCLR%);

	internal:
		static MessageContent ToNative(MessageContentCLR^);
		static MessageContentCLR^ FromNative(MessageContent);
	};

	public ref class MessageCLR {
	public:
		MessageIdCLR identifier;
		Int64 time = 0;
		MessageContentCLR^ content;
		MessageCLR() :time(0), identifier("") {};
		MessageCLR(const MessageCLR%);
	internal:
		static Message ToNative(MessageCLR^);
		static MessageCLR^ FromNative(Message);
	};

	namespace Enums {
		public enum class operation_result
		{
			Ok,
			AuthError,
			NetworkError,
			InternalError
		};

		public enum class message_status
		{
			Sending,
			Sent,
			FailedToSend,
			Delivered,
			Seen
		};

		public enum class message_content_type
		{
			Text,
			Image,
			Video
		};

		public enum class encryption_algorithm
		{
			None,
			RSA_1024
		};
	}

	namespace Callbacks {
		typedef Action<Enums::operation_result> LoginCallbackHandler;
		typedef Action<Enums::operation_result, UserListCLR> RequestUsersCallbackHandler;

		namespace Native {
			public class LoginCallback : public ILoginCallback {
			public:
				LoginCallback(LoginCallbackHandler^);
				void OnOperationResult(operation_result::Type result) override;
			};

			public class RequestUsersCallback :public IRequestUsersCallback {
			public:
				RequestUsersCallback(RequestUsersCallbackHandler^);
				void OnOperationResult(operation_result::Type result, const UserList& userList) override;
			};
		}

		namespace CLR {
			public ref class LoginCallbackRegistrator {
			public:
				// todo: no second argument, make getter in LoginCallback
				static void Reg(Native::LoginCallback*, LoginCallbackHandler^);
				static void Trigger(Native::LoginCallback*, int);
				static void Cancel(Native::LoginCallback* loginCallback);
			private:				
				static Generics::Dictionary<size_t, LoginCallbackHandler^> dictionary;
			};

			public ref class RequestUsersCallbackRegistrator {
			public:
				static void Reg(Native::RequestUsersCallback*, RequestUsersCallbackHandler^);
				static void Trigger(Native::RequestUsersCallback*, int, UserListCLR);
				static void Cancel(Native::RequestUsersCallback*);
			//private:
				static Generics::Dictionary<size_t, RequestUsersCallbackHandler^> dictionary;
			};
		}
	}

	namespace Observers {
		typedef Action<UserIdCLR, ::MessengerCLR::MessageCLR^> MessagesObserverHandler_Received;
		typedef Action<MessageIdCLR, Enums::message_status> MessagesObserverHandler_StatusChanged;

		namespace Native {
			public class MessagesObserver :public IMessagesObserver {
			public:
				MessagesObserver(MessagesObserverHandler_Received^, MessagesObserverHandler_StatusChanged^);
				void OnMessageStatusChanged(const MessageId& msgId, message_status::Type status) override;
				void OnMessageReceived(const UserId& senderId, const Message& msg) override;
			};
		}

		namespace CLR {
			public ref class MessagesObserverRegistrator {
			public:
				static void Reg(
					Native::MessagesObserver*, 
					MessagesObserverHandler_Received^, 
					MessagesObserverHandler_StatusChanged^);
				static void Trigger_Received(Native::MessagesObserver*,
					UserIdCLR, MessageCLR^);
				static void Trigger_StatusChanged(Native::MessagesObserver*,
					MessageIdCLR,int);
				
				static void Cancel(Native::MessagesObserver*);
			private:
				static Generics::Dictionary<size_t, Tuple<MessagesObserverHandler_Received^,MessagesObserverHandler_StatusChanged^>^> dictionary;
			};
		}


	}



	public ref class Messenger
	{

	public:
		Messenger(MessengerSettingsCLR^);
		void Login(UserIdCLR, String^, SecurityPolicyCLR^, Callbacks::LoginCallbackHandler^);
		void Disconnect();
		void RequestActiveUsers(Callbacks::RequestUsersCallbackHandler^);
		MessageCLR^ SendMessage(UserIdCLR, MessageContentCLR^);
		void SendMessageSeen(UserIdCLR, MessageIdCLR);
		void RegisterObserver(Observers::MessagesObserverHandler_Received^, Observers::MessagesObserverHandler_StatusChanged^);
		//void UnregisterObserver(IMessagesObserverCLR^);

	private:
		std::shared_ptr<IMessenger>* messengerImpl;
	};



}
