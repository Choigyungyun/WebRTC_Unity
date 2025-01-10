using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoRoomPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button backVideoRoomButton;

    private void OnEnable()
    {
        backVideoRoomButton.onClick.AddListener(OnClickBackVideoRoom);
    }

    private void OnDisable()
    {
        backVideoRoomButton.onClick.RemoveListener(OnClickBackVideoRoom);
    }

    private void OnClickBackVideoRoom()
    {
        UIEvent.BackVideoRoomEvent?.Invoke();
    }
}
