using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_EnnemiTurn : GameState
{
    public GameState_EnnemiTurn(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        Debug.Log("It's Ennemi Turn");
        _ctx._isPlayerTurn = false;
        _ctx._uiEndTurnButton.SetActive(false);
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

    }
    public override void AUnitIsClick(UnitScript unitScript)
    {
    }
    public override void ATileIsClick(BaseTile baseTile)
    {
    }
    public override void ATileIsOverMouse(BaseTile baseTile)
    {
    }
    public override void AUnitIsOverMouse(UnitScript unitScript)
    {
    }
    public override void ATileIsExitMouse(BaseTile baseTile)
    {
    }
    public override void AUnitIsExitMouse(UnitScript unitScript)
    {
    }
}
