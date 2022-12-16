using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_ChoseUnitToHeal : GameState
{
    public GameState_ChoseUnitToHeal(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        //Retirer l'UI
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
        if(_ctx._unitHealable.Contains(unitScript))
        {
            NetHeal netHealUnit = new NetHeal();
            netHealUnit.indexUnitH = unitScript._unitInfo.Index;
            Client.Instance.SendToServer(netHealUnit);
            _ctx._uiGame.SetActive(true);
            SwitchState(_states.NoAction());
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
        if (!unitScript.IsInvisible && _ctx._unitHealable.Contains(unitScript)) unitScript._outline.SetActive(true);
    }
    public override void ATileIsExitMouse(BaseTile baseTile)
    {
    }
    public override void AUnitIsExitMouse(UnitScript unitScript)
    {
        unitScript._outline.SetActive(false);
    }
}
