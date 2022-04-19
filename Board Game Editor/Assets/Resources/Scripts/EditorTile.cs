using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorTile : MonoBehaviour, EffectTypeEnum
{
    EditorController ctrl;
    Vector3 snapPos;

    public Texture2D icon;
    public GameObject iconPlane;
    TMP_Text numberText;
    int number = 0;
    
    public bool selected = false;

    public EffectTypeEnum.Types effect = EffectTypeEnum.Types.None;
    public float effectValue = 0;

    public GameObject parent;
    public List<GameObject> children;

    
    // Start is called before the first frame update
    void Start()
    {
        ctrl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<EditorController>();
        numberText = GetComponentInChildren<TMP_Text>();
        if(parent != null){
            number = parent.GetComponent<EditorTile>().number + 1;
            numberText.text = number.ToString();
        }else{
            numberText.text = "Start";
        }
        SetColor();
    }

    void Update() {
        if(ctrl.selection.Contains(gameObject)){
            selected = true;
        }
        else{
            selected = false;
        }
        
        if(children.Count > 0){
            if(effect == EffectTypeEnum.Types.End){
                effect = EffectTypeEnum.Types.None;
            }
        }else{
            effect = EffectTypeEnum.Types.End;
        }
        SetColor();
    }


    // Draw child tiles
    bool dragging = false;
    void OnMouseDrag() {
        if(!dragging) ctrl.lastTile = gameObject;
        ctrl.Draw();
        dragging = true;
    }
    private void OnMouseUp() {
        dragging = false;
    }

    private void OnDestroy() {
        
    }

    public void SetColor(){
        var renderer = gameObject.GetComponentInChildren<Renderer>();
        switch(effect)
        {
            case EffectTypeEnum.Types.None:
                icon = null;
                break;
            case EffectTypeEnum.Types.Start:
                icon = null; // add start icon
                break;
            case EffectTypeEnum.Types.End:
                icon = (Texture2D)Resources.Load("UI/Art/FinishIcon");
                break;
            case EffectTypeEnum.Types.Move:
                icon = (Texture2D)Resources.Load("UI/Art/MoveIcon");
                break;
            case EffectTypeEnum.Types.MoveRandom:
                icon = (Texture2D)Resources.Load("UI/Art/RandomIcon");
                break;
            case EffectTypeEnum.Types.Trap:
                icon = (Texture2D)Resources.Load("UI/Art/TrapIcon");
                break;
            case EffectTypeEnum.Types.Attack:
                icon = (Texture2D)Resources.Load("UI/Art/AttackIcon");
                break;
        }
        iconPlane.GetComponent<Renderer>().material.SetTexture("_Texture2D", icon);
        if(selected){
            renderer.material.SetInt("_Highlight", 1);
        }
        else{
            renderer.material.SetInt("_Highlight", 0);
        }
    }
  
}
