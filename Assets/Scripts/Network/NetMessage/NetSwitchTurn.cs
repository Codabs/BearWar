using Unity.Networking.Transport;
public class NetSwitchTurn : NetMessage
{
    public NetSwitchTurn()
    {
        Code = OpCode.SWITCH_TURN;
    }
    public NetSwitchTurn(DataStreamReader reader)
    {
        Code = OpCode.SWITCH_TURN;
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
        NetUtility.C_SWITCH_TURN?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_SWITCH_TURN?.Invoke(this, cnn);
    }
}
