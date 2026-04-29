using UnityEngine;

public class SkillSO : ScriptableObject
{
    public SkillCondition skillCondition;

    public string skillName; // 이름
    public float skillDamage; // 기초 데미지
    public float skillCooldown; // 쿨다운
    public int skillCost; // 사용 코스트

    public GameObject skill_Prefab; // 스킬 이펙트 프리팹

    public Vector2 offset; // 스킬 발동 위치 오프셋
}
