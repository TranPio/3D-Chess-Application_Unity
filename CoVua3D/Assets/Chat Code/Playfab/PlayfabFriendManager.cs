using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabFriendManager : MonoBehaviour
{
    [SerializeField] private GameObject friendListItemPrefab;
    [SerializeField] private Transform friendListContainer;

    public void GetFriendsList()
    {
        var request = new GetFriendsListRequest { IncludeFacebookFriends = true, IncludeSteamFriends = true };
        PlayFabClientAPI.GetFriendsList(request, OnGetFriendsListSuccess, OnGetFriendsListError);
    }

    private void OnGetFriendsListSuccess(GetFriendsListResult result)
    {
        Debug.Log("Friends list retrieved successfully.");
        foreach (var friend in result.Friends)
        {
            string displayName = string.IsNullOrEmpty(friend.TitleDisplayName) ? friend.Username : friend.TitleDisplayName;
            Debug.Log("Friend: " + displayName);
        }

        PopulateScrollView(result.Friends);
    }

    private void OnGetFriendsListError(PlayFabError error)
    {
        Debug.LogError("Error retrieving friends list: " + error.GenerateErrorReport());
    }

    private void PopulateScrollView(List<FriendInfo> friends)
    {
        // Clear previous list items
        foreach (Transform child in friendListContainer)
        {
            Destroy(child.gameObject);
        }

        // Create a new item for each friend
        foreach (var friend in friends)
        {
            GameObject newFriendItem = Instantiate(friendListItemPrefab, friendListContainer);
            FriendListItem friendListItem = newFriendItem.GetComponent<FriendListItem>();

            if (friendListItem != null)
            {
                string displayName = string.IsNullOrEmpty(friend.TitleDisplayName) ? friend.Username : friend.TitleDisplayName;
                friendListItem.SetFriendInfo(displayName, friend.FriendPlayFabId);
            }
        }
    }
}
