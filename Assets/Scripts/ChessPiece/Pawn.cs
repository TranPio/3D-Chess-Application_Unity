using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves( ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r= new List<Vector2Int> ();

        int direction = (team==0) ? 1 : -1;

        //one in front
        if (board[curentX,curentY+direction]==null)
            r.Add(new Vector2Int (curentX,curentY+direction));

        //two in front
        if (board[curentX,curentY+direction]==null)
        {
            //White team
            if (team == 0 && curentY == 1 && board[curentX, curentY + (direction * 2)]==null)
                r.Add(new Vector2Int (curentX,curentY + (direction * 2)));

            //Black team
            if (team == 1 && curentY == 6 && board[curentX, curentY + (direction * 2)] == null)
                r.Add(new Vector2Int(curentX, curentY + (direction * 2)));
        }

        //kill move
        if(curentX != tileCountX-1)
            if (board[curentX + 1,curentY + direction]!=null && board[curentX + 1, curentY + direction].team != team)
                r.Add(new Vector2Int(curentX + 1,curentY + direction));


        if (curentX != 0)
            if (board[curentX - 1, curentY + direction] != null && board[curentX - 1, curentY + direction].team != team)
                r.Add(new Vector2Int(curentX - 1, curentY + direction));

        return r;
    }

    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
    {
        int direction = (team==0)? 1 : -1;

        //En passant
        if(moveList.Count>0)
        {

            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            if (board[lastMove[1].x, lastMove[1].y].type==ChessPieceType.Pawn) // if the last piece moved was a pawn
            {
                if (Mathf.Abs(lastMove[0].y - lastMove[1].y)==2) // if the last move was a +2 in either direction
                {
                    if (board[lastMove[1].x, lastMove[1].y].team != team) // if the move was from the other team
                    {
                        if (lastMove[1].y==curentY) // if both pawns are on the samw Y
                        {
                            if (lastMove[1].x==curentX-1) //Landed Left
                            {
                                availableMoves.Add(new Vector2Int(curentX - 1, curentY + direction));
                                return SpecialMove.EnPassant;
                            }
                            if (lastMove[1].x == curentX + 1) //Landed right
                            {
                                availableMoves.Add(new Vector2Int(curentX + 1, curentY + direction));
                                return SpecialMove.EnPassant;

                            }
                        }
                    }
                }
            }
        }

        return SpecialMove.None;
    }
}
