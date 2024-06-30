
using System.Collections.Generic;
using UnityEngine;

public class Rook1 : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List <Vector2Int> r= new List <Vector2Int>();

        //Down
        for(int i=curentY-1;i>=0;i--)
        {
            if (board[curentX,i] == null)
                r.Add(new Vector2Int(curentX,i));

            if (board[curentX, i] != null)
            {

                if (board[curentX,i].team!=team)
                    r.Add(new Vector2Int(curentX, i));

                break;
            }
        }

        //Up
        for (int i = curentY + 1; i < tileCountY;i++)
        {
            if (board[curentX, i] == null)
                r.Add(new Vector2Int(curentX, i));

            if (board[curentX, i] != null)
            {

                if (board[curentX, i].team != team)
                    r.Add(new Vector2Int(curentX, i));

                break;
            }
        }


        //Left
        for (int i = curentX - 1; i >= 0; i--)
        {
            if (board[i,curentY] == null)
                r.Add(new Vector2Int(i, curentY));

            if (board[i, curentY] != null)
            {

                if (board[i, curentY].team != team)
                    r.Add(new Vector2Int(i, curentY));

                break;
            }
        }

        //Left
        for (int i = curentX +1; i<tileCountX; i++)
        {
            if (board[i, curentY] == null)
                r.Add(new Vector2Int(i, curentY));

            if (board[i, curentY] != null)
            {

                if (board[i, curentY].team != team)
                    r.Add(new Vector2Int(i, curentY));

                break;
            }
        }

        return r;
    }



}
