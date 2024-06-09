using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public bool IsLoggedIn = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm được gọi khi cần quay lại OpenHome
    public void ReturnToOpenHome()
    {
        // Gọi hàm OpenHome trong script FireBase (hoặc bất kỳ script nào cần thiết)
        FireBase firebaseScript = FindObjectOfType<FireBase>();
        if (firebaseScript != null)
        {
            firebaseScript.OpenHome();
        }
    }

    // Hàm này sẽ được gọi khi người chơi đăng nhập hoặc đăng xuất
    public void SetLoggedIn(bool loggedIn)
    {
        IsLoggedIn = loggedIn;
    }
}
