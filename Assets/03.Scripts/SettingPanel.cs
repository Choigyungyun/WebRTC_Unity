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
        [SerializeField] private TMP_InputField nickNameInputField;

        [Header("Texts")]
        [SerializeField] private TMP_Text currentURLText;
        [SerializeField] private TMP_Text currentProtocolText;
        [SerializeField] private TMP_Text currentNickNameText;

        private void Awake()
        {
            InitSettingPanel();
        }

        private void OnEnable()
        {
            // DataEvent
            DataEvent.SetDefaultWebSocketEvent += SetDefaultWebSocket;

            // Buttons
            backSettingButton.onClick.AddListener(OnClickBackSetting);
            applySettingButton.onClick.AddListener(OnClickApplySetting);

            // Inputs
            addressInputField.onValueChanged.AddListener(CheckInputValueChanaged);
            portInputField.onValueChanged.AddListener(CheckInputValueChanaged);
            nickNameInputField.onValueChanged.AddListener(CheckInputValueChanaged);
        }

        private void OnDisable()
        {
            // DataEvent
            DataEvent.SetDefaultWebSocketEvent -= SetDefaultWebSocket;

            // Buttons
            backSettingButton.onClick.RemoveListener(OnClickBackSetting);
            applySettingButton.onClick.RemoveListener(OnClickApplySetting);

            // Inputs
            addressInputField.onValueChanged.RemoveListener(CheckInputValueChanaged);
            portInputField.onValueChanged.RemoveListener(CheckInputValueChanaged);
            nickNameInputField.onValueChanged.RemoveListener(CheckInputValueChanaged);
        }

        #region Data �̺�Ʈ �Լ�
        private void SetDefaultWebSocket(string url, string protocol, string name)
        {
            currentURLText.text = url;
            currentProtocolText.text = protocol;
            currentNickNameText.text = name;
        }
        #endregion

        #region ��ư Ŭ�� �̺�Ʈ �Լ�
        private void InitSettingPanel()
        {
            applySettingButton.interactable = false;
        }

        private void OnClickBackSetting()
        {
            ClearAllFieldText();

            UIEvent.BackSettingEvent?.Invoke();
        }

        private void OnClickApplySetting()
        {
            currentURLText.text = addressInputField.text;
            currentProtocolText.text = portInputField.text;

            ClearAllFieldText();

            UIEvent.ApplySettingEvent?.Invoke();
        }
        #endregion

        #region ��ǲ �̺�Ʈ �Լ�
        private void CheckInputValueChanaged(string value)
        {
            UpdateApplyButtonState(!string.IsNullOrEmpty(value));
        }


        private void UpdateApplyButtonState(bool isNull)
        {
            if (applySettingButton.interactable == isNull)
            {
                return;
            }
            applySettingButton.interactable = isNull;
        }
        #endregion

        private void ClearAllFieldText()
        {
            addressInputField.text = string.Empty;
            portInputField.text = string.Empty;
            nickNameInputField.text = string.Empty;
        }
    }
}
