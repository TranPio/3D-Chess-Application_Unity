using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Chessman
{
    // Constructor của lớp Rook
    public Rook()
    {
        value = 50; // Thiết lập giá trị của quân Xe là 50
    }

    // Phương thức tính các nước đi có thể của quân Xe
    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8]; // Mảng boolean 2 chiều đại diện cho bàn cờ
        int x = currentX; // Vị trí hiện tại của quân Xe theo trục x
        int y = currentY; // Vị trí hiện tại của quân Xe theo trục y

        // Di chuyển xuống
        while (y-- > 0)
        {
            if (!RookMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        x = currentX; // Đặt lại vị trí ban đầu theo trục x
        y = currentY; // Đặt lại vị trí ban đầu theo trục y
        // Di chuyển sang phải
        while (x++ < 7)
        {
            if (!RookMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        x = currentX; // Đặt lại vị trí ban đầu theo trục x
        y = currentY; // Đặt lại vị trí ban đầu theo trục y
        // Di chuyển sang trái
        while (x-- > 0)
        {
            if (!RookMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        x = currentX; // Đặt lại vị trí ban đầu theo trục x
        y = currentY; // Đặt lại vị trí ban đầu theo trục y
        // Di chuyển lên trên
        while (y++ < 7)
        {
            if (!RookMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        return moves; // Trả về mảng các nước đi hợp lệ
    }

    // Phương thức hỗ trợ kiểm tra và đánh dấu nước đi
    private bool RookMove(int x, int y, ref bool[,] moves)
    {
        Chessman piece = BoardManager.Instance.Chessmans[x, y]; // Lấy quân cờ tại vị trí (x, y)
        // Nếu ô cờ trống
        if (piece == null)
        {
            if (!this.KingInDanger(x, y)) // Kiểm tra xem vua có bị nguy hiểm không
                moves[x, y] = true; // Đánh dấu nước đi hợp lệ
            return true; // Tiếp tục vòng lặp
        }
        // Nếu ô cờ có quân của đối thủ
        else if (piece.isWhite != isWhite)
        {
            if (!this.KingInDanger(x, y)) // Kiểm tra xem vua có bị nguy hiểm không
                moves[x, y] = true; // Đánh dấu nước đi hợp lệ
        }

        // Nếu ô cờ có quân của mình, không làm gì

        return false; // Dừng vòng lặp
    }
}
