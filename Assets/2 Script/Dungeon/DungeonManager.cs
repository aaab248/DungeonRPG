using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public DungeonGenerator generator;
    public DungeonMapData mapData;

    public Dictionary<string, Room> roomObjects = new(); // 방 아이디, 방 오브젝트 딕셔너리

    public Room currentRoom;
    public Transform player;

    public Image fadeImage;

    private void Start()
    {
        mapData = generator.GenerateDungeon();
        mapData.startRoom = mapData.allRooms[0];

        SpawnRoom();

        // 시작 방 활성화
        if(roomObjects.TryGetValue(mapData.startRoom.id, out Room room))
        {
            currentRoom = room;
            mapData.currentRoom = room.roomInfo;

            room.gameObject.SetActive(true);
        }
    }

    // 방 오브젝트 생성
    private void SpawnRoom()
    {
        foreach (RoomInfo roomInfo in mapData.allRooms)
        {
            Vector3 vec = new Vector3(roomInfo.roomPos.x * 50f, roomInfo.roomPos.y * 50f, 0);

            // 방 prefab 생성
            //Room roomObject = Instantiate(roomInfo.roomData.roomPrefab, generator.transform).GetComponent<Room>();

            Room roomObject = Instantiate(
                roomInfo.roomData.roomPrefab, vec, Quaternion.identity, generator.transform).GetComponent<Room>();

            roomObject.Initialize(roomInfo);

            roomObjects.Add(roomInfo.id, roomObject);
        }
    }

    // 다음 방으로 이동
    public void MoveToNextRoom(string roomID, Portal_Direction targetDir)
    {
        if (roomObjects.TryGetValue(roomID, out Room room))
        {
            StartCoroutine(TransitionRoom(currentRoom, room, targetDir));

            Debug.Log(GameManager.Instance.game_state);

        }
        else
        {
            Debug.Log("방ID에 해당하는 방 오브젝트가 없습니다.");
        }
    }

    // 방 이동 효과
    IEnumerator TransitionRoom(Room currentRoom, Room targetRoom, Portal_Direction targetDir)
    {
        // 현재 방 이동 -> 현재 방 비활성화 -> 다음 방 활성화 -> 플레이어 위치 이동
        Vector3 originalPos = currentRoom.transform.position;
        Vector3 targetPos = originalPos + (DirToVector(targetDir) * 30);

        // 카메라 파트 (임시)
        Camera cam = Camera.main;
        Vector3 camOriginalPos = cam.transform.position;
        Vector3 camOffset = camOriginalPos + (DirToVector(targetDir) * 15f);

        player.gameObject.SetActive(false);

        fadeImage.gameObject.SetActive(true);

        Color c = fadeImage.color;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            float eased = EasedInOut(Mathf.Clamp01(t));

            // 페이드 인
            c.a = Mathf.Lerp(0f, 1f, eased);
            fadeImage.color = c;

            yield return null;
        }

        //cam.transform.position = new Vector3(targetRoom.transform.position.x, targetRoom.transform.position.y, cam.transform.position.z);
        player.position = targetRoom.transform.position;

        // 카메라 방향 원래 위치 이동 (임시)
        float returnT = 0f;
        while (returnT < 1f)
        {
            returnT += Time.deltaTime * 1.5f;
            float eased = EasedInOut(returnT);

            // 페이드 아웃
            c.a = Mathf.Lerp(1f, 0f, eased);
            fadeImage.color = c;

            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
        fadeImage.gameObject.SetActive(false);

        player.gameObject.SetActive(true);

        GameManager.Instance.Change_GameState(Game_State.Playing);

        mapData.currentRoom = targetRoom.roomInfo;
        currentRoom = targetRoom;
    }

    private float EasedInOut(float x)
    {
        return x * x * (3f - 2f * x);
    }

    // 방향 값 벡터 변환
    private Vector3 DirToVector(Portal_Direction dir)
    {
        if (dir == Portal_Direction.UP) return Vector3.up;
        else if (dir == Portal_Direction.Down) return Vector3.down;
        else if (dir == Portal_Direction.Left) return Vector3.left;
        else if (dir == Portal_Direction.Right) return Vector3.right;
        else
            return Vector3.zero;
    }
}
