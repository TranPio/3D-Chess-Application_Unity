using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public abstract class Chessman : MonoBehaviour
{
    // Thuộc tính lưu tọa độ X hiện tại
    public int currentX { set; get; }
    // Thuộc tính lưu tọa độ Y hiện tại
    public int currentY { set; get; }
    // Thuộc tính kiểm tra quân cờ có phải là quân trắng không
    public bool isWhite;
    // Thuộc tính lưu giá trị của quân cờ
    public int value;
    // Thuộc tính kiểm tra quân cờ đã di chuyển chưa
    public bool isMoved = false;

    // Phương thức sao chép đối tượng Chessman
    public Chessman Clone()
    {
        return (Chessman)this.MemberwiseClone();
    }

    // Phương thức thiết lập vị trí của quân cờ
    public void SetPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }

    // Phương thức xác định các nước đi có thể của quân cờ (phương thức ảo)
    public virtual bool[,] PossibleMoves()
    {
        bool[,] arr = new bool[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                arr[i, j] = false;
            }
        }
        return arr;
    }

    // Phương thức kiểm tra quân cờ có bị đe dọa không
    public bool InDanger()
    {
        Chessman piece = null;

        int x = currentX;
        int y = currentY;

        // Kiểm tra mối đe dọa từ phía dưới
        if (y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y - 1];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là xe hoặc hậu
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Down");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ phía phải
        if (x + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là xe hoặc hậu
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Right");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ phía trái
        if (x - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là xe hoặc hậu
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Left");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ phía trên
        if (y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y + 1];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là xe hoặc hậu
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Up");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ đường chéo trái lên
        if (x + 1 <= 7 && y - 1 >= 0 && isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y - 1];
            // Nếu ô không trống và quân cờ đối phương là tốt
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x + 1 <= 7 && y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y - 1];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7 && y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là tượng hoặc hậu
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen LR Down");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ đường chéo trái xuống
        if (x + 1 <= 7 && y + 1 <= 7 && !isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y + 1];
            // Nếu ô không trống và quân cờ đối phương là tốt
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x + 1 <= 7 && y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y + 1];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7 && y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là tượng hoặc hậu
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen LR Up");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ đường chéo phải xuống
        if (x - 1 >= 0 && y - 1 >= 0 && isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y - 1];
            // Nếu ô không trống và quân cờ đối phương là tốt
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y - 1];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0 && y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là tượng hoặc hậu
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen RL Down");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ đường chéo phải lên
        if (x - 1 >= 0 && y + 1 <= 7 && !isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y + 1];
            // Nếu ô không trống và quân cờ đối phương là tốt
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x - 1 >= 0 && y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y + 1];
            // Nếu ô không trống và quân cờ đối phương là vua
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0 && y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                continue;

            // Nếu quân cờ cùng màu
            else if (piece.isWhite == isWhite)
                break;

            // Nếu quân cờ của đối phương là tượng hoặc hậu
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen RL Up");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // Kiểm tra mối đe dọa từ mã
        // Xuống trái
        if (KnightThreat(x - 1, y - 2))
            return true;

        // Xuống phải
        if (KnightThreat(x + 1, y - 2))
            return true;

        // Phải xuống
        if (KnightThreat(x + 2, y - 1))
            return true;

        // Phải lên
        if (KnightThreat(x + 2, y + 1))
            return true;

        // Trái xuống
        if (KnightThreat(x - 2, y - 1))
            return true;

        // Trái lên
        if (KnightThreat(x - 2, y + 1))
            return true;

        // Lên trái
        if (KnightThreat(x - 1, y + 2))
            return true;

        // Lên phải
        if (KnightThreat(x + 1, y + 2))
            return true;

        return false;
    }

    // Phương thức kiểm tra mối đe dọa từ mã
    public bool KnightThreat(int x, int y)
    {
        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
        {
            Chessman piece = BoardManager.Instance.Chessmans[x, y];
            // Nếu ô trống
            if (piece == null)
                return false;

            // Nếu quân cờ cùng màu
            if (piece.isWhite == isWhite)
                return false;

            // Quân cờ đối phương
            // Nếu quân cờ đối phương là mã
            if (piece.GetType() == typeof(Knight))
            {
                Debug.Log("Threat from Knight");
                return true;        // Có, có mối đe dọa từ mã
            }
        }

        return false;
    }

    // Phương thức kiểm tra vua có bị đe dọa không sau khi di chuyển
    public bool KingInDanger(int x, int y)
    {
        // Phần quan trọng:
        // Chúng ta sắp di chuyển quân cờ trên bàn cờ (không phải trên giao diện) mà chưa nhận được lệnh
        // Để kiểm tra xem nước đi này có đặt vua vào tình trạng nguy hiểm không
        // Sau khi kiểm tra, chúng ta sẽ hoàn tác nước đi đã thực hiện
        // Và mọi thay đổi sẽ chỉ được hoàn tác trong phương thức này
        // Điều này sẽ không ảnh hưởng đến giao diện

        // ------------- Sao lưu bắt đầu -------------
        // Lưu trữ tham chiếu của quân cờ tại vị trí mà chúng ta sắp di chuyển đến
        Chessman tmpChessman = BoardManager.Instance.Chessmans[x, y];
        int tmpCurrentX = currentX;
        int tmpCurrentY = currentY;
        // ------------- Sao lưu kết thúc -------------

        // Rời vị trí, thực hiện di chuyển, cập nhật tọa độ
        BoardManager.Instance.Chessmans[currentX, currentY] = null;
        BoardManager.Instance.Chessmans[x, y] = this;
        this.SetPosition(x, y);

        // Chúng ta sẽ lưu kết quả vào biến result
        bool result = false;
        // Bây giờ kiểm tra xem vua có bị đe dọa không
        if (isWhite)
            result = BoardManager.Instance.WhiteKing.InDanger();
        else
            result = BoardManager.Instance.BlackKing.InDanger();

        // Bây giờ hoàn tác
        this.SetPosition(tmpCurrentX, tmpCurrentY);
        BoardManager.Instance.Chessmans[tmpCurrentX, tmpCurrentY] = this;
        BoardManager.Instance.Chessmans[x, y] = tmpChessman;

        // Trả về kết quả
        return result;
    }
}
