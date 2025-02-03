using MultiPartyWebRTC;
using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    MainScene = 0,
    EchoTestRoomScene,
    StreamingScene,
    VideoCallScene,
    SIPGatewayScene,
    VideoRoomScene,
    M_VideoRoomScene,
    AudioBridgeScene,
    TextARoomScene,
}
public class SequenceManager : UniqueInstance<SequenceManager>
{
    [SerializeField] private GameObject[] canvases;

    private GameObject currentCanvas;

    private void Start()
    {
        if (!TryInstantiateCanvas(SceneType.MainScene))
        {
            return;
        }
    }

    private void OnEnable()
    {
        UIEvent.EchoTestClickEvent += LoadEchoTest;
        UIEvent.StreamingClickEvent += LoadStreaming;
        UIEvent.VideoCallClickEvent += LoadVideoCall;
        UIEvent.SIPGatewayClickEvent += LoadSIPGateway;
        UIEvent.VideoRoomClickEvent += LoadVideoRoom;
        UIEvent.VideoRoomMultiStreamClickEvent += LoadMVideoRoom;
        UIEvent.AudioBridgeClickEvent += LoadAudioBridge;
        UIEvent.TextRoomClickEvent += LoadTextRoom;

        UIEvent.HangUpVideoRoomEvent += LoadMain;
    }

    private void OnDisable()
    {
        UIEvent.EchoTestClickEvent -= LoadEchoTest;
        UIEvent.StreamingClickEvent -= LoadStreaming;
        UIEvent.VideoCallClickEvent -= LoadVideoCall;
        UIEvent.SIPGatewayClickEvent -= LoadSIPGateway;
        UIEvent.VideoRoomClickEvent -= LoadVideoRoom;
        UIEvent.VideoRoomMultiStreamClickEvent -= LoadMVideoRoom;
        UIEvent.AudioBridgeClickEvent -= LoadAudioBridge;
        UIEvent.TextRoomClickEvent -= LoadTextRoom;

        UIEvent.HangUpVideoRoomEvent -= LoadMain;
    }

    private void LoadScene(SceneType scene)
    {
        if(currentCanvas != null)
        {
            Destroy(currentCanvas);
        }

        StartCoroutine(LoadSceneAsync(scene));
    }

    private IEnumerator LoadSceneAsync(SceneType scene)
    {
        if (!TryStartSceneLoad(scene, out AsyncOperation asyncLoad))
        {
            yield break;
        }

        yield return WaitForSceneLoad(asyncLoad);

        if (!TryInstantiateCanvas(scene))
        {
            yield break;
        }
    }

    private IEnumerator WaitForSceneLoad(AsyncOperation asyncLoad)
    {
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private bool TryStartSceneLoad(SceneType scene, out AsyncOperation asyncLoad)
    {
        asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());

        if (asyncLoad == null)
        {
            Debug.LogError($"Failed to load scene: {scene}");
            return false;
        }

        asyncLoad.allowSceneActivation = false;
        return true;
    }

    private bool TryInstantiateCanvas(SceneType scene)
    {
        if (canvases == null || canvases.Length == 0)
        {
            Debug.LogError("Canvas array is null or empty.");
            return false;
        }

        int canvasIndex = (int)scene;

        if (canvasIndex < 0 || canvasIndex >= canvases.Length)
        {
            Debug.LogError($"Invalid canvas index: {canvasIndex}. SceneType: {scene}");
            return false;
        }

        // 기존 Canvas 제거
        if (TryCheckSameCanavsInScene())
        {
            Destroy(currentCanvas);
            Debug.Log(currentCanvas.name);
        }

        // 새로운 Canvas 인스턴스화
        currentCanvas = Instantiate(canvases[canvasIndex]);
        return true;
    }

    private bool TryCheckSameCanavsInScene()
    {
        if(currentCanvas == null)
        {
            return false;
        }
        else
        {
            if (GameObject.Find(currentCanvas.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    #region 씬 로드 함수
    private void LoadMain()
    {
        LoadScene(SceneType.MainScene);
    }
    private void LoadEchoTest()
    {
        LoadScene(SceneType.EchoTestRoomScene);
    }
    private void LoadStreaming()
    {
        LoadScene(SceneType.StreamingScene);
    }
    private void LoadVideoCall()
    {
        LoadScene(SceneType.VideoCallScene);
    }
    private void LoadSIPGateway()
    {
        LoadScene(SceneType.SIPGatewayScene);
    }
    private void LoadVideoRoom()
    {
        LoadScene(SceneType.VideoRoomScene);
    }
    private void LoadMVideoRoom()
    {
        LoadScene(SceneType.M_VideoRoomScene);
    }
    private void LoadAudioBridge()
    {
        LoadScene(SceneType.AudioBridgeScene);
    }
    private void LoadTextRoom()
    {
        LoadScene(SceneType.TextARoomScene);
    }
    #endregion
}
