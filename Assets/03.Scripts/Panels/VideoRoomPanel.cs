using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Peer;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            DataEvent.OnRoomPublishersUpdateEvent += InstanceRemotePeer;
            DataEvent.LeavingRemotePeerEvent += LeavingPeer;

            InstanceLocalPeer();
        }

        private void OnDisable()
        {
            handUpVideoRoomButton.onClick.RemoveListener(OnClickHangUpVideoRoom);

            streamToggle.onValueChanged.RemoveListener(ChangeStreamState);
            microphoneToggle.onValueChanged.RemoveListener(ChangeMicrophoneState);

            DataEvent.OnRoomPublishersUpdateEvent -= InstanceRemotePeer;
            DataEvent.LeavingRemotePeerEvent -= LeavingPeer;

            DestroyAllPeers();
        }

        private void InstanceLocalPeer()
        {
            GameObject localPeerObject = Instantiate(this.localPeerObject, peerContent);
            localPeerObject.transform.SetSiblingIndex(0);
        }

        private void InstanceRemotePeer(int totalPeers, List<JObject> publisherDatas)
        {
            for (int peerIndex = 0; peerIndex < totalPeers; peerIndex++)
            {
                GameObject remotePeerObject = Instantiate(this.remotePeerObject, peerContent);
                RemotePeer remotePeer = remotePeerObject.GetComponent<RemotePeer>();

                remotePeer.SetRemotePeer(publisherDatas[peerIndex]);
            }
        }

        private void LeavingPeer(string nickname)
        {
            GameObject leaver = !peerContent.Find(nickname) ? null : peerContent.Find(nickname).gameObject;
            if (leaver == null)
            {
                return;
            }
            Destroy(leaver);
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
            DestroyAllPeers();
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