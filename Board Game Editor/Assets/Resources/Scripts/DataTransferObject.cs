using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataTransferObject
{
    public Vector3 position;
    public EffectTypeEnum.Types effect;
    public float effectValue;
    public int parent;
    public List<int> children = new List<int>();
    
    
    public void Clean(){
        children.Clear();
    }
}
