using System.Collections;
using UnityEngine;

public enum MovementType
{
    Default, Crouch, Dash, Slide
}

public class FirstPersonController : MonoBehaviour
{
    #region Variables serializadas
    [Header("General Stats")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float rotationSpeed = 4;
    [SerializeField] private float jumpForce = 4;
    [SerializeField] MovementType movementType;

    [Header("Dash Stats")]
    [SerializeField] private float dashForce = 10;
    [SerializeField] private float dashDuration = 10;
    [SerializeField] private float dashCooldown = 10;

    [Header("Slide And Crouch Stats")]
    [SerializeField] private float crouchSpeedModifier = 0.5f;
    [SerializeField] private float crouchPivotDisplacement = 0.6f;
    [SerializeField] private float slideForce = 2;
    [SerializeField] private float slideMaxTime = 0.2f;
    [SerializeField] private Collider defaultCollider, crouchCollider;

    [Header("Refs")]
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject grapplingHook;
    #endregion

    #region Variables privadas
    private Rigidbody rb;
    private GroundChecker gc;
    private LedgeClimber ledgeClimber;
    private CapsuleCollider col;
    private Vector3 input;
    private float pivotRotationX;
    private bool canDash = true;
    private float speedModifier = 1;
    private CeilingChecker ceilingChecker;
    #endregion

    #region Métodos por defecto
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gc = GetComponent<GroundChecker>();
        ledgeClimber = GetComponent<LedgeClimber>();
        col = GetComponent<CapsuleCollider>();
        ceilingChecker = GetComponent<CeilingChecker>();
    }

    private void Start()
    {
        
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
            if (movementType == MovementType.Default)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
            else if (movementType == MovementType.Crouch || movementType == MovementType.Slide)
            {
                CrouchAndSlide();
            }
        }

        if (Input.GetButtonDown("Dash") && canDash) // Dash
        {
            Dash();
        }

        if (Input.GetButtonDown("Crouch")) // Crouch
        {
            CrouchAndSlide();
        }

        if (Input.GetButtonDown("GrapplingHook")) // Grappling Hook
        {
            grapplingHook.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.Dash || movementType == MovementType.Slide) return;

        Vector3 movement = input * moveSpeed * speedModifier; // Calcular la velocidad de movimiento
        movement.y = rb.linearVelocity.y; // Conservar la velocidad vertical para mantener la gravedad

        rb.linearVelocity = movement; // Aplicar el movimiento al Rigidbody
    }
    #endregion

    #region Métodos personalizados
    void Dash()
    {
        rb.linearVelocity = Vector3.zero;
        movementType = MovementType.Dash;
        ChangeSpeedModifier();
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

    void CrouchAndSlide()
    {
        if ((movementType == MovementType.Crouch || movementType == MovementType.Slide) && !ceilingChecker.hasCeiling) // Ponerse de pie
        {
            movementType = MovementType.Default;
            defaultCollider.enabled = true;
            crouchCollider.enabled = false;
            pivot.position += Vector3.up * crouchPivotDisplacement;
            ChangeSpeedModifier();
        }
        else if (movementType == MovementType.Default && input.sqrMagnitude > 0.001f) // Deslizarse por el suelo (si te estas moviendo)
        {
            Crouch(MovementType.Slide);
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(input * slideForce, ForceMode.VelocityChange);

            Coroutine slidingCoroutine = null;
            if (slidingCoroutine != null)
            {
                StopCoroutine(slidingCoroutine);
                slidingCoroutine = null;
            }
            slidingCoroutine = StartCoroutine(SlidingTimeCrt());
        }
        else if (movementType == MovementType.Default) // Agacharse (si estas quieto)
        {
            Crouch(MovementType.Crouch);
        }
    }

    void Crouch(MovementType movType)
    {
        movementType = movType;
        defaultCollider.enabled = false;
        crouchCollider.enabled = true;
        pivot.position -= Vector3.up * crouchPivotDisplacement;
        ChangeSpeedModifier();
    }

    void ChangeSpeedModifier() // Cambiar el modificador de velocidad
    {
        switch (movementType)
        {
            case MovementType.Default:
                speedModifier = 1;
                break;
            case MovementType.Crouch:
                speedModifier = crouchSpeedModifier;
                break;
            case MovementType.Dash:
                speedModifier = 1;
                break;
            case MovementType.Slide:
                speedModifier = crouchSpeedModifier;
                break;
            default:
                break;
        }
    }
    #endregion

    #region Corrutinas
    IEnumerator IsDashingCrt()
    {
        yield return new WaitForSeconds(dashDuration); // Tiempo para terminar el dash
        movementType = MovementType.Default;
        ChangeSpeedModifier();
        rb.useGravity = true;

        yield return new WaitForSeconds(dashCooldown - dashDuration); // Tiempo para recargar el dash
        while (!gc.isGrounded)
        {
            yield return null;
        }
        canDash = true;
    }

    IEnumerator SlidingTimeCrt()
    {
        yield return new WaitForSeconds(slideMaxTime);
        if (movementType == MovementType.Slide)
        {
            movementType = MovementType.Crouch;
        }
    }
    #endregion
}
