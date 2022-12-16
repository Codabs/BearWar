using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_ChoseEnnemiToAttack : GameState
{
    public GameState_ChoseEnnemiToAttack(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        //Retirer l'UI
        _ctx._uiGame.SetActive(true);
        _ctx._uiEndTurnButton.SetActive(false);

        Debug.Log("The Player have to chose a Ennemi");
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        //On retire l'UI qui nous donne les info sur l ennemi
        UIManager_Script.Instance.RemoveEnnemiSelect();
        _ctx._uiEndTurnButton.SetActive(true);
    }
    public override void CheckStateChange()
    {
    }
    public override void InitializeSubState()
    {

    }
    public override void AUnitIsClick(UnitScript unitScript)
    {
        if (_ctx._unitAttackable.Contains(unitScript))
        {
            NetAttackUnit netAttackUnit = new NetAttackUnit();
            netAttackUnit.indexUnitA = _ctx.unit._unitInfo.Index;
            netAttackUnit.indexUnitD = unitScript._unitInfo.Index;
            netAttackUnit.Distance = PathFinderManager.Instance.CalculateDistanceCost(_ctx.unit._tileOccupied, unitScript._tileOccupied) / 10;
            _ctx._uiGame.SetActive(true);
            SwitchState(_states.NoAction());
            Client.Instance.SendToServer(netAttackUnit);
        }
        else
        {
            unitScript.StartCoroutine("DeactivateColliderFor5sec");
        }
    }
    public override void ATileIsClick(BaseTile baseTile)
    {

    }
    public override void ATileIsOverMouse(BaseTile baseTile)
    {
    }
    public override void AUnitIsOverMouse(UnitScript unitScript)
    {
        if (!unitScript.IsInvisible && _ctx._unitAttackable.Contains(unitScript)) 
        {
            UIManager_Script.Instance.SetEnnemiSelect(unitScript);
            unitScript._outline.SetActive(true); 
        }
    }
    public override void ATileIsExitMouse(BaseTile baseTile)
    {
    }
    public override void AUnitIsExitMouse(UnitScript unitScript)
    {
        unitScript._outline.SetActive(false);
    }
}
