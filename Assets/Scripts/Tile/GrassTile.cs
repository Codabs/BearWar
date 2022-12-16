using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : BaseTile
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private List<Sprite> _grassSprites;

    public override void Init(int x, int y)
    {
        //var _with = GridManager.Instance.Width;
        //var _height = GridManager.Instance.Height;
        //Trouver si elle est invoquer sur un bout de la grid
        /*if (x == _with || x == 0)
        {
            if (y == 0)
            {
                //Coin
                spriteRenderer.sprite = _grassSprites[0];
                return;
            }
            if (y == _height)
            {
                //Coin
                spriteRenderer.sprite = _grassSprites[1];
                return;
            }

            return;
        }
        if (y == _height || y == 0)
        {
            if (x == 0)
            {
                spriteRenderer.sprite = _grassSprites[2];
                return;
            }
            if (y == _height)
            {
                spriteRenderer.sprite = _grassSprites[4];
                return;
            }
            return;
        }*/
        var isOffSet = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);
        spriteRenderer.color = isOffSet ? _offsetColor : _baseColor;
    }
}
