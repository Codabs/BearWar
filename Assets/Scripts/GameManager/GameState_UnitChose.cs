using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_UnitChose : GameState
{

    //
    //VARIABLE
    //

    private List<BaseTile> TilesWalkable = new List<BaseTile>();
    List<BaseTile> path = new List<BaseTile>();

    //
    //FONCTION PUBLIC
    //
    public GameState_UnitChose(GameManager _currentContext, GameState_Factory _factory) : base(_currentContext, _factory)
    {
        InitializeSubState();
    }
    public override void EnterState()
    {
        if (_ctx.unit == null) SwitchState(_states.NoAction());
        UIManager_Script.Instance.SetUnitSelect(_ctx.unit);
        foreach (BaseTile tile in GridManager.Instance.FindAllOfWalkingTiles(_ctx.unit, _ctx.unit._unitInfo.pm * 10))
        {
            TilesWalkable.Add(tile);
            tile._outlineWalkable.SetActive(true);
        }
        Debug.Log("Player chose a Unit");
    }
    public override void UpdateState()
    {
        if(Input.GetMouseButtonDown(1))
        {
            _ctx.unit = null;
            foreach(BaseTile tile in TilesWalkable)
            {
                tile._outlineWalkable.SetActive(false);
            }
            SwitchState(_states.NoAction());
        }
    }
    public override void ExitState()
    {
        Debug.Log("The Unit Is Doing Somethings");
    }
    public override void CheckStateChange()
    {

    }
    public override void InitializeSubState()
    {

    }
    public override void AUnitIsClick(UnitScript unitScript)
    {
        if(unitScript != _ctx.unit)
        {
            unitScript.StartCoroutine("DeactivateColliderFor5sec");
        }
    }
    public override void ATileIsClick(BaseTile baseTile)
    {
        if(TilesWalkable.Contains(baseTile))
        {
            ClearPath();
            //Nettoyage
            foreach (BaseTile tile in TilesWalkable)
            {
                tile.SpriteRenderer.color = Color.white;
                tile._outline.SetActive(false);
                tile._outlineWalkable.SetActive(false);
            }
            _ctx.tile = baseTile;
            if (baseTile == _ctx.unit._tileOccupied)
            {
                SwitchState(_states.ChoseAction());
            }
            SwitchState(_states.UnitIsMoving());
        }
    }
    public override void ATileIsOverMouse(BaseTile baseTile)
    {
        if (TilesWalkable.Contains(baseTile))
        {
            ClearPath();
            Debug.Log(_ctx.unit._tileOccupied);
            path = PathFinderManager.Instance.PathFinding(_ctx.unit._tileOccupied, baseTile);
            if (path != null)
                _ctx.ShowPathToTile(path);
        }
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

    //
    //PRIVATE FONCTION
    //
    private void ClearPath()
    {
        if (path == null) return;
        foreach (BaseTile tile in path)
            tile._outline.SetActive(false);
    }
}
