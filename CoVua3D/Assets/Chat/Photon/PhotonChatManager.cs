using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    #region Setup

    [SerializeField] private GameObject joinChatButton;
    private ChatClient chatClient;
    private bool isConnected;
    [SerializeField] private string username;

    public void UsernameOnValueChange(string valueIn)
    {
        username = valueIn;
    }

    public void ChatConnectOnClick()
    {
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(username));
        Debug.Log("Connecting");
    }

    #endregion Setup

    #region General

    [SerializeField] private GameObject chatPanel;
    private string privateReceiver = "";
    private string currentChat;
    [SerializeField] private TMP_InputField chatField;
    [SerializeField] private TMP_Text chatDisplay;
    public static Action<string, string> OnRoomInvite = delegate { };

    private void Awake()
    {
        UIFriend.OnInviteFriend += HandleFriendInvite;
    }

    private void OnDestroy()
    {
        UIFriend.OnInviteFriend -= HandleFriendInvite;
        DisconnectChat();
    }

    private void HandleFriendInvite(string recipient)
    {
        if (chatClient != null && chatClient.CanChat)
        {
            chatClient.SendPrivateMessage(recipient, PhotonNetwork.CurrentRoom.Name);
        }
        else
        {
            Debug.LogWarning("Chat client is not initialized or not connected.");
        }
    }

    private void Start()
    {
        chatPanel.SetActive(false);
        joinChatButton.SetActive(true);
    }

    private void Update()
    {
        if (isConnected && chatClient != null)
        {
            chatClient.Service();
        }

        if (!string.IsNullOrEmpty(chatField.text) && Input.GetKeyDown(KeyCode.Return))
        {
            if (string.IsNullOrEmpty(privateReceiver))
            {
                SubmitPublicChatOnClick();
            }
            else
            {
                SubmitPrivateChatOnClick();
            }
        }
    }

    #endregion General

    #region PublicChat

    public void SubmitPublicChatOnClick()
    {
        if (string.IsNullOrEmpty(chatField.text)) return;

        if (string.IsNullOrEmpty(privateReceiver) && chatClient != null && chatClient.CanChat)
        {
            chatClient.PublishMessage("RegionChannel", chatField.text);
            chatField.text = "";
            currentChat = "";
        }
        else
        {
            Debug.LogWarning("Cannot send public message: Chat client is not initialized or not connected.");
        }
    }

    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }

    #endregion PublicChat

    #region PrivateChat

    public void ReceiverOnValueChange(string valueIn)
    {
        privateReceiver = valueIn;
    }

    public void SubmitPrivateChatOnClick()
    {
        if (string.IsNullOrEmpty(chatField.text)) return;

        if (!string.IsNullOrEmpty(privateReceiver) && chatClient != null && chatClient.CanChat)
        {
            chatClient.SendPrivateMessage(privateReceiver, chatField.text);
            chatField.text = "";
            currentChat = "";
            OnRoomInvite?.Invoke(privateReceiver, currentChat);
        }
        else
        {
            Debug.LogWarning("Cannot send private message: Chat client is not initialized or not connected.");
        }
    }

    #endregion PrivateChat

    #region Callbacks

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(message);
    }

    public void OnChatStateChange(ChatState state)
    {
        if (state == ChatState.Uninitialized)
        {
            isConnected = false;
            joinChatButton.SetActive(true);
            chatPanel.SetActive(false);
        }
    }

    public void OnConnected()
    {
        Debug.Log("Connected");
        joinChatButton.SetActive(false);
        chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnDisconnected()
    {
        isConnected = false;
        joinChatButton.SetActive(true);
        chatPanel.SetActive(false);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (chatDisplay != null)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                string msg = string.Format("{0}: {1}", senders[i], messages[i]);
                chatDisplay.text += "\n" + msg;
                Debug.Log(msg);
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
         if (chatDisplay != null)
        {
            string msg = $"<color=red>(Private) {sender}: {message}</color>";
            chatDisplay.text += "\n " + msg;
            Debug.Log(msg);
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        chatPanel.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels) { }

    public void OnUserSubscribed(string channel, string user) { }

    public void OnUserUnsubscribed(string channel, string user) { }

    #endregion Callbacks

    private void DisconnectChat()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }
}
