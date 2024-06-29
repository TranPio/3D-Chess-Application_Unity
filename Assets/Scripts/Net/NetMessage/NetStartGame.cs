using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using System.Net.Sockets;

public class NetStartGame : NetMessage
{
    public NetStartGame()
    {
        Code = OpCode.START_GAME;

    }
    public NetStartGame(DataStreamReader reader)
    {
        Code = OpCode.START_GAME;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);

    }
    public override void Deserialize(DataStreamReader reader)
    {
        
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_START_GAME?.Invoke(this);

    }
    public override void ReceivedOnServer(NetworkConnection con)
    {
        NetUtility.S_START_GAME?.Invoke(this, con);
    }
}
