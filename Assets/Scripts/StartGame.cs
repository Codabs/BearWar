using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void _StartGame()
    {
        if(PlayersManager.Instance.NbPlayer == 1)
            Client.Instance.SendToServer(new NetStart());
    }
}
