using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_PlayerTurn : GameState
{
    public GameState_PlayerTurn (GameManager _currentContext, GameState_Factory _factory) : base (_currentContext, _factory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        Debug.Log("It's Player Turn");
        _ctx._isPlayerTurn = true;
        _ctx._uiEndTurnButton.SetActive(true);
        _ctx.IsTheShopUsed = false;
        foreach (UnitScript unit in _ctx.unitScripts)
        {
            unit._hadMove = false;
        }
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
    }
    public override void CheckStateChange()
    {

    }
    public override void InitializeSubState()
    {
        SetSubState(_states.NoAction());
    }
    public override void AUnitIsClick(UnitScript unitScript)
    {
        SubState.AUnitIsClick(unitScript);
    }
    public override void ATileIsClick(BaseTile baseTile)
    {
        SubState.ATileIsClick(baseTile);
    }
    public override void ATileIsOverMouse(BaseTile baseTile)
    {
        SubState.ATileIsOverMouse(baseTile);
    }
    public override void AUnitIsOverMouse(UnitScript unitScript)
    {
        SubState.AUnitIsOverMouse(unitScript);
    }
    public override void ATileIsExitMouse(BaseTile baseTile)
    {
        SubState.ATileIsExitMouse(baseTile);
    }
    public override void AUnitIsExitMouse(UnitScript unitScript)
    {
        SubState.AUnitIsExitMouse(unitScript);
    }
}
