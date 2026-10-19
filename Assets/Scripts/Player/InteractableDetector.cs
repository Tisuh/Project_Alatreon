using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private float sphereRadius = 1;
    [SerializeField] private float detectionRange = 1;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private InteractableItem interactableInHand;
    [SerializeField] private InteractableItem detectedInteractable;
    [SerializeField] private Transform pickUpPoint;

    private Ray ray;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && detectedInteractable != null)
        {
            interactableInHand = detectedInteractable;
            
        }

        if (interactableInHand == null)
        {
            Detect();
        }
    }

    void Detect()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, detectionRange, interactableLayer))
        {
            if (detectedInteractable == null)
            {
                detectedInteractable = hit.collider.GetComponent<InteractableItem>();
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
