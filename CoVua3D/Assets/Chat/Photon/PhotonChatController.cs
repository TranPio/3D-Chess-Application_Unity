using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using System;
using Photon.Chat.Demo;
using ExitGames.Client.Photon;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    [SerializeField] private string nickName;
    private ChatClient chatClient;
#region Unity Methods
    private void Awake() 
    {
        nickName = PlayerPrefs.GetString("USERNAME");
    }
    private void Start()
    {
        chatClient = new ChatClient(this);
        ConnectoToPhotonChat();
    }

    private void Update()
    {
        chatClient.Service();
    }

    #endregion

    #region Private Methods
    private void ConnectoToPhotonChat()
    {
        Debug.Log("Connecting to Photon Chat");
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(nickName);
        ChatAppSettings chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    #endregion

    #region Public Methods
    public void SendDirectMessage(string Recipient, string message)
    {
        chatClient.SendPrivateMessage(Recipient, message);
    }
    #endregion

#region Photon Chat Callbacks
    
    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnDisconnected()
    {
        Debug.Log("You have disconnected from the Photon Chat");
    }

    public void OnConnected()
    {
        Debug.Log("You have connected to the Photon Chat");
        SendDirectMessage("Hoai Phu", "Hi Hoai Phu");
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if(!string.IsNullOrEmpty(message.ToString()))
        {
            //Chanel name format [Sender : Recipient]
            string[] splitNames = channelName.Split(new char[] {':'});
            string senderName = splitNames[0];
            if(!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"{sender}: {message}");
            }
        }
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
    #endregion 
}
