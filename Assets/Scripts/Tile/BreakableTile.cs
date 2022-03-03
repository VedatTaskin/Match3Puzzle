using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : Tile
{
    public Sprite[] spriteList;
    public Color normalColor;

    [HideInInspector] public int breakableValue;

    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        tileType = TileType.Breakable;
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
        breakableValue--;
        spriteRenderer.sprite = spriteList[breakableValue];

        if (breakableValue==0)
        {
            tileType = TileType.Normal;
            spriteRenderer.color = normalColor;
        }
    }
}
