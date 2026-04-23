using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_ChasingStateSO", menuName = "EnemyState/Chasing")]
public class Enemy_ChasingState : ScriptableState<Enemy, Enemy_StateType>
{
    public override void OnStateEnter(Enemy context)
    {
        context.SetChasingPosition();

        context.ResetStateTimer();

        context.SetAnimation("chasing", true);

    }

    public override void OnStateExit(Enemy context)
    {
        context.ResetStateTimer();

        context.SetAnimation("chasing", false);
    }

    public override void OnStateUpdate(Enemy context)
    {
        if (context.MovingType == Enemy_MovingType.Impulse)
        {
            context.Move(context.TargetVec);
        }

        else if(context.MovingType != Enemy_MovingType.Continuous)
        {
            context.SetChasingPosition();

            context.Move(context.TargetVec);
        }

    }
}
