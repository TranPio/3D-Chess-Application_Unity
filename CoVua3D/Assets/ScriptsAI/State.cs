using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    // Các biến trạng thái hiện tại
    // Lưu thông tin về quân cờ đã được di chuyển, bao gồm quân cờ đó, vị trí cũ và mới của quân cờ, và thông tin về việc quân cờ đã được di chuyển hay chưa

    public (Chessman chessman, (int x, int y) oldPosition, (int x, int y) newPosition, bool isMoved) movedChessman;
    // Lưu thông tin về quân cờ đã bị bắt, bao gồm quân cờ đó và vị trí nơi quân cờ đó bị bắt

    public (Chessman chessman, (int x, int y) Position) capturedChessman;
    // Lưu trạng thái hiện tại của nước đi En Passant, bao gồm tọa độ của ô cờ mà nước đi này có thể áp dụng

    public (int x, int y) EnPassantStatus;
    // Lưu thông tin về nước đi Phong cấp, bao gồm thông tin về việc có phong cấp hay không và quân cờ được phong cấp là gì

    public (bool wasPromotion, Chessman promotedChessman) PromotionMove;
    // Lưu thông tin về nước đi Castling (Thủ hậu), bao gồm thông tin về việc có thực hiện Castling hay không và phía của Vua (Thủ hậu hay không)

    public (bool wasCastling, bool isKingSide) CastlingMove;
    // Độ sâu của trạng thái trong cây tìm kiếm
    public int depth;

    // Phương thức để thiết lập trạng thái
    //Phương thức này được sử dụng để thiết lập giá trị cho các biến trạng thái của đối tượng State
    public void SetState((Chessman chessman, (int x, int y) oldPosition, (int x, int y) newPosition, bool isMoved) movedChessman,
                          (Chessman chessman, (int x, int y) Position) capturedChessman,
                          (int x, int y) EnPassantStatus,
                          (bool wasPromotion, Chessman promotedChessman) PromotionMove,
                          (bool wasCastling, bool isKingSide) CastlingMove,
                          int depth)
    {
        this.movedChessman = movedChessman;
        this.capturedChessman = capturedChessman;
        this.EnPassantStatus = EnPassantStatus;
        this.PromotionMove = PromotionMove;
        this.CastlingMove = CastlingMove;
        this.depth = depth;
    }
    //các biến trạng thái (movedChessman, capturedChessman, EnPassantStatus, PromotionMove, CastlingMove, và depth) 
    //của đối tượng State sẽ được cập nhật với các giá trị tương ứng từ các đối số truyền vào
}
