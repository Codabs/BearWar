using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static UnityEngine.ParticleSystem;

public class SceneCombatController : Singleton<SceneCombatController>
{
    [SerializeField] private float TempsAvantAttaque;
    [SerializeField] private float TempsAvantDefense;
    public GameObject sceneCombat;
    [SerializeField] private float TempsAvantFin;
    public void Attaque(UnitData attaquant, UnitData defenseur, int distance) => StartCoroutine(_Attaque(attaquant, defenseur, distance));
    public void Soin(UnitData Receveur)
    {
        Receveur.pv += 6;
    }
    public GameObject LeftSide, RightSide;
    public Animator AnimAttaque, AnimDefense;
    public TMP_Text DefAttaque, DefDefense;
    public Slider VieAttaque, VieDefense;
    public ParticleSystem Particule;

    private GameObject ObjAtt, ObjDef;

    // distance 1 = cac
    [Space(5), Header("Test")]
    public ScObjUnit scObjUnit1;
    public ScObjUnit scObjUnit2;
    public int distTest;
    public void testAttaque()
    {
        UnitData a = new UnitData(null, scObjUnit1);
        UnitData b = new UnitData(null, scObjUnit2);
        Attaque(a, b, distTest);
    }
    public IEnumerator _Attaque(UnitData attaquant, UnitData defenseur, int distance)
    {
        sceneCombat.SetActive(true);
        yield return null;
        Debug.Log("Combat");

        // set parameter of fight
        DefAttaque.text = attaquant.Defense.ToString();
        DefDefense.text = defenseur.Defense.ToString();

        VieAttaque.maxValue = attaquant.ScUnit.pv;
        VieAttaque.value = attaquant.pv;
        VieDefense.maxValue = defenseur.ScUnit.pv;
        VieDefense.value = defenseur.pv;

        ObjAtt = Instantiate(attaquant.ScUnit.PrefabsAttaque, LeftSide.transform);
        ObjDef = Instantiate(defenseur.ScUnit.PrefabsDefense, LeftSide.transform);

        AnimAttaque = ObjAtt.GetComponent<Animator>();
        AnimDefense = ObjDef.GetComponent<Animator>();

        AnimAttaque.runtimeAnimatorController = attaquant.ScUnit.Animator;
        AnimDefense.runtimeAnimatorController = defenseur.ScUnit.DefenseAnimator;

        yield return null;
        Debug.Log("IdleState");

        yield return new WaitForSeconds(TempsAvantAttaque);
        Debug.Log("AttackState");

        AnimAttaque.Play("Attack");

        yield return new WaitForSeconds(TempsAvantDefense);
        Debug.Log("ReceiveState");
        Particule.gameObject.transform.position = ObjDef.transform.position;
        Particule.Play();

        //AnimAttaque.gameObject.GetComponent<AudioSource>().PlayOneShot(attaquant.ScUnit.Hit);
        AnimDefense.Play("Receive");
        yield return new WaitForSeconds(0.1f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/gameplay/degats/grizzly_hit");

        float actualHP = defenseur.pv;
        Debug.Log((defenseur.damage - attaquant.Defense) * Accointance(defenseur.ScUnit.unitType, attaquant.ScUnit.unitType));
        int newHPDef = defenseur.pv - (attaquant.damage - defenseur.Defense) * Accointance(attaquant.ScUnit.unitType, defenseur.ScUnit.unitType);
        if (newHPDef <= 0)
        {
            while(actualHP > 0)
            {
                actualHP -= 0.1f;
                VieDefense.value = Mathf.RoundToInt(actualHP);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            VieDefense.value = 0;
            AnimDefense.Play("Death");
            Particule.gameObject.transform.position = ObjDef.transform.position;
            Particule.Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/gameplay/degats/grizzly_death");

            Destroy(defenseur._GameObject);

            foreach (var Data in PlayersManager.Instance.PlayersData) 
                 if (Data.unitsData.Contains(defenseur)) 
                    Data.unitsData.Remove(defenseur);

        }
        else
        {
            while (actualHP > newHPDef)
            {
                actualHP -= 0.1f;
                VieDefense.value = Mathf.RoundToInt(actualHP);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            VieDefense.value = newHPDef;
            defenseur.pv = newHPDef;

        }

        if (newHPDef > 1 && defenseur.range >= distance)
        {
            StartCoroutine(Riposte(attaquant, defenseur));
            yield break;
        }

        yield return new WaitForSeconds(TempsAvantFin);
        Debug.Log("Leave");
        Destroy(ObjAtt);
        if(ObjDef != null) Destroy(ObjDef);
        sceneCombat.SetActive(false);
    }

    public IEnumerator Riposte(UnitData attaquant, UnitData defenseur)
    {
        yield return new WaitForSeconds(TempsAvantAttaque);
        Debug.Log("Riposte");

        AnimDefense.Play("Attack");

        yield return new WaitForSeconds(TempsAvantDefense);
        Debug.Log("Receive");
        Particule.gameObject.transform.position = ObjAtt.transform.position;
        Particule.Play();

        AnimAttaque.Play("Receive");
        
        yield return new WaitForSeconds(0.1f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/gameplay/degats/grizzly_hit");

        float actualHP = attaquant.pv;
        Debug.Log((defenseur.damage - attaquant.Defense) * Accointance(defenseur.ScUnit.unitType, attaquant.ScUnit.unitType) / 2);
        int newHPAtt = attaquant.pv - ((defenseur.damage - attaquant.Defense) * Accointance(defenseur.ScUnit.unitType, attaquant.ScUnit.unitType) / 2);
        if (newHPAtt <= 0)
        {
            while (actualHP > 0)
            {
                actualHP -= 0.1f;
                VieAttaque.value = Mathf.RoundToInt(actualHP);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            VieDefense.value = 0;
            AnimAttaque.Play("Death");
            Particule.gameObject.transform.position = ObjAtt.transform.position;
            Particule.Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/gameplay/degats/grizzly_death");

            Destroy(attaquant._GameObject);

            foreach (var Data in PlayersManager.Instance.PlayersData)
                if (Data.unitsData.Contains(attaquant))
                    Data.unitsData.Remove(attaquant);

        }
        else
        {
            while (actualHP > newHPAtt)
            {
                actualHP -= 0.1f;
                VieAttaque.value = Mathf.RoundToInt(actualHP);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            VieAttaque.value = newHPAtt;
            attaquant.pv = newHPAtt;

        }

        yield return new WaitForSeconds(TempsAvantFin);
        Debug.Log("Leave");
        Destroy(ObjDef);
        if (ObjAtt != null) Destroy(ObjAtt);
        sceneCombat.SetActive(false);
    }

    public int Accointance(UnitType attaquant, UnitType Defenseur)
    {
        switch (attaquant)
        {
            case UnitType.TANK:
                if (Defenseur == UnitType.CAC) return 2;
                else return 1;
            case UnitType.CAC:
                if (Defenseur == UnitType.DISTANCE) return 2;
                else return 1;
            case UnitType.DISTANCE:
                if (Defenseur == UnitType.TANK) return 2;
                else return 1;
            case UnitType.HEALER:
                return 1;
            default:
                return 1;
        }
    }
}
