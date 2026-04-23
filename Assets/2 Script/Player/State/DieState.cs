using UnityEngine;


[CreateAssetMenu(fileName = "DieStateSO", menuName = "PlayerState/Die")]
public class DieState : ScriptableState<Player, Player_StateType>
{
    public override void OnStateEnter(Player player)
    {
        player.SetVelocity(Vector2.zero);

        player.GetComponent<Collider2D>().enabled = false;
        player.SetAnimation("die");
    }
    public override void OnStateExit(Player player) { }

    public override void OnStateUpdate(Player player) { }
}
