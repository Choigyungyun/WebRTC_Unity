using MultiPartyWebRTC.Internal;
using MultiPartyWebRTC.Event;
using System.Collections.Generic;
using TMPro;
using Unity.WebRTC;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace MultiPartyWebRTC
{
    public class SettingPanel : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button backSettingButton;
        [SerializeField] private Button applySettingButton;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField urlInputField;
        [SerializeField] private TMP_InputField protocolInputField;
        [SerializeField] private TMP_InputField nicknameInputField;
        [SerializeField] private TMP_InputField iceServerInputField;
        [SerializeField] private TMP_InputField streamSizeWidthInputField;
        [SerializeField] private TMP_InputField streamSizeHeightInputField;

        [Header("Texts")]
        [SerializeField] private TMP_Text currentURLText;
        [SerializeField] private TMP_Text currentProtocolText;
        [SerializeField] private TMP_Text currentNicknameText;
        [SerializeField] private TMP_Text currentICEServerText;

        [Header("DropDowns")]
        [SerializeField] private TMP_Dropdown streamSizeDropdown;
        [SerializeField] private TMP_Dropdown codecSelectDropDown;

        private List<Vector2Int> streamSizeList = new();
        private List<RTCRtpCodecCapability> availableCodecs;

        private static readonly string[] excludeCodecMimeType = { "video/red", "video/ulpfec", "video/rtx" };

        private void Awake()
        {
            InitSettingPanel();
        }

        private void OnEnable()
        {
            // DataEvent
            DataEvent.UpdateWebSocketDataEvent += UpdateWebSocketSettingValue;
            DataEvent.UpdateUserProfileDataEvent += UpdateUserProfileSettingValue;

            // Buttons
            backSettingButton.onClick.AddListener(OnClickBackSetting);
            applySettingButton.onClick.AddListener(OnClickApplySetting);

            // Inputs
            urlInputField.onValueChanged.AddListener(HandleInputValueChanaged);
            protocolInputField.onValueChanged.AddListener(HandleInputValueChanaged);
            nicknameInputField.onValueChanged.AddListener(HandleInputValueChanaged);
            streamSizeWidthInputField.onValueChanged.AddListener(HandleInputValueChanaged);
            streamSizeHeightInputField.onValueChanged.AddListener(HandleInputValueChanaged);

            // Dropdowns
            streamSizeDropdown.onValueChanged.AddListener(OnChangeStreamSizeSelect);
            codecSelectDropDown.onValueChanged.AddListener(OnChangeCodecSelect);
        }

        private void OnDisable()
        {
            // DataEvent
            DataEvent.UpdateWebSocketDataEvent -= UpdateWebSocketSettingValue;
            DataEvent.UpdateUserProfileDataEvent -= UpdateUserProfileSettingValue;

            // Buttons
            backSettingButton.onClick.RemoveListener(OnClickBackSetting);
            applySettingButton.onClick.RemoveListener(OnClickApplySetting);

            // Inputs
            urlInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);
            protocolInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);
            nicknameInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);
            streamSizeWidthInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);
            streamSizeHeightInputField.onValueChanged.RemoveListener(HandleInputValueChanaged);

            // Dropdowns
            streamSizeDropdown.onValueChanged.RemoveListener(OnChangeStreamSizeSelect);
            codecSelectDropDown.onValueChanged.RemoveListener(OnChangeCodecSelect);
        }

        #region Data 이벤트 함수
        private void UpdateWebSocketSettingValue(string url, string protocol)
        {
            currentURLText.text = url;
            currentProtocolText.text = protocol;
        }

        private void UpdateUserProfileSettingValue(string nickname) => currentNicknameText.text = nickname;
        #endregion

        #region 버튼 클릭 이벤트 함수
        private void OnClickBackSetting()
        {
            ClearAllFieldText();

            UIEvent.BackSettingPanelClickEvent?.Invoke();
        }

        private void OnClickApplySetting()
        {
            DataEvent.ApplySettingDataEvent?.Invoke(CheckApplySettingData(currentURLText, urlInputField.text),
                                                    CheckApplySettingData(currentProtocolText, protocolInputField.text),
                                                    CheckApplySettingData(currentNicknameText, nicknameInputField.text),
                                                    CheckStreamSize(streamSizeDropdown.value),
                                                    CheckCodec(codecSelectDropDown.value));

            ClearAllFieldText();
        }
        #endregion

        #region 인풋 이벤트 함수
        private string CheckApplySettingData(TMP_Text currentText, string inputString)
            => string.IsNullOrEmpty(inputString) ? currentText.text : (currentText.text = inputString);

        #endregion

        #region 드랍다운 이벤트 함수
        private void GetScreenSizeList()
        {
            foreach (Resolution resolution in Screen.resolutions)
            {
                streamSizeList.Add(new Vector2Int(resolution.width, resolution.height));
            }

            List<TMP_Dropdown.OptionData> optionList = streamSizeList.Select(size => new TMP_Dropdown.OptionData($"{size.x} x {size.y}")).ToList();
            optionList.Insert(0, new TMP_Dropdown.OptionData("Custom"));
            streamSizeDropdown.options = optionList;

            bool existInList = streamSizeList.Contains(WebRTCSetting.StreamSize);
            if (existInList)
            {
                streamSizeDropdown.value = streamSizeList.IndexOf(WebRTCSetting.StreamSize);
            }
            else
            {
                streamSizeDropdown.value = streamSizeList.Count;
                streamSizeWidthInputField.text = Screen.width.ToString();
                streamSizeHeightInputField.text = Screen.height.ToString();
                streamSizeWidthInputField.interactable = true;
                streamSizeHeightInputField.interactable = true;
            }
        }

        private void GetCodecList()
        {
            RTCRtpCapabilities capabilities = RTCRtpSender.GetCapabilities(TrackKind.Video);
            availableCodecs = capabilities.codecs
                .Where(codec => !excludeCodecMimeType.Contains(codec.mimeType))
                .ToList();
            List<TMP_Dropdown.OptionData> list = availableCodecs
                .Select(codec => new TMP_Dropdown.OptionData { text = codec.mimeType + " " + codec.sdpFmtpLine })
                .ToList();

            codecSelectDropDown.options.AddRange(list);
            RTCRtpCodecCapability previewCodec = WebRTCSetting.VideoCodec;
            codecSelectDropDown.value = previewCodec == null
                ? 0
                : availableCodecs.FindIndex(x =>
                    x.mimeType == previewCodec.mimeType && x.sdpFmtpLine == previewCodec.sdpFmtpLine) + 1;
        }

        private void OnChangeStreamSizeSelect(int index)
        {
            bool isCustom = index == 0;

            streamSizeWidthInputField.interactable = isCustom;
            streamSizeHeightInputField.interactable = isCustom;

            HandleDropdownValueChanged(streamSizeDropdown, index);
        }

        private void OnChangeCodecSelect(int index)
        {
            WebRTCSetting.VideoCodec = index == 0 ? null : availableCodecs[index];

            HandleDropdownValueChanged(codecSelectDropDown, index);
        }

        private Vector2Int CheckStreamSize(int index) => index == 0 ?
            new Vector2Int(int.Parse(streamSizeWidthInputField.text), int.Parse(streamSizeHeightInputField.text)) : streamSizeList[index];

        private RTCRtpCodecCapability CheckCodec(int index) => index == 0 ?
            null : availableCodecs[index];
        #endregion

        private void InitSettingPanel()
        {
            applySettingButton.interactable = false;

            GetScreenSizeList();
            GetCodecList();
        }

        private void HandleInputValueChanaged(string value) => UpdateApplyButtonState(!string.IsNullOrEmpty(value));
        private void HandleDropdownValueChanged(TMP_Dropdown dropdown, int index)
        {
            if (dropdown.value != index)
            {
                UpdateApplyButtonState(true);
            }
            else
            {
                UpdateApplyButtonState(false);
            }
        }

        private void UpdateApplyButtonState(bool isNull)
        {
            if (applySettingButton.interactable == isNull)
            {
                return;
            }
            applySettingButton.interactable = isNull;
        }

        private void ClearAllFieldText()
        {
            urlInputField.text = string.Empty;
            protocolInputField.text = string.Empty;
            nicknameInputField.text = string.Empty;
        }
    }
}
