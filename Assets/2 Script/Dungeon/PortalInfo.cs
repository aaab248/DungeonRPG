using UnityEngine;

// 포탈 정보
[System.Serializable]
public struct PortalInfo
{
    public Portal_Direction dir; // 현재 포탈 방향 위치

    public string targetRoomID; // 다음 방 아이디
    public Portal_Direction targetSpawnDir; // 다음 방 포탈 방향 위치
}
