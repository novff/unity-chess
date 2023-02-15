using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
     public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCount)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int x,y;
        //Up and Right pair
        x = currentX + 1;
        y = currentY + 2;
        if (x < tileCount && y < tileCount){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }
        x = currentX + 2;
        y = currentY + 1;
        if (x < tileCount && y < tileCount){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }
        //Up and Left pair
        x = currentX - 1;
        y = currentY + 2;
        if (x >= 0 && y < tileCount){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }
        x = currentX - 2;
        y = currentY + 1;
        if (x >= 0 && y < tileCount){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }
        //Down and Left pair
        x = currentX - 1;
        y = currentY - 2;
        if (x >= 0 && y >= 0){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }
        x = currentX - 2;
        y = currentY - 1;
        if (x >= 0 && y >= 0){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }
        //Down and Right pair
        x = currentX + 1;
        y = currentY - 2;
        if (x < tileCount && y >= 0){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }
        x = currentX + 2;
        y = currentY - 1;
        if (x < tileCount && y >= 0){
            if(board[x, y] == null || board[x, y].team != team)
                moves.Add(new Vector2Int(x,y));
        }


        return moves;
    }
}
