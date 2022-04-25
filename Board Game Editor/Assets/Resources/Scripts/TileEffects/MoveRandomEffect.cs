using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomEffect : TileEffect
{   
    public override void Apply(GameManager gameManager, float effectValue){
        var tileCount = (int)Random.Range(-effectValue, effectValue);
        while(tileCount == 0){
            tileCount = (int)Random.Range(-effectValue, effectValue);
        }
        gameManager.spacesToMove = (int)tileCount;
        Debug.Log("Move random" + tileCount);
    }
}
