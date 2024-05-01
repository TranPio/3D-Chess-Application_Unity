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
using Google;
using System.Net;



public class FireBase : MonoBehaviour
{
    public GameObject loginpanel, signuppanel, homepanel, profilepanel, forgetpasspanel, TbaoPanel, CloseTbaone, settingLogout, ConfirmAcc;
    public InputField emaillogin, passwordlogin, usernamesignup, emailsignup, passwordsignup, forgetpass;
    public Text tbao_Text, tbao_Mess, profileName, profileEmail, tbaomksignup, tbaomklogin, tbaoemailsignup, tbaoemaillogin, emailconfirm, emailcf2;
    public Toggle rememberMe, hienmklogin, hienmksignup;
    public string GoogleWebAPI = "214552480712-4puo48o8qgurbipl5dton1iq2sq5q4fs.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;
    public Image ProfileAva;
    public string imageUrl;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
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
        ConfirmAcc.SetActive(false);
    }
    public void OpenSignup()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(true);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
        settingLogout.SetActive(false);
        ConfirmAcc.SetActive(false);
    }
    public void OpenSetting()
    {
        settingLogout.SetActive(true);
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
        ConfirmAcc.SetActive(false);
    }
    public void OpenHome()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(true);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
        settingLogout.SetActive(false);
        ConfirmAcc.SetActive(false);
    }
    public void OpenProfile()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(true);
        forgetpasspanel.SetActive(false);
        settingLogout.SetActive(false);
        ConfirmAcc.SetActive(false);
    }
    public void Openforgetpass()
    {
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(true);
        settingLogout.SetActive(false);
        ConfirmAcc.SetActive(false);
    }
    public void OpenConfirmAcc(bool isEmailsent, string emailcf)
    {
        ConfirmAcc.SetActive(true);
        loginpanel.SetActive(false);
        signuppanel.SetActive(false);
        homepanel.SetActive(false);
        profilepanel.SetActive(false);
        forgetpasspanel.SetActive(false);
        settingLogout.SetActive(false);
       if (isEmailsent)
        {
            emailconfirm.text = "" + emailcf;
        }
        else
        {
            Tbao("Lỗi!", "Không gửi được email xác minh tài khoản");
        }

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

    public void SendMailConfirm()
    {
        StartCoroutine(SendEmailVerificationAsync());
    }

    private IEnumerator SendEmailVerificationAsync()
    {
        if (user != null)
        {
            var sendEmailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(() => sendEmailTask.IsCompleted);
            if (sendEmailTask.Exception != null)
            {
                FirebaseException firebaseEx = sendEmailTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.Cancelled:
                        Debug.LogError("SendEmailVerificationAsync was canceled.");
                        Tbao("Lỗi!", "SendEmailVerificationAsync was canceled.");
                        break;
                    case AuthError.InvalidEmail:
                        Debug.LogError("SendEmailVerificationAsync encountered an error: Invalid email.");
                        Tbao("Lỗi!", "SendEmailVerificationAsync encountered an error: Invalid email.");
                        break;
                    case AuthError.TooManyRequests:
                        Debug.LogError("SendEmailVerificationAsync encountered an error: Too many requests.");
                        Tbao("Lỗi!", "SendEmailVerificationAsync encountered an error: Too many requests.");
                        break;
                    case AuthError.InvalidRecipientEmail:
                        Debug.LogError("SendEmailVerificationAsync encountered an error: Invalid recipient email.");
                        Tbao("Lỗi!", "SendEmailVerificationAsync encountered an error: Invalid recipient email.");
                        break;
                }
                OpenConfirmAcc(false, user.Email);
            }
            else
            {
                Debug.Log("Email sent successfully.");
                OpenConfirmAcc(true, user.Email);
            }

        }
    }
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
        //Debug.Log("Kiểm tra email của bạn");
        forgetPasswordsubmit(forgetpass.text);
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
                            Tbao("Lỗi!", "Email đã tồn tại. Vui lòng đăng ký bằng email khác!");
                            break;
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
            if (user.IsEmailVerified)
            {
                UpdateProfile(username);
                OpenLogin();
            }
            else
            {
                SendMailConfirm();
            }
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
            if(user.IsEmailVerified)
            {
                Debug.Log("Đăng nhập thành công");
                Tbao("", "Đăng nhập thành công");
                OpenHome();
            }
            else
            {
                SendMailConfirm();
            }
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
    void forgetPasswordsubmit(string forgetpassemail )
    {
        auth.SendPasswordResetEmailAsync(forgetpassemail).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync đã bị hủy.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
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
            Tbao("Thành công!", "Email đặt lại mật khẩu đã được gửi thành công.");
            Debug.Log("Email đặt lại mật khẩu đã được gửi thành công.");
        });
    }
    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWebAPI,
            RequestIdToken = true
        };
    }
    public void GoogleSigninClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnGoogleAuthenticatedFinished);
    }
    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Fault");
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Login Canceled");
        }
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if(task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                user = auth.CurrentUser;
                profileName.text = "" + user.DisplayName;
                profileEmail.text = "" + user.Email;
                Tbao("Thành công!", "Đăng nhập thành công");
                OpenHome();

                //StartCoroutine(Lo)
                StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
                //S
            });
        }
    }
    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }
        return imageUrl;
    }
    IEnumerator LoadImage(string imageUri)
    {
        WWW www = new WWW(imageUri);
        yield return www;
        ProfileAva.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}

