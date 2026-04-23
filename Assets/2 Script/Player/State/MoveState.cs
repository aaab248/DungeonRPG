using UnityEngine;

[CreateAssetMenu(fileName = "MoveStateSO", menuName = "PlayerState/Move")]
public class MoveState : ScriptableState<Player, Player_StateType>
{
    public override void OnStateEnter(Player player)
    {
        player.SetAnimation("move", true);
    }

    public override void OnStateExit(Player player)
    {
        player.SetAnimation("move", false);
    }

    public override void OnStateUpdate(Player player)
    {
        // 움직일 때 방향 전환되는 것에 따라 스프라이트 Flip
        if (player.InputMove.x != 0)
        {
            int dir = player.InputMove.x >= 0 ? 1 : -1;
            player.TryFlip(dir);
        }

        // 이동속도 설정
        player.SetVelocity(player.moveSpeed * player.InputMove);
    }
}
