using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttackDataSO")]
public class ComboAttackData : ScriptableObject
{
    public List<AttackData> comboAttackData;
}
