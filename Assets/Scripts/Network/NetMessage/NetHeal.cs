using Unity.Networking.Transport;
public class NetHeal : NetMessage
{
    public int indexUnitH { set; get; }
    public NetHeal()
    {
        Code = OpCode.HEAL_UNIT;
    }
    public NetHeal(DataStreamReader reader)
    {
        Code = OpCode.HEAL_UNIT;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(indexUnitH);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        indexUnitH = reader.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_HEAL_UNIT?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_HEAL_UNIT?.Invoke(this, cnn);
    }
}
