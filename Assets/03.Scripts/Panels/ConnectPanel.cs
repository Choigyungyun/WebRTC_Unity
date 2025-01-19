using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPartyWebRTC
{
    public class ConnectPanel : MonoBehaviour
    {
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
            videoRoomButton.onClick.AddListener(OnClickVideoRoom);
        }

        private void OnDisable()
        {
            videoRoomButton.onClick.RemoveListener(OnClickVideoRoom);
        }

        private void OnClickVideoRoom() => UIEvent.VideoRoomClickEvent?.Invoke();
    }
}