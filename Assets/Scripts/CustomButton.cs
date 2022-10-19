using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MultiGraphicButton))]
public class CustomButton : Button
{
    
    public List<TargetGraphicButton> targetGraphicsButtons;
 
    public MultiGraphicButton targetGraphics;
 
    protected override void Start()
    {
        base.Start();
    }
 

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        //get the graphics, if it could not get the graphics, return here
        if (!GetGraphicsButton())
            return;


        Color targetColor; 
        foreach (TargetGraphicButton targetGraphicsButton in targetGraphicsButtons){
            if(targetGraphicsButton.graphic == null) return;
            targetColor =
                state == SelectionState.Disabled ? targetGraphicsButton.disabledColor :
                state == SelectionState.Highlighted ? targetGraphicsButton.highlightedColor :
                state == SelectionState.Normal ? targetGraphicsButton.normalColor :
                state == SelectionState.Pressed ? targetGraphicsButton.pressedColor :
                state == SelectionState.Selected && targetGraphics.selected ? targetGraphicsButton.selectedColor : 
                state == SelectionState.Selected && !targetGraphics.selected ? targetGraphicsButton.normalColor : Color.white;

            if(targetGraphics.selected && state == SelectionState.Normal) {
                targetColor = targetGraphicsButton.selectedColor;
            }


            targetGraphicsButton.graphic.CrossFadeColor(targetColor, instant ? 0 : colors.fadeDuration, true, true);
        }

    }
 
    private bool GetGraphicsButton()
    {
        if(!targetGraphics) targetGraphics = GetComponent<MultiGraphicButton>();
        targetGraphicsButtons = targetGraphics?.GetTargetGraphicsButton;

        return targetGraphicsButtons != null && targetGraphicsButtons.Count > 0;
    }
}
 