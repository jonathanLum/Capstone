using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorTile : MonoBehaviour, EffectTypeEnum
{
    EditorController ctrl;
    Vector3 snapPos;

    [SerializeField] private Canvas canvas;
    public Texture2D icon;
    public GameObject iconPlane;
    TMP_Text numberText;
    int number = 0;
    
    public bool selected = false;

    public EffectTypeEnum.Types effect = EffectTypeEnum.Types.None;
    public float effectValue = 1;
    [SerializeField] private TMP_Text effectValText;

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
        effectValText.text = effectValue.ToString();
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
                CanvasVis(false);
                break;
            case EffectTypeEnum.Types.Start:
                icon = null;
                CanvasVis(false);
                break;
            case EffectTypeEnum.Types.End:
                icon = (Texture2D)Resources.Load("Textures/FinishIcon");
                CanvasVis(false);
                break;
            case EffectTypeEnum.Types.Move:
                icon = (Texture2D)Resources.Load("Textures/MoveIcon");
                CanvasVis(true);
                break;
            case EffectTypeEnum.Types.MoveRandom:
                icon = (Texture2D)Resources.Load("Textures/RandomIcon");
                CanvasVis(true);
                break;
            case EffectTypeEnum.Types.Trap:
                icon = (Texture2D)Resources.Load("Textures/TrapIcon");
                CanvasVis(false);
                break;
            case EffectTypeEnum.Types.Attack:
                icon = (Texture2D)Resources.Load("Textures/AttackIcon");
                CanvasVis(false);
                break;
        }
        iconPlane.GetComponent<Renderer>().material.SetTexture("_Texture2D", icon);
    }

    void CanvasVis(bool val){
        canvas.gameObject.SetActive(val);
    }

    public void IncrementEffectVal(){
        effectValue++;
        if(effectValue > 10)
            effectValue = 10;
        if(effectValue == 0)
            effectValue = 1;
        effectValText.text = effectValue.ToString();
    }

    public void DecrementEffectVal(){
        effectValue--;
        if(effectValue < -10)
            effectValue = -10;
        if(effectValue == 0)
            effectValue = -1;
        effectValText.text = effectValue.ToString();
    }
}
