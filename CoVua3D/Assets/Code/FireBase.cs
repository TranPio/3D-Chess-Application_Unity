using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using TMPro.Examples;
using Firebase;
using Firebase.Auth;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using static UnityEditor.ShaderData;
using System.Net.Mime;
using UnityEditor.VersionControl;



public class FireBase : MonoBehaviour
{
    public GameObject loginpanel, signuppanel, homepanel, profilepanel, forgetpasspanel, TbaoPanel, CloseTbaone, settingLogout;
    public InputField emaillogin, passwordlogin, usernamesignup, emailsignup, passwordsignup, forgetpass;
    public Text tbao_Text, tbao_Mess, profileName, profileEmail, tbaomksignup, tbaomklogin, tbaoemailsignup, tbaoemaillogin;
    public Toggle rememberMe, hienmklogin, hienmksignup;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    bool isSignIn = false;
    public void TogglePasswordVisibility(InputField x, Toggle y)
    {
        x.contentType = y.isOn ? InputField.ContentType.Standard : InputField.ContentType.Password;
        x.ForceLabelUpdate();
    }

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        { 
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(System.String.Format(
                                     "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
        hienmksignup.onValueChanged.AddListener(delegate { TogglePasswordVisibility(passwordsignup, hienmksignup); });
        hienmklogin.onValueChanged.AddListener(delegate { TogglePasswordVisibility(passwordlogin, hienmklogin); });
    }
    public void OpenLogin()
    {
        loginpanel.SetActive(true);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
        settingLogout.SetActive(false);
    }
    public void OpenSignup()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(true);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
        settingLogout.SetActive(false);
    }
    public void OpenSetting()
    {
        settingLogout.SetActive(true);
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
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
        settingLogout.SetActive(false);
    }
    public void OpenProfile()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(true);
        forgetpasspanel.SetActive(false);
        settingLogout.SetActive(false);
    }
    public void Openforgetpass()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(true);
        settingLogout.SetActive(false);
    }
    public void Hienmk()
    {
        
    }

    public void CloseTbaoNe()
    {
        CloseTbaone.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
    private bool IsValidPassword(string password)
    {
        string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,}$";
        return Regex.Match(password, regex).Success;
    }
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        string trimmedEmail = email.Trim(); 
        string regex = @"^[\w!#$%&'*+/=?^`{|}~-]+(?:\.[\w!#$%&'*+/=?^`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[a-zA-Z]{2,}$";
        return Regex.Match(trimmedEmail, regex).Success;
    }
    //private bool KiemtraEmail()
    //{
    //    AuthError.EmailAlreadyInUse:
    //            message = "Email của bạn đã tồn tại";
    //}

    public void Login()
    {
        tbaoemaillogin.text = "";
        tbaomklogin.text = "";
        if (emaillogin.text == "" || passwordlogin.text == "")
        {
            Debug.Log("Vui lòng điền đầy đủ thông tin");
            Tbao("Lỗi!", "Vui lòng điền đầy đủ thông tin");
            return;
        }
        else if (!IsValidEmail(emaillogin.text))
        {
            tbaoemaillogin.text = "Vui lòng điền đúng định dạng email abc@gmail.com";
            return;
        }
        else if (!IsValidPassword(passwordlogin.text))
        {
            tbaomklogin.text = "Mật khẩu phải chứa ít nhất 8 ký tự, 1 chữ hoa, 1 chữ thường và 1 số";
            return;
        }
        SigninUser(emaillogin.text, passwordlogin.text);
        Debug.Log("Đăng nhập thành công");
        Tbao("", "Đăng nhập thành công");
        //OpenHome();
    }
    public void Signup()
    {
        tbaomksignup.text = "";
        tbaoemailsignup.text = "";
        if (emailsignup.text == "" || passwordsignup.text == "" || usernamesignup.text == "")
        {
            Debug.Log("Vui lòng điền đầy đủ thông tin");
            Tbao("Lỗi!", "Vui lòng điền đầy đủ thông tin");
            return;
        }
        else if (!IsValidEmail(emailsignup.text))
           {
            tbaoemailsignup.text = "Vui lòng điền đúng định dạng email abc@gmail.com";
           return;
           }
        else if (!IsValidPassword(passwordsignup.text))
        {
            tbaomksignup.text = "Mật khẩu phải chứa ít nhất 8 ký tự, 1 chữ hoa, 1 chữ thường và 1 số";
            return;
        }
        CreateUser(emailsignup.text, passwordsignup.text, usernamesignup.text);
    }
    public void ForgetPass()
    {
        if (forgetpass.text == "")
        {
            Debug.Log("Vui lòng điền địa chỉ email");
            Tbao("Lỗi!", "Vui lòng điền địa chỉ email");
            return;
           
        }
        Debug.Log("Kiểm tra email của bạn");
        Tbao("", "Vui lòng kiểm tra email của bạn");
        OpenLogin();
    }
    public void Tbao(string title, string message)
    {
        tbao_Text.text =""+ title;
        tbao_Mess.text = "" + message;
        TbaoPanel.SetActive(true);

    }
    public void CloseTbao()
    {
        tbao_Text.text = "" ;
        tbao_Mess.text = "" ;
        TbaoPanel.SetActive(false);

    }
    public void Logout()
    {
        auth.SignOut();
        profileEmail.text = "";
        profileName.text = "";
        OpenLogin();
    }

    void CreateUser(string email, string password, string username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        if (errorCode == AuthError.EmailAlreadyInUse)
                        {
                            Tbao("Lỗi!", GetErrorMessage(errorCode));
                        }
                        else
                        {
                           // Debug.Log("Đăng ký thành công");
                            Tbao("", "Đăng ký thành công");
                        }
                    }
                }
                return;
            }
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            UpdateProfile(username);
            OpenLogin();
        });
        UpdateProfile(username);
       usernamesignup.text = "";
        emailsignup.text = "";
        passwordsignup.text = "";
    }

    public void SigninUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        Tbao("Lỗi!", GetErrorMessage(errorCode));
                    }
                }
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            profileName.text = ""+ result.User.DisplayName;
            profileEmail.text = ""+ result.User.Email;
            OpenHome();
        });
        emaillogin.text = "";
        passwordlogin.text = "";

    }
    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
    void UpdateProfile(string Username)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = Username,
                PhotoUrl = new System.Uri("https://dummyimage.com/200"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }
    bool isSigned = false;
    private void Update()
    {
        if (isSignIn)
        {
            if(!isSigned)
            {
                    isSigned = true;
                    profileName.text = "" + user.DisplayName;
                    profileEmail.text = "" + user.Email;
                
            }
           
        }
    }
    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Tài khoản không tồn tại";
                break;
            case AuthError.MissingPassword:
                message = "Thiếu mật khẩu";
                break;
            case AuthError.WrongPassword:
                message = "Sai mật khẩu";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Email của bạn đã tồn tại";
                break;
            case AuthError.MissingEmail:
                message = "Thiếu email";
                break;
            default:
                message = "Lỗi không hợp lệ";
                break;
        }
        return message;
    }
}

