using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leave : MonoBehaviour
{
    public void _Leave() => Client.Instance.SendToServer(new NetLogout());
}
