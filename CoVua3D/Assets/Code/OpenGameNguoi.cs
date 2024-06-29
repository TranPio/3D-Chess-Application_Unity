using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OpenGameNguoi : MonoBehaviour
{
    public void OnPlayGameButtonClicked()
    {
        // Đường dẫn tới file exe của trò chơi
        string gameExePath = "C:\\Users\\Trần Hoài Phú\\source\\repos\\Team14_NT106.O22.ANTT - Copy (3)-chaydc\\Test Build Game\\ChessTeam14.exe";

        // Kiểm tra xem file exe có tồn tại không trước khi chạy
        if (System.IO.File.Exists(gameExePath))
        {
            // Chạy file exe
            Process.Start(gameExePath);
        }
        else
        {
            //Debug.Log("Không thể tìm thấy file exe của trò chơi.");
        }
    }
}
    
