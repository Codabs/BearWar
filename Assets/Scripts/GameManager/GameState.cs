using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    //
    //Variable
    //

    protected bool IsRootState = false;
    protected GameManager _ctx;
    protected GameState_Factory _states;
    protected GameState _currentSuperState;
    protected GameState _currentSubState;
    public GameState SubState { get { return _currentSubState; } }
    public GameState (GameManager _currentContext, GameState_Factory _Currentfactory)
    {
        _ctx = _currentContext;
        _states = _Currentfactory;
    }

    //
    //FONCTION
    //

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckStateChange();
    public abstract void InitializeSubState();
    public abstract void AUnitIsClick(UnitScript unitScript);
    public abstract void ATileIsClick(BaseTile baseTile);
    public abstract void AUnitIsOverMouse(UnitScript unitScript);
    public abstract void ATileIsOverMouse(BaseTile baseTile);
    public abstract void AUnitIsExitMouse(UnitScript unitScript);
    public abstract void ATileIsExitMouse(BaseTile baseTile);
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();
    }
    public void SwitchState(GameState _newState)
    {
        //
        ExitState();
        //
        _newState.EnterState();
        //
        if (IsRootState)
        {
            _ctx._currentGameState = _newState;
        }
        else
        {
            _currentSuperState.SetSubState(_newState);
        }
    }
    public void SetSuperState(GameState _newSuperState)
    {
        _currentSuperState = _newSuperState;
    }
    public void SetSubState(GameState _newSubState)
    {
        _currentSubState = _newSubState;
        _currentSubState.SetSuperState(this);
        _currentSubState.EnterState();
    }
}
