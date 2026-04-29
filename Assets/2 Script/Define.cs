
public enum Game_State
{
    Playing,
    Stop
}

public enum Room_Type
{
    Basic,
    Boss
}

public enum Portal_Direction
{
    None,
    UP,
    Down,
    Left,
    Right
}

/// <summary>
/// 플레이어 상태 타입
/// </summary>
public enum Player_StateType
{
    Idle,
    Hit,
    Die,
    Attack,
    Move,
    Dash,
}
/// <summary>
/// 플레이어 스킬 상태
/// </summary>
public enum SkillCondition
{
    Ready,
    Using,
    NotEnoughPoint,
    CoolDown
}




/// <summary>
/// 적 상태 타입
/// </summary>
public enum Enemy_StateType
{
    Idle,
    Hit,
    Die,

    Attack,
    Roaming,
    Chasing,
    Return
}

public enum Enemy_MovingType
{
    Impulse,
    Continuous
}

