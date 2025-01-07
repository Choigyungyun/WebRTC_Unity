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

        [Header("Texts")]
        [SerializeField] private TMP_Text currentAddressText;
        [SerializeField] private TMP_Text currentPortText;

        private void Awake()
        {
            InitSettingPanel();
        }

        private void OnEnable()
        {
            // Buttons
            backSettingButton.onClick.AddListener(OnClickBackSetting);
            applySettingButton.onClick.AddListener(OnClickApplySetting);

            // Inputs
            addressInputField.onValueChanged.AddListener(CheckInputValueChanaged);
            portInputField.onValueChanged.AddListener(CheckInputValueChanaged);
        }

        private void OnDisable()
        {
            // Buttons
            backSettingButton.onClick.RemoveListener(OnClickBackSetting);
            applySettingButton.onClick.RemoveListener(OnClickApplySetting);

            // Inputs
            addressInputField.onValueChanged.RemoveListener(CheckInputValueChanaged);
            portInputField.onValueChanged.RemoveListener(CheckInputValueChanaged);
        }

        #region 버튼 클릭 이벤트
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
            currentAddressText.text = addressInputField.text;
            currentPortText.text = portInputField.text;

            ClearAllFieldText();

            UIEvent.ApplySettingEvent?.Invoke();
        }
        #endregion

        #region 인풋 이벤트
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
        }
    }
}
