using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : TileEffect
{
    public override void Apply(GameManager gameManager, float effectValue){
        gameManager.attacking = true;
        Debug.Log("Attack chosen player");
    }
}
