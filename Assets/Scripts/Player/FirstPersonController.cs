using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float rotationSpeed = 4;
    [SerializeField] private float jumpForce = 4;
    [SerializeField] private float dashForce = 10;

    [Header("Refs")]
    [SerializeField] private Transform pivot;

    private Rigidbody rb;
    private Vector3 input;
    private float pivotRotationX;
    private GroundChecker gc;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gc = GetComponent<GroundChecker>();
    }

    private void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // Capturar input WASD
        input = transform.TransformDirection(input); // Transformar el vector de input para que sea relativo al jugador

        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X"), 0)); // Rotacion en Y (movimiento horizontal del raton)

        pivotRotationX -= Input.GetAxisRaw("Mouse Y") * rotationSpeed; // Guardar rotacion en X (movimiento vertical del raton) en una variable
        pivotRotationX = Mathf.Clamp(pivotRotationX, -90, 90); // Limitar rotacion en X
        pivot.localEulerAngles = new Vector3(pivotRotationX, 0, 0); // Aplicar la rotacion en el eje X

        if (Input.GetButtonDown("Jump") && gc.isGrounded) // Salto
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = input * moveSpeed; // Calcular la velocidad de movimiento
        movement.y = rb.linearVelocity.y; // Conservar la velocidad vertical para mantener la gravedad

        rb.linearVelocity = movement; // Aplicar el movimiento al Rigidbody
    }
}
