# WebRTC Unity with Janus

<div align="center">

  <img src="https://github.com/user-attachments/assets/d33c1241-51e6-475a-8606-580369f07927" width="49%" height="230"/>
  <img src="https://github.com/user-attachments/assets/065b1f46-4e7a-4040-a4d0-9dbdfa5d3767" width="49%" height="230"/>
  <img src="https://github.com/user-attachments/assets/b662437c-19f2-485b-807a-6be8caf2530a" width="49%" height="230"/>
  <img src="https://github.com/user-attachments/assets/8fd52c82-6de0-4a25-9568-4f25de5939c6" width="49%" height="230"/>

  Unity Client

  <img src="https://github.com/user-attachments/assets/ac543379-6f02-4306-9ddf-1a76524f9a7e" width="49%" height="49%"/>
  
  Janus Client
</div>

### 1. WebRTC란 무엇입니까?
WebRTC란 웹 브라우저 간에 플러그인의 도움 없이 서로 통신 할 수 있도록 설계된 API입니다. 음성통화, 영상통화, P2P 파일공유 등으로 활용 될 수 있습니다.

### 2. WebRTC Unity with Janus는 무엇입니까?
Meetecho에서 설계 및 개발한 오픈 소스, 범용 WebRTC 서버 Janus와 Unity간의 WebSocket통신을 이용한 WebRTC 연결 클라이언트 입니다.
기본적으로 WebRTC방식에는 Mesh/P2P, MCU, SFU 방식이 있는데 WebRTC Unity with Janus에서는 SFU 방식을 따르고 있습니다.

### 3. SFU 방식은 무엇입니까?
Mesh/P2P 처럼 클라이언트와 클라이언트간의 통신이 이루어 질 경우 각 클라이언트에서 모든 스트림을 처리하는 방식이 아닌 종단 간 미디어 트래픽을 중계하는 중앙 서버를 두어 서버가 스트림 데이터를 처리하는 방식입니다. 각각의 클라이언트들은 필요한 스트림만 서버로부터 받아 디코딩 후 렌더링합니다.
## 개발 환경
 + Unity Version : 2022.3.34f1 LTS
 + Packages :
   - WebSocket-Sharp                  : https://github.com/sta/websocket-sharp
   - NewtonSoft-Json.NET              : https://www.newtonsoft.com/json
   - WebRTC Unity(WebRTC 3.0.0-pre.8) : https://docs.unity3d.com/Packages/com.unity.webrtc@3.0/manual/index.html
 + Janus WebRTC Server              : https://janus.conf.meetecho.com/index.html, https://github.com/meetecho/janus-gateway

 + 개발 기간 : 2024.12 ~ 2025.02.18 (약 2개월 )

## 시스템
### 구성도
![WebRTC_Unity_with_Janus 다이어그램 drawio](https://github.com/user-attachments/assets/009f3569-5c07-416a-bc08-28bc26bbb7e7)

### 통신 순서
```
Return value = JsonObjet from Janus server
Event = Unity Action Event

Warning : Trickle messages do not necessarily need to be compared with the transaction messages you previously sent.
          However, if you want to check all sent messages, be sure to save all transaction amounts to a list to compare and check them.

Connect Session
1. Message Handler : Create
	 - Return value :
			session_id

PeerConnect
1. Local Peer - CreateOffer

2. Local Peer - SetLocalDescription

3. Local Peer : Attach
	 - Return value :
			handle_id
			
4. Local Peer : Join_Publish
	 - Return value :
			publishers - Number of remote peers and stream datas
	 - Event :
		if ( publishers.Count > 0)
		{
			VideoRoomPanel - Instance RemotePeers
						   - Remote peer acquires stream data
		}
		else
		{
			Proceed to question 13
		}

5. Remote Peer : Attach
	 - Return value :
			handle_id

6. Remote Peer : Join_Subscriber
	 - Return value :
			Offer_SDP
	 - Event :
		SetRemoteDescription

7. Remote Peer - SetRemoteDescription

8. Remote Peer - CreateAnswer

9. Remote Peer - SetLocalPeerDescription

10. Remote Peer - Add ICE Candidates on List

11. Remote Peer : Start & If all remote peers sent a Start Message Proceed to question 13
	 - Return value :
			started = "ok"
	 - Event :
		Network Manager - OnRemotePeerCompleted();
		Remote Peer - SendCandidateCompletedResponse();

12. Remote Peer : Trickle
	 - If you sent all ICE Candidates as a Trickle message, you must also send a Completed message as a Trickle message

13. Local Peer : Configure
	 - Return value :
			Answer_SDP
	 - Event :
		SetRemoteDescription
		
14. Local Peer : SetRemoteDescription

15. Local Peer : Add ICE Candidtas on List

16. Local Peer : Trickle
	 - If you sent all ICE Candidates as a Trickle message, you must also send a Completed message as a Trickle message
	 
17. Check webrtcup Message and media Message from Janus server
```
## 영상
https://github.com/Choigyungyun/WebRTC_Unity/tree/main/Test_Video
## 다운로드
https://github.com/Choigyungyun/WebRTC_Unity/tree/main/Test_Build
