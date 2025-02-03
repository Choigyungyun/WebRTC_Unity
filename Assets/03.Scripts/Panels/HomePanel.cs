using UnityEngine;
using UnityEngine.UI;
using MultiPartyWebRTC.Event;
using TMPro;
using UnityEngine.SceneManagement;

namespace MultiPartyWebRTC
{
    public class HomePanel : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button connectButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI versionText;

        private void Awake()
        {
            versionText.text = Application.version + "v";
        }

        private void OnEnable()
        {
            connectButton.onClick.AddListener(OnClickVideoRoom);
            settingButton.onClick.AddListener(OnClickSetting);
            quitButton.onClick.AddListener(OnClickQuit);
        }

        private void OnDisable()
        {
            connectButton.onClick.RemoveListener(OnClickVideoRoom);
            settingButton.onClick.RemoveListener(OnClickSetting);
            quitButton.onClick.RemoveListener(OnClickQuit);
        }

        private void OnClickVideoRoom()
        {
            UIEvent.ConnectClickEvent?.Invoke();
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
