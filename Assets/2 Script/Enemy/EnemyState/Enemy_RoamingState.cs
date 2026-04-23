using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_RoamingStateSO", menuName = "EnemyState/Roaming")]
public class Enemy_RoamingState : ScriptableState<Enemy, Enemy_StateType>
{
    public override void OnStateEnter(Enemy context)
    {
        context.SetRoamingPosition();
        context.ResetStateTimer();

        context.SetAnimation("roaming", true);
    }

    public override void OnStateExit(Enemy context)
    {
        context.ResetStateTimer();

        context.SetAnimation("roaming", false);
    }

    public override void OnStateUpdate(Enemy context)
    {
        context.Move(context.TargetVec);
    }
}
