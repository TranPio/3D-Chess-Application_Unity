using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayProfile : MonoBehaviour
{
    public Text profileGioitinh, profileQuequan, profileNgaysinh;
   

    private DatabaseReference reference;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
       // LoadProfileData(FireBase.userIdNow);
    }
    public async void LoadProfileData()
    {
        string userId=FireBase.userIdNow;
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
                string gioitinh = userDataSnapshot.Child("gioitinh").Value?.ToString()?.Trim() ?? "Unknown";
                string ngaysinh = userDataSnapshot.Child("ngaysinh").Value?.ToString()?.Trim() ?? "Unknown";
                string quequan = userDataSnapshot.Child("quequan").Value?.ToString()?.Trim() ?? "Unknown";

                Debug.Log("KIEMTRA HIEN THI -------------------" + gioitinh + " " + ngaysinh + " " + quequan);

                profileGioitinh.text = gioitinh;
                profileNgaysinh.text = ngaysinh;
                profileQuequan.text = quequan;
                Debug.Log("KIEMTRA HIEN THI GA MAN HINH -------------------" + profileGioitinh.text.Trim() + " " + profileNgaysinh.text.Trim() + " " + profileQuequan.text.Trim());

            }
            else
            {
                Debug.LogError("Thông tin người dùng không tồn tại.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi khi lấy thông tin người dùng: " + e.Message);
        }
    }
   
}
