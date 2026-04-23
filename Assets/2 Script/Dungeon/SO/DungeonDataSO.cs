using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DungeonDataSO",menuName = "Dungeon/DungonDataSO")]
public class DungeonDataSO : ScriptableObject
{
    public int maxRoomCount; // 최대 방 개수
    public int minRoomCount; // 최소 방 개수

    public List<RoomDataSO> roomDataSO; // 던전 구성 방 SO 
}
