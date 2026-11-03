using UnityEngine;

public abstract class BasePickUp : Interactable
{
    private InteractableDetector intDet;

    private Collider col;
    private Rigidbody rb;

    [SerializeField] private float throwForce = 50;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }



    public override void Interact(Transform pickUpPoint)
    {
        PickUpItem(pickUpPoint);
    }



    protected void PickUpItem(Transform pickUpPoint) // Recoger objeto
    {
        transform.SetParent(pickUpPoint); // Emparentar

        transform.localPosition = Vector3.zero; // Reposicionar
        transform.localEulerAngles = Vector3.zero; // Rotar

        col.enabled = false; // Deshabilitar collider
        rb.isKinematic = true; // Habilitar isKinematic
        gameObject.layer = LayerMask.NameToLayer("Arms"); // Cambiar capa a Arms

        intDet = GetComponentInParent<InteractableDetector>(); // Guardar referencia al InteractableDetector del player
        intDet.interactableInHand = GetComponent<BasePickUp>();
    }

    public void DropItem()
    {
        transform.SetParent(null);

        col.enabled = true; // Habilitar collider
        rb.isKinematic = false; // Deshabilitar isKinematic
        gameObject.layer = LayerMask.NameToLayer("Interactable"); // Cambiar capa a Interactable

        intDet = null; // Eliminar referencia al InteractableDetector del player
    }

    public void ThrowItem() // Lanzar objeto
    {
        DropItem(); // Reutilizar DropItem
        rb.AddForce(Camera.main.transform.forward.normalized * throwForce, ForceMode.VelocityChange); // Aplicar fuerza
    }

    public abstract void TryToShoot();
}
