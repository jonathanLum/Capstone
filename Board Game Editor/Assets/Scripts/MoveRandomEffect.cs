using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveRandomEffect : TileEffect
{
    public int range;
    int tileCount;

    void OnEnable(){
        tileCount = (int)Random.Range(-range, range);
    }
    
    public override void Apply(GameObject target){
        Debug.Log("Move " + tileCount);
    }
}
