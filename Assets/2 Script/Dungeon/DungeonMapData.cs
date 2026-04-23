using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonMapData
{
    public List<RoomInfo> allRooms = new(); // 모든 방 정보

    public RoomInfo currentRoom; // 현재 방
    public RoomInfo startRoom; // 시작 방
}
