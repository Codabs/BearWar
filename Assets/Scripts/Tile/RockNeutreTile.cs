using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockNeutreTile : BaseTile
{
    [SerializeField] private List<Sprite> _rockSprite;
    [SerializeField] private List<Sprite> _plaineSprite;
    [SerializeField] private List<Sprite> _banquiseSprite;
    public override void Init(int x, int y)
    {
        SetTerrain(x, y);
        //StartCoroutine("WaitFOrAllTheTileToSpawn");
    }
    IEnumerator WaitFOrAllTheTileToSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (BaseTile tile in GridManager.Instance.GetNeighborTiles(this))
        {
            if (tile.IsMountain)
            {
                spriteRenderer.sprite = _rockSprite[Random.Range(0, _rockSprite.Count)];
            }
        }
    }
    private void SetTerrain(int x, int y)
    {
        if(y < 10)
        {
            spriteRenderer.sprite = _plaineSprite[Random.Range(0, _plaineSprite.Count)];
            _walkSoundFmodBank = "event:/gameplay/pas/plain_footstep";
    }
        else
        {
            spriteRenderer.sprite = _banquiseSprite[Random.Range(0, _banquiseSprite.Count)];
            _walkSoundFmodBank = "event:/gameplay/pas/snow_footstep";
        }
    }
}
