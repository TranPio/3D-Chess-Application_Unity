using UnityEngine;


public enum ChessPieceType
{
    None=0,
    Pawn=1, //Tốt
    Rook=2, //Xe
    Knight=3,//Mã
    Bishop=4,//Tượng
    Queen=5,// Hậu
    King=6 //Vua
}
public class ChessPiece : MonoBehaviour
{
    public int team;
    public int curentX;
    public int curentY;
    public ChessPieceType type;

    private Vector3 desiredPosition;
    private Vector3 desiredScale;
}
