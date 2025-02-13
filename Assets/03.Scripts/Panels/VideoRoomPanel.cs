using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPartyWebRTC
{
    public class VideoRoomPanel : MonoBehaviour
    {
        [Header("Peer prefabs")]
        [SerializeField] private GameObject localPeerObject;
        [SerializeField] private GameObject remotePeerObject;

        [Header("Parents of peer objects")]
        [SerializeField] private Transform peerContent;

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

            InstanceLocalPeer();
        }

        private void OnDisable()
        {
            handUpVideoRoomButton.onClick.RemoveListener(OnClickHangUpVideoRoom);

            streamToggle.onValueChanged.RemoveListener(ChangeStreamState);
            microphoneToggle.onValueChanged.RemoveListener(ChangeMicrophoneState);

            DestroyAllPeers();
        }

        private void InstanceLocalPeer()
        {
            GameObject localPeer = Instantiate(localPeerObject, peerContent);
            localPeer.transform.SetSiblingIndex(0);
        }

        private void DestroyAllPeers()
        {
            if(peerContent.childCount == 0 || peerContent == null)
            {
                return;
            }
            foreach (Transform child in peerContent)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnClickHangUpVideoRoom()
        {
            UIEvent.HangUpVideoRoomEvent?.Invoke();
        }

        private void ChangeStreamState(bool isOn)
        {
            UIEvent.VideoRoomStreamToggleEvent?.Invoke(isOn);
        }

        private void ChangeMicrophoneState(bool isOn)
        {
            UIEvent.VideoRoomMicrophoneToggleEvent?.Invoke(isOn);
        }
    }
}