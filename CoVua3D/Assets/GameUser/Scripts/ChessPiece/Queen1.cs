using System.Collections.Generic;
using UnityEngine;

public class Queen1 : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        //Down
        for (int i = curentY - 1; i >= 0; i--)
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

        //Up
        for (int i = curentY + 1; i < tileCountY; i++)
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
            if (board[i, curentY] == null)
                r.Add(new Vector2Int(i, curentY));

            if (board[i, curentY] != null)
            {

                if (board[i, curentY].team != team)
                    r.Add(new Vector2Int(i, curentY));

                break;
            }
        }

        //Left
        for (int i = curentX + 1; i < tileCountX; i++)
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

        //Top right
        for (int x = curentX + 1, y = curentY + 1; x < tileCountX && y < tileCountY; x++, y++)
        {
            if (board[x, y] == null)
                r.Add(new Vector2Int(x, y));
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

                break;
            }
        }

        //Top left
        for (int x = curentX - 1, y = curentY + 1; x >= 0 && y < tileCountY; x--, y++)
        {
            if (board[x, y] == null)
                r.Add(new Vector2Int(x, y));
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

                break;
            }
        }

        //Bottom right
        for (int x = curentX + 1, y = curentY - 1; x < tileCountX && y >= 0; x++, y--)
        {
            if (board[x, y] == null)
                r.Add(new Vector2Int(x, y));
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

                break;
            }
        }

        //Bottom left
        for (int x = curentX - 1, y = curentY - 1; x >= 0 && y >= 0; x--, y--)
        {
            if (board[x, y] == null)
                r.Add(new Vector2Int(x, y));
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

                break;
            }
        }

        return r;
    }

}
