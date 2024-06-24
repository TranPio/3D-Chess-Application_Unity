using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIDisplayFriend : MonoBehaviour
{
    [SerializeField] private Transform friendContainer;
    [SerializeField] private UIFriend uiFriendPrefabs;
    private void Awake() {
        PhotonFriendController.OnDisplayFriends += HandleDisplayFriends;
    }
    private void OnDestroy() {
        PhotonFriendController.OnDisplayFriends -= HandleDisplayFriends;
    }

    private void HandleDisplayFriends(List<FriendInfo> friends)
    {
        foreach (Transform child in friendContainer)
        {
            Destroy(child);
        }
        foreach (FriendInfo friend in friends)
        {
            UIFriend uifriend = Instantiate(uiFriendPrefabs, friendContainer);
            uifriend.Initialize(friend);
        }
    }
}
