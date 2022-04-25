using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveEffect : TileEffect
{
    public override void Apply(GameManager gameManager, float effectValue){
        gameManager.spacesToMove = (int)effectValue;
        Debug.Log("Move " + effectValue);
    }
}
