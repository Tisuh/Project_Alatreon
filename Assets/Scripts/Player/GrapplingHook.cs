using Unity.VisualScripting;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 20;
    [SerializeField] private float grabRange = 1;
    [SerializeField] private float maxRange = 20;

    [Header("Refs")]
    [SerializeField] private Transform playerTf;
    [SerializeField] private Transform throwPointTf;
    [SerializeField] private Transform pickUpPointTf;
    [SerializeField] private InteractableDetector playerIntDet;

    private Rigidbody rb;
    [SerializeField] private bool isReturning = false;
    private BasePickUp grabbedPickUp;

    void OnEnable()
    {
        ThrowGrapplingHook();
    }

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        if (transform.parent != null) transform.SetParent(null); // Desemparenta este objeto del jugador si esta emparentado
        
        if (gameObject.activeInHierarchy) gameObject.SetActive(false); // Desactiva el objeto en caso de estar activado
    }

    void Update()
    {
        if (isReturning)
        {
            ReturnToPlayer();
        }
        else if (Vector3.Distance(transform.position, playerTf.position) >= maxRange)
        {
            isReturning = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isReturning = true; // Marca el bool isReturning a true en el momento en que colisiona con cualquier cosa

        if (collision.collider.CompareTag("PickUp")) // Compara que el objeto colisionado tenga el tag PickUp
        {
            GrabItem(collision);
        }
    }

    public void ThrowGrapplingHook()
    {
        transform.position = throwPointTf.position; // Transporta este objeto al punto de lanzamiento
        transform.rotation = throwPointTf.rotation; // Rota este objeto para darle la rotación del punto de lanzamiento

        rb.linearVelocity = Vector3.zero; // Reinicia la velocidad del gancho para que no se acumulen fuerzas
        rb.AddForce(throwPointTf.forward.normalized * speed, ForceMode.VelocityChange); // Añade fuerza hacia donde esta mirando el jugador
    }

    void GrabItem(Collision collision)
    {
        collision.collider.gameObject.transform.SetParent(this.transform); // El objeto colisionado se emparenta con este mismo
        grabbedPickUp = collision.collider.GetComponent<BasePickUp>();  // Se guarda referencia al componente BasePickUp

        grabbedPickUp.GetComponent<Rigidbody>().isKinematic = true;
        grabbedPickUp.ChangeLayers("GrapplingHook"); // Cambia el objeto agarrado a la capa GrapplingHook para que interactable detector del player no lo detecte
    }

    void ReturnToPlayer()
    {
        rb.linearVelocity = Vector3.zero; // Elimina la velocidad del gancho para que se pueda mover en dirección al jugador sin negarse con el addforce
        transform.position = Vector3.MoveTowards(transform.position, playerTf.position, speed * Time.deltaTime); // Mueve este objeto hacia la posicion del jugador
        if (Vector3.Distance(transform.position, playerTf.position) < grabRange) // Comprueba la distancia del gancho al jugador
        {
            isReturning = false;

            if (grabbedPickUp != null)
            {
                grabbedPickUp.Interact(pickUpPointTf);
                grabbedPickUp = null;
            }

            gameObject.SetActive(false);
        }
    }
}
