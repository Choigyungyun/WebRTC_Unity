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

        private WebSocketHandler webSocketHandler;

        private Stack<GameObject> panelStack = new();

        private void OnEnable()
        {
            UIEvent.VideoRoomClickEvent += ShowVideoRoomPanel;
            UIEvent.SettingClickEvent += ShowSettingPanel;

            UIEvent.BackSettingEvent += HideSettingPanel;
            UIEvent.ApplySettingEvent += ApplySetting;
        }

        private void OnDisable()
        {
            UIEvent.VideoRoomClickEvent -= ShowVideoRoomPanel;
            UIEvent.SettingClickEvent -= ShowSettingPanel;

            UIEvent.BackSettingEvent -= HideSettingPanel;
            UIEvent.ApplySettingEvent -= ApplySetting;
        }

        private void Awake()
        {
            webSocketHandler = new WebSocketHandler();
        }

        private void Start()
        {
            InitPanel();
        }

        #region 이벤트 함수
        private void ShowVideoRoomPanel()
        {
            PanelStackControl(videoRoomPanel, true);
        }

        private void ShowSettingPanel()
        {
            PanelStackControl(settingPanel, true);
        }

        private void HideSettingPanel()
        {
            PanelStackControl(settingPanel, false);
        }

        private void ApplySetting()
        {

        }
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
