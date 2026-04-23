using UnityEngine;

[CreateAssetMenu(fileName = "HitStateSo", menuName = "PlayerState/Hit")]
public class HitState : ScriptableState<Player, Player_StateType>
{
    public override void OnStateEnter(Player player)
    {
        player.SetInvincible(true, -1f);

        player.SetAnimation("hit");
    }
    public override void OnStateExit(Player player)
    {
        player.IsHit = false;

        player.SetInvincible(false, 0.35f);
    }

    public override void OnStateUpdate(Player player)
    {

    }

}
