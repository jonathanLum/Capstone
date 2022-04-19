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
    Renderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponentInChildren<Renderer>();
        ctrl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<EditorController>();
        numberText = GetComponentInChildren<TMP_Text>();
        if(parent != null){
            number = parent.GetComponent<EditorTile>().number + 1;
            numberText.text = number.ToString();
        }else{
            numberText.text = "Start";
            effect = EffectTypeEnum.Types.Start;
        }
        SetIcon();
    }

    void Update() {
        if(ctrl.selection.Contains(gameObject)){
            selected = true;
            rend.material.SetInt("_Highlight", 1);
        }
        else{
            selected = false;
            rend.material.SetInt("_Highlight", 0);
        }
        
        if(effect != EffectTypeEnum.Types.Start){
            if(children.Count > 0){
                if(effect == EffectTypeEnum.Types.End){
                    effect = EffectTypeEnum.Types.None;
                }
            }else{
                effect = EffectTypeEnum.Types.End;
            }
            SetIcon();
        }
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

    public void SetIcon(){
        switch(effect)
        {
            case EffectTypeEnum.Types.None:
                icon = null;
                break;
            case EffectTypeEnum.Types.Start:
                icon = (Texture2D)Resources.Load("Textures/StartIcon");
                break;
            case EffectTypeEnum.Types.End:
                icon = (Texture2D)Resources.Load("Textures/FinishIcon");
                break;
            case EffectTypeEnum.Types.Move:
                icon = (Texture2D)Resources.Load("Textures/MoveIcon");
                break;
            case EffectTypeEnum.Types.MoveRandom:
                icon = (Texture2D)Resources.Load("Textures/RandomIcon");
                break;
            case EffectTypeEnum.Types.Trap:
                icon = (Texture2D)Resources.Load("Textures/TrapIcon");
                break;
            case EffectTypeEnum.Types.Attack:
                icon = (Texture2D)Resources.Load("Textures/AttackIcon");
                break;
        }
        iconPlane.GetComponent<Renderer>().material.SetTexture("_Texture2D", icon);
    }
}
