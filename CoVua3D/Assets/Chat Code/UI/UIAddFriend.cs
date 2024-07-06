using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class UIAddFriend : MonoBehaviour
{
    [SerializeField] private string displayName;

    // Action được khai báo static để các đối tượng khác có thể đăng ký nhận sự kiện
    public static Action<string> OnAddFriend = delegate{};

    // Phương thức để thiết lập tên bạn bè cần thêm
    public void SetAddFriendName(string name)
    {
        displayName = name;
    }

    // Phương thức được gọi khi người dùng muốn thêm bạn
    public void AddFriend()
    {
        // Kiểm tra xem displayName có rỗng hoặc null hay không
        if (string.IsNullOrEmpty(displayName)) return;
        
        // Kiểm tra với PlayFab để xem displayName có trùng không
        CheckIfFriendExists(displayName);
    }

    private void CheckIfFriendExists(string displayName)
    {
        // Gọi API của PlayFab để lấy danh sách bạn bè
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(),
            result =>
            {
                bool friendExists = result.Friends.Exists(friend => friend.TitleDisplayName == displayName);

                if (friendExists)
                {
                    Debug.LogWarning("Friend already exists in the list.");
                }
                else
                {
                    // Gọi sự kiện OnAddFriend và truyền displayName làm tham số nếu không trùng
                    OnAddFriend?.Invoke(displayName);
                }
            },
            error =>
            {
                Debug.LogError("Failed to get friends list: " + error.GenerateErrorReport());
            }
        );
    }
}
