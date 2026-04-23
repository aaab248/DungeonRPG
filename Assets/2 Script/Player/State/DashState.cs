using UnityEditor.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "DashStateSO", menuName = "PlayerState/Dash")]
public class DashState : ScriptableState<Player, Player_StateType>
{

    public override void OnStateEnter(Player player)
    {
        // 현재 진행 방향 저장
        Vector2 dashDir;

        if (player.InputMove == Vector2.zero)
        {
            dashDir = new Vector2((player.transform.eulerAngles.y >= 90 ? -1 : 1), 0);
        }
        else if(player.InputMove.Equals(Vector2.up) || player.InputMove.Equals(Vector2.down))
        {
            dashDir = new Vector2(0, player.InputMove.y * 0.75f);
        }
        else
        {
            dashDir = player.InputMove;
        }

        player.SetInvincible(true, -1f);
        player.GetComponent<Collider2D>().enabled = false;

        if (dashDir.x != 0)
        {
            int flipDir = dashDir.x > 0 ? 1 : -1;
            player.TryFlip(flipDir);
        }

        player.SetVelocity(dashDir * player.dashSpeed);

        player.sp.TryUse(1);
        player.SetAnimation("dash");

        player.EndDashTime = Time.time;
    }

    public override void OnStateExit(Player player)
    {
        player.SetInvincible(false, 0.2f);
        player.GetComponent<Collider2D>().enabled = true;

        player.SetVelocity(Vector2.zero);
    }

    public override void OnStateUpdate(Player player)
    {
        player.CreateDashAfterImage();
    }
}
