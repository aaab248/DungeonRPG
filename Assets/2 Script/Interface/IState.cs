
public interface IState<T_Context>
{
    void OnStateEnter(T_Context context);
    void OnStateExit(T_Context context);
    void OnStateUpdate(T_Context context);
}
