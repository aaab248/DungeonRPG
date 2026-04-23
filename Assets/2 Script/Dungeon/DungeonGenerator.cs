using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public RoomDataSO roomDataSO; // 방 데이터 SO
    public int roomCount; // 최대 방 개수

    private DungeonMapData targetMap; // RoomInfo 값들 저장용

    private List<Vector2Int> positions = new(); // 임의 위치 값
    private Dictionary<Vector2Int, RoomInfo> roomGrid = new(); // 각 방 Grid 위치


    public DungeonMapData GenerateDungeon()
    {
        // 초기화
        targetMap = new();
        roomGrid.Clear();
        positions.Clear();

        GenerateRoomPositions();

        GenerateRoom();

        ConnectRooms();

        return targetMap;
    }


    // 방 초기 좌표 값 설정 
    private void GenerateRoomPositions()
    {
        HashSet<Vector2Int> used = new();
        Vector2Int current = Vector2Int.zero;
        used.Add(current);
        positions.Add(current);

        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            Vector2Int next = current + dirs[Random.Range(0, dirs.Length)];

            int safety = 0;

            while (used.Contains(next))
            {
                next = current + dirs[Random.Range(0, dirs.Length)];
                safety++;
                if (safety > 50) break;
            }

            positions.Add(next);
            used.Add(next);
            current = next;
        }
    }
    
    private void GenerateRoom()
    {
        // 방 생성
        for (int i = 0; i < positions.Count; i++)
        {
            // roomInfo 값 생성
            RoomInfo room = new($"Room_{i}", positions[i], roomDataSO);

            room.portals.Clear();
            roomGrid[positions[i]] = room;

            targetMap.allRooms.Add(room);
        }
    }

    // 방들 연결 
    private void ConnectRooms()
    {
        HashSet<Vector2Int> connected = new();
        Queue<Vector2Int> frontier = new();

        connected.Add(positions[0]);
        frontier.Enqueue(positions[0]);

        while (frontier.Count > 0)
        {
            Vector2Int pos = frontier.Dequeue();
            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach(var dir in dirs)
            {
                Vector2Int neighbor = pos + dir;

                // 그 방향에 방 존재 x
                if (!positions.Contains(neighbor))
                    continue;
                // 그 방향에 연결된 방 존재 x -> 방 연결
                if(!connected.Contains(neighbor))
                {
                    ConnectTwoRoom(pos, neighbor);
                    connected.Add(neighbor);
                    frontier.Enqueue(neighbor);
                }
            }
        }
    }

    // 두 방 연결
    private void ConnectTwoRoom(Vector2Int a, Vector2Int b)
    {
        // 딕셔너리 값에 방이 존재 X
        if(!roomGrid.ContainsKey(a) || !roomGrid.ContainsKey(b))
            return;

        // 포탈 방향 설정용
        Vector2Int dir = b - a;
        Portal_Direction dirA = VectorToDir(dir);
        Portal_Direction dirB = VectorToDir(-dir);

        RoomInfo roomA = roomGrid[a];
        RoomInfo roomB = roomGrid[b];

        // 타겟 방을 향한 포탈이 없을 시
        if (!roomA.portals.Exists(p=>p.targetRoomID == roomB.id))
        {
            // 포탈 정보 입력
            roomA.portals.Add(new PortalInfo
            {
                dir = dirA,
                targetRoomID = roomB.id,
                targetSpawnDir = dirB
            });
        }

        //위와 동일
        if (!roomB.portals.Exists(p => p.targetRoomID == roomA.id))
        {
            // 포탈 정보 입력
            roomB.portals.Add(new PortalInfo
            {
                dir = dirB,
                targetRoomID = roomA.id,
                targetSpawnDir = dirA
            });
        }
    }

    private Portal_Direction VectorToDir(Vector2Int vec)
    {
        if (vec == Vector2Int.up) return Portal_Direction.UP;
        else if (vec == Vector2Int.down) return Portal_Direction.Down;
        else if (vec == Vector2Int.right) return Portal_Direction.Right;
        else if (vec == Vector2Int.left) return Portal_Direction.Left;
        else
            return Portal_Direction.None;
    }
}
