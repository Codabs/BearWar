using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Factory
{
    readonly GameManager _context;

    public GameState_Factory(GameManager _currentContext)
    {
        _context = _currentContext;
    }
    public GameState_PlayerTurn PlayerTurn()
    {
        return new GameState_PlayerTurn(_context, this);
    }
    public GameState_EnnemiTurn EnnemiTurn()
    {
        return new GameState_EnnemiTurn(_context, this);
    }
    public GameState_StartGame StartGame()
    {
        return new GameState_StartGame(_context, this);
    }
    public GameState_NoAction NoAction()
    {
        return new GameState_NoAction(_context, this);
    }
    public GameState_UnitChose UnitChose()
    {
        return new GameState_UnitChose(_context, this);
    }
    public GameState_UnitIsMoving UnitIsMoving()
    {
        return new GameState_UnitIsMoving(_context, this);
    }
    public GameState_Shop Shop()
    {
        return new GameState_Shop(_context, this);
    }
    public GameState_ChoseAction ChoseAction()
    {
        return new GameState_ChoseAction(_context, this);
    }
    public GameState_ChoseEnnemiToAttack ChoseAttack()
    {
        return new GameState_ChoseEnnemiToAttack(_context, this);
    }
    public GameState_ChoseUnitToHeal ChoseHeal()
    {
        return new GameState_ChoseUnitToHeal(_context, this);
    }
}
