using UnityEngine;
using System.Collections.Generic;

public class PlayerStateController : StateController<Player,Player_StateType>
{
    public PlayerStateController(Player player, List<ScriptableState<Player, Player_StateType>> stateList) : base(player, stateList) 
    { 
        // 생성 상태 => idle
        state_Dict.TryGetValue(Player_StateType.Idle, out currentState);
    }

    // 전이 조건 체크
    protected override void TransitionCheck()
    {
        // playing 상태 아닐시 조작 제한
        if(GameManager.Instance.game_state != Game_State.Playing)
        {
            if(currentState.stateType != Player_StateType.Idle)
            {
                ChangeState(Player_StateType.Idle);
            }
            return;
        }

        // 아무 상태에서 전이 가능
        if (context.IsHit && currentState.stateType != Player_StateType.Hit)
        {
            // -> Hit
            ChangeState(Player_StateType.Hit);
        }
        if(context.IsDead && currentState.stateType != Player_StateType.Die)
        {
            // -> Die
            ChangeState(Player_StateType.Die);
        }

        // 특정 상태에서 전이 가능
        switch (currentState.stateType)
        {
            // Idle 상태
            case Player_StateType.Idle:
                if(context.InputMove != Vector2.zero)
                {
                    // -> Move
                    ChangeState(Player_StateType.Move); 
                }
                else if (context.InputDash && context.DashCondition == SkillCondition.Ready)
                {
                    // -> Dash
                    ChangeState(Player_StateType.Dash);
                }
                else if(context.InputAttack)
                {
                    // -> Attack
                    ChangeState(Player_StateType.Attack);
                }
                    break;

            // Move 상태
            case Player_StateType.Move:
                if (context.InputMove == Vector2.zero)
                {
                    // -> Idle
                    ChangeState(Player_StateType.Idle);
                }
                else if (context.InputDash && context.DashCondition == SkillCondition.Ready)
                {
                    // -> Dash
                    ChangeState(Player_StateType.Dash);
                }
                else if (context.InputAttack)
                {
                    // -> Attack
                    ChangeState(Player_StateType.Attack);
                }
                    break;

            // Attack 상태
            case Player_StateType.Attack:
                if (context.canNextAttack && context.GetAnimationEnd)
                {
                    // -> Attack (연속 공격)
                    ChangeState(Player_StateType.Attack);
                }
                else if (context.InputDash && context.DashCondition == SkillCondition.Ready) 
                {
                    // -> Dash (공격 캔슬 대시)
                    ChangeState(Player_StateType.Dash);
                }
                    break;
        }

        // 상태 애니메이션 끝나면 idle 상태로 전환
        if (context.GetAnimationEnd)
        {
            ChangeState(Player_StateType.Idle);
        }
    }
}
