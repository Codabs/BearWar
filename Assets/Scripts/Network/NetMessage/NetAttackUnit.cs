using Unity.Networking.Transport;
public class NetAttackUnit : NetMessage
{
    public int indexUnitA { set; get; }
    public int indexUnitD { set; get; }
    public int Distance { get; set; }
    public NetAttackUnit()
    {
        Code = OpCode.ATTACK_UNIT;
    }
    public NetAttackUnit(DataStreamReader reader)
    {
        Code = OpCode.ATTACK_UNIT;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(indexUnitA);
        writer.WriteInt(indexUnitD);
        writer.WriteInt(Distance);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        indexUnitA = reader.ReadInt();
        indexUnitD = reader.ReadInt();
        Distance = reader.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_ATTACK_UNIT?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_ATTACK_UNIT?.Invoke(this, cnn);
    }
}
