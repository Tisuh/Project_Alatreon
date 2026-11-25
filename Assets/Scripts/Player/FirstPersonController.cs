using System.Collections;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float rotationSpeed = 4;
    [SerializeField] private float jumpForce = 4;

    [Header("Dash Stats")]
    [SerializeField] private float dashForce = 10;
    [SerializeField] private float dashDuration = 10;
    [SerializeField] private float dashCooldown = 10;

    //[Header("Slide And Crouch Stats")]
    //[SerializeField] private Vector3 crouchColCenterOffset;
    //[SerializeField] private float crouchHeight = 1;
    //[SerializeField] private float slideDuration;

    [Header("Refs")]
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject grapplingHook;

    private Rigidbody rb;
    private GroundChecker gc;
    private LedgeClimber ledgeClimber;
    private CapsuleCollider col;
    private Vector3 input;
    private float pivotRotationX;
    private bool isDashing;
    private bool canDash = true;
    //private bool isCrouching;
    //private float defaultHeight;
    //private Vector3 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gc = GetComponent<GroundChecker>();
        ledgeClimber = GetComponent<LedgeClimber>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        //defaultHeight = col.height;
    }

    private void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // Capturar input WASD
        input = transform.TransformDirection(input); // Transformar el vector de input para que sea relativo al jugador

        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X") * rotationSpeed, 0)); // Rotacion en Y (movimiento horizontal del raton)

        pivotRotationX -= Input.GetAxisRaw("Mouse Y") * rotationSpeed; // Guardar rotacion en X (movimiento vertical del raton) en una variable
        pivotRotationX = Mathf.Clamp(pivotRotationX, -90, 90); // Limitar rotacion en X
        pivot.localEulerAngles = new Vector3(pivotRotationX, 0, 0); // Aplicar la rotacion en el eje X

        if (Input.GetButtonDown("Jump") && gc.isGrounded) // Salto
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        if (Input.GetButtonDown("Dash") && canDash) // Dash
        {
            Dash();
        }

        //if (Input.GetButtonDown("Crouch"))
        //{
        //    CrouchAndSlide();
        //}

        if (Input.GetButtonDown("GrapplingHook")) // Grappling Hook
        {
            grapplingHook.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        Vector3 movement = input * moveSpeed; // Calcular la velocidad de movimiento
        movement.y = rb.linearVelocity.y; // Conservar la velocidad vertical para mantener la gravedad

        rb.linearVelocity = movement; // Aplicar el movimiento al Rigidbody
    }

    void Dash()
    {
        rb.linearVelocity = Vector3.zero;
        isDashing = true;
        canDash = false;
        rb.useGravity = false;

        if (input.sqrMagnitude > 0.001f) // Dashea en la direccion de movimiento del personaje
        {
            rb.AddForce(input * dashForce, ForceMode.VelocityChange);
        }
        else // Dashea hacia delante si no hay movimiento
        {
            rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);
        }

        StartCoroutine(IsDashingCrt());
    }

    //void CrouchAndSlide()
    //{
    //    if (!isCrouching)
    //    {
    //        col.center = crouchColCenterOffset;
    //        col.height = crouchHeight;
    //        isCrouching = true;
    //    }
    //    else
    //    {
    //        col.center = Vector3.zero;
    //        col.height = defaultHeight;
    //        isCrouching = false;
    //    }
    //}

    //void Crouch()
    //{
        
    //}

    //void Slide()
    //{

    //}



    IEnumerator IsDashingCrt()
    {
        yield return new WaitForSeconds(dashDuration); // Tiempo para terminar el dash
        isDashing = false;
        rb.useGravity = true;

        yield return new WaitForSeconds(dashCooldown - dashDuration); // Tiempo para recargar el dash
        while (!gc.isGrounded)
        {
            yield return null;
        }
        canDash = true;
    }
}
