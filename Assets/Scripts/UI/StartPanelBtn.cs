using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartPanelBtn : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public SceneController sceneController;

    public void OnPointerClick(PointerEventData eventData)
    {
        sceneController.PlayAudio(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sceneController.PlayAudio(1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
