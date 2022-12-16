using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinderManager : MonoBehaviour
{
    //
    //Variable
    //

    [SerializeField] private int Move_Diagonal_Cost;
    [SerializeField] private int Move_Forward_Cost;
    public bool _showA = false;
    public static PathFinderManager Instance;
    private List<BaseTile> _openList;
    private List<BaseTile> _closedList;

    //
    //GETTER AND SETTER
    //

    //public BaseTile TargetTile { set { _targetTile = value; } }

    //
    //MONOBEHAVIOUR
    //

    private void Awake()
    {
        Instance = this;
    }

    //
    //Fonction PUBLIC
    //

    public List<BaseTile> PathFinding(BaseTile startTile, BaseTile endTile)
    {
        if (endTile.IsWalkable == false || endTile._unitOnMe != null && !endTile._isInvisible) return null;
        if (startTile == endTile)
        {
            List<BaseTile> tile = new();
            tile.Add(endTile);
            return tile;
        }
        _openList = new List<BaseTile> { startTile };
        _closedList = new List<BaseTile>();
        foreach (BaseTile _tile in GridManager.Instance.Tiles.Values)
        {
            if (_tile != null)
            {
                //Reset la grid
                _tile.G_Cost = int.MaxValue;
                _tile.CalculateFCost();
                _tile._cameFromTile = null;
            }
        }

        startTile.G_Cost = 0;
        startTile.H_Cost = CalculateDistanceCost(startTile, endTile);
        startTile.CalculateFCost();
        //GridManager.Instance.ErasedPathFinding();
        int resctrition = 0;
        while (_openList.Capacity > 0 && resctrition < 50)
        {
            resctrition++;
            BaseTile _currentTile = GetLowestFCostNode(_openList);
            if(_currentTile == endTile)
            {
                //This is the End
                var path = CalculatePath(endTile);
                if (_showA) SetAllColorGreen(path);
                return path;
            }

            _openList.Remove(_currentTile);
            _closedList.Add(_currentTile);

            foreach (BaseTile neighborTile in GridManager.Instance.GetNeighborTiles(_currentTile))
            {
                if (_closedList.Contains(neighborTile)) continue;
                if (!neighborTile.IsWalkable || neighborTile._unitOnMe != null && !neighborTile._isInvisible)
                {
                    _closedList.Add(neighborTile);
                    continue;
                }
                float tentativeGCost = CalculateDistanceCost(_currentTile, neighborTile);
                if(tentativeGCost < neighborTile.G_Cost)
                {
                    neighborTile._cameFromTile = _currentTile;
                    neighborTile.G_Cost = neighborTile.IsMountain? tentativeGCost + _currentTile.G_Cost + 10 : tentativeGCost + _currentTile.G_Cost + 0;
                    neighborTile.H_Cost = CalculateDistanceCost(neighborTile, endTile);
                    neighborTile.CalculateFCost();
                    if(_showA) neighborTile.ShowPathFinding();
                    if (!_openList.Contains(neighborTile))
                    {
                        _openList.Add(neighborTile);
                    }
                }

            }
        }
        //Theirs is no Path
        //Debug.Log("NoPath");
        return null;
    }

    public void ShowCost()
    {
        if (_showA)
        {
            _showA = false;
        }
        else
        {
            _showA = true;
        }
    }

    public int CalculateDistanceCost(BaseTile a, BaseTile b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remining = Mathf.Abs(xDistance - yDistance);
        return Move_Diagonal_Cost * Mathf.Min(xDistance, yDistance) + Move_Forward_Cost * remining;
    }
    //
    //FONCTION PRIVEE
    //
    private BaseTile GetLowestFCostNode(List<BaseTile> _tileList)
    {
        if (_tileList[0] == null) return null;
        BaseTile lowestFCost = _tileList[0];
        if (_tileList.Count == 0 || _tileList.Count == -1) return lowestFCost;
        for (int i = 1; i < _tileList.Count; i++)
        {
            if (_tileList[i].F_Cost < lowestFCost.F_Cost)
            {
                lowestFCost = _tileList[i];
            }
        }
        return lowestFCost;
    }
    private List<BaseTile> CalculatePath(BaseTile _endTile)
    {
        List<BaseTile> path = new();
        path.Add(_endTile);
        BaseTile currentTile = _endTile;
        while(currentTile._cameFromTile != null)
        {
            path.Add(currentTile._cameFromTile);
            currentTile = currentTile._cameFromTile;
        }
        path.Reverse();
        return path;
    }

    private void SetAllColorGreen(List<BaseTile> path)
    {
        foreach(BaseTile tile in path)
        {
            if (tile != null) tile.SetColorToGreen();
        }
    }
}
