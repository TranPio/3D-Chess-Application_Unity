using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using PlayFab.DataModels;
using PlayFab.ProfilesModels;
using System.Collections.Generic;
using PlayFab.Json;
using System.Collections;
using System.Text.RegularExpressions;
using Photon.Realtime;
using PlayFab.PfEditor.Json;

public class PlayFabController : MonoBehaviour
{
    public static PlayFabController PFC;

    private string userEmail;
    private string userPassword;
    private string username;

    public GameObject loginPanel;
    public GameObject addLoginPanel;
    public GameObject recoverButton;
    public GameObject rankPanel;
    public GameObject menuPanel;
    public GameObject friendMenu;
    public GameObject friendPanel;
    public GameObject chatRoomPanel;
    public GameObject invitePanel;
    public GameObject leaderboardPanel;
    public Transform listingContainer;
    public GameObject listingPrefab;

    private void OnEnable()
    {
        if (PlayFabController.PFC == null)
        {
            PlayFabController.PFC = this;
        }
        else
        {
            if (PlayFabController.PFC != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
        loginPanel.SetActive(true);
    }

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "5340D"; // Replace with your actual TitleId
        }
 // Xóa hoặc vô hiệu hóa đoạn mã đăng nhập tự động dưới đây
        // if (PlayerPrefs.HasKey("EMAIL"))
        // {
        //     userEmail = PlayerPrefs.GetString("EMAIL");
        //     userPassword = PlayerPrefs.GetString("PASSWORD");
        //     var request = new LoginWithEmailAddressRequest
        //     {
        //         Email = userEmail,
        //         Password = userPassword
        //     };
        //     PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        // }
        // else
        // {
        // #if UNITY_ANDROID
        //     var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        //     PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
        // #endif
        // }
    }

    #region Login

    private void OnLoginAndroidSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        GetStats();
        loginPanel.SetActive(true);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        loginPanel.SetActive(true);
        recoverButton.SetActive(true);
        GetStats();
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = username }, OnDisplayName, OnLoginAndroidFailure);
        GetStats();
        loginPanel.SetActive(false);
    }

    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + " is your new display name");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnLoginAndroidFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EmailAddressNotAvailable)
        {
            Debug.LogError("The email address is already in use. Please use a different email address or try logging in.");
        }
        else
        {
            Debug.LogError(error.GenerateErrorReport());
        }
    }

    public void GetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }

    public void GetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }

    public void GetUsername(string usernameIn)
    {
        username = usernameIn;
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
        {
            Debug.LogError("Email and password are required for login.");
            return;
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = userEmail,
            //Password = userPassword
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(username))
        {
            Debug.LogError("Email, password, and username are required.");
            return;
        }

        if (!IsEmailValid(userEmail))
        {
            Debug.LogError("Invalid email address.");
            return;
        }

        if (!IsUsernameValid(username))
        {
            Debug.LogError("Invalid username. Username can only contain letters, numbers, and underscores.");
            return;
        }

        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = userEmail,
            Password = userPassword,
            Username = username
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    private bool IsEmailValid(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    private bool IsUsernameValid(string username)
    {
        string usernamePattern = @"^[a-zA-Z0-9_]+$"; // Only letters, numbers, and underscores
        return Regex.IsMatch(username, usernamePattern);
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    public void OpenAddLogin()
    {
        addLoginPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void OpenFriendMenu()
    {
        friendMenu.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void OpenMenuRank()
    {
        rankPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void OpenFriendList()
    {
        friendPanel.SetActive(true);
        friendMenu.SetActive(false);
        loginPanel.SetActive(false);
    }

    public void OpenChatRoom()
    {
        friendMenu.SetActive(false);
        loginPanel.SetActive(false);
    }

    public void OpenRank()
    {
        rankPanel.SetActive(false);
        loginPanel.SetActive(false);
    }

    public void CloseMenuRank()
    {
        rankPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void CloseRank()
    {
        leaderboardPanel.SetActive(false);
        rankPanel.SetActive(true);
    }

    public void CloseFriendMenu()
    {
        menuPanel.SetActive(true);
        friendMenu.SetActive(false);
    }

    public void CloseFriendList()
    {
        friendPanel.SetActive(false);
        friendMenu.SetActive(true);
    }

    public void CloseChat()
    {
        chatRoomPanel.SetActive(false);
        friendMenu.SetActive(true);
    }

    public void OnClickAddLogin()
    {
        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(username))
        {
            Debug.LogError("Email, password, and username are required.");
            return;
        }

        if (!IsUsernameValid(username))
        {
            Debug.LogError("Username contains invalid characters.");
            return;
        }

        var addLoginRequest = new AddUsernamePasswordRequest
        {
            Email = userEmail,
            Password = userPassword,
            Username = username
        };
        PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, OnAddLoginFailure);
    }

    private void OnAddLoginSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        GetStats();
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        addLoginPanel.SetActive(false);
    }

    private void OnAddLoginFailure(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.AccountAlreadyLinked)
        {
            Debug.LogError("Account already linked to a different account.");
        }
        else
        {
            Debug.LogError(error.GenerateErrorReport());
        }
    }

    #endregion Login

    #region PlayerStats

    public int playerHighScore;

     public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "PlayerHighScore", Value = playerHighScore },
                
            }
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
    void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch(eachStat.StatisticName)
            {
                case "PlayerHighScore":
                    playerHighScore = eachStat.Value;
                    break;
            }
        }
    }
    // Build the request object and access the API
    public void StartCloudUpdatePlayerStats()
{
    PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
    {
        FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
        FunctionParameter = new { highScore = playerHighScore }, // The parameter provided to your function
        GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
    }, OnCloudUpdateStats, OnErrorShared);
}

