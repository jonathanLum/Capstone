using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomEffect : TileEffect
{
    private int value;
    public override void Apply(GameManager gameManager, float effectValue)
    {
        var tileCount = (int)Random.Range(-effectValue, effectValue);
        while (tileCount == 0)
        {
            tileCount = (int)Random.Range(-effectValue, effectValue);
        }
        value = tileCount;
        gameManager.spacesToMove = (int)tileCount;
        Debug.Log("Move random" + tileCount);
    }

    public override string message
    {
        get
        {
            return "Move " + value.ToString();
        }
    }
}
