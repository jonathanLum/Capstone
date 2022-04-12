using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileEffect effect;
    public GameObject parent;
    public GameObject[] children;

    public Material mat;
    public Material[] mats;

    public Renderer childColor;
    
    // Start is called before the first frame update
    void Start()
    {
        //effect = ScriptableObject.CreateInstance<MoveEffect>();
        //effect.Apply(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
