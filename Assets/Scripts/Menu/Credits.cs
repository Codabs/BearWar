using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour
{
    public void QuitCredits() { 
        MenuPrincipal mp = FindObjectOfType<MenuPrincipal>();
        if (mp != null)
        {
            mp.gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }
}
