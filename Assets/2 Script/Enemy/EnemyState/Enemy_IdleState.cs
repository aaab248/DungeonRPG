using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_IdleStateSO", menuName = "EnemyState/Idle")]
public class Enemy_IdleState : ScriptableState<Enemy, Enemy_StateType>
{
    public override void OnStateEnter(Enemy context)
    {
        context.IdleDuration = Random.Range(0.5f, 2f);

        context.SetAnimation("idle");
    }

    public override void OnStateExit(Enemy context)
    {

    }

    public override void OnStateUpdate(Enemy context)
    {
        context.SetVelocity(Vector2.zero);
    }
}
