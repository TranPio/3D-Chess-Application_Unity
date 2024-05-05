using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using System.Collections.Generic;
public class UserSearch : MonoBehaviour
{
    public InputField searchInput;
    public Button searchButton;
    public Text searchResultText;
    public Transform searchResultListContent;
    public GameObject searchResultPrefab;
    DatabaseReference reference;
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // Gắn sự kiện cho nút tìm kiếm
        searchButton.onClick.AddListener(SearchUsers);
    }

    // Update is called once per frame
    void SearchUsers()
    {
        string searchTerm = searchInput.text.Trim();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            reference.Child("Users").OrderByChild("username").StartAt(searchTerm).EndAt(searchTerm + "\uf8ff")
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        if (snapshot != null && snapshot.ChildrenCount > 0)
                        {
                            // Xóa danh sách kết quả tìm kiếm trước đó
                            foreach (Transform child in searchResultListContent)
                            {
                                Destroy(child.gameObject);
                            }

                            // Hiển thị kết quả tìm kiếm
                            foreach (var childSnapshot in snapshot.Children)
                            {
                                string username = childSnapshot.Child("username").Value.ToString();
                                GameObject resultItem = Instantiate(searchResultPrefab, searchResultListContent);
                                resultItem.GetComponentInChildren<Text>().text = username;
                            }

                            searchResultText.text = "Đã tìm thấy " + snapshot.ChildrenCount + " kết quả.";
                        }
                        else
                        {
                            searchResultText.text = "Không tìm thấy kết quả nào.";
                        }
                    }
                });
        }
        else
        {
            searchResultText.text = "Vui lòng nhập từ khóa tìm kiếm.";
        }
    }
}
