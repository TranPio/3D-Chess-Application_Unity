using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Server : MonoBehaviour
{
    #region Singleton implementation
    public static Server Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public NetworkDriver driver;
    private NativeList<NetworkConnection> connections;

    private bool isActive = false;
    private const float keepAliveTickRate = 20.0f;
    private float lastKeepAlive;

    //Methods
    public Action connectionDropped;
    public void Init(ushort port)
    {
        //init the driver
        driver = NetworkDriver.Create();
        NetworkEndpoint endpoint = NetworkEndpoint.AnyIpv4;
        endpoint.Port = port;

        if (driver.Bind(endpoint) != 0)
        {
            Debug.Log("Unable to bind on port " + endpoint.Port);
            return;
        }
        else
        {
            driver.Listen();

            Debug.Log("Currently listening on port " + endpoint.Port);
        }

        connections = new NativeList<NetworkConnection>(2, Allocator.Persistent);
        isActive = true;
    }
    public void Shutdown()
    {
        if (isActive)
        {
            driver.Dispose();

            connections.Dispose();
            isActive = false;
        }
    }
    public void OnDestroy()
    {
        Shutdown();
    }

    public void Update()
    {
        if ((!isActive))
        {
            return;
        }
        //Duy tri su song
        KeepAlive();

        driver.ScheduleUpdate().Complete();//cap nhat lich trinh dieu khien

        CleanupConnections();//
        AcceptNewConnections();
        UpdateMessagePump();


    }
    private void KeepAlive()
    {
        if ((Time.time - lastKeepAlive > keepAliveTickRate))
        {

            lastKeepAlive = Time.time;
            Broadcast(new NetKeepAlive());

        }
    }
    private void CleanupConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }

        }
    }
    private void AcceptNewConnections()
    {
        //accept new connections
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
        }

    }
    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    NetUtility.OnData(stream, connections[i], this);

                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnect from server");
                    connections[i] = default(NetworkConnection);
                    connectionDropped?.Invoke();
                    Shutdown(); //this does not happen usually, its just because we're in a two person game

                }
            }
        }
    }

    //Server specific
    public void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    private ChessBoard chessBoard; // Tham chiếu đến ChessBoard

    public void SetChessBoardReference(ChessBoard board)
    {
        chessBoard = board;
    }

    // Phương thức xử lý di chuyển từ client gửi tới server
    private void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn)
    {
        NetMakeMove mm = msg as NetMakeMove;

        // Cập nhật thời gian còn lại từ client
        if (mm.teamId == 0 && chessBoard != null)
        {
            chessBoard.whiteTimer.Reset(mm.timeRemaining);
        }
        else if (mm.teamId == 1 && chessBoard != null)
        {
            chessBoard.blackTimer.Reset(mm.timeRemaining);
        }

        // Gửi thông điệp thời gian cho client
        SendTimeToClient(cnn, mm.timeRemaining);

        // Thực hiện các kiểm tra và broadcast tin nhắn cho tất cả client
        Broadcast(msg);
    }
    public void SendTimeToClient(NetworkConnection connection, float timeRemaining)
    {
        var timeMsg = new NetTimeMessage();
        timeMsg.TimeRemaining = timeRemaining;
        SendToClient(connection, timeMsg);
    }

    public void Broadcast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].IsCreated)
            {
                //Debug.Log($"Sending {msg.Code} to : {connections[i].InternalId}");
                SendToClient(connections[i], msg);
            }
        }
    }



}

