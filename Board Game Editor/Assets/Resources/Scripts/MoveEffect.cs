using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveEffect : TileEffect
{
    public int tileCount;
    
    public override void Apply(Player target){
        Debug.Log("Move " + tileCount);
    }
}
