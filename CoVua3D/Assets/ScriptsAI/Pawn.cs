using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman
{
    // Constructor của lớp Pawn
    public Pawn()
    {
        value = 10; // Thiết lập giá trị của quân Tốt là 10
    }

    // Phương thức tính các nước đi có thể của quân Tốt
    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8]; // Mảng boolean 2 chiều đại diện cho bàn cờ
        int x = currentX; // Vị trí hiện tại của quân Tốt theo trục x
        int y = currentY; // Vị trí hiện tại của quân Tốt theo trục y

        Chessman leftChessman = null; // Quân cờ bên trái của quân Tốt
        Chessman rightChessman = null; // Quân cờ bên phải của quân Tốt
        Chessman forwardChessman = null; // Quân cờ phía trước của quân Tốt

        int[] EnPassant = BoardManager.Instance.EnPassant; // Lấy thông tin En Passant từ BoardManager

        if (isWhite) // Nếu là quân cờ màu Trắng
        {
            if (y > 0) // Nếu chưa đi đến đầu bàn cờ
            {
                // Kiểm tra quân cờ bên trái
                if (x > 0) leftChessman = BoardManager.Instance.Chessmans[x - 1, y - 1];
                // Kiểm tra quân cờ bên phải
                if (x < 7) rightChessman = BoardManager.Instance.Chessmans[x + 1, y - 1];
                // Kiểm tra quân cờ phía trước
                forwardChessman = BoardManager.Instance.Chessmans[x, y - 1];
            }
            // Di chuyển về phía trước
            if (forwardChessman == null)
            {
                if (!this.KingInDanger(x, y - 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y - 1] = true; // Đánh dấu nước đi hợp lệ
            }
            // Di chuyển điagonal bên trái
            if (leftChessman != null && !leftChessman.isWhite)
            {
                if (!this.KingInDanger(x - 1, y - 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x - 1, y - 1] = true; // Đánh dấu nước đi hợp lệ
            }
            else if (leftChessman == null && EnPassant[1] == y - 1 && EnPassant[0] == x - 1)
            {
                if (!this.KingInDanger(x - 1, y - 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x - 1, y - 1] = true; // Đánh dấu nước đi hợp lệ
            }
            // Di chuyển điagonal bên phải
            if (rightChessman != null && !rightChessman.isWhite)
            {
                if (!this.KingInDanger(x + 1, y - 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x + 1, y - 1] = true; // Đánh dấu nước đi hợp lệ
            }
            else if (rightChessman == null && EnPassant[1] == y - 1 && EnPassant[0] == x + 1)
            {
                if (!this.KingInDanger(x + 1, y - 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x + 1, y - 1] = true; // Đánh dấu nước đi hợp lệ
            }
            // Di chuyển 2 bước về phía trước trong lần di chuyển đầu tiên
            if (y == 6 && forwardChessman == null && BoardManager.Instance.Chessmans[x, y - 2] == null)
            {
                if (!this.KingInDanger(x, y - 2)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y - 2] = true; // Đánh dấu nước đi hợp lệ
            }
        }
        else // Nếu là quân cờ màu Đen
        {
            if (y < 7) // Nếu chưa đi đến đầu bàn cờ
            {
                // Kiểm tra quân cờ bên trái
                if (x > 0) leftChessman = BoardManager.Instance.Chessmans[x - 1, y + 1];
                // Kiểm tra quân cờ bên phải
                if (x < 7) rightChessman = BoardManager.Instance.Chessmans[x + 1, y + 1];
                // Kiểm tra quân cờ phía trước
                forwardChessman = BoardManager.Instance.Chessmans[x, y + 1];
            }
            // Di chuyển về phía trước
            if (forwardChessman == null)
            {
                if (!this.KingInDanger(x, y + 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y + 1] = true; // Đánh dấu nước đi hợp lệ
            }
            // Di chuyển điagonal bên trái
            if (leftChessman != null && leftChessman.isWhite)
            {
                if (!this.KingInDanger(x - 1, y + 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x - 1, y + 1] = true; // Đánh dấu nước đi hợp lệ
            }
            else if (leftChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x - 1)
            {
                if (!this.KingInDanger(x - 1, y + 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x - 1, y + 1] = true; // Đánh dấu nước đi hợp lệ
            }
            // Di chuyển điagonal bên phải
            if (rightChessman != null && rightChessman.isWhite)
            {
                if (!this.KingInDanger(x + 1, y + 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x + 1, y + 1] = true; // Đánh dấu nước đi hợp lệ
            }
            else if (rightChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x + 1)
            {
                if (!this.KingInDanger(x + 1, y + 1)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x + 1, y + 1] = true; // Đánh dấu nước đi hợp lệ
            }
            // Di chuyển 2 bước về phía trước trong lần di chuyển đầu tiên
            if (y == 1 && forwardChessman == null && BoardManager.Instance.Chessmans[x, y + 2] == null)
            {
                if (!this.KingInDanger(x, y + 2)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y + 2] = true; // Đánh dấu nước đi hợp lệ
            }
        }

        return moves; // Trả về mảng các nước đi hợp lệ
    }
}
