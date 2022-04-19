using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomEffect : TileEffect
{   
    public override void Apply(GameManager gameManager, float effectValue){
        var tileCount = (int)Random.Range(-effectValue, effectValue);
        Debug.Log("Move random" + tileCount);
    }
}
