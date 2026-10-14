using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private float sphereRadius = 1;
    [SerializeField] private float detectionRange = 1;
    [SerializeField] private LayerMask interactableLayer;

    private Ray ray;

    void Update()
    {
        Detect();
    }

    void Detect()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, detectionRange, interactableLayer))
        {
            Debug.Log($"Estoy detectando el objeto: {hit.collider}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.wheat;
        Gizmos.DrawWireSphere(ray.origin, sphereRadius);
        Gizmos.DrawRay(ray);
    }
}
