using UnityEngine;

public class LedgeClimber : MonoBehaviour
{
    [Header("Original Raycast")]
    [SerializeField] private Vector3 ledgeClimberOriginOffset;
    [SerializeField] private float ledgeClimberOriginRange;
    
    [Header("Second Raycast")]
    [SerializeField] private Vector3 ledgeClimberPointOffset;
    [SerializeField] private float ledgeClimberPointRange;

    [Header("General")]
    [SerializeField] private LayerMask ledgeClimberLayerMask;
    [SerializeField] private Vector3 targetPositionOffset;

    private RaycastHit hit;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButton("Jump")) // Comprueba que mantienes boton SALTAR
        {
            Physics.Raycast(transform.position + ledgeClimberOriginOffset, transform.forward, out hit, ledgeClimberOriginRange, ledgeClimberLayerMask); // Lanzar raycast hacia delante
            if (hit.collider) // Comprueba que el raycast golpee algo
            {
                if (!Physics.Raycast(hit.point + ledgeClimberPointOffset, Vector3.down, ledgeClimberPointRange, ledgeClimberLayerMask)) // Lanzar raycast hacia abajo para comprobar que haya espacio
                {
                    if (Physics.Raycast(hit.point + ledgeClimberPointOffset, Vector3.down, out RaycastHit secondHit, 2, ledgeClimberLayerMask)) // Lanzar raycast para comprobar a que punto mover el personaje
                    {
                        transform.position = secondHit.point + targetPositionOffset;
                        rb.linearVelocity = Vector3.zero;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.azure;
        if (Input.GetButton("Jump"))
        {
            Gizmos.DrawRay(transform.position + ledgeClimberOriginOffset, transform.forward * ledgeClimberOriginRange);

            Gizmos.DrawRay(hit.point + ledgeClimberPointOffset, Vector3.down * ledgeClimberPointRange);
        }
    }
}
