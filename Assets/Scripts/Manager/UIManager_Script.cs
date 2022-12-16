using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_Script : MonoBehaviour
{
    public static UIManager_Script Instance;

    [SerializeField] private GameObject _tileUI;
    [SerializeField] private TextMeshProUGUI _textTile;
    [SerializeField] private Image _tileSprite;
    [SerializeField] private TextMeshProUGUI _textDef;
    [SerializeField] private TextMeshProUGUI _textPM;

    [SerializeField] private GameObject _unitUI;
    [SerializeField] private TextMeshProUGUI _unitName;
    [SerializeField] private Image _unitSprite;
    [SerializeField] private TextMeshProUGUI _unitPv;
    [SerializeField] private TextMeshProUGUI _unitAttaque;
    [SerializeField] private TextMeshProUGUI _unitVision;
    [SerializeField] private TextMeshProUGUI _unitDefense;

    [SerializeField] private GameObject _ennemiUI;
    [SerializeField] private TextMeshProUGUI _ennemiName;
    [SerializeField] private Image _ennemiSprite;
    [SerializeField] private TextMeshProUGUI _ennemiPv;
    [SerializeField] private TextMeshProUGUI _ennemiAttaque;
    [SerializeField] private TextMeshProUGUI _ennemiVision;
    [SerializeField] private TextMeshProUGUI _ennemiDefense;

    private void Awake()
    {
        Instance = this;
        _tileUI.SetActive(false);
        _unitUI.SetActive(false);
        _ennemiUI.SetActive(false);
    }
    public void SetTileUI(BaseTile tile)
    {
        _tileUI.SetActive(true);
        _textTile.text = tile.groundType.ToString();
        _tileSprite.sprite = tile.SpriteRenderer.sprite;
        switch(tile.groundType)
        {
            case GroundType.Base:
            {
                    _textDef.text = "Defence: " + "0";
                    _textPM.text = "PM: " + "1";
                    break;
            }
            case GroundType.Ville:
            {
                    _textDef.text = "Defence: " + "1";
                    _textPM.text = "PM: " + "1";
                    break;
            }
            case GroundType.Sol:
            {
                    _textDef.text = "Defence: " + "0";
                    _textPM.text = "PM: " + "1";
                    break;
            }
            case GroundType.Montagne:
            {
                    _textDef.text = "Defence: " + "2";
                    _textPM.text = "PM: " + "2";
                    break;
            }
            case GroundType.Eau:
            {
                    _textDef.text = "Defence: " + "0";
                    _textPM.text = "PM: " + "0";
                    break;
            }
        }
    }
    public void SetUnitSelect(UnitScript unit)
    {
        _unitUI.SetActive(true);
        ScObjUnit unitData = unit._unitInfo.ScUnit;
        _unitName.text = unitData.name;
        _unitSprite.sprite = unit._unitSprite;
        _unitPv.text = "PV: " + unit._unitInfo.pv.ToString();
        _unitAttaque.text = "Attaque: " + unitData.damage.ToString();
        _unitVision.text = "Vision: " + unitData.vision.ToString();
        _unitDefense.text = "Defence: " + unit._unitInfo.Defense.ToString();
        SetTileUI(unit._tileOccupied);
    }
    public void SetEnnemiSelect(UnitScript unit)
    {
        _ennemiUI.SetActive(true);
        ScObjUnit unitData = unit._unitInfo.ScUnit;
        _ennemiName.text = unitData.name;
        _ennemiSprite.sprite = unit._unitSprite;
        _ennemiPv.text = "PV: " + unit._unitInfo.pv.ToString();
        _ennemiAttaque.text = "Attaque: " + unitData.damage.ToString();
        _ennemiVision.text = "Vision: " + unitData.vision.ToString();
        _ennemiDefense.text = "Defence: " + unit._unitInfo.Defense.ToString();
    }
    public void RemoveEnnemiSelect()
    {
        _ennemiUI.SetActive(false);
    }
    public void RemoveTileSelect()
    {
        _tileUI.SetActive(false);
    }
    public void RemoveUnitSelect()
    {
        _unitUI.SetActive(false);
    }
}
