# TPSTemplet

# 이 프로젝트 내의 블루아카이브 관련 저작권은 넥슨게임즈에 있음을 밝힙니다.

본 게임은 넥슨IP 사용 지침에 따라 배포 계획은 없으며, 개인 포트폴리오 및 개발 역량을 위한 프로젝트 입니다.


TPSTemplet (Unity 6 & Netcode)
유니티 6(Unity 6) 기반의 3인칭 슈팅(TPS) 시스템 템플릿입니다. 본 프로젝트는 유니티 넷코드와 카메라를 다루었습니다.

🎥 Camera Controller Core Features

1. 플레이어 카메라 할당 (Netcode)
Unity.Netcode를 활용하여 네트워크 상의 여러 플레이어 중 IsOwner가 참인 로컬 플레이어를 동적으로 찾아 카메라를 할당합니다.

LateUpdate: 플레이어의 물리 이동이 완료된 후 카메라 위치를 계산하여 카메라가 먼저 이동하는 것을 방지합니다

2. Procedural Camera Feedback
Dynamic Camera Shake: StartShake 메서드를 통해 폭발이나 피격 시 무작위 벡터값을 이용한 절차적 흔들림을 제공합니다. Lerp를 이용해 흔들림이 서서히 멈추는 감쇠 효과가 구현되어 있습니다.

Shooting Recoil: 사격 시 ApplyRecoil을 호출하여 카메라의 Pitch 값을 직접 제어함으로써, 단순한 UI 반동이 아닌 실제 시야가 위로 튀는 물리적 반동을 재현합니다.

3. Smart Collision & Avoidance
Raycast Wall Detection: 플레이어와 카메라 사이에 Wall 레이어의 장애물이 감지될 경우, hit.point로 카메라를 즉시 이동시켜 시야 차단을 방지합니다.

Debug Visualization: 에디터 상에서 카메라 레이캐스트 경로를 Debug.DrawRay로 시각화하여 거리 조절을 용이하게 설계했습니다.

4. Smooth Aiming System
FOV Interpolation: 조준 시 Mathf.Lerp를 사용하여 시야각(Field of View)을 defaultFov(40)에서 zoomFov(20)로 부드럽게 전환합니다.

Offset-based LookAt: 단순 캐릭터 중심이 아닌, CameraOffset과 Quaternion 회전이 결합된 lookPosition을 산출하여 이상적인 3인칭 숄더 뷰 시점을 유지합니다.
