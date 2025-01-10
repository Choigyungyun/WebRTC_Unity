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
        [SerializeField] private TMP_InputField urlInputField;
        [SerializeField] private TMP_InputField ProtocolInputField;
        [SerializeField] private TMP_InputField nicknameInputField;

        [Header("Texts")]
        [SerializeField] private TMP_Text currentURLText;
        [SerializeField] private TMP_Text currentProtocolText;
        [SerializeField] private TMP_Text currentNicknameText;

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
            urlInputField.onValueChanged.AddListener(HandleInputValueChanaged);
            ProtocolInputField.onValueChanged.AddListener(HandleInputValueChanaged);
            nicknameInputField.onValueChanged.AddListener(HandleInputValueChanaged);
        }

        private void OnDisable()
        {
            // DataEvent
            DataEvent.SetDefaultWebSocketEvent -= SetDefaultWebSocket;

            // Buttons
            backSettingButton.onClick.RemoveListener(OnClickBackSetting);
            applySettingButton.onClick.RemoveListener(OnClickApplySetting);

            // Inputs
            urlInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);
            ProtocolInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);
            nicknameInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);
        }

        #region Data 이벤트 함수
        private void SetDefaultWebSocket(string url, string protocol, string name)
        {
            currentURLText.text = url;
            currentProtocolText.text = protocol;
            currentNicknameText.text = name;
        }
        #endregion

        #region 버튼 클릭 이벤트 함수
        private void OnClickBackSetting()
        {
            ClearAllFieldText();

            UIEvent.BackSettingEvent?.Invoke();
        }

        private void OnClickApplySetting()
        {
            UIEvent.ApplySettingEvent?.Invoke(CheckApplySettingData(currentURLText, urlInputField.text),
                                              CheckApplySettingData(currentProtocolText, ProtocolInputField.text),
                                              CheckApplySettingData(currentNicknameText, nicknameInputField.text));

            ClearAllFieldText();
        }
        #endregion

        #region 인풋 이벤트 함수
        private void InitSettingPanel()
        {
            applySettingButton.interactable = false;
        }

        private void HandleInputValueChanaged(string value)
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

        private string CheckApplySettingData(TMP_Text currentText, string inputString) => string.IsNullOrEmpty(inputString) ? currentText.text : (currentText.text = inputString);

        #endregion

        private void ClearAllFieldText()
        {
            urlInputField.text = string.Empty;
            ProtocolInputField.text = string.Empty;
            nicknameInputField.text = string.Empty;
        }
    }
}
