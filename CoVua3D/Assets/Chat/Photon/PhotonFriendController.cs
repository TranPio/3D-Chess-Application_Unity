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

    private void HandleFriendsUpdate(List<PlayfabFriendInfo> friends)
    {
        if(friends.Count != 0)
        {
            string[] friendDisplayNames = friends.Select(f => f.TitleDisplayName).ToArray();
            PhotonNetwork.FindFriends(friendDisplayNames);
        }
    }
    public override void OnFriendListUpdate(List<PhotonFriendInfo> friendList)
    {
        OnDisplayFriends?.Invoke(friendList);
    }
}
