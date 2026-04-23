using UnityEngine;

public class Skeleton : Enemy // Roaming, Chasing, Attack
{
    public override void Attack()
    {
    }

    public override void Move(Vector2 targetVec)
    {
        SetVelocity(targetVec * basicSpeed);
    }

    public override void SpecialActing()
    {
    }

}
