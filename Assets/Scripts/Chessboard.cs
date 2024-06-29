using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Unity.Mathematics;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public enum SpecialMove
{
    None=0,
    EnPassant,
    Castling,
    Promotion
}

//Mới thêm cho socket
[UnityEngine.Scripting.Preserve]

//public static class NetworkConnectionExtensions
//{
//    [ExtensionOfNativeClass]
//    public static Socket ToSocket(this NetworkConnection connection)
//    {
//        var fieldInfo = typeof(NetworkConnection).GetField("m_SocketFD", BindingFlags.NonPublic | BindingFlags.Instance);
//        if (fieldInfo != null)
//        {
//            var socketFD = (int)fieldInfo.GetValue(connection);
//            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        }
//        return null;
//    }
//}

public class ChessBoard : MonoBehaviour
{
    [Header("Art stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize=1.0f;
    [SerializeField] private float yOffset=0.005f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;
    [SerializeField] private float deathSize = 0.5f;
    [SerializeField] private float deathSpacing = 0.5f;
    [SerializeField] private float dragOffset = 1f;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private Transform rematchIndicator;
    [SerializeField] private Button rematchButton;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMaterials;
    
    // Cập nhật thời gian lên màn hình
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI whiteTimeText;
    [SerializeField] private TextMeshProUGUI blackTimeText;
    //[SerializeField] private float initialTime = 10.0f; //mỗi lượt 90 giây
    private float initialTime = 10.0f;




    //Logic
    private ChessPiece[,] chessPieces;
    private ChessPiece currentlyDragging;
    private List <Vector2Int> availableMoves= new List <Vector2Int>();
    private List <ChessPiece> deadWhites=new List <ChessPiece>();
    private List <ChessPiece> deadBlacks=new List <ChessPiece>();
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;
    private bool isWhiteTurn;
    private SpecialMove specialMove;
    private List<Vector2Int[]> moveList= new List<Vector2Int[]>();

    //multi logic
    private int playerCount = -1;
    private int currentTeam = -1;
    private bool localGame = true;
    private bool[] playerRematch = new bool[2];
    ////Xu ly time
    //private int defaultTime = 90;
    //private int whiteTime, blackTime;
    


    //Timer
    public Timer whiteTimer;
    public Timer blackTimer;
   

    private void Start()
    {
        isWhiteTurn = true;

        GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);

        SpawnAllPiece();
        PositionAllPiece();

        RegisterEvents();
        // Timer
        whiteTimer = new Timer(initialTime);
       // whiteTimer = new Timer();
        blackTimer = new Timer(initialTime);
        UpdateTimers();
    }

    private void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out info,100,LayerMask.GetMask("Tile","Hover","Highlight")))
        {
            //get the indexes of the tile i've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            //if we're hovering a tile after not hovering any tiles
            if(currentHover==-Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            //if we were already hovering a tile, change the previous one
            if (currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            //if we press down on the mouse
            if(Input.GetMouseButtonDown(0))
            {
                if (chessPieces[hitPosition.x, hitPosition.y]!=null)
                {
                    //is it our turn?
                    if((chessPieces[hitPosition.x, hitPosition.y].team==0&&isWhiteTurn && currentTeam ==0)|| (chessPieces[hitPosition.x, hitPosition.y].team == 1 && !isWhiteTurn && currentTeam==1))
                    {
                        currentlyDragging = chessPieces[hitPosition.x,hitPosition.y];

                        //Get a list of where i can go , hightlight tiles as well
                        availableMoves = currentlyDragging.GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
                        // Get a list of special moves as well
                        specialMove = currentlyDragging.GetSpecialMoves(ref chessPieces, ref moveList, ref availableMoves);
                        
                        PreventCheck(); // ngan chan bi chieu tuong
                        HightlightTiles();
                    }
                }
            }


            //if we are releasing the mouse button
            if(currentlyDragging!=null&& Input.GetMouseButtonUp(0))
            {
                 
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.curentX, currentlyDragging.curentY);

                if (ContainsValidMove(ref availableMoves, new Vector2Int(hitPosition.x,hitPosition.y)))
                {
                    MoveTo(previousPosition.x,previousPosition.y, hitPosition.x, hitPosition.y);

                    //Net implementtation
                    NetMakeMove mm = new NetMakeMove();
                    mm.originalX = previousPosition.x;
                    mm.originalY = previousPosition.y;
                    mm.destinationX=hitPosition.x;
                    mm.destinationY=hitPosition.y;
                    mm.teamId = currentTeam;
                    Client.Instance.SendToServer(mm);
                }
                else
                {
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                    currentlyDragging = null;
                    RemoveHightlightTiles();
                }

               
            }

        }
        else
        {
            if(currentHover!=-Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves,currentHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }

            if(currentlyDragging&&Input.GetMouseButtonUp(0))
            {
                currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.curentX, currentlyDragging.curentY));

                currentlyDragging = null;
                RemoveHightlightTiles();

            }
        }

        // if we're dragging a piece
        if(currentlyDragging)
        {
            Plane horizontalPlan = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if(horizontalPlan.Raycast(ray,out distance))
                currentlyDragging.SetPosition(ray.GetPoint(distance)+Vector3.up*dragOffset);
        }

        //Cập nhật thời gian cho mỗi lượt
        UpdateTimers();
       


    }
    // Hàm cập nhật Timer
    private void UpdateTimers()
    {
        if (isWhiteTurn && currentTeam == 0)
        {
            whiteTimer.Start();
            //blackTimer.Stop();
            blackTimer.Reset(initialTime);
            whiteTimer.Update();
            //whiteTimeText.text = FormatTime(whiteTimer.GetTimeRemaining());
            //blackTimeText.text = FormatTime(blackTimer.GetTimeRemaining());
            whiteTimeText.text = FormatTime(whiteTimer.GetTimeRemaining());
            blackTimeText.text = FormatTime(blackTimer.Timezero());

            CheckTimeOut();
        }
        else if (!isWhiteTurn && currentTeam == 1)
        {
            blackTimer.Start();
          //  whiteTimer.Stop();
            whiteTimer.Reset(initialTime);

            blackTimer.Update();
            //blackTimeText.text = FormatTime(blackTimer.GetTimeRemaining());
            //whiteTimeText.text = FormatTime(whiteTimer.GetTimeRemaining());
            whiteTimeText.text = FormatTime(whiteTimer.Timezero());
            blackTimeText.text = FormatTime(blackTimer.GetTimeRemaining());

            CheckTimeOut();
        }

        // Kiểm tra xem có hết thời gian không
       // CheckTimeOut();
    }
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CheckTimeOut()
    {
        if (whiteTimer.IsTimeUp())
        {
            // Xử lý hết thời gian cho đội trắng
            CheckMate(1); // Đội trắng thua vì hết thời gian
        }
        else if (blackTimer.IsTimeUp())
        {
            // Xử lý hết thời gian cho đội đen
            CheckMate(0); // Đội đen thua vì hết thời gian
        }
    }
    // Cập nhật thời gian khi chuyển lượt chơi
    private void SwitchTurn()
    {
        isWhiteTurn = !isWhiteTurn;

        if (!isWhiteTurn)
        {
            whiteTimer.Stop();
            blackTimer.Start();
        }
        else
        {
            blackTimer.Stop();
            whiteTimer.Start();
        }
    }
    // Generate the board
    private void GenerateAllTiles(float tileSize, int tileCountX,int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + boardCenter;

        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);
    }

    
    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0},Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material=tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize)-bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize)-bounds;
        vertices[2] = new Vector3((x+1) * tileSize, yOffset, y * tileSize)-bounds;
        vertices[3] = new Vector3((x+1) * tileSize, yOffset, (y+1) * tileSize)-bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    //Spawning of the piece
    private void SpawnAllPiece()
    {
        chessPieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];

        int whiteTeam = 0;
        int blackTeam=1;

        //White team
        chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam);
        chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);
        chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);
        chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Queen, whiteTeam);
        chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.King, whiteTeam);
        chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);
        chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);
        chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam);
     

        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            chessPieces[i, 1] = SpawnSinglePiece(ChessPieceType.Pawn, whiteTeam);

            //ChessPiece pawn = chessPieces[i, 1];

            //pawn = SpawnSinglePiece(ChessPieceType.Pawn, whiteTeam);

            // chessPieces[i,1].SetPosition(new Vector3(i * tileSize, yOffset +0.3f, 1 * tileSize) - bounds + new Vector3(tileSize / 2, 0.3f, tileSize / 2));

        }

        //Black team
        chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam);
        chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam);
        chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam);
        chessPieces[3, 7] = SpawnSinglePiece(ChessPieceType.Queen, blackTeam);
        chessPieces[4, 7] = SpawnSinglePiece(ChessPieceType.King, blackTeam);
        chessPieces[5, 7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam);
        chessPieces[6, 7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam);
        chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam);
        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            chessPieces[i, 6] = SpawnSinglePiece(ChessPieceType.Pawn, blackTeam);

        }
    }
    private ChessPiece SpawnSinglePiece(ChessPieceType type, int team)
    {
        ChessPiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

        cp.type = type;
        cp.team = team;
        cp.GetComponent<MeshRenderer>().material = teamMaterials[team];

        return cp;
    }
  
    // Positioning
    private void PositionAllPiece()
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (chessPieces[x, y] != null)
                    PositionSinglePiece(x, y,true);
        
    }
    private void PositionSinglePiece(int x, int y, bool force=false)
    {
        chessPieces[x, y].curentX = x;
        chessPieces[x, y].curentY = y;
        chessPieces[x, y].SetPosition(GetTileCenter(x, y),force);
    }
    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x*tileSize,0,y*tileSize)-bounds+new Vector3(tileSize/2,0,tileSize/2);
    }

    //Hightlight tiles
    private void HightlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Highlight");
    }
    private void RemoveHightlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");

        availableMoves.Clear();
    }

    // CheckMate
    private void CheckMate(int team)
    {
        DisplayVictory(team);
       // this.enabled = false;
    }

    private void DisplayVictory(int winningTeam)
    { 
        victoryScreen.SetActive(true);
        rematchButton.interactable = true;
        rematchIndicator.transform.GetChild(0).gameObject.SetActive(false);
        rematchIndicator.transform.GetChild(1).gameObject.SetActive(false);
        victoryScreen.transform.GetChild(winningTeam).gameObject.SetActive(true);
    }
    public void OnRematchButton()
    {
        if(localGame)
        {
            NetRematch wrm = new NetRematch();
            wrm.teamId = 0;
            wrm.wantRematch = 1;
            Client.Instance.SendToServer(wrm);

            NetRematch brm = new NetRematch();
            brm.teamId = 1;
            brm.wantRematch = 1;
            Client.Instance.SendToServer(brm);
        }
        else
        {
            NetRematch rm= new NetRematch();
            rm.teamId = currentTeam;
            rm.wantRematch = 1;
            Client.Instance.SendToServer(rm);
        }
    }

    public void GameReset()
    {
        //UI

        rematchButton.interactable = true;

        rematchIndicator.transform.GetChild(0).gameObject.SetActive(false);
        rematchIndicator.transform.GetChild(1).gameObject.SetActive(false);

        victoryScreen.transform.GetChild(0).gameObject.SetActive(false);
        victoryScreen.transform.GetChild(1).gameObject.SetActive(false);
        victoryScreen.SetActive(false);

        //Fields reset
        currentlyDragging = null;
        // availableMoves = new List <Vector2Int>();
        availableMoves.Clear();
        moveList.Clear();
        playerRematch[0] = playerRematch[1] = false;

        //clean up
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (chessPieces[x, y] != null)
                    Destroy(chessPieces[x, y].gameObject);

                chessPieces[x, y] = null;
            }
        }

        for (int i = 0; i < deadWhites.Count; i++)
            Destroy(deadWhites[i].gameObject);
        for (int i = 0; i < deadBlacks.Count; i++)
            Destroy(deadBlacks[i].gameObject);

        deadWhites.Clear();
        deadBlacks.Clear();

        SpawnAllPiece();
        PositionAllPiece();
        isWhiteTurn = true;
    }
    
    public void OnMenuButton()
    {
        NetRematch rm = new NetRematch();
        rm.teamId = currentTeam;
        rm.wantRematch = 0;
        Client.Instance.SendToServer(rm);

        GameReset();
        GameUI.Instance.OnLeaveFromGameMenu();

        Invoke("ShutdownRelay", 1.0f);
       

        //REset some values

        playerCount = -1;
        currentTeam = -1;
    }

    // Special Moves
    private void ProcessSpecialMove()
    {
        if(specialMove==SpecialMove.EnPassant)
        {
            var newMove= moveList[moveList.Count-1];
            ChessPiece myPawn = chessPieces[newMove[1].x, newMove[1].y];
            var targetPawnPosition= moveList[moveList.Count-2];
            ChessPiece enemyPawn = chessPieces[targetPawnPosition[1].x, targetPawnPosition[1].y];

            if(myPawn.curentX==enemyPawn.curentX)
            {
                if(myPawn.curentY==enemyPawn.curentY-1|| myPawn.curentY == enemyPawn.curentY + 1)
                {
                    if(enemyPawn.team==0)
                    {
                        deadWhites.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(
                            new Vector3(8 * tileSize, yOffset, -1 * tileSize)
                            - bounds
                            + new Vector3(tileSize / 2, 0.3f, tileSize / 2)
                            + (Vector3.forward * deathSpacing) * deadWhites.Count);
                    }
                    else
                    {
                        deadBlacks.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(new Vector3(-1 * tileSize, yOffset, 8 * tileSize)
                            - bounds
                            + new Vector3(tileSize / 2, 0.3f, tileSize / 2)
                            + (Vector3.back * deathSpacing) * deadBlacks.Count);
                    }

                    chessPieces[enemyPawn.curentX, enemyPawn.curentY] = null;
                }
            }

        }

        if(specialMove==SpecialMove.Promotion)
        {
            Vector2Int[] lastMove = moveList[moveList.Count-1];
            ChessPiece targetPawn = chessPieces[lastMove[1].x, lastMove[1].y];  

            if(targetPawn.type==ChessPieceType.Pawn)
            {
                if(targetPawn.team==0 && lastMove[1].y==7)
                {
                    ChessPiece newQueen = SpawnSinglePiece(ChessPieceType.Queen, 0);
                    newQueen.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
                    Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
                    chessPieces[lastMove[1].x, lastMove[1].y]= newQueen;
                    PositionSinglePiece(lastMove[1].x, lastMove[1].y, true);

                }
                if (targetPawn.team == 1 && lastMove[1].y == 0)
                {
                    ChessPiece newQueen = SpawnSinglePiece(ChessPieceType.Queen, 1);
                    newQueen.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
                    Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
                    chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
                    PositionSinglePiece(lastMove[1].x, lastMove[1].y, true);

                }
            }
        }

        if(specialMove==SpecialMove.Castling)
        {
            Vector2Int[] lastMove= moveList[moveList.Count-1];

            //left Rook
            if (lastMove[1].x==2)
            {
                if (lastMove[1].y==0) //White side
                {
                    ChessPiece rook = chessPieces[0, 0];
                    chessPieces[3, 0] = rook;
                    PositionSinglePiece(3, 0);
                    chessPieces[0, 0] = null;
                }
                else if (lastMove[1].y==7) //Black side
                {
                    ChessPiece rook = chessPieces[0, 7];
                    chessPieces[3, 7] = rook;
                    PositionSinglePiece(3, 7);
                    chessPieces[0, 7] = null;
                }
            }


            //Right Rook
            if (lastMove[1].x == 6)
            {
                if (lastMove[1].y == 0) //White side
                {
                    ChessPiece rook = chessPieces[7, 0];
                    chessPieces[5, 0] = rook;
                    PositionSinglePiece(5, 0);
                    chessPieces[7, 0] = null;
                }
                else if (lastMove[1].y == 7) //Black side
                {
                    ChessPiece rook = chessPieces[7, 7];
                    chessPieces[5, 7] = rook;
                    PositionSinglePiece(5, 7);
                    chessPieces[7, 7] = null;
                }
            }


        }
    }

    private void PreventCheck()
    {
        ChessPiece targetKing= null;
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (chessPieces[x,y]!=null)
                    if (chessPieces[x, y].type == ChessPieceType.King)
                        if (chessPieces[x, y].team == currentlyDragging.team)
                            targetKing = chessPieces[x, y];
       
        //Since we're sending ref availableMoves, we will be deleting moves that are putting us in check
        SimulateMoveForSinglePiece(currentlyDragging, ref availableMoves, targetKing);


    }

    private void SimulateMoveForSinglePiece(ChessPiece cp, ref List<Vector2Int> moves, ChessPiece targetKing)
    {
        //Save the current values, to reset after the function call
        int actualX = cp.curentX;
        int actualY = cp.curentY;
        List<Vector2Int> movesToRemove = new List<Vector2Int>();

        // Going through all the moves, simulate them and check if we're in check

        for (int i = 0;i<moves.Count;i++)
        {
            int simX = moves[i].x;
            int simY = moves[i].y;

            Vector2Int kingPositionThisSim = new Vector2Int(targetKing.curentX, targetKing.curentY);
            // Did we  simulate the king's move
            if(cp.type == ChessPieceType.King)
                kingPositionThisSim=new Vector2Int(simX, simY);

            //Copy the [,] anh not a reference
            ChessPiece[,] simulation = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];
            List<ChessPiece> simAttackingPieces = new List<ChessPiece>();
            for (int x= 0;x<TILE_COUNT_X;x++)
            {
                for (int y= 0;y<TILE_COUNT_Y;y++)
                {
                    if (chessPieces[x,y] != null)
                    {
                        simulation[x, y] = chessPieces[x,y];
                        if (simulation[x, y].team != cp.team)
                            simAttackingPieces.Add(simulation[x, y]);
                    }
                }
            }

            // Simulate that move
            simulation[actualX, actualY] = null;
            cp.curentX = simX;
            cp.curentY = simY;
            simulation[simX, simY] = cp;

            // did one of the piece got taken down during our simulation
            var deadPiece = simAttackingPieces.Find(c => c.curentX == simX && c.curentY == simY);
            if(deadPiece!=null)
                simAttackingPieces.Remove(deadPiece);

            // Get all the simulated attacking pieces moves
            List<Vector2Int> simMoves= new List<Vector2Int>();
            for (int a = 0; a < simAttackingPieces.Count; a++)
            {
                var pieceMoves = simAttackingPieces[a].GetAvailableMoves(ref simulation, TILE_COUNT_X, TILE_COUNT_Y);
                for (int b = 0; b < pieceMoves.Count; b++)
                    simMoves.Add(pieceMoves[b]);
            }

            // is the king in trouble? if so, remove the move
            if(ContainsValidMove(ref simMoves, kingPositionThisSim))
            {
                movesToRemove.Add(moves[i]);
            }

            //Restore the actual CP data
            cp.curentX = actualX;
            cp.curentY=actualY;

        }


        //remove from the current available move list
        for (int i = 0; i < movesToRemove.Count; i++)
            moves.Remove(movesToRemove[i]);
        

    }

    private bool CheckForCheckmate()
    {
        var lastMove= moveList[moveList.Count-1];
        int targetTeam = (chessPieces[lastMove[1].x, lastMove[1].y].team == 0) ? 1 : 0;


        List<ChessPiece> attackingPieces=new List<ChessPiece>();
        List<ChessPiece> defendingPieces=new List<ChessPiece>();
        ChessPiece targetKing = null;
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (chessPieces[x, y] != null)
                {
                    if (chessPieces[x, y].team == targetTeam)
                    {                        
                        defendingPieces.Add(chessPieces[x, y]);
                        if (chessPieces[x,y].type==ChessPieceType.King)
                            targetKing = chessPieces[x, y];
                    }
                    else
                    {
                        attackingPieces.Add(chessPieces[x, y]);
                    }
                }

        //is the king attacked right now?
        List<Vector2Int> currentAvailableMoves=new List<Vector2Int>();
        for (int i = 0;i<attackingPieces.Count;i++)
        {
            var pieceMoves = attackingPieces[i].GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
            for (int b = 0; b < pieceMoves.Count; b++)
                currentAvailableMoves.Add(pieceMoves[b]);
        }

        //Are we in check right now?
        if(ContainsValidMove(ref currentAvailableMoves, new Vector2Int(targetKing.curentX, targetKing.curentY)))
        {
            //King is under attack, can we move something to help him?
            for (int i = 0; i < defendingPieces.Count; i++)
            {
                List<Vector2Int> defendingMoves = defendingPieces[i].GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
                // since we're sending ref avalableMoves, we will be deleting moves that are putting us in check 
                SimulateMoveForSinglePiece(defendingPieces[i], ref defendingMoves, targetKing);

                if (defendingMoves.Count != 0)
                    return false;
            }

            return true; // checkmate exit
        }

        return false;
    }

    //Operations
    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2Int pos)
    {
        for (int i=0;i<moves.Count;i++)
            if (moves[i].x == pos.x && moves[i].y == pos.y)
                return true;

        return false;
    }
    private void MoveTo(int originalX, int originalY, int x, int y)
    {
        ChessPiece cp = chessPieces[originalX, originalY];
        Vector2Int previousPosition=new Vector2Int(originalX,originalY);

        //is there another piece on the target position?
        if (chessPieces[x, y] != null)
        {
            ChessPiece ocp = chessPieces[x, y];

            if(cp.team==ocp.team)
                return;

            // If its the enemy team
            if(ocp.team==0)
            {
                if (ocp.type == ChessPieceType.King)
                    CheckMate(1);

                deadWhites.Add(ocp);
                //Hiển thị quân cờ bị ăn
                ocp.SetScale(Vector3.one * deathSize);
                ocp.SetPosition(new Vector3(8 * tileSize, yOffset, -1 * tileSize)
                    - bounds
                    + new Vector3(tileSize / 2, 0.3f, tileSize / 2)
                    + (Vector3.forward * deathSpacing) * deadWhites.Count);

            }
            else
            {
                if (ocp.type == ChessPieceType.King)
                    CheckMate(0);

                deadBlacks.Add(ocp);
                ocp.SetScale(Vector3.one * deathSize);
                ocp.SetPosition(new Vector3(-1* tileSize, yOffset, 8 * tileSize)
                    - bounds
                    + new Vector3(tileSize / 2, 0.3f, tileSize / 2)
                    + (Vector3.back * deathSpacing) * deadBlacks.Count);
            }

        }
        //Di chuyển quân cờ
        chessPieces[x, y] = cp;
        chessPieces[previousPosition.x, previousPosition.y] = null;

        //Cập nhật vị trí cho quân cờ
        PositionSinglePiece(x, y);

        // Đổi lượt đi
       isWhiteTurn =! isWhiteTurn;
       // SwitchTurn();
        // Local co the di chuyen quan co o ca 2 team
        if (localGame)
            currentTeam = (currentTeam == 0) ? 1 : 0;
        moveList.Add(new Vector2Int[] { previousPosition, new Vector2Int(x, y) });

        //Tình huống di chuyển đặc biệt
        ProcessSpecialMove();

        if (currentlyDragging)
            currentlyDragging = null;
        RemoveHightlightTiles();

        if (CheckForCheckmate())
            CheckMate(cp.team);

        //// Gửi thông tin nước đi lên server
        //NetMakeMove mm = new NetMakeMove();
        //mm.originalX = previousPosition.x;
        //mm.originalY = previousPosition.y;
        //mm.destinationX = x;
        //mm.destinationY = y;
        //mm.teamId = currentTeam;
        //mm.timeRemaining = (currentTeam == 0) ? whiteTimer.GetTimeRemaining() : blackTimer.GetTimeRemaining(); // Thêm thời gian còn lại vào tin nhắn

        //Client.Instance.SendToServer(mm);
        return;

    }

    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (tiles[x, y] == hitInfo)
                    return new Vector2Int(x, y);

        return -Vector2Int.one; //-1 -1
         

        
    }

    #region
    private void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;

        NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
        NetUtility.S_REMATCH += OnRematchServer;

        NetUtility.C_WELCOME += OnWelcomeClient;

        NetUtility.C_START_GAME += OnStartGameClient;

        NetUtility.C_MAKE_MOVE += OnMakeMoveClient;
        NetUtility.C_REMATCH += OnRematchClient;
        GameUI.Instance.SetLocalGame += OnSetLocalGame;

    }

  

    private void UnRegisterEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;

        NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
        NetUtility.S_REMATCH -= OnRematchServer;

        NetUtility.C_WELCOME -= OnWelcomeClient;

        NetUtility.C_START_GAME -= OnStartGameClient;
        NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;
        NetUtility.C_REMATCH -= OnRematchClient;


        GameUI.Instance.SetLocalGame -= OnSetLocalGame;
    }




    //Server
    //Sưa lại socket
    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        //Client has connected, assign a team and return the message back to him
        NetWelcome nw = msg as NetWelcome;

        //assign a team
        nw.AssignedTeam = ++playerCount;

        //return back to the client
        Server.Instance.SendToClient(cnn, nw);

        //we're full, let's start the game
        if (nw.AssignedTeam == 1)
        {
            Server.Instance.Broadcast(new NetStartGame());
        }

    }
    






    private void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn)
    {
        //Receive the message, broadcast it back
        NetMakeMove mm= msg as NetMakeMove;

        //This is where you could do some validation checks!
        // Kiểm tra và cập nhật thời gian còn lại từ client
        //if (mm.teamId == 0)
        //{
        //    whiteTimer.Reset(mm.timeRemaining);
        //}
        //else
        //{
        //    blackTimer.Reset(mm.timeRemaining);
        //}
        mm.timeRemaining = (currentTeam == 0) ? whiteTimer.GetTimeRemaining() : blackTimer.GetTimeRemaining();

        // Thực hiện các kiểm tra khác và gửi lại tin nhắn cho tất cả client
        //Receive, and just broadcast it back
        Server.Instance.Broadcast(msg);
        

    }
    private void OnRematchServer(NetMessage msg, NetworkConnection cnn)
    {
        
        Server.Instance.Broadcast(msg);

    }

    //Client
    private void OnWelcomeClient(NetMessage msg)
    {
        //Receive the connection message
        NetWelcome nw= msg as NetWelcome;

        currentTeam = (currentTeam == 0) ? 1 : 0;
        //assign the team
        currentTeam =nw.AssignedTeam;

        Debug.Log($"My assigned team is {nw.AssignedTeam}");

        if(localGame && currentTeam ==0)
        {
            Server.Instance.Broadcast(new NetStartGame());
        }
    }
    private void OnStartGameClient(NetMessage message)
    {
        // we iust need to change the camera
        GameUI.Instance.ChangeCamera((currentTeam == 0) ? CameraAngle.whiteTeam : CameraAngle.blackTeam);
       
        

        //    GameUI.Instance.menuAnimator.Settrigger("InGameMenu");
    }
    private void OnMakeMoveClient(NetMessage msg)
    {
        //TextTimer.Instance.StartTimer(90);
        NetMakeMove mm= msg as NetMakeMove;
       
        Debug.Log($"MM : {mm.teamId} : {mm.originalX} {mm.originalY} -> {mm.destinationX} {mm.destinationY}");
        
        if (mm.teamId != currentTeam)
        {
            ChessPiece target = chessPieces[mm.originalX, mm.originalY];

            availableMoves = target.GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
            specialMove = target.GetSpecialMoves(ref chessPieces, ref moveList, ref availableMoves);

            // Cập nhật lại thời gian của đội đối thủ dựa trên thông tin từ server
            //if (currentTeam == 0)
            //    blackTimer.Reset(mm.timeRemaining);
            //else
            //    whiteTimer.Reset(mm.timeRemaining);
            if (currentTeam == 0)
                blackTimer.Reset(initialTime);
            else
                whiteTimer.Reset(initialTime);
            MoveTo(mm.originalX,mm.originalY,mm.destinationX,mm.destinationY);
            
        }
    }
    private void OnRematchClient(NetMessage msg)
    {
        //Receive the connection message
        NetRematch rm = msg as NetRematch;

        //Set the boolean for rematch
        playerRematch[rm.teamId] = rm.wantRematch == 1;
        
        //Activete the piece of UI
        if(rm.teamId!= currentTeam)
        {
            rematchIndicator.transform.GetChild((rm.wantRematch == 1) ? 0 : 1).gameObject.SetActive(true);
            if(rm.wantRematch!=1)
            {
                rematchButton.interactable = false;
            }

        }
        //If both wants to rematch
        if (playerRematch[0] && playerRematch[1])
        {

            GameReset();

        }
    }

    //xu ly time
    //private void ResetClock()
    //{
    //    whiteTime = defaultTime;
    //    blackTime=defaultTime;
    //    UpdateTextTime();

    //}

    //private void UpdateTextTime()
    //{
        
    //}

    //
    private void ShutdownRelay()
    {
        Client.Instance.Shutdown();
        Server.Instance.Shutdown();
    }
    private void OnSetLocalGame(bool obj)
    {
        playerCount = -1;
        currentTeam = -1;
        localGame = obj;
    }
    #endregion
}
