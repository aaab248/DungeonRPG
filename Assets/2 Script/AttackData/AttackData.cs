using UnityEngine;

[CreateAssetMenu(fileName ="AttackDataSO")]
public class AttackData : ScriptableObject
{
    public float damage; // 공격 데미지
    public float knockbackForce; // 넉백 강도
    public Vector2 knockbackDir; // 넉백 방향
    public Vector2 hitBoxSize; // 히트 박스 크기
    public Vector2 hitBoxOffset; // 히트 박스 위치설정
}
