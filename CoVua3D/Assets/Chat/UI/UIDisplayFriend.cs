using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIDisplayFriend : MonoBehaviour
{
    [SerializeField] private Transform friendContainer;
    [SerializeField] private UIFriend uiFriendPrefab;

    private List<FriendInfo> currentFriends = new List<FriendInfo>(); // Danh sách bạn bè hiện tại

    private void OnEnable()
    {
        PhotonFriendController.OnDisplayFriends += HandleDisplayFriends;
        RefreshFriendsList(); // Làm mới danh sách bạn bè khi panel được bật
    }

    private void OnDisable()
    {
        PhotonFriendController.OnDisplayFriends -= HandleDisplayFriends;
    }

    private void HandleDisplayFriends(List<FriendInfo> friends)
    {
        currentFriends = friends; // Cập nhật danh sách bạn bè hiện tại
        RefreshFriendsList(); // Làm mới danh sách bạn bè trên giao diện
    }

    private void RefreshFriendsList()
    {
        // Xóa tất cả các đối tượng con hiện có trong friendContainer
        foreach (Transform child in friendContainer)
        {
            Destroy(child.gameObject);
        }

        // Tạo đối tượng UIFriend mới cho mỗi FriendInfo trong danh sách bạn bè hiện tại
        foreach (FriendInfo friend in currentFriends)
        {
            UIFriend uifriend = Instantiate(uiFriendPrefab, friendContainer);
            uifriend.Initialize(friend);
        }
    }
}
