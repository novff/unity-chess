using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType
{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6
}

public class ChessPiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;

    private Vector3 desiredPosition;
    //private Vector3 desiredScale;

    private void Update() 
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition,Time.deltaTime * 30f);
    }

    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCount)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        //moves.Add(new Vector2Int(3,3));
        

        return moves;
    }

    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        desiredPosition = position;
        if(force)
            transform.position = desiredPosition;
        
    }

    
    
}
