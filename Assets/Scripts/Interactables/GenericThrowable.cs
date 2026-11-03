using UnityEngine;

public class GenericThrowable : BasePickUp
{
    public override void TryToShoot()
    {
        ThrowItem();
    }
}
