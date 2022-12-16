using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_StartGame : GameState
{
    public GameState_StartGame(GameManager _currentContext, GameState_Factory _states) : base(_currentContext, _states)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        Debug.Log("The Game has Started");
        //On génére un Grid
        GridManager.Instance.GenerateGrid();

        //Add Units
        _ctx.playerData.unitScripts.Clear();
        UnitManager.Instance.SpawnUnit(0, 0, 10, 2);
        UnitManager.Instance.SpawnUnit(0, 1, 12, 2);
        UnitManager.Instance.SpawnUnit(1, 0, 10, 18);
        UnitManager.Instance.SpawnUnit(1, 1, 8, 18);

        //On Update le fog pour les units
        FogManager.Instance.RestoreFog();
        FogManager.Instance.UpdateFogOnAllUnit(_ctx.unitScripts);

        _ctx.Money = 1500;
        _ctx.Score = 0;
        _ctx.IsTheShopUsed = false;

        //On Donne le Tours au Joueur
        if (_ctx.playerData.Index == 0)
        {
            SwitchState(_states.PlayerTurn());
        }
        else if (_ctx.playerData.Index == 1)
        {
            SwitchState(_states.EnnemiTurn());
        }
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        Debug.Log("Lancement de la partie fini");
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
