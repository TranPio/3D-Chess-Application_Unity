﻿using System;
using System.Net;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine;

public class Client : MonoBehaviour
{
    #region Singleton implementation
    public static Client Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public NetworkDriver driver;
    private NetworkConnection connection;

    private bool isActive = false;


    public Action connectionDropped;
    //Methods
    public void Init(string ip, ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndpoint endpoint = NetworkEndpoint.Parse(ip, port);

        connection = driver.Connect(endpoint); //LOCAL HOSTING localhost/127.0.0.1
        Debug.Log("Attemping to connect to Server on " + endpoint.Address);

        isActive = true;

        RegisterToEvent();
    }
    public void Shutdown()
    {
        if (isActive)
        {
            UnregisterToEvent();
            driver.Dispose();
            isActive = false;
            connection = default(NetworkConnection);
        }
    }
    public void OnDestroy()
    {
        Shutdown();
    }
    public void Update()
    {
        if (!isActive)
        {
            return;
        }


        driver.ScheduleUpdate().Complete();//cap nhat lich trinh dieu khien
        CheckAlive();

        UpdateMessagePump();


    }
    private void CheckAlive()
    {
        if (!connection.IsCreated && isActive)
        {
            Debug.Log("Something went wrong, lost connection to server");
            connectionDropped?.Invoke();
            Shutdown();
        }
    }
    // Xử lý thời gian cho client

    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                SendToServer(new NetWelcome());
                Debug.Log("we're connected");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                NetUtility.OnData(stream, default(NetworkConnection));
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke();
                Shutdown();
            }
        }


    }

    public void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }

    //Event parsing
    private void RegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
        NetUtility.C_TIME_MESSAGE += OnTimeMessageClient;

    }

    private void UnregisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;

    }
    private void OnKeepAlive(NetMessage nm)
    {
        //Send it back, to keep both side alive
        SendToServer(nm);
    }
    private void OnTimeMessageClient(NetMessage msg)
    {
        NetTimeMessage timeMessage = msg as NetTimeMessage;
        // Xử lý thông tin thời gian nhận được từ Server
        float timeRemaining = timeMessage.TimeRemaining;
        // Cập nhật giao diện người dùng hoặc logic trò chơi dựa trên thời gian nhận được
    }
}

