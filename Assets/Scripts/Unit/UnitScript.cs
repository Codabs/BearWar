using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMOD;

public class UnitScript : MonoBehaviour
{
    //
    //Variable
    //

    [SerializeField] private Animator _animator;
    [SerializeField] private List<SpriteRenderer> _spriteRenderers;
    public Sprite _unitSprite;

    public GameObject _outline;
    public GameObject _outlineSelected;
    [SerializeField] private TrailRenderer _trail;

    public BaseTile _tileOccupied;
    public List<BaseTile> _tileUnitCanSee = new List<BaseTile>();

    public GameObject Sprite;
    public ScObjUnit scObjUnit;
    public UnitType unitType;
    public UnitData _unitInfo;
    [SerializeField] private float _speed;
    [SerializeField] private string _attackSound = "event:/UnitSound/Flamethrower";

    public bool _hadMove = false;
    private readonly bool _isMoving;
    private bool _isInvisible = false;
    protected string faction;

    [SerializeField] Vector3 _unitOffset;

    //
    //Getters and Setters
    //

    public bool IsMoving { get { return _isMoving; } }
    public string Faction { get { return faction; } }
    public Vector3 Offset { get { return _unitOffset; } }
    public bool IsInvisible { get { return _isInvisible; } }

    //
    //MONOBEHAVIOUR
    //
    private void Awake()
    {
        _unitInfo = new UnitData(gameObject, scObjUnit);
        _unitInfo.tileType = GroundType.Sol;
    }
    //Mouse
    private void OnMouseEnter()
    {
        GameManager.Instance.PlayerOverMouseOnThisUnit(this);
        FMODUnity.RuntimeManager.PlayOneShot("event:/MouseSound/MouseOver");
    }
    private void OnMouseExit()
    {
        GameManager.Instance.PlayerExitMouseOnThisUnit(this);
    }
    private void OnMouseDown()
    {
        GameManager.Instance.PlayerClickOnThisUnit(this);
        FMODUnity.RuntimeManager.PlayOneShot("event:/MouseSound/MouseClick");
    }

    //
    //Fonction
    //

    public IEnumerator MoveUnitInAPath(BaseTile tileEnd)
    {
        List<BaseTile> tiles = PathFinderManager.Instance.PathFinding(_tileOccupied, tileEnd);
        _animator.Play("Walk_BlendTree");
        bool _isUpdatingFog = GameManager.Instance.playerData.unitScripts.Contains(this);
        foreach (BaseTile tile in tiles)
        {
            if (tile._unitOnMe != null && tile._unitOnMe != this) { 
                //_animator.Play("Idle_BlendTree");
                GameManager.Instance.UnitStopMoving(this);
                break; 
            }
            var XDistance = _tileOccupied.x - tile.x;
            var YDistance = _tileOccupied.y - tile.y;
            //On joue l'animation
            _animator.SetFloat("moveX", XDistance);
            _animator.SetFloat("moveY", YDistance);
            SetSortingOrder(tile.SpriteRenderer.sortingOrder);
            transform.DOMove(tile.transform.position + _unitOffset, _speed);
            tile.PlayWalkSound();
            yield return new WaitForSeconds(_speed);
            _tileOccupied._unitOnMe = null;
            _tileOccupied = tile;
            _unitInfo.tileType = tile.groundType;
            tile._unitOnMe = this;
            if(_isUpdatingFog)
            {
                foreach (BaseTile tileSee in _tileUnitCanSee)
                    tileSee.IsVisible(false, this);
                _tileUnitCanSee = FogManager.Instance.UpdateFog(_unitInfo.vision * 10, this);
            }

            if (tile._isInvisible) Invisible(true);
            else Invisible(false);
            tile._outline.SetActive(false);
        }
        _animator.Play("Idle_BlendTree");
        GameManager.Instance.UnitStopMoving(this);
    }
    public void AttackOtherUnit(UnitScript unitAttacked)
    {
        //Turn in direction of the unit attacked
        _animator.SetFloat("moveX", 0);
        _animator.SetFloat("moveY", 0);
        //Play Animation
        _animator.Play("Attack_BlenTree");
    }
    public void DestroyInStyle()
    {
        Destroy(gameObject);
    }
    public void StopAttacking()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_attackSound);
        _animator.Play("Idle_BlendTree");
    }
    public void Selection()
    {
        _outlineSelected.SetActive(true);
    }
    public void DeSelection()
    {
        _outlineSelected.SetActive(false);
    }
    public void SetSortingOrder(int _order)
    {
        _trail.sortingOrder = _order;
        for(int i = 0; i <= _spriteRenderers.Count - 1; i++)
        {
            _spriteRenderers[i].sortingOrder = _order + 1 + i;
        }
        _outline.GetComponent<SpriteRenderer>().sortingOrder = _order + 2 + _spriteRenderers.Count;
        _outlineSelected.GetComponent<SpriteRenderer>().sortingOrder = _order + 3 + _spriteRenderers.Count;
    }
    public void Invisible(bool value)
    {
        /*if(value)
        {
            foreach(SpriteRenderer sprite in _spriteRenderers)
            {
                sprite.color = new Vector4(0, 0, 0, 0);
            }
            _outline.SetActive(false);
            _outlineSelected.SetActive(false);
        }
        else
        {
            foreach (SpriteRenderer sprite in _spriteRenderers)
            {
                sprite.color = new Vector4(0, 0, 0, 1);
            }
        }*/
        Sprite.SetActive(!value);
        SetCollider(!value);
        _isInvisible = value;
    }
    public IEnumerator DeactivateColliderFor5sec()
    {
        foreach(SpriteRenderer sprite in _spriteRenderers)
        {
            sprite.color = new Vector4(1, 1, 1, 0.5f);
        }
        SetCollider(false);
        yield return new WaitForSeconds(5f);
        foreach (SpriteRenderer sprite in _spriteRenderers)
        {
            sprite.color = new Vector4(1, 1, 1, 1);
        }
        SetCollider(true);
    }
    public void SetCollider(bool value)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = value;
    }
    private void OnDestroy()
    {
        foreach(BaseTile tile in _tileUnitCanSee)
        {
            if (tile == null) continue;
            tile.IsVisible(false, this);
        }
    }
}
