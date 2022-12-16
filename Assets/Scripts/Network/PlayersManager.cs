using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersManager : SingletonND<PlayersManager>
{
    #region Variables

    public string PlayerName { get; private set; } = "Player";
    public int NbPlayer { get; set; } = -1; // how many player there is (whithout host)
    public List<PlayerData> PlayersData = new List<PlayerData>(); // all data for each players.
    public PlayerData MyData;

    #endregion

    #region Fonctions

    public void SetPseudo(string pseudo) => PlayerName = pseudo;
    public void DestroyUnit(UnitData unitData)
    {
        foreach (PlayerData data in PlayersData)
        {
            if (data.unitsData.Contains(unitData))
            {
                Destroy(unitData._GameObject);
                data.unitsData.Remove(unitData);
            }
        }
    }
    #endregion

    [System.Serializable] public class PlayerData
    {
        [SerializeField, Header("Player")] private FixedString32Bytes pseudo = "Player";
        public FixedString32Bytes Pseudo
        {
            set { pseudo = value; }
            get { return pseudo; }
        }
        [SerializeField] private int index;
        public int Index
        {
            set { index = value; }
            get { return index; }
        }
        [SerializeField] private int money = 1500;
        public int Money
        {
            set { money = value; }
            get { return money; }
        }

        [SerializeField] private int baseHP = 25;
        public int BaseHP
        {
            set { baseHP = value; }
            get { return baseHP; }
        }

        [Space(5), Header("Unitée(s)")] public List<UnitData> unitsData = new List<UnitData>();
        public List<UnitScript> unitScripts = new List<UnitScript>();
    }

    private void Start()
    {
        RegisterEvents();
    }
    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region Network

    void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.C_WELCOME += OnWelcomeClient;

        NetUtility.S_START += OnStartServer;
        NetUtility.C_START += OnStartClient;

        NetUtility.S_SWITCH_TURN += OnSwitchTurnServer;
        NetUtility.C_SWITCH_TURN += OnSwitchTurnClient;

        NetUtility.S_SPAWN_UNIT += OnSpawnUnitServer;
        NetUtility.C_SPAWN_UNIT += OnSpawnUnitClient;

        NetUtility.S_MOVE_UNIT += OnMoveUnitServer;
        NetUtility.C_MOVE_UNIT += OnMoveUnitClient;

        NetUtility.S_ATTACK_UNIT += OnAttackUnitServer;
        NetUtility.C_ATTACK_UNIT += OnAttackUnitClient;

        NetUtility.S_HEAL_UNIT += OnHealUnitServer;
        NetUtility.C_HEAL_UNIT += OnHealUnitClient;

        NetUtility.S_BASE_ATTACK += OnBaseAttackServer;
        NetUtility.C_BASE_ATTACK += OnBaseAttackClient;

        NetUtility.S_LOGOUT += OnLogoutServer;
        NetUtility.C_LOGOUT += OnLogoutClient;
    }

    void UnregisterEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.C_WELCOME -= OnWelcomeClient;

        NetUtility.S_START -= OnStartServer;
        NetUtility.C_START -= OnStartClient;

        NetUtility.S_SWITCH_TURN -= OnSwitchTurnServer;
        NetUtility.C_SWITCH_TURN -= OnSwitchTurnClient;

        NetUtility.S_SPAWN_UNIT -= OnSpawnUnitServer;
        NetUtility.C_SPAWN_UNIT -= OnSpawnUnitClient;

        NetUtility.S_MOVE_UNIT -= OnMoveUnitServer;
        NetUtility.C_MOVE_UNIT -= OnMoveUnitClient;

        NetUtility.S_ATTACK_UNIT -= OnAttackUnitServer;
        NetUtility.C_ATTACK_UNIT -= OnAttackUnitClient;

        NetUtility.S_HEAL_UNIT -= OnHealUnitServer;
        NetUtility.C_HEAL_UNIT -= OnHealUnitClient;

        NetUtility.S_BASE_ATTACK -= OnBaseAttackServer;
        NetUtility.C_BASE_ATTACK -= OnBaseAttackClient;

        NetUtility.S_LOGOUT -= OnLogoutServer;
        NetUtility.C_LOGOUT -= OnLogoutClient;
    }

    void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        NetWelcome nw = msg as NetWelcome;

        // if game already have an adversary, refuse the connection
        if (NbPlayer >= 1)
        {
            Server.Instance.SendToClient(cnn, new NetLogout());
            return;
        }

        // add 1 to nb of player and tell the connection his assigned index
        nw.AssignedPlayer = ++NbPlayer;

        // create new data of connection for the host
        if (cnn != Client.Instance.connection)
        {
            PlayerData _playerData = new PlayerData();
            PlayersData.Add(_playerData);
            _playerData.Index = 1;
            _playerData.Pseudo = nw.Pseudo;
        }

        // tell connection our pseudo
        if (PlayerName == "" || PlayerName == "Player" && cnn != Client.Instance.connection) nw.Pseudo = "ADVERSARY";
        else nw.Pseudo = PlayerName;

        Server.Instance.SendToClient(cnn, nw);
    }
    void OnWelcomeClient(NetMessage msg)
    {
        Debug.Log($"Received {msg}");
        NetWelcome nw = msg as NetWelcome;

        if (nw.AssignedPlayer != 0) 
        {
            for (int i = 0; i < 2; i++)
            {
                PlayerData _playerData = new PlayerData();
                PlayersData.Add(_playerData);
                _playerData.Index = i;
                if (nw.AssignedPlayer == i) _playerData.Pseudo = PlayerName;
                else _playerData.Pseudo = nw.Pseudo;
            }
        } // for client
        else 
        {
            PlayerData _playerData = new PlayerData();
            PlayersData.Add(_playerData);
            _playerData.Index = 0;
            _playerData.Pseudo = PlayerName;
        } // for host

        MyData = PlayersData[nw.AssignedPlayer];
        if (nw.AssignedPlayer == 0) ConnectInterface.Instance.WaintingScreenHost();
        else ConnectInterface.Instance.WaintingScreenClient();
    }

    void OnStartServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    void OnStartClient(NetMessage msg)
    {
        foreach (var item in PlayersData)
        {
            item.Money = 1500;
        }

        SceneManager.LoadScene(1);
        
    }

    void OnSwitchTurnServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    void OnSwitchTurnClient(NetMessage msg)
    {
        if (GameManager.Instance._isPlayerTurn)
        {
            GameManager.Instance.EndTurn();
        }
        else GameManager.Instance.StartTurn();
    }

    void OnSpawnUnitServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    void OnSpawnUnitClient(NetMessage msg)
    {
        NetSpawnUnit nsu = msg as NetSpawnUnit;
        UnitManager.Instance.SpawnUnit(nsu.indexPlayer, nsu.indexTypeUnit, nsu.TileX, nsu.TileY);
    }

    void OnMoveUnitServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    void OnMoveUnitClient(NetMessage msg)
    {
        NetMoveUnit nmu = msg as NetMoveUnit;
        UnitScript unit = UnitManager.Instance.FindUnitData(nmu.indexTypeUnit)._GameObject.GetComponent<UnitScript>();
        StartCoroutine(unit.MoveUnitInAPath(GridManager.Instance.GetTileAtPosition(new Vector2(nmu.TileX, nmu.TileY))));
    }

    void OnAttackUnitServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    void OnAttackUnitClient(NetMessage msg)
    {
        NetAttackUnit nsu = msg as NetAttackUnit;
        SceneCombatController.Instance.Attaque(UnitManager.Instance.FindUnitData(nsu.indexUnitA), UnitManager.Instance.FindUnitData(nsu.indexUnitD), nsu.Distance);
    }

    void OnHealUnitServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    void OnHealUnitClient(NetMessage msg)
    {
        NetHeal nh = msg as NetHeal;
        SceneCombatController.Instance.Soin(UnitManager.Instance.FindUnitData(nh.indexUnitH));
    }

    void OnBaseAttackServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    void OnBaseAttackClient(NetMessage msg)
    {
        NetBaseAttack nba = msg as NetBaseAttack;

        PlayersData[nba.PlayerIndex].BaseHP -= UnitManager.Instance.FindUnitData(nba.indexUnitA).damage;
        if (PlayersData[nba.PlayerIndex].BaseHP <= 0 && MyData != PlayersData[nba.PlayerIndex])
        {
            //loose
            SceneManager.LoadScene(3);
        }
        else if (PlayersData[nba.PlayerIndex].BaseHP <= 0 && MyData == PlayersData[nba.PlayerIndex])
        {
            //win
            SceneManager.LoadScene(2);
        }
    }

    private void OnLogoutServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    private void OnLogoutClient(NetMessage msg)
    {
        Client.Instance.ShutDown();
        Server.Instance.ShutDown();
    }

    private void OnApplicationQuit()
    {
        Client.Instance.SendToServer(new NetLogout());
    }

    #endregion
}
