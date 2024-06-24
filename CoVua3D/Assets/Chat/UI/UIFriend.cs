using System;
using TMPro;
using Photon.Realtime;
using UnityEngine;

public class UIFriend : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    [SerializeField] private FriendInfo friend;

    public static Action<string> OnRemoveFriend = delegate {};

    public void Initialize(FriendInfo friendInfo)
    {
        friend = friendInfo;
        if (friend != null)
        {
            friendNameText.SetText(friend.UserId);
        }
        else
        {
            Debug.LogError("FriendInfo is null. Cannot initialize UIFriend.");
        }
    }

    public void RemoveFriend()
    {
        OnRemoveFriend?.Invoke(friend.UserId);

        // Sau khi gọi Invoke, bạn có thể xóa đối tượng chứa script này bằng cách sử dụng Destroy(gameObject)
        Destroy(gameObject);
    }
}
