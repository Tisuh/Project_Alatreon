using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded;

    [SerializeField] private Vector3 groundCheckerOffset = new Vector3(0, -1, 0);
    [SerializeField] private Vector3 groundCheckerSize = new Vector3(0.75f, 0.1f, 0.75f);
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        isGrounded = Physics.CheckBox(transform.position + groundCheckerOffset, groundCheckerSize * 0.5f, Quaternion.identity, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + groundCheckerOffset, groundCheckerSize);
    }
}
