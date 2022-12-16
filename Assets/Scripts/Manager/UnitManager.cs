using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //
    //Variable
    //

    public static UnitManager Instance;
    [SerializeField] private GameObject _unitSelected;
    [SerializeField] private GameObject _heroTank;
    [SerializeField] private GameObject _heroCac;
    [SerializeField] private GameObject _heroDistance;
    [SerializeField] private GameObject _heroHealer;
    [SerializeField] private GameObject _ennemiTank;
    [SerializeField] private GameObject _ennemiCac;
    [SerializeField] private GameObject _ennemiDistance;
    [SerializeField] private GameObject _ennemiHealer;
    int _indexOfUnitSpawn = -1;
    private List<UnitData> _unitDatas = new List<UnitData>(); 

    //
    //GETTER AND SETTER
    //

    //
    //MONOBEHAVIOUR
    //

    private void Awake()
    {
        Instance = this;
    }

    //
    //FONCTION
    //
    public UnitData FindUnitData(int index)
    {
        foreach (var Data in _unitDatas)
        {
            if (Data.Index == index)
            {
                return Data;
            }
        }
        return null;
    }
    /// <summary>
    /// Spawn The Unit
    /// </summary>
    public UnitScript SpawnUnit(int playerIndex, int UnitRef, int TileX, int TileY)
    {
        BaseTile tile = GridManager.Instance.GetTileAtPosition(new Vector2(TileX, TileY));
        GameObject spawnedUnit = null;
        UnitScript script = null;
        if (playerIndex == 0)
        {
            switch (UnitRef)
            {
                case 0:
                    spawnedUnit = Instantiate(_heroTank);
                    break;
                case 1:
                    spawnedUnit = Instantiate(_heroCac);
                    break;
                case 2:
                    spawnedUnit = Instantiate(_heroDistance);
                    break;
                case 3:
                    spawnedUnit = Instantiate(_heroHealer);
                    break;
            }
        }
        else 
        {
            switch (UnitRef)
            {
                case 0:
                    spawnedUnit = Instantiate(_ennemiTank);
                    break;
                case 1:
                    spawnedUnit = Instantiate(_ennemiCac);
                    break;
                case 2:
                    spawnedUnit = Instantiate(_ennemiDistance);
                    break;
                case 3:
                    spawnedUnit = Instantiate(_ennemiHealer);
                    break;
            }
        }
        script = spawnedUnit.GetComponent<UnitScript>();
        Debug.Log($"{PlayersManager.Instance} , {playerIndex} , {script}");
        PlayersManager.Instance.PlayersData[playerIndex].unitScripts.Add(script);

        _unitDatas.Add(script._unitInfo);
        script._unitInfo.Index = ++_indexOfUnitSpawn;

        //init
        spawnedUnit.transform.position = tile.transform.position + script.Offset;
        tile._unitOnMe = script;
        script._tileOccupied = tile;
        script.SetSortingOrder(tile.SpriteRenderer.sortingOrder);

        return script;
    }
    public bool CanUnitBAttackUnitC(UnitScript unitB, UnitScript unitC)
    {
        var tileB = unitB._tileOccupied;
        var tileC = unitC._tileOccupied;
        var Distance = PathFinderManager.Instance.CalculateDistanceCost(tileB, tileC);
        if (Distance <= unitB._unitInfo.range * 10 && !unitC.IsInvisible) return true;
        else return false;
    }
    public int GetUnitCost(int UnitRef)
    {
        int cost = 0;
        switch(UnitRef)
        {
            case 0:
                cost = _heroTank.GetComponent<UnitScript>()._unitInfo.ScUnit.cout;
                break;
            case 1:
                cost = _heroCac.GetComponent<UnitScript>()._unitInfo.ScUnit.cout;
                break;
            case 2:
                cost = _heroDistance.GetComponent<UnitScript>()._unitInfo.ScUnit.cout;
                break;
            case 3:
                cost = _heroHealer.GetComponent<UnitScript>()._unitInfo.ScUnit.cout;
                break;
        }
        return cost;
    }
}
