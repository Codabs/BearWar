using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Networking.Transport;

public class ConnectInterface : Singleton<ConnectInterface>
{

    public TMP_InputField PortHost;
    ushort portHost;

    public TMP_InputField PortClient;
    ushort portClient;
    public TMP_InputField IP;

    public Server server => Server.Instance;
    public Client client => Client.Instance;

    public GameObject WaitingZone;
    public GameObject WaitingZoneHost;
    public GameObject ActuelZone;

    #region Connection

    public void OnOnlineHostButton()
    {
        if (PortHost.text == "" || PortHost.text == null)
        {
            server.Init(25566);
            client.Init("127.0.0.1", 25566);
        }
        else if(PortHost.text == ushort.Parse(PortHost.text).ToString() && ushort.Parse(PortHost.text) > 1025 && ushort.Parse(PortHost.text) < 65000)
        {
            portHost = ushort.Parse(PortHost.text);

            server.Init(portHost);
            client.Init("127.0.0.1", portHost);
        }

        else Debug.Log("Wrong port");
    }

    public void OnOnlineConnectButton()
    {
        if (PortClient.text == "" || PortClient.text == null)
        {
            portClient = 25566;

            if (IP.text == null || IP.text == "") client.Init("127.0.0.1", 25566);
            else client.Init(IP.text, portClient);
        }
        else if (PortClient.text == ushort.Parse(PortClient.text).ToString() && ushort.Parse(PortClient.text) > 1025 && ushort.Parse(PortClient.text) < 65000)
        {
            portClient = ushort.Parse(PortClient.text);

            if (IP.text == null || IP.text == "") client.Init("127.0.0.1", portClient);
            else client.Init(IP.text, portClient);
        }
        
        else Debug.Log("Wrong IP or port");
    }

    #endregion

    public void WaintingScreenHost()
    {
        WaitingZoneHost.SetActive(true);
        ActuelZone.SetActive(false);
    }
    public void WaintingScreenClient()
    {
        WaitingZone.SetActive(true);
        ActuelZone.SetActive(false);
    }
}
