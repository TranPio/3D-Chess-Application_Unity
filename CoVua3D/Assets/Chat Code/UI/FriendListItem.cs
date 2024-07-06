using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    //[SerializeField] private Text friendIdText;

    public void SetFriendInfo(string displayName, string id)
    {
        friendNameText.text = displayName;
        //friendIdText.text = id;
    }
}
