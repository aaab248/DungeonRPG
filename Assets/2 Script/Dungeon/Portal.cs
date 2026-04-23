using UnityEngine;

public class Portal : MonoBehaviour
{
    private DungeonManager dungeonManager;

    public PortalInfo portalInfo;

    public Portal_Direction dir;

    private void Start()
    {
        dungeonManager = FindAnyObjectByType<DungeonManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // 게임상태 변경
            GameManager.Instance.Change_GameState(Game_State.Menu);

            dungeonManager.MoveToNextRoom(portalInfo.targetRoomID, portalInfo.targetSpawnDir);
        }
    }
}
