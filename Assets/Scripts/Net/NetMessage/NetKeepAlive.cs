using System.IO;
using Unity.Collections;
using System.Net.Sockets;

using Unity.Networking.Transport;

public class NetKeepAlive : NetMessage
{
    public NetKeepAlive() // <--making the box
    {
        Code = OpCode.KEEP_ALIVE;

    }
    public NetKeepAlive(DataStreamReader reader) //<--receiving the box
    {
        Code=OpCode.KEEP_ALIVE;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);

    }
    //public override byte[] Serialize()
    //{
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        using (BinaryWriter writer = new BinaryWriter(stream))
    //        {
    //            writer.Write((byte)Code);
    //            // Thêm các dữ liệu khác nếu cần thiết
    //        }
    //        return stream.ToArray();
    //    }
    //}

    public override void Deserialize(DataStreamReader reader)
    {
       
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_KEEP_ALIVE?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_KEEP_ALIVE?.Invoke(this, cnn);
    }
    //public override void ReceivedOnServer(Netconnection cnn)
    //{
    //    NetUtility.S_KEEP_ALIVE?.Invoke(this, cnn);
    //}


}
