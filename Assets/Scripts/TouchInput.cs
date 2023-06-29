using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    // Buttons
    [SerializeField] HoldableUIButton leftButton;
    [SerializeField] HoldableUIButton rightButton;
    [SerializeField] HoldableUIButton grabButton;
    [SerializeField] HoldableUIButton upButton;
    [SerializeField] HoldableUIButton downButton;

    // Input axes
    float horizontalAxis = 0f;
    float verticalAxis = 0f;

    bool jumpButtonDown = false;
    bool fire1ButtonDown = false;

    bool jumpButtonPressRegistered = false;
    bool fire1ButtonPressRegistered = false;

    // Update is called once per frame
    void Update()
    {
        UpdateHorizontalAxis();
        UpdateVerticalAxis();
        UpdateButtonDown();
    }

    // Update the horizontal axis
    void UpdateHorizontalAxis()
    {
        horizontalAxis = 0;

        // Left button
        if (leftButton.buttonPressed)
            horizontalAxis -= 1;
        
        // Right button
        if (rightButton.buttonPressed)
            horizontalAxis += 1;
    }    
    
    // Update the vertical axis
    void UpdateVerticalAxis()
    {
        verticalAxis = 0;

        // Up button
        if (upButton.buttonPressed)
            verticalAxis += 1;
        
        // Down button
        if (downButton.buttonPressed)
            verticalAxis -= 1;
    }

    // Update the button down state
    void UpdateButtonDown()
    {
        // Grab button is for grabbing
        if (grabButton.buttonPressed)
        {
            if (!fire1ButtonPressRegistered)
            {
                fire1ButtonDown = true;
                fire1ButtonPressRegistered = true;
            }
        }
        else
            fire1ButtonPressRegistered = false;
    
        // Up button is jump button
        if (upButton.buttonPressed)
        {
            if (!jumpButtonPressRegistered)
            {
                jumpButtonDown = true;
                jumpButtonPressRegistered = true;
            }
        }
        else
            jumpButtonPressRegistered = false;
    }

    // Get axis
    public float GetAxis(string axisName)
    {
        float returnValue = 0f;

        switch (axisName)
        {
            case "Horizontal":
                returnValue = horizontalAxis;
                break;
            case "Vertical":
                returnValue = verticalAxis;
                break;
        }

        return returnValue;
    }

    // Get the button down state
    public bool GetButtonDown(string buttonName)
    {
        bool returnValue = false;

        switch (buttonName)
        {
            case "Jump":
                returnValue = jumpButtonDown;
                jumpButtonDown = false;
                break;
                
            case "Fire1":
                returnValue = fire1ButtonDown;
                fire1ButtonDown = false;
                break;
        }

        return returnValue;
    }
}
