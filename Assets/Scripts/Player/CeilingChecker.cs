using UnityEngine;

public class CeilingChecker : GroundChecker
{
    public bool hasCeiling => isGrounded;
}
