using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateController<T_Context,T_Type> where T_Type : Enum
{
    protected T_Context context;    // 클래스 context
    protected Dictionary<T_Type, ScriptableState<T_Context, T_Type>> state_Dict;    // stateType, state 딕셔너리

    protected ScriptableState<T_Context, T_Type> currentState;    // 현재 state

    /// <summary>
    /// StateController 생성자
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_stateList"></param>
    public StateController(T_Context _context, List<ScriptableState<T_Context,T_Type>> _stateList)
    {
        this.context = _context;
        state_Dict = _stateList.ToDictionary(state => state.stateType, state => state);

        Debug.Log(context.ToString() + "생성");
    }

    public void OnUpdate()
    {
        TransitionCheck();
        currentState.OnStateUpdate(context);
    }

    // 딕셔너리 상태 타입 기준 => 상태 변경
    public void ChangeState(T_Type type)
    {
        if(state_Dict.TryGetValue(type,out var nextState))
        {
            Debug.Log(context.ToString() + currentState.stateType.ToString() + "->" + type.ToString());

            currentState.OnStateExit(context);
            currentState = nextState;
            currentState.OnStateEnter(context);
        }
        else
        {
            Debug.LogWarning("StateDictionay에 {" + type + "} 값이 없습니다");
        }

    }

    // 전이 조건 체크
    protected abstract void TransitionCheck();

}
