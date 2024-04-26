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
}
