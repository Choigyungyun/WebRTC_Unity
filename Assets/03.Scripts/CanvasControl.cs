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
        [SerializeField] private GameObject connectPanel;
        [SerializeField] private GameObject videoRoomPanel;

        private Stack<GameObject> panelStack = new();

        private void OnEnable()
        {
            // Home Panel Evets
            UIEvent.SettingClickEvent += ShowSettingPanel;
            UIEvent.ConnectClickEvent += ShowConnectPanel;

            // Setting Panel Evets
            UIEvent.BackSettingPanelClickEvent += HideSettingPanel;

            // Connect Panel Events
            UIEvent.BackConnectPanelEvent += HideConnectPanel;
        }

        private void OnDisable()
        {
            // Home Panel Evets
            UIEvent.ConnectClickEvent -= ShowConnectPanel;
            UIEvent.SettingClickEvent -= ShowSettingPanel;

            // Setting Panel Evets
            UIEvent.BackSettingPanelClickEvent -= HideSettingPanel;

            // Connect Panel Events
            UIEvent.BackConnectPanelEvent -= HideConnectPanel;
        }

        private void Start()
        {
            InitPanel();
        }

        #region 이벤트 함수
        // Panel show
        private void ShowSettingPanel() => PanelStackControl(settingPanel, true, false);
        private void ShowConnectPanel() => PanelStackControl(connectPanel, true, false);
        private void ShowVideoRoomPanel() => PanelStackControl(videoRoomPanel, true, false);

        // Panel hide
        private void HideSettingPanel() => PanelStackControl(settingPanel, false, false);
        private void HideConnectPanel() => PanelStackControl(connectPanel, false, false);

        // Setting panel events

        // Connect panel events

        // VideoRoom panel events

        #endregion

        private void InitPanel()
        {
            DeactivateAllPanel();

            ResetPanelStackWithFistPanel();
        }

        private void PanelStackControl(GameObject panel, bool enablePanel, bool isAlive)
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
            if(panelStack.Count == 0)
            {
                Debug.LogWarning("The panel does not exist in the Stack.");
                return;
            }

            if(!isAlive)
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
