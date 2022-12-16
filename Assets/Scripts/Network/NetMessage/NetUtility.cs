using System;
using UnityEngine;
using Unity.Networking.Transport;


public enum OpCode
{
    KEEP_ALIVE = 1,
    WELCOME = 2,
    LOGOUT = 3,
    START = 4,
    SWITCH_TURN = 5,
    MOVE_UNIT = 6,
    SPAWN_UNIT = 7,
    ATTACK_UNIT = 8,
    HEAL_UNIT = 9,
    BASE_ATTACK = 10,
}


public static class NetUtility
{
    public static void OnData(DataStreamReader stream, NetworkConnection cnn, Server server = null)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();
        switch (opCode)
        {
            case OpCode.KEEP_ALIVE: msg = new NetKeepAlive(stream); break;
            case OpCode.WELCOME: msg = new NetWelcome(stream); break;
            case OpCode.LOGOUT: msg = new NetLogout(stream); break;
            case OpCode.START: msg = new NetStart(stream); break;
            case OpCode.SWITCH_TURN: msg = new NetSwitchTurn(stream); break;
            case OpCode.MOVE_UNIT: msg = new NetMoveUnit(stream); break;
            case OpCode.SPAWN_UNIT: msg = new NetSpawnUnit(stream); break;
            case OpCode.ATTACK_UNIT: msg = new NetAttackUnit(stream); break;
            case OpCode.HEAL_UNIT: msg = new NetHeal(stream); break;
            case OpCode.BASE_ATTACK: msg = new NetBaseAttack(stream); break;

            default:
                Debug.LogError("Message received had no OpCode");
                break;
        }

        if (server != null)
            msg.ReceivedOnServer(cnn);
        else
            msg.ReceivedOnClient();
    }

    // Net Messages
    public static Action<NetMessage> C_KEEP_ALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage> C_LOGOUT;
    public static Action<NetMessage> C_START;
    public static Action<NetMessage> C_SWITCH_TURN;
    public static Action<NetMessage> C_MOVE_UNIT;
    public static Action<NetMessage> C_SPAWN_UNIT;
    public static Action<NetMessage> C_ATTACK_UNIT;
    public static Action<NetMessage> C_HEAL_UNIT;
    public static Action<NetMessage> C_BASE_ATTACK;
    public static Action<NetMessage, NetworkConnection> S_KEEP_ALIVE;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_LOGOUT;
    public static Action<NetMessage, NetworkConnection> S_START;
    public static Action<NetMessage, NetworkConnection> S_SWITCH_TURN;
    public static Action<NetMessage, NetworkConnection> S_MOVE_UNIT;
    public static Action<NetMessage, NetworkConnection> S_SPAWN_UNIT;
    public static Action<NetMessage, NetworkConnection> S_ATTACK_UNIT;
    public static Action<NetMessage, NetworkConnection> S_HEAL_UNIT;
    public static Action<NetMessage, NetworkConnection> S_BASE_ATTACK;
}
