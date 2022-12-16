using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_ChoseAction : GameState
{
    public GameState_ChoseAction(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        //Déactiver l'UI_Game
        _ctx._uiGame.SetActive(false);
        //Activer l'UI_Chose

        _ctx._uiButtonWait.SetActive(true);

        _ctx._unitAttackable.Clear();
        _ctx._unitHealable.Clear();
        List<UnitScript> unitsTargetable = new();
        bool _isABaseNear = false;
        foreach (BaseTile tile in _ctx.unit._tileUnitCanSee)
        {
            UnitScript unit = tile._unitOnMe;
            if (unit != null)
            {
                unitsTargetable.Add(unit);
            }
            if(tile.IsEnnemiBase && 
                PathFinderManager.Instance.CalculateDistanceCost
                (_ctx.unit._tileOccupied, tile) <= _ctx.unit._unitInfo.range * 10)
            {
                _isABaseNear = true;
            }
        } 
        bool _isHealer = false;
        _isHealer = _ctx.unit._unitInfo.ScUnit.unitType == UnitType.HEALER;
        foreach (UnitScript unit in unitsTargetable)
        {
            if (UnitManager.Instance.CanUnitBAttackUnitC(_ctx.unit, unit))
            {
                if(_isHealer && _ctx.playerData.unitScripts.Contains(unit))
                {
                    Debug.LogWarning(unit.name);
                    _ctx._unitHealable.Add(unit);
                }
                else if(!_isHealer && !_ctx.playerData.unitScripts.Contains(unit))
                {
                    _ctx._unitAttackable.Add(unit);
                }
            }
        }
        if (_ctx._unitAttackable.Count > 0)
        {
            _ctx._uiButtonFight.SetActive(true);
        }
        if(_ctx._unitHealable.Count > 0)
        {
            //Ne pas affichier si toute les unit heal sont full pv
            _ctx._uiButtonHeal.SetActive(true);
        }
        if(_isABaseNear)
        {
            _ctx._uiButtonFightBase.SetActive(true);
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/ui menu/sound_click_open");
        Debug.Log("The Unit finished moving and the player have to chose a Action");
    }
    public override void UpdateState()
    {
    }
    public override void ExitState()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ui menu/sound_click_close");
        Debug.Log("The Player Chose A Action");
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
        UIManager_Script.Instance.SetTileUI(baseTile);
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
