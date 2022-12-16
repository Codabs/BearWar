using Unity.Networking.Transport;
public class NetBaseAttack : NetMessage
{
    public int PlayerIndex { set; get; }
    public int indexUnitA { set; get; }
    public NetBaseAttack()
    {
        Code = OpCode.BASE_ATTACK;
    }
    public NetBaseAttack(DataStreamReader reader)
    {
        Code = OpCode.BASE_ATTACK;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(PlayerIndex);
        writer.WriteInt(indexUnitA);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        PlayerIndex = reader.ReadInt();
        indexUnitA = reader.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_BASE_ATTACK?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_BASE_ATTACK?.Invoke(this, cnn);
    }
}
