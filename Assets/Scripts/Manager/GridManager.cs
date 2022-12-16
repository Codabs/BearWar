using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //
    //VARIABLE
    //
    private static GridManager _instance;
    [SerializeField] private Vector3 orgineTile;
    [SerializeField] int[,] _levelDesign = new int[,]
    {
    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    {1, 1, 1, 0, 0, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
    {1, 1, 1, 0, 0, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
    {1, 1, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
    {1, 1, 0, 0, 0, 0, 1, 0, 2, 0, 0, 1, 2, 0, 2, 0, 0, 0, 0, 1, 1},
    {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 2, 0, 2, 0, 0, 0, 0, 0, 1},
    {1, 0, 0, 0, 0, 0, 1, 0, 3, 0, 0, 0, 0, 0, 2, 2, 0, 0, 2, 2, 1},
    {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1},
    {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 4, 4, 1},
    {1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 4, 4, 1},
    {1, 4, 4, 0, 0, 0, 3, 0, 1, 0, 3, 0, 1, 0, 3, 0, 0, 0, 4, 4, 1},
    {1, 4, 4, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
    {1, 4, 4, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    {1, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
    {1, 2, 2, 0, 0, 2, 2, 0, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 0, 0, 1},
    {1, 0, 0, 0, 0, 0, 2, 0, 2, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
    {1, 1, 0, 0, 0, 0, 2, 0, 2, 1, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 1},
    {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 1},
    {1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 0, 0, 1, 1, 1},
    {1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 0, 0, 1, 1, 1},
    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    };
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] private Transform _tileParent;
    [SerializeField] float _tileSize;
    [SerializeField] Vector2 _offSpringY;
    [SerializeField] Vector2 _offSpringX;
    [SerializeField] List<BaseTile> _tileList;
    [SerializeField] GameObject cam;
    private Dictionary<Vector2, BaseTile> _tiles;
    private BaseTile _tileSelectioned;
    public List<BaseTile> _buildingsTiles = new List<BaseTile>();
    [SerializeField] private bool IsDiagonalMovementEnable = false;

    //
    //GETTER AND SETTER
    //

    public static GridManager Instance { get { return _instance; } }
    public int Width { get { return _width; } }
    public int Height { get { return _height; } }

    public Dictionary<Vector2, BaseTile> Tiles { get { return _tiles; } }
    public BaseTile TileSelectioned { get { return _tileSelectioned; } set { _tileSelectioned = value; } }

    //
    //MONOBEHAVIOUR
    //

    private void Awake()
    {
        _instance = this;
    }

    //
    //PUBLIC FONCTION
    //

    public void GenerateGrid()
    {
        //Je detruit l'ancienne grille
        DestroyGridAndCleanTheDictionary();

        //var _offSpring = new Vector2(0, 0);
        //On crée les tiles
        for (int x = 0; x < _width; x++)
        {
            var posX = x * _offSpringX; 
            for (int y = 0; y < _height; y++)
            {
                //Choisir un tile aléatoire
                var _newTile = _tileList[_levelDesign[x,y]];

                //On calcule ou la créer
                var posY = y * _offSpringY;

                Vector3 pos = new(posX.x + posY.x, posX.y + posY.y);
                //On fait spawn une Tile
                
                var spawnedTile = Instantiate(_newTile, pos, Quaternion.identity, _tileParent);

                //On change son nom dans l'editeur
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.x = x;
                spawnedTile.y = y;
                //?
                spawnedTile.Init(x, y);

                //Je stock la Tile et ça position
                _tiles[new Vector2(x, y)] = spawnedTile;

                //On change l'ordre d'affichage 
                spawnedTile.SpriteRenderer.sortingOrder = _height - y + x;
                spawnedTile._outline.GetComponent<SpriteRenderer>().sortingOrder = _height - y + x + 1;
                spawnedTile._outlineFog.GetComponent<SpriteRenderer>().sortingOrder = _height - y + x + 2;
                spawnedTile._outlineWalkable.GetComponent<SpriteRenderer>().sortingOrder = _height - y + x + 1;
                //_offSpring = _offSpring + new Vector3(_offSpringX, _offSpringY);
            }
        }

        GetAllBaseTile();
        cam.transform.position = orgineTile;
    }
    public void DestroyGridAndCleanTheDictionary()
    {
        if (_tiles != null)
        {
            foreach(BaseTile i in _tiles.Values)
            {
                if (i != null)
                    DestroyImmediate(i.gameObject);
            }
        }
        //Nettoyage
        _tiles = new Dictionary<Vector2, BaseTile>();
        //UnitManager.Instance.DestoyAllHeros();
    }
    public BaseTile GetTileAtPosition(Vector2 vector2)
    {
        if (_tiles.TryGetValue(vector2, out BaseTile tile))
        {
            return tile;
        }

        Debug.LogWarning("Tile not found");
        return null;
    }
    public List<BaseTile> GetNeighborTiles(BaseTile _startingTile)
    {
        var tileX = _startingTile.x;
        var tileY = _startingTile.y;
        //Debug.Log(tileX);
        //Debug.Log(tileY);
        var _neighborTiles = new List<BaseTile>();
        if (tileX - 1 >= 0)
        {
            //Left
            _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX - 1, tileY)));
            //Left Down
            if (IsDiagonalMovementEnable && tileY - 1 > 0) _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX - 1, tileY - 1)));
            //Left Up
            if (IsDiagonalMovementEnable && tileY + 1 > GridManager.Instance.Height) _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX - 1, tileY + 1)));
        }
        if (tileX + 1 < GridManager.Instance.Width)
        {
            //Right
            _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX + 1, tileY)));
            //Right Down
            if (IsDiagonalMovementEnable && tileY - 1 >= 0) _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX + 1, tileY - 1)));
            //Right Up
            if (IsDiagonalMovementEnable && tileY + 1 < GridManager.Instance.Height) _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX + 1, tileY + 1)));
        }
        //Down
        if(tileY - 1 >= 0) _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX, tileY - 1)));
        //Up
        if (tileY + 1 < GridManager.Instance.Height) _neighborTiles.Add(GetTileAtPosition(new Vector2(tileX, tileY + 1)));
        var _tileList = new List<BaseTile>();
        foreach (BaseTile tile in _neighborTiles)
        {
            //Null check 2
            if(tile != null)
            {
                _tileList.Add(tile);
            }
        }

        return _tileList;
    }
    public List<BaseTile> GetSpawnableTile()
    {
        return _tiles.Values.Where(a => a.IsWalkable).ToList();
    }
    public void ErasedOutLineTile()
    {
        foreach (BaseTile tile in _tiles.Values)
        {
            tile._outline.SetActive(false);
        }
    }
    public void ErasedPathFinding()
    {
        foreach (BaseTile tile in _tiles.Values)
        {
            tile.ErasePathfinding();
        }
    }
    public void SetDiagonalMovementBool()
    {
        if(IsDiagonalMovementEnable)
        {
            IsDiagonalMovementEnable = false;
        }
        else
        {
            IsDiagonalMovementEnable = true;
        }
    }
    public List<BaseTile> FindAllOfWalkingTiles(UnitScript _unit, int PM)
    {
        List<BaseTile> _walkingTiles = new List<BaseTile>();
        BaseTile _unitTile = _unit._tileOccupied;
        _walkingTiles.Add(_unitTile);
        foreach (BaseTile tile in GetAllTilesInSquare(_unitTile, PM))
        {
            if (tile._unitOnMe != null) continue;
            PathFinderManager.Instance.PathFinding(_unitTile, tile);
            if(tile.G_Cost <= PM)
            {
                _walkingTiles.Add(tile);
            }
        }
        return _walkingTiles;
    }
    public List<BaseTile> GetAllTilesInSquare(BaseTile tileChose, int Distance)
    {
        List<BaseTile> _allTiles = new List<BaseTile>();
        foreach(BaseTile tile in _tiles.Values)
        {
            if (PathFinderManager.Instance.CalculateDistanceCost(tileChose, tile) <= Distance)
            {
                _allTiles.Add(tile);
            }
        }
        return _allTiles;
    }
    private void GetAllBaseTile()
    {
        foreach(BaseTile tile in _tiles.Values)
        {
            if (tile.IsBuilding) _buildingsTiles.Add(tile);
        }
    }
}

