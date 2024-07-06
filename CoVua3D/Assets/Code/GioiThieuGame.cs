using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GioiThieuGame : MonoBehaviour
{
    public static GioiThieuGame Instance;
    public GameObject GioiThieu;
    public GameObject TaskBar, HoSo, HuongDan, CaiDat, ChoiVoiMay, ChoiVoiBanbe;

    private static bool gioiThieuDaChay = false;

    void Start()
    {
        if (gioiThieuDaChay)
        {
            GioiThieu.SetActive(false);
            TaskBar.SetActive(false);
        }
        else
        {
            GioiThieu.SetActive(true);
            TaskBar.SetActive(true);
            gioiThieuDaChay = true; // Đánh dấu là đã chạy giới thiệu lần đầu
        }
    }

    public void OpenHoSo()
    {
        TaskBar.SetActive(false);
        HoSo.SetActive(true);
    }

    public void OpenHuongDan()
    {
        HoSo.SetActive(false);
        HuongDan.SetActive(true);
    }

    public void OpenCaiDat()
    {
        HuongDan.SetActive(false);
        CaiDat.SetActive(true);
    }

    public void OpenChoiVoiMay()
    {
        CaiDat.SetActive(false);
        ChoiVoiMay.SetActive(true);
    }

    public void OpenChoiVoiBanbe()
    {
        ChoiVoiMay.SetActive(false);
        ChoiVoiBanbe.SetActive(true);
    }

    public void CloseGioiThieu()
    {
        GioiThieu.SetActive(false);
    }
}
