using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    //
    //VARIABLE
    //

    public static GameManager Instance;

    public GameState _currentGameState;
    public GameState_Factory _states;

    //Units Actions
    public UnitScript unit;
    public BaseTile tile;
    public List<BaseTile> path;

    //UI
    public GameObject _uiGame;
    public GameObject _uiEndTurnButton;
    public GameObject _uiShop;
    public Button _tankBuy;
    public Button _cacBuy;
    public Button _distanceBuy;
    public Button _healerBuy;

    //UI Chose
    public GameObject _uiButtonWait;
    public GameObject _uiButtonFight;
    public GameObject _uiButtonHeal;
    public GameObject _uiButtonFightBase;

    public TextMeshProUGUI _pointsText;

    public bool _isTesting;
    public PlayersManager.PlayerData playerData => _isTesting ? PlayersManager.Instance.MyData: PlayersManager.Instance.PlayersData[0] ;

    //Ennemis Unit
    public UnitScript _ennemiTank;
    public UnitScript _ennemiCac;

    [SerializeField] private int _maxPointForWin;
    public int _YoursPoints = 0;
    private int _EnnemiScore = 0;
    public int _ScoreMaxToWin = 15;
    [SerializeField] private TextMeshProUGUI _moneyText;
    private bool _isTheShopUsed = false;
    public List<UnitScript> _unitAttackable = new();
    public UnitScript _unitAttacked;
    public List<UnitScript> _unitHealable = new();
    public UnitScript _unitHealed;

    public bool scenework;
    public UnitScript _unitBought;
    public BaseTile _batimentCaptured;

    public bool _isPlayerTurn = false;

    //
    //Getters And Setters
    //

    public int Money { get { return playerData.Money; } set { playerData.Money = value; _moneyText.text = value.ToString(); } }
    public int Score { get { return _YoursPoints; } set { _YoursPoints = value; _pointsText.text = "Score: " + value.ToString(); } }
    public bool IsTheShopUsed { get { return _isTheShopUsed; } set { _isTheShopUsed = value; } }
    public List<UnitScript> unitScripts { get { return playerData.unitScripts; } }

    //
    //MONOBEHAVIOUR
    //
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        //Create all other states
        _states = new GameState_Factory(this);
        //First State
        _currentGameState = _states.StartGame();
        _currentGameState.EnterState();
    }

    private void Update()
    {
        _currentGameState.UpdateStates();
    }

    //
    //FONCTION
    //

    public void PlayerClickOnThisUnit(UnitScript unitClick)
    {
        _currentGameState.AUnitIsClick(unitClick);
    }
    public void PlayerOverMouseOnThisUnit(UnitScript unitClick)
    {
        _currentGameState.AUnitIsOverMouse(unitClick);
    }
    public void PlayerExitMouseOnThisUnit(UnitScript unitClick)
    {
        _currentGameState.AUnitIsExitMouse(unitClick);
    }
    public void ShowPathToTile(List<BaseTile> path)
    {
        //On active outline pour monter le chemin que va prendre l'uniter
        foreach(BaseTile tile in path)
        {
            tile._outline.SetActive(true);
        }
    }
    public void PlayerClickOnThisTile(BaseTile tileClick)
    {
        _currentGameState.ATileIsClick(tileClick);
    }
    public void PlayerOverMouseOnThisTile(BaseTile tileClick)
    {
        _currentGameState.ATileIsOverMouse(tileClick);
    }
    public void PlayerExitMouseOnThisTile(BaseTile tileClick)
    {
        _currentGameState.ATileIsExitMouse(tileClick);
    }
    public void MoveUnit(UnitScript unit, BaseTile tileEnd)
    {
        StartCoroutine(unit.MoveUnitInAPath(tileEnd));
    }
    public void RecontructTheGame()
    {
        _currentGameState = _states.StartGame();
        _currentGameState.EnterState();
    }
    public void EndTurnButton()
    {
        if (_isTesting)
            Client.Instance.SendToServer(new NetSwitchTurn());
        else
            StartTurn();
    }
    public void EndTurn()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Autre/findetour");
        foreach(BaseTile tile in GridManager.Instance._buildingsTiles)
        {
            if(playerData.unitScripts.Contains(tile._unitOnMe))
            {
                Score++;
            }
        }
        if (playerData.Index == 1) CheckIfWin();
        _currentGameState.SwitchState(_states.EnnemiTurn());
    }
    public void StartTurn()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Autre/findetour");
        foreach (BaseTile tile in GridManager.Instance._buildingsTiles)
        {
            if (!playerData.unitScripts.Contains(tile._unitOnMe) && tile._unitOnMe != null)
            {
                _EnnemiScore++;
            }
        }
        if (playerData.Index == 0) CheckIfWin();
        _currentGameState.SwitchState(_states.PlayerTurn());
    }
    public void Wait()
    {
        ResetUIToGameUI();
        _uiGame.SetActive(true);
        _currentGameState.SubState.SwitchState(_states.NoAction());
    }
    public void Attaque()
    {
        ResetUIToGameUI();
        _currentGameState.SubState.SwitchState(_states.ChoseAttack());
    }
    public void Heal()
    {
        ResetUIToGameUI();
        _currentGameState.SubState.SwitchState(_states.ChoseHeal());
    }
    public void BuyAUnit(int i)
    {
        if(tile == null)
        {
            _currentGameState.SubState.SwitchState(_states.NoAction());
            return;
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/gameplay/achat/buy_complete");
        Money -= unit.scObjUnit.cout;
        _isTheShopUsed = true;
        _currentGameState.SubState.SwitchState(_states.NoAction());
        NetSpawnUnit netSpawnUnit = new NetSpawnUnit();
        netSpawnUnit.indexPlayer = playerData.Index;
        netSpawnUnit.indexTypeUnit = i;
        netSpawnUnit.TileX = tile.x;
        netSpawnUnit.TileY = tile.y;
        Client.Instance.SendToServer(netSpawnUnit);
    }
    private void ResetUIToGameUI()
    {
        _uiButtonWait.SetActive(false);
        _uiButtonFight.SetActive(false);
        _uiButtonHeal.SetActive(false);
        _uiButtonFightBase.SetActive(false);
    }
    public void UnitStopMoving(UnitScript unit1)
    {
        unit1._hadMove = true;
        //Feedback add
        if(playerData.unitScripts.Contains(unit1))
        {
            unit1.DeSelection();
            _currentGameState.SubState.SwitchState(_states.ChoseAction());
        }
    }
    public void AttackEnnemiBase()
    {
        ResetUIToGameUI();
        NetBaseAttack netBaseAttack = new();
        netBaseAttack.indexUnitA = unit._unitInfo.Index;
        netBaseAttack.PlayerIndex = playerData.Index;
        _currentGameState.SubState.SwitchState(_states.NoAction());
        Client.Instance.SendToServer(netBaseAttack);
    }
    private void CheckIfWin()
    {
        if(_ScoreMaxToWin < Score || _ScoreMaxToWin < _EnnemiScore)
        {
            if(Score == _EnnemiScore)
            {
                return;
            }
            else if(Score > _EnnemiScore)
            {
                Win();
            }
            else if(_EnnemiScore > Score)
            {
                Lose();
            }
            else
            {
                Debug.LogError("Pas possible");
            }
        }
    }
    public void Win()
    {
        _currentGameState.SwitchState(_states.EnnemiTurn());
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    public void Lose()
    {
        _currentGameState.SwitchState(_states.EnnemiTurn());
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
}
