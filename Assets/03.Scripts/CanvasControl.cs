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

        private void Start()
        {
            InitPanel();
        }

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

        private void PanelStackControl(GameObject panel, bool enablePanel)
        {
            panelStack.Peek().SetActive(false);

            if (enablePanel)
            {
                if (panelStack.Contains(panel))
                {
                    return;
                }
                panelStack.Push(panel);
            }
            else
            {
                if (!panelStack.Contains(panel))
                {
                    return;
                }
                panelStack.Pop();
            }

            panelStack.Peek().SetActive(true);
        }

        private void InitPanel()
        {
            panelStack.Clear();
            panelStack.Push(homePanel);
            panelStack.Peek().SetActive(true);
        }
    }
}
