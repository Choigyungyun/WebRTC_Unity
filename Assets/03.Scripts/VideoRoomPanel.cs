using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoRoomPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button handUpVideoRoomButton;
    [SerializeField] private Button streamButton;
    [SerializeField] private Button microphoneButton;

    private void OnEnable()
    {
        handUpVideoRoomButton.onClick.AddListener(OnClickHangUpVideoRoom);
        streamButton.onClick.AddListener(OnClickStream);
        microphoneButton.onClick.AddListener(OnClickMicroPhone);
    }

    private void OnDisable()
    {
        handUpVideoRoomButton.onClick.RemoveListener(OnClickHangUpVideoRoom);
        streamButton.onClick.RemoveListener(OnClickStream);
        microphoneButton.onClick.RemoveListener(OnClickMicroPhone);
    }

    private void OnClickHangUpVideoRoom()
    {
        UIEvent.HangUpVideoRoomEvent?.Invoke();
    }

    private void OnClickStream()
    {
        UIEvent.VideoRoomStreamClickEvent?.Invoke();
    }

    private void OnClickMicroPhone()
    {
        UIEvent.VideoRoomMicrophoneClickEvent?.Invoke();
    }

    private void ChangeButtonStateColor(Image buttonImage)
    {

    }
}
