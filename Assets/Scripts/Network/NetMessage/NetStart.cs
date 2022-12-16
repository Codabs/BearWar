using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetStart : NetMessage
{
    public NetStart()
    {
        Code = OpCode.START;
    }
    public NetStart(DataStreamReader reader)
    {
        Code = OpCode.START;
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
        NetUtility.C_START?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_START?.Invoke(this, cnn);
    }
}
