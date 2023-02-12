using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCount)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int direction = (team == 0) ? 1: -1;

        //one front
        if(board[currentX, currentY + direction] == null)
            moves.Add(new Vector2Int(currentX, currentY + direction));
        //two fromt
        if(board[currentX, currentY + direction] == null)
        {
            if(team == 0 && currentY == 1 && board[currentX, currentY + (direction * 2)] == null)
                moves.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            if(team == 1 && currentY == 6 && board[currentX, currentY + (direction * 2)] == null)
                moves.Add(new Vector2Int(currentX, currentY + (direction * 2)));
        }
            moves.Add(new Vector2Int(currentX, currentY + direction));
        //diagonal takes
        if(currentX != tileCount - 1)
            if(board[currentX + 1, currentY + direction] != null && board[currentX + 1, currentY + direction].team != team)
                moves.Add(new Vector2Int(currentX + 1, currentY + direction));
        if(currentX != 0)
            if(board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != team)
                moves.Add(new Vector2Int(currentX - 1, currentY + direction));

        //en passant
        
        return moves;
    }
}
