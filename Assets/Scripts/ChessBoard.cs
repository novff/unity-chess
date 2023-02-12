using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    //variables and consts
    private const int TILE_COUNT = 8; 
    private GameObject[,] tiles;
    private ChessPiece[,] ChessPieces;
    [SerializeField] private Material tileMaterial;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private ChessPiece currentlyDragging;
    private List<ChessPiece> deadWhite = new List<ChessPiece>();
    private List<ChessPiece> deadBlack = new List<ChessPiece>();
    private List<Vector2Int> availableMoves = new List<Vector2Int>();

    [SerializeField] private GameObject[] Prefabs;
    //Initialization and gameloop
    private void Awake() 
    {
        GenerateTiles(1,TILE_COUNT);
        SpawnPieces();
        PositionPieces();
    }
    private void Update() 
    {
        if(!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }
        RaycastHit hitInfo;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 100, LayerMask.GetMask("Tile", "Hover", "Highlight")))
        {
            Vector2Int hitPosition = LookupTileIndex(hitInfo.transform.gameObject);

            if(currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }
            if(currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves,currentHover) ? LayerMask.NameToLayer("Highlight") :LayerMask.NameToLayer("Tile"));
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }
            if(Input.GetMouseButtonDown(0))//on lmb press
            {
                if(ChessPieces[hitPosition.x, hitPosition.y] != null)
                {
                    //turn check unimplemented
                    if(true)
                    {
                        currentlyDragging = ChessPieces[hitPosition.x,hitPosition.y];

                        availableMoves = currentlyDragging.GetAvailableMoves(ref ChessPieces, TILE_COUNT);
                        HighlightTiles();
                    }
                }
            }
            if(currentlyDragging != null && Input.GetMouseButtonUp(0))//on lmb release
            {
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);
                
                
                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);
                if(!validMove)
                    currentlyDragging.SetPosition(new Vector3(previousPosition.x + 0.5f, previousPosition.y + 0.5f, 0.5f));
                
                currentlyDragging = null;
                RemoveHighlightTiles();
            }
        }
        else
        {
            if (currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves,currentHover) ? LayerMask.NameToLayer("Highlight") :LayerMask.NameToLayer("Tile"));
                currentHover = -Vector2Int.one;
            }
            if(currentlyDragging && Input.GetMouseButtonUp(0))
            {
                currentlyDragging.SetPosition(new Vector3(currentlyDragging.currentX + 0.5f, currentlyDragging.currentY + 0.5f, 0.5f));
                currentlyDragging = null;
                RemoveHighlightTiles();
            }
        }
        if(currentlyDragging)
        {
            Plane HorizontalPlane = new Plane(Vector3.back, Vector3.back * 1.1f);
            float distance =0.0f;
            if(HorizontalPlane.Raycast(ray, out distance))
                currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.back * 2);
        }
    }



    //tile generation 
    private void GenerateTiles(float tileSize, int tileCount)
    {
        tiles = new GameObject[tileCount,tileCount];
        for (int x = 0; x < tileCount; x++)
            for (int y = 0; y < tileCount; y++)
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);
    }
    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, (y + 1) * tileSize);
        vertices[2] = new Vector3((x + 1) * tileSize, y * tileSize);
        vertices[3] = new Vector3((x + 1) * tileSize, (y + 1) * tileSize);

        int[] tris = new int[] {0, 1, 2, 1, 3, 2};

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");

        tileObject.AddComponent<BoxCollider>();


        return tileObject;
    }
    //piece creation
    private void SpawnPieces()
    {
        ChessPieces = new ChessPiece[TILE_COUNT,TILE_COUNT];

        //whiteTeam
        ChessPieces[0,0] = SpawnSinglePiece(ChessPieceType.Rook, 0);
        ChessPieces[1,0] = SpawnSinglePiece(ChessPieceType.Knight, 0);
        ChessPieces[2,0] = SpawnSinglePiece(ChessPieceType.Bishop, 0);
        ChessPieces[3,0] = SpawnSinglePiece(ChessPieceType.Queen, 0);
        ChessPieces[4,0] = SpawnSinglePiece(ChessPieceType.King, 0);
        ChessPieces[5,0] = SpawnSinglePiece(ChessPieceType.Bishop, 0);
        ChessPieces[6,0] = SpawnSinglePiece(ChessPieceType.Knight, 0);
        ChessPieces[7,0] = SpawnSinglePiece(ChessPieceType.Rook, 0);
        for(int i = 0; i < TILE_COUNT; i++)
            ChessPieces[i,1] = SpawnSinglePiece(ChessPieceType.Pawn, 0);
        //blackTeam
        ChessPieces[0,7] = SpawnSinglePiece(ChessPieceType.Rook, 1);
        ChessPieces[1,7] = SpawnSinglePiece(ChessPieceType.Knight, 1);
        ChessPieces[2,7] = SpawnSinglePiece(ChessPieceType.Bishop, 1);
        ChessPieces[3,7] = SpawnSinglePiece(ChessPieceType.Queen, 1);
        ChessPieces[4,7] = SpawnSinglePiece(ChessPieceType.King, 1);
        ChessPieces[5,7] = SpawnSinglePiece(ChessPieceType.Bishop, 1);
        ChessPieces[6,7] = SpawnSinglePiece(ChessPieceType.Knight, 1);
        ChessPieces[7,7] = SpawnSinglePiece(ChessPieceType.Rook, 1);
        for(int i = 0; i < TILE_COUNT; i++)
            ChessPieces[i,6] = SpawnSinglePiece(ChessPieceType.Pawn, 1);
        
    }
    private ChessPiece SpawnSinglePiece(ChessPieceType type, int team)
    {
        ChessPiece cp = Instantiate(Prefabs[(int)type + (6 * team)], transform).GetComponent<ChessPiece>();
        
        cp.type = type;
        cp.team = team;

        return cp;

    }   
    //putting pieces in correct places
    private void PositionPieces()
    {
        for (int x = 0; x < TILE_COUNT; x++)
            for (int y = 0; y < TILE_COUNT; y++)
                if (ChessPieces[x,y] != null)
                    PositionSinglePiece(x, y, true);
    }
    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        ChessPieces[x, y].currentX = x;
        ChessPieces[x, y].currentY = y;
        ChessPieces[x, y].SetPosition(new Vector3(x + 0.5f, y + 0.5f, 0.5f), force);

    }
    //moves highlight 
    private void HighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Highlight");
        }
    }
    private void RemoveHighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");
        }
        availableMoves.Clear();
    }


    //actions
    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_COUNT; x++)
            for (int y = 0; y < TILE_COUNT; y++)
                if (tiles[x, y] == hitInfo)
                    return new Vector2Int(x, y);
        //otherwise            
        return -Vector2Int.one; //crashes if Vector2Int is invalid is invalid
    }
    private bool MoveTo(ChessPiece cp, int x, int y)
    {
        if(!ContainsValidMove(ref availableMoves, new Vector2(x, y)))
            return false;

        Vector2Int previousPosition = new Vector2Int(cp.currentX, cp.currentY);

        if(ChessPieces[x, y] != null)
        {
            ChessPiece othercp = ChessPieces[x, y];
            if(cp.team == othercp.team)
            {
                return false;
            }
            if(othercp.team  == 0)
            {
                deadWhite.Add(othercp);
                othercp.transform.localScale = Vector3.zero;
            }
            else
            {
                deadBlack.Add(othercp);
                othercp.transform.localScale = Vector3.zero;
            }
            
        }

        ChessPieces[x, y] = cp;
        ChessPieces[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x, y);
        return true;
    }
    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 position)
    {
        for (int i = 0; i < moves.Count; i++)
            if(moves[i].x == position.x && moves[i].y == position.y)
                return true;
        return false;
    }

}   


 