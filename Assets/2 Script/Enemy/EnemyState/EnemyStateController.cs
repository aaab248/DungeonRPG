using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : StateController<Enemy, Enemy_StateType>
{
    public EnemyStateController(Enemy enemy, List<ScriptableState<Enemy, Enemy_StateType>> stateList) : base(enemy, stateList)
    {
        // 생성 시점 상태 설정 구현
        state_Dict.TryGetValue(Enemy_StateType.Idle, out currentState);
    }

    // 전이 조건 체크
    protected override void TransitionCheck()
    {
        // 피격 시 -> Hit
        if (context.IsHit)
        {
            ChangeState(Enemy_StateType.Hit);
        }

        // 사망 시 -> Die ( 1회 적용 )
        if(context.Isdie && currentState.stateType != Enemy_StateType.Die)
        {
            ChangeState(Enemy_StateType.Die);
        }

        // 애니메이션 종료 -> Idle
        if (context.GetAnimationEnd)
        {
            ChangeState(Enemy_StateType.Idle);
        }

        // 상태 딜레이중 상태 전환 불가능
        if (context.StateDelay)
            return;

        switch (currentState.stateType)
        {
            case Enemy_StateType.Idle:

                // 상대 탐지됨 -> Chasing
                if (context.IsDetect)
                {
                    ChangeState(Enemy_StateType.Chasing);
                }

                // IdleState 시간 지남 -> Roaming
                else if (Time.time - context.StateTimer > context.IdleDuration)
                {
                    ChangeState(Enemy_StateType.Roaming);
                }
 
                break;

            case Enemy_StateType.Roaming:

                // 상대 탐지됨 -> Chasing
                if (context.IsDetect)
                {
                    ChangeState(Enemy_StateType.Chasing);
                }
                // 로밍 지속 시간 끝
                //if(!context.IsRoaming)
                //{
                //    ChangeState(Enemy_StateType.Idle);
                //}
                break;

            case Enemy_StateType.Chasing:

                // 탐지된 적 없음 -> Idle
                if(!context.IsDetect)
                {
                    ChangeState(Enemy_StateType.Idle);
                }
                break;
        }

    }
        
}

