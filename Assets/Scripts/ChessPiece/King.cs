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
}
