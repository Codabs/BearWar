using Unity.Collections;
using Unity.Networking.Transport;
public class NetWelcome : NetMessage
{
    public int AssignedPlayer { set; get; }
    public FixedString32Bytes Pseudo { set; get; }
    public NetWelcome()
    {
        Code = OpCode.WELCOME;
    }
    public NetWelcome(DataStreamReader reader)
    {
        Code = OpCode.WELCOME;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(AssignedPlayer);
        writer.WriteFixedString32(Pseudo);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        AssignedPlayer = reader.ReadInt();
        Pseudo = reader.ReadFixedString32();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_WELCOME?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_WELCOME?.Invoke(this, cnn);
    }
}
