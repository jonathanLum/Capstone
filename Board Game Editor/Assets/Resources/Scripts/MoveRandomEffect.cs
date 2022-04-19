using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveRandomEffect : TileEffect
{
    public int range;
    int tileCount;
    
    public override void Apply(Player target){
        tileCount = (int)Random.Range(-range, range);
        Debug.Log("Move " + tileCount);
    }
}
