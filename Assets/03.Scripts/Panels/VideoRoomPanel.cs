using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPartyWebRTC
{
    public class VideoRoomPanel : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private GameObject localPeerObject;
        [SerializeField] private GameObject remotePeerObject;

        [Header("Buttons")]
        [SerializeField] private Button handUpVideoRoomButton;

        [Header("Toggles")]
        [SerializeField] private Toggle streamToggle;
        [SerializeField] private Toggle microphoneToggle;

        private void OnEnable()
        {
            handUpVideoRoomButton.onClick.AddListener(OnClickHangUpVideoRoom);

            streamToggle.onValueChanged.AddListener(ChangeStreamState);
            microphoneToggle.onValueChanged.AddListener(ChangeMicrophoneState);
        }

        private void OnDisable()
        {
            handUpVideoRoomButton.onClick.RemoveListener(OnClickHangUpVideoRoom);

            streamToggle.onValueChanged.RemoveListener(ChangeStreamState);
            microphoneToggle.onValueChanged.RemoveListener(ChangeMicrophoneState);
        }

        private void OnClickHangUpVideoRoom()
        {
            UIEvent.HangUpVideoRoomEvent?.Invoke();
        }

        private void ChangeStreamState(bool isOn) => UIEvent.VideoRoomStreamToggleEvent?.Invoke(isOn);

        private void ChangeMicrophoneState(bool isOn) => UIEvent.VideoRoomMicrophoneToggleEvent?.Invoke(isOn);
    }
}