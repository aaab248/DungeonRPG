using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_HitStateSO", menuName = "EnemyState/Hit")]
public class Enemy_HitState : ScriptableState<Enemy, Enemy_StateType>
{
    public override void OnStateEnter(Enemy context)
    {
        context.SetAnimation("hit");

        context.IsHit = false;

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
