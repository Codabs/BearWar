using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Win : GameState
{
    public GameState_Win(GameManager _currentContext, GameState_Factory _Currentfactory) : base(_currentContext, _Currentfactory)
    {
        IsRootState = true;
    }
    public override void EnterState()
    {
        _ctx._uiGame.SetActive(false);
        //_ctx._uiWin.SetActive(true);
    }

    public override void ExitState()
    {

    }
    public override void ATileIsClick(BaseTile baseTile)
    {
    }

    public override void ATileIsExitMouse(BaseTile baseTile)
    {
    }

    public override void ATileIsOverMouse(BaseTile baseTile)
    {
    }

    public override void AUnitIsClick(UnitScript unitScript)
    {
    }

    public override void AUnitIsExitMouse(UnitScript unitScript)
    {
    }

    public override void AUnitIsOverMouse(UnitScript unitScript)
    {
    }

    public override void CheckStateChange()
    {
    }
    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {

    }
}
