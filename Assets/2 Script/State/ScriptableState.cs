using System;
using UnityEngine;

public abstract class ScriptableState<T_Context, T_Type > : ScriptableObject, IState<T_Context> where T_Type : Enum
{
    public T_Type stateType;

    public abstract void OnStateEnter(T_Context context);
    public abstract void OnStateExit(T_Context context);
    public abstract void OnStateUpdate(T_Context context);
}
