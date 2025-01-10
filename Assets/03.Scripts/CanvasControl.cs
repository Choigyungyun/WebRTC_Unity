using MultiPartyWebRTC.Event;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class CanvasControl : MonoBehaviour
    {
        [Header("Panel Objects")]
        [SerializeField] private GameObject homePanel;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject videoRoomPanel;

        private Stack<GameObject> panelStack = new();

        private void OnEnable()
        {
            // Home Panel Evets
            UIEvent.VideoRoomClickEvent += ShowVideoRoomPanel;
            UIEvent.SettingClickEvent += ShowSettingPanel;

            // Setting Panel Evets
            UIEvent.BackSettingEvent += HideSettingPanel;
            UIEvent.ApplySettingEvent += ApplySetting;

            // Video Room Panel Events
            UIEvent.BackVideoRoomEvent += HideVideoRoomPanel;
        }

        private void OnDisable()
        {
            // Home Panel Evets
            UIEvent.VideoRoomClickEvent -= ShowVideoRoomPanel;
            UIEvent.SettingClickEvent -= ShowSettingPanel;

            // Home Panel Evets
            UIEvent.BackSettingEvent -= HideSettingPanel;
            UIEvent.ApplySettingEvent -= ApplySetting;

            // Video Room Panel Events
            UIEvent.BackVideoRoomEvent -= HideVideoRoomPanel;
        }

        private void Start()
        {
            InitPanel();
        }

        #region 이벤트 함수
        // Home Panel Event 함수
        private void ShowVideoRoomPanel() => PanelStackControl(videoRoomPanel, true);
        private void ShowSettingPanel() => PanelStackControl(settingPanel, true);

        // Settin Panel Event 함수
        private void HideSettingPanel() => PanelStackControl(settingPanel, false);
        private void ApplySetting(string url, string protocol, string nickname)
        {

        }

        // Video Room Event 함수
        private void HideVideoRoomPanel() => PanelStackControl(videoRoomPanel, false);
        #endregion

        private void InitPanel()
        {
            DeactivateAllPanel();

            ResetPanelStackWithFistPanel();
        }

        private void PanelStackControl(GameObject panel, bool enablePanel)
        {
            if (enablePanel)
            {
                ActivatePanel(panel);
            }
            else
            {
                DeactivatePanel(panel);
            }
        }

        private void ActivatePanel(GameObject panel)
        {
            if(panelStack.Count == 0)
            {
                Debug.LogWarning("The panel does not exist in the Stack.");
                return;
            }

            panelStack.Peek().SetActive(false);

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

        private void DeactivateAllPanel()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void ResetPanelStackWithFistPanel()
        {
            panelStack.Clear();

            panelStack.Push(homePanel);
            panelStack.Peek().SetActive(true);
        }
    }
}
