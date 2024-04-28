using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        //right
        if(curentX + 1 < tileCountX)
        {
            //right
            if (board[curentX + 1,curentY ] == null)
                r.Add(new Vector2Int(curentX + 1,curentY));
            else if (board[curentX + 1,curentY].team != team)
                r.Add(new Vector2Int(curentX + 1, curentY));

            //top right
            if (curentY + 1 < tileCountY)
            
                if (board[curentX + 1, curentY + 1] == null)
                    r.Add(new Vector2Int(curentX + 1, curentY + 1));
                else if (board[curentX + 1, curentY + 1].team != team)
                    r.Add(new Vector2Int(curentX + 1, curentY + 1));
            

            //bottom right
            if (curentY - 1 >= 0)
            
                if (board[curentX + 1, curentY - 1 ] == null)
                    r.Add(new Vector2Int(curentX + 1, curentY - 1));
                else if (board[curentX + 1, curentY - 1].team != team)
                    r.Add(new Vector2Int(curentX + 1, curentY - 1));
            
        }


        //left
        if (curentX - 1 >= 0)
        {
            //Left
            if (board[curentX - 1, curentY] == null)
                r.Add(new Vector2Int(curentX - 1, curentY));
            else if (board[curentX - 1, curentY].team != team)
                r.Add(new Vector2Int(curentX - 1, curentY));

            //top left
            if (curentY + 1 < tileCountY)

                if (board[curentX - 1, curentY + 1] == null)
                    r.Add(new Vector2Int(curentX - 1, curentY + 1));
                else if (board[curentX - 1, curentY + 1].team != team)
                    r.Add(new Vector2Int(curentX - 1, curentY + 1));


            //bottom left
            if (curentY - 1 >= 0)

                if (board[curentX - 1, curentY - 1] == null)
                    r.Add(new Vector2Int(curentX - 1, curentY - 1));
                else if (board[curentX - 1, curentY - 1].team != team)
                    r.Add(new Vector2Int(curentX - 1, curentY - 1));

        }

        //up
        if(curentY + 1 < tileCountY)
            if (board[curentX,curentY + 1]==null || board[curentX,curentY + 1].team != team)
                r.Add(new Vector2Int(curentX,curentY + 1 ));

        //down
        if (curentY - 1 >= 0)
            if (board[curentX, curentY - 1] == null || board[curentX, curentY - 1].team != team)
                r.Add(new Vector2Int(curentX, curentY - 1));


        return r;
    }

    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
    {
        SpecialMove r = SpecialMove.None;

        var kingMove = moveList.Find(m => m[0].x == 4 && m[0].y == ((team == 0) ? 0 : 7));
        var leftRook = moveList.Find(m => m[0].x == 0 && m[0].y == ((team == 0) ? 0 : 7));
        var rightRook = moveList.Find(m => m[0].x == 7 && m[0].y == ((team == 0) ? 0 : 7));

        if(kingMove==null && curentX==4)
        {

            //White team
            if(team==0)
            {
                //Left Rook
                if(leftRook==null)
                    if (board[0,0].type==ChessPieceType.Rook)
                        if (board[0,0].team==0)
                            if (board[3,0]==null)
                                if (board[2,0]==null)
                                    if (board[1,0]==null)
                                    {
                                        availableMoves.Add(new Vector2Int(2, 0));
                                        r = SpecialMove.Castling;
                                    }

                //Right rook
                if (rightRook == null)
                    if (board[7, 0].type == ChessPieceType.Rook)
                        if (board[7, 0].team == 0)
                            if (board[5, 0] == null)
                                 if (board[6, 0] == null)
                                 {
                                        availableMoves.Add(new Vector2Int(6, 0));
                                        r = SpecialMove.Castling;
                                 }

            }
            //Black Team
            else
            {
                //Left Rook
                if (leftRook == null)
                    if (board[0, 7].type == ChessPieceType.Rook)
                        if (board[0, 7].team == 1)
                            if (board[3, 7] == null)
                                if (board[2, 7] == null)
                                    if (board[1, 7] == null)
                                    {
                                        availableMoves.Add(new Vector2Int(2, 7));
                                        r = SpecialMove.Castling;
                                    }
                //Right rook
                if (rightRook == null)
                    if (board[7, 7].type == ChessPieceType.Rook)
                        if (board[7, 7].team == 1)
                            if (board[5, 7] == null)
                                if (board[6, 7] == null)
                                {
                                    availableMoves.Add(new Vector2Int(6, 7));
                                    r = SpecialMove.Castling;
                                }

            }
        }

        return r;
    }
}
