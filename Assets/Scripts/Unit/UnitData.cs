using UnityEngine;

public enum UnitType
{
    TANK = 1,
    CAC = 2,
    DISTANCE = 3,
    HEALER = 4,
}

public class UnitData
{
    public ScObjUnit ScUnit;
    public GameObject _GameObject;

    public int Index { get; set; }

    [Space(5), Header("Stats")]
    
    [SerializeField] private int PV;
    public int pv
    {
        set 
        { 
            PV = value;

            if (PV <= 0) PlayersManager.Instance.DestroyUnit(this);
            else if (PV > ScUnit.pv) PV = ScUnit.pv;
        }
        get { return PV; }
    }

    public int damage;
    public int range
    {
        get
        {
            if (tileType == GroundType.Montagne) { return ScUnit.range + 2; }
            else { return ScUnit.range; }
        }
    }

    public int pm;
    public int vision
    {
        get
        {
            if (tileType == GroundType.Montagne) { return ScUnit.vision + 2; }
            else { return ScUnit.vision; }
        }
    }


    [Space(5), Header("Tile")]
    public GroundType tileType;
    public int Defense
    {
        get
        {
            switch (tileType)
            {
                case GroundType.Eau:
                    return 0;
                case GroundType.Sol:
                    return 0;
                case GroundType.Montagne:
                    return 2;
                case GroundType.Ville:
                    return 1;
                case GroundType.Base:
                    return 0;
                default:
                    return 0;
            }
        }
        private set { }
    }


    public UnitData(GameObject prefab, ScObjUnit scObjUnit)
    {
        _GameObject = prefab;
        ScUnit = scObjUnit;
        pv = scObjUnit.pv;
        damage = scObjUnit.damage;
        pm = scObjUnit.pm;
    }
}
