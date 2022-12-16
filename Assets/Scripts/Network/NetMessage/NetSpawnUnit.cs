using Unity.Networking.Transport;
public class NetSpawnUnit : NetMessage
{
    public int indexPlayer { set; get; }
    public int indexTypeUnit { set; get; }
    public int TileX { set; get; }
    public int TileY { set; get; }

    public NetSpawnUnit()
    {
        Code = OpCode.SPAWN_UNIT;
    }
    public NetSpawnUnit(DataStreamReader reader)
    {
        Code = OpCode.SPAWN_UNIT;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(indexPlayer);
        writer.WriteInt(indexTypeUnit);
        writer.WriteInt(TileX);
        writer.WriteInt(TileY);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        indexPlayer = reader.ReadInt();
        indexTypeUnit = reader.ReadInt();
        TileX = reader.ReadInt();
        TileY = reader.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_SPAWN_UNIT?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_SPAWN_UNIT?.Invoke(this, cnn);
    }
}
