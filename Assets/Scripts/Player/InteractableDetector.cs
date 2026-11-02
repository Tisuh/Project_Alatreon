using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private float sphereRadius = 1;
    [SerializeField] private float detectionRange = 1;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] public BasePickUp interactableInHand;
    [SerializeField] private Interactable detectedInteractable;
    [SerializeField] private Transform PickUpPoint;

    private Ray ray;

    void Update()
    {
        if (interactableInHand == null)
        {
            Detect();
        }
        else if (Input.GetButtonDown("Interact"))
        {
            interactableInHand.DropItem();
            interactableInHand = null;
        }
        else if (Input.GetButtonDown("Throw"))
        {
            interactableInHand.ThrowItem();
            interactableInHand = null;
        }
        else if (Input.GetButtonDown("Shoot"))
        {
            interactableInHand.Shoot();
            interactableInHand = null;
        }

        if (Input.GetButtonDown("Interact") && detectedInteractable != null)
        {
            detectedInteractable.Interact(PickUpPoint);
            detectedInteractable = null;
        }
    }

    void Detect()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, detectionRange, interactableLayer))
        {
            if (detectedInteractable == null)
            {
                detectedInteractable = hit.collider.GetComponent<Interactable>();
            }
        }
        else if (detectedInteractable != null)
        {
            detectedInteractable = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.wheat;
        Gizmos.DrawWireSphere(ray.origin, sphereRadius);
        Gizmos.DrawRay(ray);
    }
}
