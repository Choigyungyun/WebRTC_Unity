using MultiPartyWebRTC.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPartyWebRTC
{
    public class SettingPanel : PanelManager
    {
        [Header("Buttons")]
        [SerializeField] private Button backSettingButton;
        [SerializeField] private Button applySettingButton;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField addressInputField;
        [SerializeField] private TMP_InputField portInputField;

        private void OnEnable()
        {
            backSettingButton.onClick.AddListener(OnClickBackSetting);
            applySettingButton.onClick.AddListener(OnClickApplySetting);
        }

        private void OnDisable()
        {
            backSettingButton.onClick.RemoveListener(OnClickBackSetting);
            applySettingButton.onClick.RemoveListener(OnClickApplySetting);
        }

        private void OnClickBackSetting()
        {
            UIEvent.BackSettingEvent?.Invoke();
        }

        private void OnClickApplySetting()
        {
            UIEvent.ApplySettingEvent?.Invoke();
        }
    }
}
