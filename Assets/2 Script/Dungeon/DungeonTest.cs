using System.Collections.Generic;
using UnityEngine;

public class DungeonTest : MonoBehaviour
{
    public DungeonManager dungeonManager;

    public float nodeSize = 1f;
    public float lineThickness = 10f;
    public Color nodeColor = Color.yellow;
    public Color lineColor = Color.black;
    public Color currentColor = Color.blue;


    private void OnDrawGizmos()
    {
        if (dungeonManager.mapData == null || dungeonManager.mapData.allRooms == null)
            return;

        Dictionary<string, Vector3> roomPosition = new();

        int cols = Mathf.CeilToInt(Mathf.Sqrt(dungeonManager.mapData.allRooms.Count));

        for(int i = 0; i<dungeonManager.mapData.allRooms.Count; i++)
        {
            RoomInfo roomData = dungeonManager.mapData.allRooms[i];
            Vector3 pos = new Vector3(roomData.roomPos.x * 5f + 20f, roomData.roomPos.y * 5f, 0);
            roomPosition[roomData.id] = pos;
        }

        Gizmos.color = lineColor;
        foreach(var room in dungeonManager.mapData.allRooms)
        {
            if(room.portals == null)
                continue;

            foreach(var portal in  room.portals)
            {
                if (!roomPosition.ContainsKey(portal.targetRoomID))
                    continue;

                Vector3 a = roomPosition[room.id];
                Vector3 b = roomPosition[portal.targetRoomID];
                Gizmos.DrawLine(a,b);
            }
        }

        foreach(var room in dungeonManager.mapData.allRooms)
        {
            Vector3 pos = roomPosition[room.id];
            if(room == dungeonManager.mapData.currentRoom)
            {
                Gizmos.color = currentColor;
            }
            else
            {
                Gizmos.color = nodeColor;
            }

            Gizmos.DrawSphere(pos, nodeSize);
        }

        #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.white;
        foreach(var room in dungeonManager.mapData.allRooms)
        {
            Vector3 pos = roomPosition[room.id];
            UnityEditor.Handles.Label(pos + Vector3.up * 0.5f, room.id);
        }
        #endif
    }
}
