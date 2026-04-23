using UnityEngine;


public class Slime : Enemy  // Roaming, Chasing
{
    private bool CanMove = false;

    public override void Move(Vector2 vec)
    {
        if (!CanMove)
        {
            SetVelocity(Vector2.zero);
        }
        else
        {
            SetVelocity(vec);
        }
    }
    public override void Attack() { }
    public override void SpecialActing() { }



    public override void ResetCondition()
    {
        base.ResetCondition();
        CanMove = false;
    }

    public void SetMove() => CanMove = true;
    public void ClearMove() => CanMove = false;

    public void SetStateDelay() => StateDelay = true;
    public void ClearStateDelay() => StateDelay = false;
}
