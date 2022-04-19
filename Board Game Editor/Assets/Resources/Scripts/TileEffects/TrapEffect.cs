using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEffect : TileEffect
{
    public override void Apply(GameManager gameManager, float effectValue){
        var escapeNum = (int)Random.Range(-effectValue, effectValue);
        Debug.Log("Roll " + escapeNum + " to escape");
    }
}
