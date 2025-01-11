using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    private bool isEnable = false;

    private Color enableColor;
    private readonly Color disableColor = Color.HSVToRGB(165.0f / 360.0f, 107.0f / 255.0f, 0.0f);

    private Button button;
    private ColorBlock buttonColors;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component is missing!");
            enabled = false;
            return;
        }

        buttonColors = button.colors;
        enableColor = buttonColors.normalColor;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ToggleColor);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ToggleColor);
    }

    private void ToggleColor()
    {
        buttonColors.normalColor = isEnable ? enableColor : disableColor;
        button.colors = buttonColors;

        isEnable = !isEnable;
    }
}
