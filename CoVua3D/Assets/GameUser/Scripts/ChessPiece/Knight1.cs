
using System.Collections.Generic;
using UnityEngine;

public class Knight1 : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        //Top right
        int x = curentX + 1;
        int y = curentY + 2;
        if(x<tileCountX&&y<tileCountY)
            if (board[x, y] == null || board[x,y].team!=team)
                r.Add(new Vector2Int(x,y));

        x = curentX + 2;
        y = curentY + 1;
        if (x < tileCountX && y < tileCountY)
            if (board[x, y] == null || board[x, y].team != team)
                r.Add(new Vector2Int(x, y));

        //Top left
        x = curentX - 1;
        y = curentY + 2;
        if(x>=0&&y<tileCountY)
            if (board[x, y] == null || board[x, y].team != team)
                r.Add(new Vector2Int(x, y));

        x = curentX - 2;
        y = curentY + 1;
        if (x >= 0 && y < tileCountY)
            if (board[x, y] == null || board[x, y].team != team)
                r.Add(new Vector2Int(x, y));

        //Bottom right
        x = curentX + 1;
        y = curentY - 2;
        if(x<tileCountX&&y>=0)
            if (board[x, y] == null || board[x, y].team != team)
                r.Add(new Vector2Int(x, y));

        x = curentX + 2;
        y = curentY - 1;
        if (x < tileCountX && y >= 0)
            if (board[x, y] == null || board[x, y].team != team)
                r.Add(new Vector2Int(x, y));

        //Bottom left
        x = curentX - 1;
        y = curentY - 2;
        if (x >= 0 && y >= 0)
            if (board[x, y] == null || board[x, y].team != team)
                r.Add(new Vector2Int(x, y));

        x = curentX - 2;
        y = curentY - 1;
        if (x >= 0 && y >= 0)
            if (board[x, y] == null || board[x, y].team != team)
                r.Add(new Vector2Int(x, y));


        return r;
    }
}
