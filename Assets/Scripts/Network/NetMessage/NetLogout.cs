using Unity.Networking.Transport;
public class NetLogout : NetMessage
{
    public NetLogout()
    {
        Code = OpCode.LOGOUT;
    }
    public NetLogout(DataStreamReader reader)
    {
        Code = OpCode.LOGOUT;
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
        NetUtility.C_LOGOUT?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_LOGOUT?.Invoke(this, cnn);
    }
}
