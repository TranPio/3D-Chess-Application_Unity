using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using UnityEngine;

public class PlayfabFriendController : MonoBehaviour
{
    private UIFriend uiFriend;
    public static Action<List<FriendInfo>> OnFriendListUpdate = delegate { };
    private List<FriendInfo> friends;
    private bool isLoggedIn;

    private void Awake() 
    {
        uiFriend = GetComponent<UIFriend>();
        friends = new List<FriendInfo>();
        PhotonConnector.GetPhotonFriends += HandleGetFriends;
        UIAddFriend.OnAddFriend += HandleAddPlayfabFriend;    
        UIFriend.OnRemoveFriend += HandleRemoveFriend;
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        }, OnLoginSuccess, OnFailure);
    }

    private void OnDestroy() 
    {
        PhotonConnector.GetPhotonFriends -= HandleGetFriends;
        UIAddFriend.OnAddFriend -= HandleAddPlayfabFriend;
        UIFriend.OnRemoveFriend -= HandleRemoveFriend;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfully logged in to PlayFab.");
        isLoggedIn = true;
    }

    private void HandleAddPlayfabFriend(string name)
    {
        if (!isLoggedIn)
        {
            Debug.LogError("Cannot add friend. User is not logged in.");
            return;
        }
        var request = new AddFriendRequest { FriendTitleDisplayName = name };
        PlayFabClientAPI.AddFriend(request, OnFriendAddedSuccess, OnFailure);
    }

    private void HandleRemoveFriend(string name)
    {
        if (!isLoggedIn)
        {
            Debug.LogError("Cannot remove friend. User is not logged in.");
            return;
        }
        FriendInfo friendToRemove = friends.FirstOrDefault(f => f.TitleDisplayName == name);
        if (friendToRemove != null)
        {
            var request = new RemoveFriendRequest { FriendPlayFabId = friendToRemove.FriendPlayFabId };
            PlayFabClientAPI.RemoveFriend(request, OnFriendRemoveSuccess, OnFailure);
        }
        else
        {
            Debug.LogWarning($"Friend '{name}' not found in the friends list.");
            // Optionally, you can inform the user or handle this case accordingly.
        }
    }

    private void HandleGetFriends()
    {
        if (!isLoggedIn)
        {
            Debug.LogError("Cannot get friends. User is not logged in.");
            return;
        }
        GetPlayfabFriends();
    }

    private void GetPlayfabFriends()
    {
        var request = new GetFriendsListRequest { XboxToken = null, IncludeFacebookFriends = false, IncludeSteamFriends = false };
        PlayFabClientAPI.GetFriendsList(request, OnFriendsListSuccess, OnFailure);
    }

    public void GetSteamFriends(List<string> steamIds)
    {
        if (!isLoggedIn)
        {
            Debug.LogError("Cannot get Steam friends. User is not logged in.");
            return;
        }
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
        if (!isLoggedIn)
        {
            Debug.LogError("Cannot add friend by PlayFab ID. User is not logged in.");
            return;
        }
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
        friends = result.Friends;
        OnFriendListUpdate?.Invoke(result.Friends);
    }

    private void OnFriendRemoveSuccess(RemoveFriendResult result)
    {
        GetPlayfabFriends();
    }

    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"PlayFab Friend Error occurred: {error.GenerateErrorReport()}");
    }
}
