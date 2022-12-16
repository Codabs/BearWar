using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameOfPlayers : MonoBehaviour
{
    public TMP_Text text;
    private void Update()
    {
        if (PlayersManager.Instance.NbPlayer == 0) 
        {
            text.text = new string($"List of connected player : \n" +
                $"\n Yourself : {PlayersManager.Instance.PlayersData[0].Pseudo}");
        }
        else if (PlayersManager.Instance.NbPlayer == 1)
        {
            text.text = new string($"List of connected player : \n"+
                $"\n Yourself : {PlayersManager.Instance.PlayersData[0].Pseudo} \n " +
                $"\n Adversary : {PlayersManager.Instance.PlayersData[1].Pseudo}");
        }
    }
}
