using Unity.Collections;
using Unity.Networking.Transport;
public class NetMoveUnit : NetMessage
{
    public int indexTypeUnit { set; get; }
    public int TileX { set; get; }
    public int TileY { set; get; }
    public NetMoveUnit()
    {
        Code = OpCode.MOVE_UNIT;
    }
    public NetMoveUnit(DataStreamReader reader)
    {
        Code = OpCode.MOVE_UNIT;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(indexTypeUnit);
        writer.WriteInt(TileX);
        writer.WriteInt(TileY);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        indexTypeUnit = reader.ReadInt();
        TileX = reader.ReadInt();
        TileY = reader.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_MOVE_UNIT?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_MOVE_UNIT?.Invoke(this, cnn);
    }
}
