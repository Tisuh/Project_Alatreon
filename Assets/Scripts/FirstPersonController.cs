using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4;

    private Rigidbody rb;
    private Vector3 input;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // Capturar input WASD

        Vector3 movement = input * moveSpeed; // Guardar input WASD en vetor movement
        movement.y = rb.linearVelocity.y; // Modificar velocidad en eje Y para que no se vea afectada la gravedad

        rb.linearVelocity = movement; // Mover al personaje
    }
}
