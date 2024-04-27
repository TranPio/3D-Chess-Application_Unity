using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using UnityEngine.UIElements;
using TMPro.Examples;

public class FireBase : MonoBehaviour
{
    public GameObject loginpanel, signuppanel, homepanel, profilepanel, forgetpasspanel, TbaoPanel;
    public InputField email, password, username, emailsignup, passwordsignup, forgetpass;
    public Text tbao_Text, tbao_Mess;
    public void OpenLogin()
    {
        loginpanel.SetActive(true);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
    }
    public void OpenSignup()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(true);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
    }
    public void OpenHome()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(true);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
    }
    public void OpenProfile()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(true);
        forgetpasspanel.SetActive(false);
    }
    public void Openforgetpass()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(true);
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
            //Tbao("Lỗi", "Vui lòng điền đầy đủ thông tin");
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
           // Tbao("Lỗi", "Vui lòng điền đầy đủ thông tin");
            return;
        }
        Debug.Log("Đăng ký thành công");
        OpenHome();
    }
    public void ForgetPass()
    {
        if (forgetpass.text == "")
        {
            Debug.Log("Vui lòng điền địa chỉ email");
            //Tbao("Lỗi", "Vui lòng điền địa chỉ email");
            return;
           
        }
        Debug.Log("Kiểm tra email của bạn");
        OpenLogin();
    }
    public void Tbao(string title, string message)
    {
        tbao_Text.text =""+ title;
        tbao_Mess.text = "" + message;
        TbaoPanel.SetActive(true);

    }
}
