using MultiPartyWebRTC.Event;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class CanvasControl : CanvasManager
    {
        [Header("Panel Objects")]
        [SerializeField] private GameObject homePanel;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject connectPanel;
        [SerializeField] private GameObject videoRoomPanel;

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
        private void ShowSettingPanel()
        {
            PanelStackControl(settingPanel, true, false);
        }
        private void ShowConnectPanel()
        {
            PanelStackControl(connectPanel, true, true);
        }

        // Panel hide
        private void HideSettingPanel()
        {
            PanelStackControl(settingPanel, false, false);
        }
        private void HideConnectPanel()
        {
            PanelStackControl(connectPanel, false, false);
        }

        #endregion

        private void InitPanel()
        {
            DeactivateAllPanel();

            ResetPanelStackWithFistPanel();
        }

        private void ResetPanelStackWithFistPanel()
        {
            panelStack.Clear();

            panelStack.Push(homePanel);
            panelStack.Peek().SetActive(true);
        }
    }
}
