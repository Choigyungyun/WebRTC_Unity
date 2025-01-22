using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    protected Stack<GameObject> panelStack = new();

    protected void PanelStackControl(GameObject panel, bool enablePanel, bool isAlive)
    {
        if (enablePanel)
        {
            ActivatePanel(panel, isAlive);
        }
        else
        {
            DeactivatePanel(panel);
        }
    }

    private void ActivatePanel(GameObject panel, bool isAlive)
    {
        if (panelStack.Count == 0)
        {
            Debug.LogWarning("The panel does not exist in the Stack.");
            return;
        }

        if (!isAlive)
        {
            panelStack.Peek().SetActive(false);
        }

        if (panelStack.Contains(panel))
        {
            Debug.LogWarning($"Panel {panel.name} is already active in the stack");
            return;
        }

        panelStack.Push(panel);
        panelStack.Peek().SetActive(true);
    }

    private void DeactivatePanel(GameObject panel)
    {
        if (panelStack.Count == 0 || panelStack.Peek() != panel)
        {
            Debug.LogWarning($"Panel {panel.name} is not the topmost active panel");
            return;
        }

        panelStack.Peek().SetActive(false);
        panelStack.Pop();

        if (panelStack.Count == 0)
        {
            Debug.LogWarning("The panel stack is empty.");
            return;
        }

        panelStack.Peek().SetActive(true);
    }

    protected void DeactivateAllPanel()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
