using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public void Resume()
    {
        gameObject.SetActive(false);
    }
    public void Option()
    {

    }
    public void Logout()
    {
        Client.Instance.SendToServer(new NetLogout());
    }
}
