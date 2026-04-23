using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInfo 
{
    public RoomDataSO roomData;

    public string id; // 방 ID
    public Room_Type type; // 방 타입
    public Vector2Int roomPos; // 방 위치(그리드값)

    public bool isCleared = false; // 방 클리어 여부

    public List<PortalInfo> portals = new(); // 포탈 정보

    // 방 정보 생성자
    public RoomInfo(string id, Vector2Int roomPos, RoomDataSO roomData)
    {
        this.id = id;
        this.roomPos = roomPos;
        this.roomData = roomData;
    }

}
