using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorTile : MonoBehaviour, EffectTypeEnum
{
    EditorController ctrl;
    Vector3 snapPos;

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
            SetColor();
        }
        else{
            selected = false;
            SetColor();
        }
        

        // Move tile
        /*if(Input.GetMouseButton(1)){
            if(ctrl.mouseLoc.x < gameObject.transform.position.x+1 &&
             ctrl.mouseLoc.x > gameObject.transform.position.x &&
             ctrl.mouseLoc.z < gameObject.transform.position.z+1 &&
             ctrl.mouseLoc.z > gameObject.transform.position.z){
                // mouse is over tile
                //selected = true;
            }
            if(selected){
                Debug.Log("move tile");
                snapPos.x = Mathf.RoundToInt(ctrl.mouseLoc.x - 0.5f); 
                snapPos.z = Mathf.RoundToInt(ctrl.mouseLoc.z - 0.5f);
                gameObject.transform.position = snapPos;
            }
        }*/
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
        switch(effect){
            case EffectTypeEnum.Types.None:
                renderer.material.color = Color.white;
                break;
            case EffectTypeEnum.Types.Move:
                renderer.material.color = Color.green;
                break;
            case EffectTypeEnum.Types.MoveRandom:
                renderer.material.color = Color.yellow;
                break;
        }
        if(selected){
            renderer.material.SetInt("_Highlight", 1);
        }
        else{
            renderer.material.SetInt("_Highlight", 0);
        }
    }
  
}
