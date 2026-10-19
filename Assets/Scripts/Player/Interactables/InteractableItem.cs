using UnityEngine;
using UnityEngine.Events;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private UnityEvent onThrowItem;

    private InteractableDetector intDet;

    private Collider col;

    public void Interact()
    {
        onInteract.Invoke();
    }



    public void PickUp(Transform pickUpPoint) // Recoger objeto
    {
        transform.SetParent(pickUpPoint); // Emparentar

        transform.localPosition = Vector3.zero; // Reposicionar
        transform.localEulerAngles = Vector3.zero; // Rotar

        col.enabled = false; // Deshabilitar collider

        intDet = GetComponentInParent<InteractableDetector>(); // Guardar referencia al InteractableDetector del player
    }

    public void ThrowItem() // Lanzar objeto
    {
        onThrowItem.Invoke();
        col.enabled = true; // Habilitar collider

        intDet = null; // Eliminar referencia al InteractableDetector del player
    }



    public void EnableHitScanWeapon()
    {
        GetComponent<HitscanWeapon>().enabled = true;
    }

    public void DisableHitScanWeapon()
    {
        GetComponent<HitscanWeapon>().enabled = false;
    }
}
