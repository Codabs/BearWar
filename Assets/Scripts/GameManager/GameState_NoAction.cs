using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_NoAction : GameState
{
    public GameState_NoAction(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        //Set UI GAME
        _ctx._uiGame.SetActive(true);
        //Update l'UI des Unite
        if (_ctx.unit != null) UIManager_Script.Instance.SetUnitSelect(_ctx.unit);
        //Remove All Other UI
        _ctx._uiShop.SetActive(false);
        //REMOVE ALL HIGHTLIGHT

        Debug.Log("The Player Is Doing Nothing");
    }
    public override void UpdateState()
    {
    }
    public override void ExitState()
    {
        Debug.Log("The Player Chose A Unit");
    }
    public override void CheckStateChange()
    {
    }
    public override void InitializeSubState()
    {

    }
    public override void AUnitIsClick(UnitScript unitScript)
    {
        if (_ctx.unitScripts.Contains(unitScript) && !unitScript._hadMove)
        {
            _ctx.unit = unitScript;
            unitScript.Selection();
            unitScript._outline.SetActive(false);
            SwitchState(_states.UnitChose());
        }
        else
        {
            unitScript.StartCoroutine("DeactivateColliderFor5sec");
        }
    }
    public override void ATileIsClick(BaseTile baseTile)
    {
        if (baseTile.IsPlayerBase && !_ctx.IsTheShopUsed && baseTile._unitOnMe == null)
        {
            baseTile._outline.SetActive(true);
            _ctx.tile = baseTile;
            //-ctx.Camera.SetFocus(basetile.gameobject);
            SwitchState(_states.Shop());
        }
    }
    public override void ATileIsOverMouse(BaseTile baseTile)
    {
        baseTile._outline.SetActive(true);
        UIManager_Script.Instance.SetTileUI(baseTile);
    }
    public override void AUnitIsOverMouse(UnitScript unitScript)
    {
        if (!unitScript.IsInvisible) { 
            unitScript._outline.SetActive(true);
            UIManager_Script.Instance.SetUnitSelect(unitScript);
        }
    }
    public override void ATileIsExitMouse(BaseTile baseTile)
    {
        baseTile._outline.SetActive(false);
    }
    public override void AUnitIsExitMouse(UnitScript unitScript)
    {
        unitScript._outline.SetActive(false);
    }
}
