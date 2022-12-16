using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    public static FogManager Instance;
    [SerializeField] private LayerMask _mountainLayer;
    private void Awake()
    {
        Instance = this;
    }
    public void UpdateFogOnAllUnit(List<UnitScript> units)
    {
        foreach(UnitScript unit in units)
        {
            print(unit.name);
            unit._tileUnitCanSee = UpdateFog(unit._unitInfo.vision * 10, unit);
        }
    }

    void Ignore(UnitScript unit)
    {

        _TileToIgnore.Clear();
        NewTileToIgnore.Clear();

        if (!unit._tileOccupied.IsMountain)
            return;

        DebugList.Clear();
        DebugList.Add(unit._tileOccupied);
        _TileToIgnore.Add(unit._tileOccupied);


        int i = 0;
        while (!Equals(DebugList, NewTileToIgnore))
        {
            Debug.Log("b1");
            _TileToIgnore = DebugList.ToList();

            foreach (var Tile in _TileToIgnore)
            {
                Debug.Log("b2");
                Debug.Log(Tile);

                if (!NewTileToIgnore.Contains(Tile))
                {
                    Debug.Log("b3");

                    NewTileToIgnore.Add(Tile);
                    List<BaseTile> _tiles = GridManager.Instance.GetNeighborTiles(Tile);
                    if (_tiles != null)
                        foreach (var _tile in _tiles)
                        {
                            Debug.Log("b4");

                            if (_tile.IsMountain && !_TileToIgnore.Contains(_tile))
                            {
                                DebugList.Add(_tile);
                                Debug.Log(_tile);
                            }
                        }
                }
            }
            i++;
            if (i > 100)
            {
                Debug.LogError("ERREUR");
                break;
            }
        }
    }
    private bool Equals(List<BaseTile> a, List<BaseTile> b)
    {
        if (a == null) return b == null;
        if (b == null || a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }
        return true;
    }

    public List<BaseTile> _TileToIgnore = new List<BaseTile>();
    List<BaseTile> DebugList = new List<BaseTile>();
    public List<BaseTile> NewTileToIgnore = new List<BaseTile>();
    public List<BaseTile> UpdateFog(int RangeOfVision, UnitScript unit)
    {
        List<BaseTile> TileTheUnitCanSee = new();
        Ignore(unit);
        foreach (BaseTile tile in GridManager.Instance.GetAllTilesInSquare(unit._tileOccupied, RangeOfVision))
        {
            Transform t1 = tile.transform;
            Transform t2 = unit._tileOccupied.transform;
            RaycastHit2D[] hit2D = Physics2D.LinecastAll(new(t1.position.x, t1.position.y), new(t2.position.x, t2.position.y), _mountainLayer);
            List<RaycastHit2D> _hit2D = new List<RaycastHit2D>();
            foreach (var item in hit2D)
            {
                Debug.Log($"Hit :{item}");
                _hit2D.Add(item);
            }

            foreach (var hit in hit2D)
            {
                if (_TileToIgnore.Contains(hit.collider.GetComponent<BaseTile>()))
                {
                    _hit2D.Remove(hit);
                }
            }

            if (_hit2D.Count <= 0 || 
               ( tile.IsMountain && _hit2D.Count == 1) )
            {
                tile.IsVisible(true, unit);
                TileTheUnitCanSee.Add(tile);
                Debug.DrawLine(t1.position, t2.position, Color.black, 100f);
                continue;
            }
            if (_hit2D.Count >= 1)
            {
                tile.IsVisible(false, unit);
                Debug.DrawLine(t1.position, t2.position, Color.black, 100f);
                continue;
            }
        }
        return TileTheUnitCanSee;
    }
    public void RestoreFog()
    {
        foreach(BaseTile tile in GridManager.Instance.Tiles.Values)
        {
            tile.ResetVisibility();
        }
    }
}
