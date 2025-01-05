using UnityEngine;
using UnityEngine.UI;
using MultiPartyWebRTC.Event;
using TMPro;
using UnityEngine.SceneManagement;

namespace MultiPartyWebRTC
{
    public class HomePanel : PanelManager
    {
        [Header("Buttons")]
        [SerializeField] private Button videoRoomButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI versionText;

        private void Awake()
        {
            versionText.text = Application.version;
        }

        private void Start()
        {
            videoRoomButton.onClick.AddListener(OnClickVideoRoom);
            settingButton.onClick.AddListener(OnClickSetting);
            quitButton.onClick.AddListener(OnClickQuit);
        }

        private void OnClickVideoRoom()
        {
            UIEvent.VideoRoomClickEvent?.Invoke();
        }

        private void OnClickSetting()
        {
            UIEvent.SettingClickEvent?.Invoke();
        }

        private void OnClickQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
