using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_UnitIsAttacking : GameState
{
    float timer = 0;
    public GameState_UnitIsAttacking(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        //Camera focus the unit
        //UnitManager.Instance._unitSelected.AttackOtherUnit(_ctx._unitAttacked);
        Debug.Log("A Unit Is Attacking");
    }
    public override void UpdateState()
    {
        CheckStateChange();
    }
    public override void ExitState()
    {
        Debug.Log("A Unit has finised of Attacking");
    }
    public override void CheckStateChange()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            _ctx._unitAttacked.DestroyInStyle();
            _ctx._unitAttacked = null;
            //UnitManager.Instance._unitSelected.StopAttacking();
            _ctx.unit.DeSelection();
            SwitchState(_states.NoAction());
        }
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
