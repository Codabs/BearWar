using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;


public abstract class BaseTile : MonoBehaviour
{
    //---------------
    //VARIABLE
    //----------------

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] public GameObject _outline;
    [SerializeField] public GameObject _outlineFog;
    [SerializeField] public GameObject _outlineWalkable;
    [SerializeField] public UnitScript _unitOnMe;

    [SerializeField] private bool _isWalkable;
    [SerializeField] private bool _isMountain;
    [SerializeField] private bool _isPlayerBase;
    [SerializeField] private bool _isEnnemiBase;
    [SerializeField] private bool _isBuilding;

    public GroundType groundType;
    public bool _isInvisible = true;
    private string _factionCaptured = "neutre";

    [SerializeField] private TextMeshPro G_Text;
    [SerializeField] private TextMeshPro H_Text;
    [SerializeField] private TextMeshPro F_Text;

    [SerializeField] private Color _pathfindingColor;
    public string _walkSoundFmodBank;
    public List<BaseTile> _pathToMe = new();
    public List<UnitScript> _unitThatSeeMe = new List<UnitScript>();

    public int x;
    public int y;

    //PathFinding A*
    private float _gCost = 0;
    private float _hCost = 0;
    private float _fCost = 0;
    public BaseTile _cameFromTile;

    //----------------
    //GETTER AND SETTER
    //----------------

    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
    public float G_Cost { get { return _gCost; } set { _gCost = value; } }
    public float H_Cost { get { return _hCost; } set { _hCost = value; } }
    public float F_Cost { get { return _fCost; } }
    public bool IsWalkable { get { return _isWalkable; } }
    public bool IsMountain { get { return _isMountain; } }
    public bool IsPlayerBase { get { return _isPlayerBase; } set { _isPlayerBase = value; } }
    public bool IsEnnemiBase { get { return _isEnnemiBase; } set { _isEnnemiBase = value; } }
    public bool IsBuilding { get { return _isBuilding; } }
    public string Faction { get { return _factionCaptured; } set { _factionCaptured = value; } }

    //----------------
    //MONOBEHAVIOUR
    //----------------
    private void OnMouseEnter()
    {
        GameManager.Instance.PlayerOverMouseOnThisTile(this);
    }
    private void OnMouseExit()
    {
        GameManager.Instance.PlayerExitMouseOnThisTile(this);
    }
    private void OnMouseDown()
    {
        GameManager.Instance.PlayerClickOnThisTile(this);
        FMODUnity.RuntimeManager.PlayOneShot("event:/MouseSound/MouseClick");
    }

    //------------------
    //PUBLIC FONCTION
    //------------------
    public virtual void Init(int x, int y)
    {
        //Fonction utiliser quand la tile est créée
    }
    /// <summary>
    /// Affiche les valeurs du pathfindings A*
    /// </summary>
    public void ShowPathFinding()
    {
        G_Text.text = _gCost.ToString();
        H_Text.text = _hCost.ToString();
        F_Text.text = _fCost.ToString();
        spriteRenderer.color = _pathfindingColor;
    }
    public void SetColorToGreen()
    {
        spriteRenderer.color = Color.green;
    }
    /// <summary>
    /// Retire le texte de la tile
    /// </summary>
    public void ErasePathfinding()
    {
        G_Text.text = "";
        H_Text.text = "";
        F_Text.text = "";
        spriteRenderer.color = Color.white;
    }
    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }
    /// <summary>
    /// Joue son de la marche
    /// </summary>
    public void PlayWalkSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_walkSoundFmodBank);
    }
    public void IsVisible(bool value, UnitScript unit)
    {
        bool _isABaseNear = false;
        foreach(BaseTile tile in GridManager.Instance.GetNeighborTiles(this))
        {
            if (tile.IsPlayerBase) _isABaseNear = true;
        }
        if(value || _isABaseNear || IsPlayerBase)
        {
            _unitThatSeeMe.Add(unit);
            if (_unitOnMe != null)
            {
                _unitOnMe.Invisible(false);
            }
            _outlineFog.SetActive(false);
        }
        else
        {
            if (_unitThatSeeMe.Contains(unit)) _unitThatSeeMe.Remove(unit);
            if(_unitThatSeeMe.Count <= 0)
            {
                if (_unitOnMe != null)
                {
                    _unitOnMe.Invisible(true);
                }
                _outlineFog.SetActive(true);
            }
            else
            {
                if (_unitOnMe != null)
                {
                    _unitOnMe.Invisible(false);
                }
                _outlineFog.SetActive(false);
            }
        }
        _isInvisible = !value;
    }
    public void ResetVisibility()
    {
        _unitThatSeeMe.Clear();
        if (_unitOnMe != null)
        {
            _unitOnMe.Invisible(true);
        }
        _outlineFog.SetActive(true);
    }
}
