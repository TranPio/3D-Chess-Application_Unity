using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGameNguoi : MonoBehaviour
{
        public static OpenGameNguoi Instance;

        public void OpenGameUser()
        {
            SceneManager.LoadSceneAsync("GamevsUser");
            //, LoadSceneMode.Additive
        }
        public void UnloadScene(string sceneName)
        {
            // Unload scene
            SceneManager.UnloadSceneAsync(sceneName);
        }
    public void CloseGameUsers()
    {
        SceneManager.LoadScene("CoVua3D");
    }
}

