using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{
    // Constructor của lớp Knight
    public Knight()
    {
        value = 30; // Thiết lập giá trị của quân Mã là 30
    }

    // Phương thức tính các nước đi có thể của quân Mã
    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8]; // Mảng boolean 2 chiều đại diện cho bàn cờ
        int x = currentX; // Vị trí hiện tại của quân Mã theo trục x
        int y = currentY; // Vị trí hiện tại của quân Mã theo trục y

        // Đi xuống bên trái
        KnightMove(x - 1, y - 2, ref moves);

        // Đi xuống bên phải
        KnightMove(x + 1, y - 2, ref moves);

        // Đi sang phải xuống dưới
        KnightMove(x + 2, y - 1, ref moves);

        // Đi sang phải lên trên
        KnightMove(x + 2, y + 1, ref moves);

        // Đi sang trái xuống dưới
        KnightMove(x - 2, y - 1, ref moves);

        // Đi sang trái lên trên
        KnightMove(x - 2, y + 1, ref moves);

        // Đi lên trái
        KnightMove(x - 1, y + 2, ref moves);

        // Đi lên phải
        KnightMove(x + 1, y + 2, ref moves);

        return moves; // Trả về mảng các nước đi hợp lệ
    }

    // Phương thức hỗ trợ tính và đánh dấu nước đi của quân Mã
    private void KnightMove(int x, int y, ref bool[,] moves)
    {
        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
        {
            Chessman piece = BoardManager.Instance.Chessmans[x, y]; // Lấy quân cờ tại vị trí (x, y)
            // Nếu ô cờ trống
            if (piece == null)
            {
                if (!this.KingInDanger(x, y)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y] = true; // Đánh dấu nước đi hợp lệ
            }
            // Nếu ô cờ có quân của đối thủ
            else if (piece.isWhite != isWhite)
            {
                if (!this.KingInDanger(x, y)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y] = true; // Đánh dấu nước đi hợp lệ
            }
            // Nếu ô cờ có quân của mình, không làm gì
        }
    }
}
