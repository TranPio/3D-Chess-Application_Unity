using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class PlayfabFriendController : MonoBehaviour
{
    private UIFriend uiFriend;
    public static Action<List<FriendInfo>> OnFriendListUpdate = delegate {};
    private List<FriendInfo> friends;
    private void Awake() 
    {
        uiFriend = GetComponent<UIFriend>();
        friends = new List<FriendInfo>();
        PhotonConnector.GetPhotonFriends += HandleGetFriends;
        UIAddFriend.OnAddFriend += HandleAddPlayfabFriend;    
        UIFriend.OnRemoveFriend += HandleRemoveFriend;
    }

    private void OnDestroy() 
    {
        PhotonConnector.GetPhotonFriends -= HandleGetFriends;
        UIAddFriend.OnAddFriend -= HandleAddPlayfabFriend;
        UIFriend.OnRemoveFriend -= HandleRemoveFriend;
    }

    private void HandleAddPlayfabFriend(string name)
    {
        var request = new AddFriendRequest{ FriendTitleDisplayName = name};
        PlayFabClientAPI.AddFriend(request, OnFriendAddedSuccess, OnFailure);
    }
     private void HandleRemoveFriend(string name)
    {
        FriendInfo friendToRemove = friends.FirstOrDefault(f => f.TitleDisplayName == name);
    if (friendToRemove != null)
    {
        var request = new RemoveFriendRequest { FriendPlayFabId = friendToRemove.FriendPlayFabId };
        PlayFabClientAPI.RemoveFriend(request, OnFrienRemoveSuccess, OnFailure);
    }
    else
    {
        Debug.LogWarning($"Friend '{name}' not found in the friends list.");
        // Optionally, you can inform the user or handle this case accordingly.
    }
    }
    private void HandleGetFriends()
    {
        GetPlayfabFriends();
    }

    private void GetPlayfabFriends()
    {
        var request = new GetFriendsListRequest { XboxToken = null, IncludeFacebookFriends = false, IncludeSteamFriends = false };
        PlayFabClientAPI.GetFriendsList(request, OnFriendsListSuccess, OnFailure);
    }
   public void GetSteamFriends(List<string> steamIds)
    {
        var request = new GetPlayFabIDsFromSteamIDsRequest { SteamStringIDs = steamIds };
        PlayFabClientAPI.GetPlayFabIDsFromSteamIDs(request, OnSteamFriendsReceived, OnFailure);
    }

    private void OnSteamFriendsReceived(GetPlayFabIDsFromSteamIDsResult result)
    {
        foreach (var steamIdMapping in result.Data)
        {
            Debug.Log($"Steam ID: {steamIdMapping.SteamStringId}, PlayFab ID: {steamIdMapping.PlayFabId}");
            AddFriendByPlayFabID(steamIdMapping.PlayFabId);
        }
    }

    private void AddFriendByPlayFabID(string playFabId)
    {
        var request = new AddFriendRequest { FriendPlayFabId = playFabId };
        PlayFabClientAPI.AddFriend(request, OnFriendAddedSuccess, OnFailure);
    }

    private void OnFriendAddedSuccess(AddFriendResult result)
    {
        Debug.Log("Friend added successfully.");
        GetPlayfabFriends();
    }
     private void OnFriendsListSuccess(GetFriendsListResult result)
    {
        Debug.Log("Successfully retrieved friends list.");
        friends=result.Friends;
        OnFriendListUpdate?.Invoke(result.Friends);
    }
    private void OnFrienRemoveSuccess(RemoveFriendResult result)
    {
        GetPlayfabFriends();
    }
    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"Playfab Friend Error occured {error.GenerateErrorReport()}");
    }

}
