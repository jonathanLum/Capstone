using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEffect : TileEffect
{
    public override void Apply(GameManager gameManager, float effectValue){
        var escapeNum = Mathf.RoundToInt(Random.Range(1f, 6f));
        gameManager.players[gameManager.currentTurn].escapeRoll = escapeNum;
        Debug.Log("Roll " + escapeNum + " to escape");
    }
}
