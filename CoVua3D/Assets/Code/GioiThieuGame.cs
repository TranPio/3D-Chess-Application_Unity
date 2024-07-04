using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GioiThieuGame : MonoBehaviour
{
    public GameObject GioiThieu;
    public GameObject TaskBar, HoSo, HuongDan, CaiDat, ChoiVoiMay, ChoiVoiBanbe;
    void Start()
    {
        GioiThieu.SetActive(true);
        TaskBar.SetActive(true);
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
