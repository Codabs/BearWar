using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_ChoseTileToPoseUnit : GameState
{
    public GameState_ChoseTileToPoseUnit(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        //DeActive UI Shop
        _ctx._uiShop.SetActive(false);
    }
    public override void UpdateState()
    {
        if(Input.GetMouseButtonDown(1))
        {
            _ctx._uiShop.SetActive(true);
            SwitchState(_states.Shop());
        }
    }
    public override void ExitState()
    {

    }
    public override void CheckStateChange()
    {
    }
    public override void InitializeSubState()
    {

    }
    public override void AUnitIsClick(UnitScript unitScript)
    {
    }
    public override void ATileIsClick(BaseTile baseTile)
    {
        if (_ctx._isPlayerTurn)
        {
            if (baseTile.IsPlayerBase)
            {
                //Client.Instance.SendToServer(new NetSpawnUnit());
                //UnitManager.Instance.SpawnUnit(_ctx._unitBought, baseTile);
                _ctx._uiGame.SetActive(true);
                SwitchState(_states.NoAction());
                //Play Buy Sound
            }
        }
        else
        {
            if (baseTile.IsEnnemiBase)
            {
                //UnitManager.Instance.SpawnUnit(_ctx._unitBought, baseTile);
                _ctx._uiGame.SetActive(true);
                SwitchState(_states.NoAction());
                //Play Buy Sound
            }
        }
    }
    public override void ATileIsOverMouse(BaseTile baseTile)
    {
        if(_ctx._isPlayerTurn)
        {
            if(baseTile.IsPlayerBase)
            {
                baseTile._outline.SetActive(true);
            }
        }
        else
        {
            if(baseTile.IsEnnemiBase)
            {
                baseTile._outline.SetActive(true);
            }
        }
    }
    public override void AUnitIsOverMouse(UnitScript unitScript)
    {
    }
    public override void ATileIsExitMouse(BaseTile baseTile)
    {
        baseTile._outline.SetActive(false);
    }
    public override void AUnitIsExitMouse(UnitScript unitScript)
    {
    }
}
