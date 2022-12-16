using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnnemiBaseTile : BaseTile
{
    [SerializeReference] private Sprite _playerBase;
    [SerializeReference] private Sprite _ennemiBase;
    public override void Init(int x, int y)
    {
        if(GameManager.Instance.playerData.Index == 0)
        {
            if (x == 8 && y == 18) EnnemiBase();
            if (x == 9 && y == 18) EnnemiBase();
            if (x == 10 && y == 18) EnnemiBase();
            if (x == 8 && y == 19) EnnemiBase();
            if (x == 9 && y == 19) EnnemiBase();
            if (x == 10 && y == 19) EnnemiBase();

            if (x == 12 && y == 2) PlayerBase();
            if (x == 11 && y == 2) PlayerBase();
            if (x == 10 && y == 2) PlayerBase();
            if (x == 12 && y == 1) PlayerBase();
            if (x == 11 && y == 1) PlayerBase();
            if (x == 10 && y == 1) PlayerBase();
        }
        else if (GameManager.Instance.playerData.Index == 1)
        {
            if (x == 8 && y == 18) PlayerBase();
            if (x == 9 && y == 18) PlayerBase();
            if (x == 10 && y == 18) PlayerBase();
            if (x == 8 && y == 19) PlayerBase();
            if (x == 9 && y == 19) PlayerBase();
            if (x == 10 && y == 19) PlayerBase();

            if (x == 12 && y == 2) EnnemiBase();
            if (x == 11 && y == 2) EnnemiBase();
            if (x == 10 && y == 2) EnnemiBase();
            if (x == 12 && y == 1) EnnemiBase();
            if (x == 11 && y == 1) EnnemiBase();
            if (x == 10 && y == 1) EnnemiBase();
        }
    }
    private void PlayerBase()
    {
        IsPlayerBase = true;
        IsEnnemiBase = false;
        spriteRenderer.sprite = _playerBase;
    }
    private void EnnemiBase()
    {
        IsPlayerBase = false;
        IsEnnemiBase = true;
        spriteRenderer.sprite = _ennemiBase;
    }
}
