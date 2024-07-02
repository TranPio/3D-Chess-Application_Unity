using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoSungThongTin : MonoBehaviour
{
    public Text profileName, profileEmail;
    public InputField Quequan;
    public TMP_InputField Ngaysinh;
    public Toggle GtinhNam, GtinhNu;
    private DatabaseReference reference;
    private bool kiemtrabosung = false;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void Bosungthongtinne()
    {
        string userID = FireBase.userIdNow;
        if (string.IsNullOrEmpty(userID))
        {
            Debug.LogError("UserID không hợp lệ.");
            return;
        }

        // Lấy thông tin người dùng từ giao diện
        string quequanValue = Quequan.text;
        string ngaysinhValue = Ngaysinh.text;
        string gioitinhValue = GtinhNam.isOn ? "Nam" : "Nữ";

        // Kiểm tra xem người dùng hiện tại đã đăng nhập chưa
        if (1 == 1)//PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
        {
            // Tạo một đối tượng mới chứa thông tin cần bổ sung
            ThongTinBoSung thongTin = new ThongTinBoSung(quequanValue, ngaysinhValue, gioitinhValue);

            // Chuyển đối tượng thành dạng JSON
            string json = JsonUtility.ToJson(thongTin);

            try
            {
                // Thực hiện cập nhật lên Realtime Database
                await reference.Child("Users").Child(userID).Child("ThongTinBoSung").SetRawJsonValueAsync(json);
                Debug.Log("Cập nhật thông tin thành công");
                kiemtrabosung = true;
            }
            catch (Exception e)
            {
                Debug.LogError("Cập nhật thông tin thất bại: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Người dùng hiện tại không được phép cập nhật thông tin");
            // Hiển thị thông báo hoặc xử lý khác khi người dùng không được phép cập nhật thông tin
        }

        if (kiemtrabosung)
        {
            // Thongbao.Instance.ShowThongbao("Thành công!", "Cập nhật thông tin thành công");
        }
    }
}

[Serializable]
public class ThongTinBoSung
{
    public string quequan;
    public string ngaysinh;
    public string gioitinh;

    public ThongTinBoSung(string _quequan, string _ngaysinh, string _gioitinh)
    {
        quequan = _quequan;
        ngaysinh = _ngaysinh;
        gioitinh = _gioitinh;
    }
}
