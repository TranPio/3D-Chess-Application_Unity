using System;
using TMPro;
using Photon.Realtime;
using UnityEngine;

public class UIFriend : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    private FriendInfo friend;

    public static Action<string> OnRemoveFriend = delegate {};
    public static Action<string> OnInviteFriend = delegate {};

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
        if (friend != null)
        {
            OnRemoveFriend?.Invoke(friend.UserId);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Cannot remove friend. FriendInfo is null.");
        }
    }  

    public void InviteFriend()
    {
        Debug.Log($"Clicked to invite friend {friend.UserId}");
        OnInviteFriend?.Invoke(friend.UserId);
    }
}
