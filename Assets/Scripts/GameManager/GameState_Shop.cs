using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameState_Shop : GameState
{
    public GameState_Shop(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        //DeActive UI
        UIManager_Script.Instance.RemoveTileSelect();
        UIManager_Script.Instance.RemoveUnitSelect();
        UIManager_Script.Instance.RemoveEnnemiSelect();

        //Active UI Shop
        _ctx._uiShop.SetActive(true);
        //Check button Tank
        int _tank = 250;
        if (_ctx.Money >= _tank && CheckIfThereAreMoreThat2UnitType(UnitType.TANK))
        {
            _ctx._tankBuy.interactable = true;
        }
        else
        {
            _ctx._tankBuy.interactable = false;
        }
        //Check button Cac
        int _cac = 150;
        if (_ctx.Money >= _cac && CheckIfThereAreMoreThat2UnitType(UnitType.CAC))
        {
            _ctx._cacBuy.interactable = true;
        }
        else
        {
            _ctx._cacBuy.interactable = false;
        }
        //Check button Distance
        int _distance = 200;
        if (_ctx.Money >= _distance && CheckIfThereAreMoreThat2UnitType(UnitType.DISTANCE))
        {
            _ctx._distanceBuy.interactable = true;
        }
        else
        {
            _ctx._distanceBuy.interactable = false;
        }
        //Check button Healer
        int _healer = 150;
        if (_ctx.Money >= _healer && CheckIfThereAreMoreThat2UnitType(UnitType.HEALER))
        {
            _ctx._healerBuy.interactable = true;
        }
        else
        {
            _ctx._healerBuy.interactable = true;
        }
        Debug.Log("The Player Is Opening Shop");
    }
    public override void UpdateState()
    {
        if(Input.GetMouseButtonDown(1))
        {
            _ctx._uiShop.SetActive(false);
            SwitchState(_states.NoAction());
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
    private bool CheckIfThereAreMoreThat2UnitType(UnitType type)
    {
        int _numberOfUnitPossed = 0;
        foreach (UnitScript unit in _ctx.unitScripts)
        {
            if (unit._unitInfo.ScUnit.unitType == type && unit != null)
            {
                _numberOfUnitPossed++;
            }
        }
        if(_numberOfUnitPossed < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
