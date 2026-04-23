using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomInfo roomInfo;
    public Portal[] portalPoints;

    public void Initialize(RoomInfo data)
    {
        roomInfo = data;

        foreach (var portalInfo in roomInfo.portals)
        {
            for (int i = 0; i < portalPoints.Length; i++)
            {
                if (portalPoints[i].dir == portalInfo.dir)
                {
                    portalPoints[i].portalInfo = portalInfo;
                    portalPoints[i].gameObject.SetActive(true);
                    break;
                }
            }
        }

        if (data.isCleared)
        {
            Debug.Log(data.id + " 클리어 완료");
        }
    }

    // 포탈 위치 반환 함수
    public Vector3 GetPortalPosition(Portal_Direction dir)
    {
        for (int i = 0; i < portalPoints.Length; i++)
        {
            if (portalPoints[i].dir == dir)
            {
                return portalPoints[i].transform.position;
            }
        }

        //그 방향 값이 존재 X
        return Vector3.zero;
    }

    // 방이 클리어 됐을 때 호출 함수
    public void ClearRoom()
    {
        roomInfo.isCleared = true;
    }

    public void SpawnEnemy() { }
    public void DisableEnemy() { }

}
