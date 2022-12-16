using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Unit", menuName ="Scriptable/Unit", order = 2)]
public class ScObjUnit : ScriptableObject
{
    public UnitType unitType;
    public int cout;
    public int pv;
    public int damage;
    public int range;
    public int pm;
    public int vision;

    [Space(5), Header("Scene combat")]
    public GameObject PrefabsAttaque;
    public GameObject PrefabsDefense;
    public AnimatorOverrideController Animator;
    public AnimatorOverrideController DefenseAnimator;
    public AudioClip Hit;
    public AudioClip receive;
}
