using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{
    // Constructor của lớp King
    public King()
    {
        value = 900; // Thiết lập giá trị của quân Vua là 900
    }

    // Phương thức tính các nước đi có thể của quân Vua
    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8]; // Mảng boolean 2 chiều đại diện cho bàn cờ
        int x = currentX; // Vị trí hiện tại của quân Vua theo trục x
        int y = currentY; // Vị trí hiện tại của quân Vua theo trục y

        // Xuống dưới
        KingMove(x, y - 1, ref moves);

        // Trái
        KingMove(x - 1, y, ref moves);

        // Phải
        KingMove(x + 1, y, ref moves);

        // Lên trên
        KingMove(x, y + 1, ref moves);

        // Đường chéo phía dưới bên trái
        KingMove(x - 1, y - 1, ref moves);

        // Đường chéo phía dưới bên phải
        KingMove(x + 1, y - 1, ref moves);

        // Đường chéo phía trên bên trái
        KingMove(x - 1, y + 1, ref moves);

        // Đường chéo phía trên bên phải
        KingMove(x + 1, y + 1, ref moves);

        // Nếu Vua chưa được di chuyển thì kiểm tra nước đi Castling
        if (!isMoved)
        {
            if (isWhite)
            {
                CheckCastlingMoves(BoardManager.Instance.WhiteRook1, BoardManager.Instance.WhiteRook2, ref moves);
            }
            else
            {
                CheckCastlingMoves(BoardManager.Instance.BlackRook1, BoardManager.Instance.BlackRook2, ref moves);
            }
        }

        return moves; // Trả về mảng các nước đi hợp lệ
    }

    // Phương thức hỗ trợ tính và đánh dấu nước đi của quân Vua
    private void KingMove(int x, int y, ref bool[,] moves)
    {
        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
        {
            Chessman piece = BoardManager.Instance.Chessmans[x, y]; // Lấy quân cờ tại vị trí (x, y)
            // Nếu ô cờ trống
            if (piece == null)
            {
                if (!KingInDanger(x, y)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y] = true; // Đánh dấu nước đi hợp lệ
            }
            // Nếu ô cờ có quân của đối thủ
            else if (piece.isWhite != isWhite)
            {
                if (!KingInDanger(x, y)) // Kiểm tra xem Vua có bị nguy hiểm không
                    moves[x, y] = true; // Đánh dấu nước đi hợp lệ
            }
            // Nếu ô cờ có quân của mình, không làm gì
        }
    }

    // Phương thức kiểm tra và thực hiện nước đi Castling
    private void CheckCastlingMoves(Chessman Rook1, Chessman Rook2, ref bool[,] moves)
    {
        int x = currentX; // Vị trí hiện tại của quân Vua theo trục x
        int y = currentY; // Vị trí hiện tại của quân Vua theo trục y
        Chessman[,] Chessmans = BoardManager.Instance.Chessmans; // Lấy bàn cờ từ BoardManager
        bool conditions; // Điều kiện để thực hiện Castling
        bool isInCheck = InDanger(); // Kiểm tra xem Vua có bị chiếu không

        // Kiểm tra Castling về phía Rook1
        if (Rook1 != null)
        {
            // ----------------- Phía bên phải (towards (0, 0)) -----------------

            // 1) Rook1 chưa được di chuyển trước đó
            // 2) Không có quân cờ nào nằm giữa
            conditions = (!Rook1.isMoved) &&
                         (moves[x - 1, y] && Chessmans[x - 2, y] == null);

            // 3) Vua hiện tại không bị chiếu
            conditions = conditions && !isInCheck;

            // Cho phép Castling nếu các điều kiện được đáp ứng
            SetCastlingMove(x, y, x - 2, ref moves, conditions);

            // ----------------- Phía bên phải kết thúc -----------------
        }

        // Kiểm tra Castling về phía Rook2
        if (Rook2 != null)
        {
            // ----------------- Phía bên trái (Away from (0, 0)) -----------------

            // 1) Rook2 chưa được di chuyển trước đó
            // 2) Không có quân cờ nào nằm giữa
            conditions = (!Rook2.isMoved) &&
                         (moves[x + 1, y] && Chessmans[x + 2, y] == null && Chessmans[x + 3, y] == null);

            // 3) Vua hiện tại không bị chiếu
            conditions = conditions && !isInCheck;

            // Cho phép Castling nếu các điều kiện được đáp ứng
            SetCastlingMove(x, y, x + 2, ref moves, conditions);

            // ----------------- Phía bên trái kết thúc -----------------
        }
    }

    // Phương thức thực hiện nước đi Castling
    private void SetCastlingMove(int x, int y, int newX, ref bool[,] moves, bool conditions)
    {
        // Nếu các điều kiện được đáp ứng
        if (conditions)
        {
            // Quan trọng:
            // Chúng ta sẽ di chuyển Vua trên bàn cờ (không phải trên scene) mà chưa nhận được lệnh di chuyển chính thức
            // Để kiểm tra xem nước đi này có đưa Vua vào tình trạng chiếu hay không
            // Sau đó, chúng ta sẽ hoàn tác lại nước đi đã thực hiện
            // Mọi thay đổi chỉ được thực hiện trong điều kiện này, trong hàm này

            // Thực hiện nước đi, cập nhật tọa độ
            BoardManager.Instance.Chessmans[x, y] = null;
            BoardManager.Instance.Chessmans[newX, y] = this;
            this.SetPosition(newX, y);

            // Kiểm tra xem Vua có bị chiếu sau nước đi này không
            bool inDanger = InDanger();

            // Hoàn tác lại nước đi
            BoardManager.Instance.Chessmans[x, y] = this;
            BoardManager.Instance.Chessmans[newX, y] = null;
            this.SetPosition(x, y);

            // Nếu không bị chiếu, cho phép nước đi Castling
            if (!inDanger)
                moves[newX, y] = true;
        }
    }
}