private static void OnCloudUpdateStats(ExecuteCloudScriptResult result)
{
    // Check if result.FunctionResult is not null
    if (result.FunctionResult != null)
    {
        // Attempt to cast FunctionResult to JsonObject
        if (result.FunctionResult is PlayFab.Json.JsonObject jsonResult)
        {
            // Check if jsonResult contains "messageValue"
            if (jsonResult.TryGetValue("messageValue", out object messageValue))
            {
                Debug.Log((string)messageValue);
            }
            else
            {
                Debug.LogWarning("messageValue not found in FunctionResult.");
            }
        }
        else
        {
            Debug.LogWarning("FunctionResult is not a JsonObject.");
        }
    }
    else
    {
        Debug.LogWarning("FunctionResult is null.");
    }
}

private static void OnErrorShared(PlayFabError error)
{
    Debug.LogError(error.GenerateErrorReport());
}

    #endregion PlayerStats

    #region Leaderboard

    public void GetLeaderboarder()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "PlayerHighScore", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeadboard, OnErrorLeaderboard);
    }
    void OnGetLeadboard(GetLeaderboardResult result)
    {
        leaderboardPanel.SetActive(true);
        //Debug.Log(result.Leaderboard[0].StatValue);
        foreach(PlayerLeaderboardEntry player in result.Leaderboard)
        {
              GameObject tempListing = Instantiate(listingPrefab, listingContainer);
        LearderboardListing LL = tempListing.GetComponent<LearderboardListing>();
        LL.playerNameText.text = player.DisplayName;
        LL.playerScoreText.text = player.StatValue.ToString();
        LL.playerRank.text = (player.Position + 1).ToString(); // Cộng thêm 1 để thứ hạng bắt đầu từ 1 thay vì 0
        Debug.Log(player.DisplayName + ": " + player.StatValue + ", Rank: " + (player.Position + 1));
        }
    }
    public void CloseLeaderboardPanel()
    {
        leaderboardPanel.SetActive(false);
        for(int i = listingContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }
        rankPanel.SetActive(true);
        loginPanel.SetActive(false);
    }
    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion Leaderboard

    #region Friends

    public GameObject friendListingPrefab;
    public Transform friendListingContainer;

    public void AddFriendButton()
    {
        var friendEmail = "friend@example.com"; // replace with actual friend email
        var request = new AddFriendRequest { FriendEmail = friendEmail };
        PlayFabClientAPI.AddFriend(request, OnAddFriend, OnFriendError);
    }

    void OnAddFriend(AddFriendResult result)
    {
        Debug.Log("Friend added successfully!");
        GetFriends();
    }

    void OnFriendError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void GetFriends()
    {
        var request = new GetFriendsListRequest();
        PlayFabClientAPI.GetFriendsList(request, OnGetFriendsList, OnFriendError);
    }

    void OnGetFriendsList(GetFriendsListResult result)
    {
        foreach (var friendInfo in result.Friends)
        {
            GameObject newGo = Instantiate(friendListingPrefab, friendListingContainer);
            ListingPrefab flp = newGo.GetComponent<ListingPrefab>();
            flp.playerNameText.text = friendInfo.FriendPlayFabId; // or another property
        }
    }

    public void SendInvite(string friendPlayFabId)
    {
        var request = new AddSharedGroupMembersRequest
        {
            SharedGroupId = "your_shared_group_id", // replace with your actual group id
            PlayFabIds = new List<string> { friendPlayFabId }
        };
        PlayFabClientAPI.AddSharedGroupMembers(request, OnInviteSent, OnFriendError);
    }

    void OnInviteSent(AddSharedGroupMembersResult result)
    {
        Debug.Log("Invite sent successfully!");
    }

    public IEnumerator WaitForFriend()
    {
        while (true)
        {
            Debug.Log("Checking for friends...");
            yield return new WaitForSeconds(5);
        }
    }

    #endregion Friends

    void DisplayPlayFabError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void DisplayError(string error)
    {
        Debug.LogError(error);
    }
}
