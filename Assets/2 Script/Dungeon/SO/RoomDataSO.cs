using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Dungeon/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public string roomID;
    public Room_Type RoomType;
    public GameObject roomPrefab;

    public List<MobSpawnSO> mobs = new(); // ½ºÆù ¸÷ Á¾·ù
}
