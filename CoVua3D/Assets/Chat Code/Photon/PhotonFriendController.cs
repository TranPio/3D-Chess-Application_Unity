using Photon.Pun;
using Photon.Realtime;
using PlayfabFriendInfo = PlayFab.ClientModels.FriendInfo;
using PhotonFriendInfo = Photon.Realtime.FriendInfo; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UIElements;

public class PhotonFriendController : MonoBehaviourPunCallbacks
{
    public static Action<List<PhotonFriendInfo>> OnDisplayFriends = delegate {};
    private void Awake() 
    {
        PlayfabFriendController.OnFriendListUpdate += HandleFriendsUpdate;   
    }
    private void OnDestroy() 
    {
        PlayfabFriendController.OnFriendListUpdate -= HandleFriendsUpdate;
    }

     private void Start()
    {
        GetFriendList(); // Lấy danh sách bạn bè khi chương trình bắt đầu
    }

    public void GetFriendList()
    {
        // Giả sử có phương thức này để lấy danh sách bạn bè từ server
        List<FriendInfo> friends = FetchFriendsFromServer();
        TriggerFriendListUpdate(friends);
    }

    private void HandleFriendsUpdate(List<PlayfabFriendInfo> friends)
    {
        if(friends.Count != 0)
        {
            string[] friendDisplayNames = friends.Select(f => f.TitleDisplayName).ToArray();
            PhotonNetwork.FindFriends(friendDisplayNames);
        }
    }

    private List<FriendInfo> FetchFriendsFromServer()
    {
        // Logic để lấy danh sách bạn bè từ server
        // Trả về danh sách bạn bè
        return new List<FriendInfo>();
    }

    public static void TriggerFriendListUpdate(List<FriendInfo> friends)
    {
        OnDisplayFriends?.Invoke(friends);
    }
    public override void OnFriendListUpdate(List<PhotonFriendInfo> friendList)
    {
        OnDisplayFriends?.Invoke(friendList);
    }

    
}
