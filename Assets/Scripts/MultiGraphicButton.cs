using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MultiGraphicButton : MonoBehaviour
{
    [SerializeField] public List<TargetGraphicButton> targetGraphics;
    public List<TargetGraphicButton> GetTargetGraphicsButton => targetGraphics;
    [HideInInspector] public CustomButton button;
    
    public bool isSelected = false;
    public bool selected {
        get{return isSelected;}
        set{ 
            isSelected = value;

            if(button == null) button = gameObject.GetComponent<CustomButton>();
            
            button.interactable = false;
            button.interactable = true;
        }
    }

}

[System.Serializable]
public class TargetGraphicButton{
    public Graphic graphic;
    public Color normalColor;
    public Color highlightedColor;
    public Color selectedColor;
    public Color pressedColor;
    public Color disabledColor;
}
