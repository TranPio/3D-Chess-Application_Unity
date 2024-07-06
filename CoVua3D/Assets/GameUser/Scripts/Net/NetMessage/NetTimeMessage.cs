using Unity.Collections;
using Unity.Networking.Transport;

public class NetTimeMessage : NetMessage
{
    public float TimeRemaining;

    public NetTimeMessage() // Constructor ?? g?i tin nh?n t? client/server
    {
        Code = OpCode.TIME_MESSAGE;
    }

    public NetTimeMessage(DataStreamReader reader) // Constructor ?? nh?n tin nh?n
    {
        Code = OpCode.TIME_MESSAGE;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteFloat(TimeRemaining);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        TimeRemaining = reader.ReadFloat();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_TIME_MESSAGE?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_TIME_MESSAGE?.Invoke(this, cnn);
    }
}
