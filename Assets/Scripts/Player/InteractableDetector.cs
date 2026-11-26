using UnityEngine;
using UnityEngine.UI;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private float sphereRadius = 1;
    [SerializeField] private float detectionRange = 1;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] public BasePickUp interactableInHand;
    [SerializeField] private BaseInteractable detectedInteractable;
    [SerializeField] private Transform PickUpPoint;
    [SerializeField] private Text interactText;

    private Ray ray;

    void Update()
    {
        if (interactableInHand == null)
        {
            Detect();
        }
        else if (Input.GetButtonDown("Interact")) // Soltar objeto
        {
            interactableInHand.DropItem();
            interactableInHand = null;
        }
        else if (Input.GetButtonDown("Throw"))
        {
            interactableInHand.ThrowItem();
            interactableInHand = null;
        }
        else if (Input.GetButton("Shoot"))
        {
            interactableInHand.TryToShoot();
        }

        if (Input.GetButtonDown("Interact") && detectedInteractable != null) // Interactuar o recoger
        {
            detectedInteractable.Interact(PickUpPoint);
            detectedInteractable = null;
            ClearInteractText();
        }
    }

    void Detect()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, detectionRange, interactableLayer))
        {
            if (detectedInteractable == null)
            {
                detectedInteractable = hit.collider.GetComponent<BaseInteractable>();

                interactText.gameObject.SetActive(true); // Activa el texto de interactuar
                interactText.text = "Press E to interact"; // Escribe el texto
            }
        }
        else if (detectedInteractable != null)
        {
            detectedInteractable = null;

            ClearInteractText();
        }
    }

    void ClearInteractText()
    {
        interactText.gameObject.SetActive(false); // Desactiva el texto de interactuar
        interactText.text = ""; // Limpia el texto
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.wheat;
        Gizmos.DrawWireSphere(ray.origin, sphereRadius);
        Gizmos.DrawRay(ray);
    }
}
