using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEffect : TileEffect
{
    private int value;
    public override void Apply(GameManager gameManager, float effectValue)
    {
        var escapeNum = Mathf.RoundToInt(Random.Range(1f, 6f));
        gameManager.players[gameManager.currentTurn].escapeRoll = escapeNum;
        value = (int)escapeNum;
        Debug.Log("Roll " + escapeNum + " to escape");
    }

    public override string message
    {
        get
        {
            return "Roll " + value.ToString() + " to escape";
        }
    }
}
