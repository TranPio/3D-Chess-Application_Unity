using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayProfile : MonoBehaviour
{
    public Text profileName, profileEmail;
    public Text profileGioitinh, profileQuequan, profileNgaysinh, profileDiem;
    public static DisplayProfile Instance;
    public string emailNow="";

    private DatabaseReference reference;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        // LoadProfileData(FireBase.userIdNow);
    }

    public async void LoadProfileData()
    {
        string userId = FireBase.userIdNow;
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserID không hợp lệ.");
            return;
        }

        try
        {
            DataSnapshot userDataSnapshot = await reference.Child("Users").Child(userId).Child("ThongTinBoSung").GetValueAsync();

            if (userDataSnapshot.Exists)
            {
                string gioitinh = userDataSnapshot.Child("gioitinh").Value?.ToString()?.Trim() ?? "";
                string ngaysinh = userDataSnapshot.Child("ngaysinh").Value?.ToString()?.Trim() ?? "";
                string quequan = userDataSnapshot.Child("quequan").Value?.ToString()?.Trim() ?? "";

                Debug.Log("KIEMTRA HIEN THI -------------------" + gioitinh + " " + ngaysinh + " " + quequan);

                profileGioitinh.text = gioitinh;
                profileNgaysinh.text = ngaysinh;
                profileQuequan.text = quequan;
                Debug.Log("KIEMTRA HIEN THI GA MAN HINH -------------------" + profileGioitinh.text.Trim() + " " + profileNgaysinh.text.Trim() + " " + profileQuequan.text.Trim());
            }
            else
            {
                Debug.Log("Thông tin bổ sung không tồn tại.");
                // Đặt các trường thông tin bổ sung trống
                profileGioitinh.text = "";
                profileNgaysinh.text = "";
                profileQuequan.text = "";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi khi lấy thông tin bổ sung: " + e.Message);
            // Đặt các trường thông tin bổ sung trống
            profileGioitinh.text = "";
            profileNgaysinh.text = "";
            profileQuequan.text = "";
        }
    }


    public async void LoadProfileData1()
    {
        string userId = FireBase.userIdNow;
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserID không hợp lệ.");
            return;
        }

        try
        {
            DataSnapshot userDataSnapshot = await reference.Child("Users").Child(userId).GetValueAsync();

            if (userDataSnapshot.Exists)
            {
                string name = userDataSnapshot.Child("username").Value?.ToString()?.Trim() ?? "";
                string email = userDataSnapshot.Child("email").Value?.ToString()?.Trim() ?? "";

                profileName.text = name;
                profileEmail.text = email;
                emailNow = email;
            }
            else
            {
                Debug.Log("Thông tin người dùng không tồn tại.");
                // Đặt các trường thông tin người dùng trống
                profileName.text = "";
                profileEmail.text = "";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi khi lấy thông tin người dùng: " + e.Message);
            // Đặt các trường thông tin người dùng trống
            profileName.text = "";
            profileEmail.text = "";
        }
    }
    public async void LoadProfileData2()
    {
        string userId = FireBase.userIdNow;
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("UserID không hợp lệ.");
            return;
        }

        try
        {
            DataSnapshot userDataSnapshot = await reference.Child("score").Child(userId).GetValueAsync();

            if (userDataSnapshot.Exists)
            {
                string diem = userDataSnapshot.Child("sc").Value?.ToString()?.Trim() ?? "";
                profileDiem.text = diem;
            }
            else
            {
                Debug.Log("Thông tin bổ sung không tồn tại.");
                // Đặt các trường thông tin bổ sung trống
                profileDiem.text = "";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi khi lấy thông tin bổ sung: " + e.Message);
            // Đặt các trường thông tin bổ sung trống
           profileDiem.text = "";
        }
    }
}
