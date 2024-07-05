using UnityEngine;
using System;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;

public class PhotonConnector : MonoBehaviourPunCallbacks
{
    public static Action GetPhotonFriends = delegate {};
    #region Unity Method
    
    private void Start() 
    {
        string randomName = $"Tester{Guid.NewGuid().ToString()}";
        ConnectToPhoton(randomName);
    }

    #endregion
    #region Private Methods
    private void ConnectToPhoton(string nickName)
    {
         Debug.Log($"Connecting to Photon as {nickName}");
         PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues(nickName);
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    private void CreatePhotonRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2; 
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }
    #endregion  
    #region Public Methods
    #endregion
    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("You have connected to the Photon Master Server");
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("You have connected to a Photon Lobby");
        GetPhotonFriends?.Invoke();
    }
    public override void OnCreatedRoom()
    {
        Debug.Log($"You have created a Photon Room named {PhotonNetwork.CurrentRoom.Name}");

    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"You have Joined the Photon Room {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnLeftRoom()
    {
        Debug.Log($"You have left the Photon Room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"You failed to join a Photon Room: {message}");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Another player has joined the room{newPlayer.UserId}");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player has left the room {otherPlayer.UserId}");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"New Master Client is {newMasterClient.UserId}");
    }
    #endregion
}
