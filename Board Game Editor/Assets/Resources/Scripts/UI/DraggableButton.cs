using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class OnAddEffect : UnityEvent<EffectTypeEnum.Types>
{
}

public class DraggableButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public OnAddEffect onAddEffect;
    
    RectTransform rectTransform;

    Vector3 originalPosition;

    public EffectTypeEnum.Types effect = EffectTypeEnum.Types.None;

    void Awake(){
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData){
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData){
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData){
        //Debug.Log("OnEndDrag");
        rectTransform.position = originalPosition;
        onAddEffect.Invoke(effect);
    }
}
