using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveEffect : TileEffect
{

    public int value;
    public override void Apply(GameManager gameManager, float effectValue)
    {
        gameManager.spacesToMove = (int)effectValue;
        value = (int)effectValue;
        Debug.Log("Move " + effectValue);
    }

    public override string text
    {
        get
        {
            if (value < 0)
            {
                return "Move back " + value.ToString();
            }

            return "Move forward " + value;
        }
    }
}
