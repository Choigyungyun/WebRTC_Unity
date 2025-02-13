using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPartyWebRTC
{
    public class ConnectPanel : MonoBehaviour
    {
        [SerializeField] private Button backConnectPanelButton;
        [SerializeField] private Button echoTestButton;
        [SerializeField] private Button streamingButton;
        [SerializeField] private Button videoCallButton;
        [SerializeField] private Button sipGatewayButton;
        [SerializeField] private Button videoRoomButton;
        [SerializeField] private Button videoRoomMultiStreamButton;
        [SerializeField] private Button audioBridgeButton;
        [SerializeField] private Button textRoomButton;

        private void OnEnable()
        {
            backConnectPanelButton.onClick.AddListener(OnClickBackConnectPanel);
            videoRoomButton.onClick.AddListener(OnClickVideoRoom);
        }

        private void OnDisable()
        {
            backConnectPanelButton.onClick.RemoveListener(OnClickBackConnectPanel);
            videoRoomButton.onClick.RemoveListener(OnClickVideoRoom);
        }

        private void ActivatePluginButtons()
        {
            videoRoomButton.interactable = true;
        }

        private void DisablePluginButtons()
        {
            echoTestButton.interactable = false;
            streamingButton.interactable = false;
            videoCallButton.interactable = false;
            sipGatewayButton.interactable = false;
            videoRoomButton.interactable = false;
            videoRoomMultiStreamButton.interactable = false;
            audioBridgeButton.interactable = false;
            textRoomButton.interactable = false;
        }

        private void OnClickBackConnectPanel()
        {
            UIEvent.BackConnectPanelEvent?.Invoke();
        }

        private void OnClickVideoRoom()
        {
            UIEvent.VideoRoomClickEvent?.Invoke();
        }
    }
}