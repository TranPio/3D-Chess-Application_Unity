using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBase : MonoBehaviour
{
    public GameObject loginpanel, signuppanel, homepanel, profilepanel;
    public InputField email, password, username, emailsignup, passwordsignup;
    public void OpenLogin()
    {
        loginpanel.SetActive(true);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
    }
    public void OpenSignup()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(true);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
    }
    public void OpenHome()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(true);
        profilepanel.SetActive(false);
    }
    public void OpenProfile()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Login()
    {
        if (email.text == "" || password.text == "")
        {
            Debug.Log("Vui lòng điền đầy đủ thông tin");
            return;
        }
        Debug.Log("Đăng nhập thành công");
        OpenHome();
    }
    public void Signup()
    {
        if (emailsignup.text == "" || passwordsignup.text == "" || username.text == "")
        {
            Debug.Log("Vui lòng điền đầy đủ thông tin");
            return;
        }
        Debug.Log("Đăng ký thành công");
        OpenHome();
    }
}
