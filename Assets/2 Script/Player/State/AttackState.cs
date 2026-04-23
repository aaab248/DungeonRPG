using UnityEngine;

[CreateAssetMenu(fileName = "AttackStateSO", menuName = "PlayerState/Attack")]
public class AttackState : ScriptableState<Player, Player_StateType>
{
    public override void OnStateEnter(Player player)
    {
        player.SetVelocity(Vector2.zero);

        player.SetAnimation("attack");

    }
    public override void OnStateExit(Player player)
    {
        if(!player.canNextAttack)
        {
            player.attackIndex = 0;
        }
        else
        {
            player.attackIndex++;
        }
        // 공격 선입력 체크, 다음 공격 체크 초기화
        player.attackInputTime = false;
        player.canNextAttack = false;

    }

    public override void OnStateUpdate(Player player)
    {
        // 공격 선입력 감지
        if(player.attackInputTime && player.InputAttack)
        {
            player.attackInputTime = false;
            player.canNextAttack = true;
        }

    }

}
