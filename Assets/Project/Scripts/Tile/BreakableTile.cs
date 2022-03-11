using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : Tile
{
    public override TileType tileType => TileType.Breakable;

    public Sprite[] spriteList;
    public Color normalColor;


    [HideInInspector] public int breakableValue;

    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        SetupBreakables();
    }

    private void SetupBreakables()
    {
        breakableValue = spriteList.Length-1;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteList == null)
        {
            Debug.Log("please check breakable sprites");
        }
        else
        {
            spriteRenderer.sprite = spriteList[breakableValue];
        }
    }

    public void SetBreakableValue()
    {
        if (breakableValue > 0)
        {
            breakableValue--;
            spriteRenderer.sprite = spriteList[breakableValue];
        }

        else if (breakableValue==0)
        {
            tileType = TileType.Normal;
            spriteRenderer.color = normalColor;
        }
    }
}
