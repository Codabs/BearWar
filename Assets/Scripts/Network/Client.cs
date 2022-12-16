using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Networking.Transport;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Client : SingletonND<Client>
{

    public NetworkDriver driver;
    public NetworkConnection connection { get; private set; }

    bool isActive = false;

    public Action connectionDropped;

    public void Init(string ip, ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);

        connection = driver.Connect(endpoint); // Local localhost / 127.0.0.1

        Debug.Log("Attemping to connect to server on " + endpoint.Address);

        isActive = true;

        RegisterToEvent();
    }
    public void ShutDown()
    {
        if (isActive)
        {
            UnregisterToEvent();
            driver.Dispose();
            isActive = false;
            connection = default(NetworkConnection);

            SceneManager.LoadScene(0);
            PlayersManager.Instance.PlayersData.Clear();
        }
    }
    public void ForceShutDown()
    {
        isActive = false;
        ShutDown();
    }
    private void OnDestroy()
    {
        ShutDown();
    }

    public void Update()
    {
        if (!isActive)
        {
            return;
        }

        driver.ScheduleUpdate().Complete();
        CheckAlive();

        UpdateMessagePump();
    }
    private void CheckAlive()
    {
        if (!connection.IsCreated && isActive)
        {
            Debug.Log("something went wrong, lost connection to server");
            connectionDropped?.Invoke();
            ShutDown();
        }
    }
    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                NetWelcome nw = new NetWelcome();
                nw.Pseudo = PlayersManager.Instance.PlayerName;
                SendToServer(nw);
                Debug.Log("Connected!");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                NetUtility.OnData(stream, default(NetworkConnection));
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client disconnected from server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke();
                ShutDown();
            }
        }
    }

    public void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        Debug.Log($" Send to server {msg}");
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }

    //event parsing
    private void RegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }
    private void UnregisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }

    private void OnKeepAlive(NetMessage nm)
    {
        // send it back to keep both alive
        SendToServer(nm);
    }
}
