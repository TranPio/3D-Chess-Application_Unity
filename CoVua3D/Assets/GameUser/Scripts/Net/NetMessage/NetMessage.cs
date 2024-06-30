using System.IO;
using System.Net.Sockets;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetMessage 
{
    public OpCode Code { set; get; }
    public virtual void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
    }

    protected virtual void SerializeMessageData(ref DataStreamWriter writer)
    {
        // Subclasses should implement this method to serialize their specific data
    }
    public virtual void Deserialize (DataStreamReader reader)
    {

    }
    public virtual void ReceivedOnClient( )
    {

    }
    public virtual void ReceivedOnServer(NetworkConnection con) 
    {   

    }
}
