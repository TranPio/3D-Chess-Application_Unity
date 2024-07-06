using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Bishop : Chessman
{
    // Constructor của lớp Bishop
    public Bishop()
    {
        value = 30; // Thiết lập giá trị của quân Tượng là 30
    }

    // Phương thức tính các nước đi có thể của quân Tượng
    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8]; // Mảng boolean 2 chiều đại diện cho bàn cờ
        int x = currentX; // Vị trí hiện tại của quân Tượng theo trục x
        int y = currentY; // Vị trí hiện tại của quân Tượng theo trục y

        // Đường chéo từ trái sang phải xuống dưới
        while (x++ < 7 && y-- > 0)
        {
            if (!BishopMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        x = currentX; // Đặt lại vị trí ban đầu theo trục x
        y = currentY; // Đặt lại vị trí ban đầu theo trục y
        // Đường chéo từ trái sang phải lên trên
        while (x++ < 7 && y++ < 7)
        {
            if (!BishopMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        x = currentX; // Đặt lại vị trí ban đầu theo trục x
        y = currentY; // Đặt lại vị trí ban đầu theo trục y
        // Đường chéo từ phải sang trái xuống dưới
        while (x-- > 0 && y-- > 0)
        {
            if (!BishopMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        x = currentX; // Đặt lại vị trí ban đầu theo trục x
        y = currentY; // Đặt lại vị trí ban đầu theo trục y
        // Đường chéo từ phải sang trái lên trên
        while (x-- > 0 && y++ < 7)
        {
            if (!BishopMove(x, y, ref moves)) // Kiểm tra và đánh dấu nước đi
                break; // Dừng vòng lặp nếu gặp quân cờ hoặc ra ngoài bàn cờ
        }

        return moves; // Trả về mảng các nước đi hợp lệ
    }

    // Phương thức hỗ trợ kiểm tra và đánh dấu nước đi
    private bool BishopMove(int x, int y, ref bool[,] moves)
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
