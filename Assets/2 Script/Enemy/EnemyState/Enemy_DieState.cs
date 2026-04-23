using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_DieStateSO", menuName = "EnemyState/Die")]
public class Enemy_DieState : ScriptableState<Enemy, Enemy_StateType>
{
    public override void OnStateEnter(Enemy context)
    {
        context.SetAnimation("die");

        context.GetComponent<Collider2D>().enabled = false;

        context.ResetCondition();

    }

    public override void OnStateExit(Enemy context)
    {
    }

    public override void OnStateUpdate(Enemy context)
    {
        context.SetVelocity(Vector2.zero);
    }
}
