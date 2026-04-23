using UnityEngine;

[CreateAssetMenu(fileName = "IdleStateSO", menuName = "PlayerState/Idle")]
public class IdleState : ScriptableState<Player,Player_StateType>
{
    public override void OnStateEnter(Player player)
    {

        player.SetAnimation("idle");
    }
    public override void OnStateExit(Player player) { }

    public override void OnStateUpdate(Player player) 
    {
        player.SetVelocity(Vector2.zero);
    }
}
 
