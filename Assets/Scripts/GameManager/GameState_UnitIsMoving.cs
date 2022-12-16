using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_UnitIsMoving : GameState
{
    public GameState_UnitIsMoving(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        _ctx._uiGame.SetActive(false);

        NetMoveUnit netMoveUnit = new NetMoveUnit();
        netMoveUnit.indexTypeUnit = _ctx.unit._unitInfo.Index;
        netMoveUnit.TileX = _ctx.tile.x;
        netMoveUnit.TileY = _ctx.tile.y;
        if (_ctx._isTesting)
            Client.Instance.SendToServer(netMoveUnit);
        else
            _ctx.MoveUnit(_ctx.unit, _ctx.tile);
        Debug.Log("A Unit Is Moving");
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        Debug.Log("A Unit has finised of moving");
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
