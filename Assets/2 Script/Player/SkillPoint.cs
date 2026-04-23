using UnityEngine;

public class SkillPoint : MonoBehaviour
{
    public int maxPoint; // 최대 스킬포인트
    public float chargeRate; // 스킬 포인트 자동충전 속도

    public int Filled { get; private set; } = 0; // 포인트 완충 개수
    public float Charging { get; private set; } = 0f; // 포인트 하나 충전 정도

    private void Update()
    {
        // 포인트 가득 참
        if (Filled >= maxPoint)
            return;

        AddCharge(chargeRate * Time.deltaTime);
    }

    /// <summary>
    /// 스킬 포인트 게이지 획득
    /// </summary>
    /// <param name="amount"></param>
    public void AddCharge(float amount)
    {
        if (amount <= 0f || Filled >= maxPoint)
            return;

        float remain = amount;

        while (remain > 0f && Filled < maxPoint)
        {
            float need = 1f - Charging;
            float addAmount = (remain < need) ? remain : need;

            Charging += addAmount;
            remain -= addAmount;

            if(Charging >= 1f)
            {
                Charging = 0f;
                Filled++;
            }

            if(Filled >= maxPoint)
            {
                Charging = 0f;
                break;
            }
        }
    }

    public bool CanUse(int cost) => Filled >= cost;
    public bool TryUse(int cost)
    {
        if (!CanUse(cost))
            return false;

        Filled -= cost;
        return true;
    }

}
