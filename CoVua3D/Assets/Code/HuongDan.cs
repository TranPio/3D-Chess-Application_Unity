using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class HuongDan : MonoBehaviour
{
    public GameObject TaskBar, MiniTaskBar;
    public GameObject VuaPanel, HauPanel, XePanel, MaPanel, TuongPanel, TotPanel;
    public Text VuaText, HauText, XeText, MaText, TuongText, TotText;
    public VideoPlayer Vua, Hau, Xe, Ma, Tuong, Tot;
    public VideoPlayer Vua1, Hau1, Xe1, Ma1, Tuong1, Tot1;
    public RawImage VuaDisplay, HauDisplay, XeDisplay, MaDisplay, TuongDisplay, TotDisplay;
    private VideoPlayer currentVideoPlayer = null;

    void Start()
    {
        // Gán sự kiện click cho button
        TaskBar.SetActive(true);
        PrepareVideoPlayer(Vua);
        PrepareVideoPlayer(Vua1);
        PrepareVideoPlayer(Hau);
        PrepareVideoPlayer(Hau1);
        PrepareVideoPlayer(Xe);
        PrepareVideoPlayer(Xe1);
        PrepareVideoPlayer(Ma);
        PrepareVideoPlayer(Ma1);
        PrepareVideoPlayer(Tuong);
        PrepareVideoPlayer(Tuong1);
        PrepareVideoPlayer(Tot);
        PrepareVideoPlayer(Tot1);
    }

    private void PrepareVideoPlayer(VideoPlayer videoPlayer)
    {
        videoPlayer.Prepare();
        videoPlayer.loopPointReached += CheckOver;
    }

    public void OpenTaskBar()
    {
        TaskBar.SetActive(true);
        MiniTaskBar.SetActive(false);
    }

    public void CloseTaskBarButton()
    {
        TaskBar.SetActive(false);
        MiniTaskBar.SetActive(true);
    }

    public void VuaButton()
    {
        VuaText.text = "Vua là quân cờ quan trọng nhất, nhưng lại là quân cờ yếu nhất. Vua chỉ có thể di chuyển một ô ở mọi hướng - lên, xuống, qua, lại và đi chéo. Vua không thể đi vào vị trí bị chiếu được (nơi mà vua sẽ bị bắt). Khi mà vua bị tấn công bởi các quân cờ khác, nó được gọi là \"chiếu\".";
        SetActivePanel(VuaPanel, Vua, VuaDisplay);
    }
    public void VuaButton2()
    {
        SetActivePanel(VuaPanel, Vua1, VuaDisplay);
    }

    public void HauButton()
    {
        HauText.text = "Quân hậu là quân cờ quyền lực nhất. Nó có thể di chuyển bất cứ hướng nào - Tiến, lùi, ngang hoặc là chéo - di chuyển xa đến đâu cũng được miễn là nó không đi xuyên qua các quân cờ khác. Và như tất cả mọi quân cờ, nếu quân hậu bắt được một quân của đối phương, thì nước đi đó kết thúc và quân hậu thay thế vị trí quân bắt được. Hãy để ý cách mà quân hậu trắng bắt quân hậu đen và bắt quân vua đen phải di chuyển.";
        SetActivePanel(HauPanel, Hau, HauDisplay);
    }
    public void HauButton2()
    {
        SetActivePanel(HauPanel, Hau1, HauDisplay);
    }

    public void XeButton()
    {
        XeText.text = "Quân xe có thể di chuyển bất cứ hướng nào - Tiến, lùi, ngang hoặc là chéo - di chuyển xa đến đâu cũng được miễn là nó không đi xuyên qua các quân cờ khác. Xe rất mạnh khi chúng bảo về nhau và phối hợp!";
        SetActivePanel(XePanel, Xe, XeDisplay);
    }
    public void XeButton2()
    {
        SetActivePanel(XePanel, Xe1, XeDisplay);
    }
    public void MaButton()
    {
        MaText.text = "Quân mã có thể di chuyển theo hình chữ L. Một bước tiến hoặc lùi, sau đó một bước ngang hoặc dọc. Quân mã là quân cờ duy nhất có thể nhảy qua các quân cờ khác.";
        SetActivePanel(MaPanel, Ma, MaDisplay);
    }
    public void MaButton2()
    {
        SetActivePanel(MaPanel, Ma1, MaDisplay);
    }

    public void TuongButton()
    {
        TuongText.text = "Quân tượng có thể di chuyển xa nhất có thể, nhưng chỉ có thể đi chéo. Mỗi quân tượng bắt đầu ở một ô màu (sáng hay tối) và sẽ luôn luôn đứng ở ô màu đó. Các quân tượng phối hợp tốt với nhau vì chúng bù đắp cho điểm yếu của nhau.";
        SetActivePanel(TuongPanel, Tuong, TuongDisplay);
    }
    public void TuongButton2()
    {
        SetActivePanel(TuongPanel, Tuong1, TuongDisplay);
    }
    public void TotButton()
    {
        TotText.text = "Quân tốt đặc biệt ở chỗ cách nó di chuyển khác cách nó bắt quân: chúng đi thẳng, nhưng bắt quân chéo. Các quân tốt chỉ có thể tiến một bước trong một lượt, trừ lượt đầu tiên chúng có thể tiến hai bước. Tốt chỉ có thể bắt quân địch ở phía chéo trước nó và chúng không thể đi hay bắt quân phía ngược lại. Nếu có một quân cờ đứng trước mặt, quân tốt không thể nào đi tiếp hay ăn quân cờ đó.\r\n\r\n";
        SetActivePanel(TotPanel, Tot, TotDisplay);
    }
    public void TotButton2()
    {
        SetActivePanel(TotPanel, Tot1, TotDisplay);
    }
    private void SetActivePanel(GameObject activePanel, VideoPlayer videoPlayer, RawImage rawImage)
    {
        GameObject[] panels = { VuaPanel, HauPanel, XePanel, MaPanel, TuongPanel, TotPanel };
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == activePanel);
        }

        // Dừng video hiện tại nếu có
        if (currentVideoPlayer != null && currentVideoPlayer.isPlaying)
        {
            currentVideoPlayer.Pause();
        }

        // Cập nhật video mới
        currentVideoPlayer = videoPlayer;
        rawImage.texture = videoPlayer.targetTexture;
        currentVideoPlayer.Play();
    }

    void CheckOver(VideoPlayer vp)
    {
        vp.Play();
    }

    public void CloseHuongDan()
    {
        SceneManager.LoadScene("CoVua3D");
    }
}
