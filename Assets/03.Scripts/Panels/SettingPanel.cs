using MultiPartyWebRTC.Internal;
using MultiPartyWebRTC.Event;
using System.Collections.Generic;
using TMPro;
using Unity.WebRTC;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using UnityEditor;

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
        [SerializeField] private TMP_Text stateApplyText;

        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown streamSizeDropdown;
        [SerializeField] private TMP_Dropdown codecSelectDropdown;

        private int currentStreamSizeIndex;
        private int currentCodecSelectIndex;
        private List<Vector2Int> streamSizeList = new();
        private List<RTCRtpCodecCapability> availableCodecs;
        private static readonly string[] ExcludeCodecMimeTypes = { "video/red", "video/ulpfec", "video/rtx" };

        private void Awake()
        {
            InitializePanel();
            PopulateStreamSizeList();
            PopulateCodecList();
        }

        private void OnEnable()
        {
            AddListeners();
            ResetStateText();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        #region Initialization
        private void InitializePanel()
        {
            applySettingButton.interactable = false;

            currentURLText.text = WebSocketSetting.WebSocketURL;
            currentProtocolText.text = WebSocketSetting.WebSocketProtocol;
            currentNicknameText.text = UserProfileSetting.Nickname;
        }

        private void AddListeners()
        {
            backSettingButton.onClick.AddListener(HandleBackButtonClick);
            applySettingButton.onClick.AddListener(ApplySettings);

            urlInputField.onValueChanged.AddListener(OnInputValueChanged);
            protocolInputField.onValueChanged.AddListener(OnInputValueChanged);
            nicknameInputField.onValueChanged.AddListener(OnInputValueChanged);
            streamSizeWidthInputField.onValueChanged.AddListener(OnInputValueChanged);
            streamSizeHeightInputField.onValueChanged.AddListener(OnInputValueChanged);

            streamSizeDropdown.onValueChanged.AddListener(OnStreamSizeDropdownChanged);
            codecSelectDropdown.onValueChanged.AddListener(OnCodecDropdownChanged);
        }

        private void RemoveListeners()
        {
            backSettingButton.onClick.RemoveListener(HandleBackButtonClick);
            applySettingButton.onClick.RemoveListener(ApplySettings);

            urlInputField.onValueChanged.RemoveListener(OnInputValueChanged);
            protocolInputField.onValueChanged.RemoveListener(OnInputValueChanged);
            nicknameInputField.onValueChanged.RemoveListener(OnInputValueChanged);
            streamSizeWidthInputField.onValueChanged.RemoveListener(OnInputValueChanged);
            streamSizeHeightInputField.onValueChanged.RemoveListener(OnInputValueChanged);

            streamSizeDropdown.onValueChanged.RemoveListener(OnStreamSizeDropdownChanged);
            codecSelectDropdown.onValueChanged.RemoveListener(OnCodecDropdownChanged);
        }
        #endregion

        #region Dropdown Handling
        private void PopulateStreamSizeList()
        {
            streamSizeList = Screen.resolutions.Select(res => new Vector2Int(res.width, res.height)).ToList();

            var options = streamSizeList
                .Select(size => new TMP_Dropdown.OptionData($"{size.x} x {size.y}"))
                .ToList();

            options.Insert(0, new TMP_Dropdown.OptionData("Custom"));
            streamSizeDropdown.options = options;

            SetInitialStreamSizeSelection();
        }

        private void SetInitialStreamSizeSelection()
        {
            bool existsInList = streamSizeList.Contains(WebRTCSetting.StreamSize);

            currentStreamSizeIndex = existsInList
                ? streamSizeList.IndexOf(WebRTCSetting.StreamSize) + 1
                : 0;

            streamSizeDropdown.value = currentStreamSizeIndex;

            if (!existsInList)
            {
                streamSizeWidthInputField.text = Screen.width.ToString();
                streamSizeHeightInputField.text = Screen.height.ToString();
                streamSizeWidthInputField.interactable = true;
                streamSizeHeightInputField.interactable = true;
            }
        }

        private void PopulateCodecList()
        {
            availableCodecs = RTCRtpSender.GetCapabilities(TrackKind.Video).codecs
                .Where(codec => !ExcludeCodecMimeTypes.Contains(codec.mimeType))
                .ToList();

            // "Default" 항목을 추가
            var options = availableCodecs
                .Select(codec => new TMP_Dropdown.OptionData($"{codec.mimeType} {codec.sdpFmtpLine}"))
                .ToList();

            options.Insert(0, new TMP_Dropdown.OptionData("Default"));  // "Default" 추가

            codecSelectDropdown.options = options;

            // "Default"가 선택된 경우, `availableCodecs`에서 "Default"와 일치하는 항목이 없으므로 `null`로 설정
            codecSelectDropdown.value = availableCodecs.FindIndex(codec =>
                codec.mimeType == WebRTCSetting.VideoCodec?.mimeType &&
                codec.sdpFmtpLine == WebRTCSetting.VideoCodec?.sdpFmtpLine) + 1;

            // "Default"가 선택되었을 경우, value를 0으로 설정
            if (codecSelectDropdown.value != 0)
            {
                return;
            }

            WebRTCSetting.VideoCodec = null;  // "Default"를 선택하면 VideoCodec은 null로 설정
        }

        private void OnStreamSizeDropdownChanged(int index)
        {
            bool isCustom = index == 0;
            streamSizeWidthInputField.interactable = isCustom;
            streamSizeHeightInputField.interactable = isCustom;

            CheckForChanges(currentStreamSizeIndex, index);
        }

        private void OnCodecDropdownChanged(int index)
        {
            CheckForChanges(currentCodecSelectIndex, index);
        }
        #endregion

        #region Button Handlers
        private void HandleBackButtonClick()
        {
            ResetDropdownValue();
            ResetStreamSizeInputs();
            ClearInputFields();

            UIEvent.BackSettingPanelClickEvent?.Invoke();
        }

        private void ApplySettings()
        {
            if (ValidateStreamSizeInputs())
            {
                SaveSettings();
                DisplayStateText("Apply success", Color.green);
            }
            else
            {
                DisplayStateText("An empty stream size value exists", Color.red);
            }
        }
        #endregion

        #region Utility Methods
        private void SaveSettings()
        {
            WebRTCSetting.StreamSize = GetSelectedStreamSize();
            WebRTCSetting.VideoCodec = GetSelectedCodec();
            WebSocketSetting.WebSocketURL = SaveTextField(currentURLText, urlInputField.text);
            WebSocketSetting.WebSocketProtocol = SaveTextField(currentProtocolText, protocolInputField.text);
            UserProfileSetting.Nickname = SaveTextField(currentNicknameText, nicknameInputField.text);

            currentStreamSizeIndex = streamSizeDropdown.value;
            currentCodecSelectIndex = codecSelectDropdown.value;

            applySettingButton.interactable = false;

            if(WebRTCSetting.VideoCodec == null)
            {
                Debug.Log($"Settings have been applied.\n" +
                          $"WebSocket URL : {WebSocketSetting.WebSocketURL}\n" +
                          $"WebSocket Protocol : {WebSocketSetting.WebSocketProtocol}\n" +
                          $"Nickname : {UserProfileSetting.Nickname}\n" +
                          $"Stream size : {WebRTCSetting.StreamSize}\n" +
                          $"Video codec : Default");
            }
            else
            {
                Debug.Log($"Settings have been applied.\n" +
                          $"WebSocket URL : {WebSocketSetting.WebSocketURL}\n" +
                          $"WebSocket Protocol : {WebSocketSetting.WebSocketProtocol}\n" +
                          $"Nickname : {UserProfileSetting.Nickname}\n" +
                          $"Stream size : {WebRTCSetting.StreamSize}\n" +
                          $"Video codec : {WebRTCSetting.VideoCodec.mimeType} {WebRTCSetting.VideoCodec.sdpFmtpLine}");
            }
        }

        private bool ValidateStreamSizeInputs() =>
            !streamSizeWidthInputField.interactable ||
            !streamSizeHeightInputField.interactable ||
            (!string.IsNullOrEmpty(streamSizeWidthInputField.text) && !string.IsNullOrEmpty(streamSizeHeightInputField.text));

        private void ResetStreamSizeInputs()
        {
            if (currentStreamSizeIndex == 0)
            {
                streamSizeWidthInputField.text = WebRTCSetting.StreamSize.x.ToString();
                streamSizeHeightInputField.text = WebRTCSetting.StreamSize.y.ToString();
            }
            else
            {
                streamSizeWidthInputField.text = string.Empty;
                streamSizeHeightInputField.text = string.Empty;
            }
        }

        private void ResetDropdownValue()
        {
            streamSizeDropdown.value = streamSizeList.IndexOf(WebRTCSetting.StreamSize) + 1;
            codecSelectDropdown.value = availableCodecs.IndexOf(WebRTCSetting.VideoCodec) + 1;
        }

        private void ClearInputFields()
        {
            urlInputField.text = string.Empty;
            protocolInputField.text = string.Empty;
            nicknameInputField.text = string.Empty;
        }

        private Vector2Int GetSelectedStreamSize() =>
            streamSizeDropdown.value == 0
                ? new Vector2Int(int.Parse(streamSizeWidthInputField.text), int.Parse(streamSizeHeightInputField.text))
                : streamSizeList[streamSizeDropdown.value - 1];

        private RTCRtpCodecCapability GetSelectedCodec() =>
            codecSelectDropdown.value == 0 ? null : availableCodecs[codecSelectDropdown.value - 1];

        private string SaveTextField(TMP_Text textField, string input) =>
            string.IsNullOrEmpty(input) ? textField.text : (textField.text = input);

        private void CheckForChanges(int currentIndex, int newIndex) =>
            applySettingButton.interactable = currentIndex != newIndex;

        private void OnInputValueChanged(string value) =>
            applySettingButton.interactable = !string.IsNullOrEmpty(value);

        private void DisplayStateText(string message, Color color)
        {
            stateApplyText.text = message;
            stateApplyText.color = color;
        }

        private void ResetStateText() =>
            stateApplyText.text = string.Empty;
        #endregion
    }
}
