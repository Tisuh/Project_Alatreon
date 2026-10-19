using UnityEngine;
using UnityEngine.Events;

public class InteractableItem : MonoBehaviour
{
    private Event interact;

    public void OnInteract()
    {
        
    }

    public void PickUp(Transform pickUpPoint, InteractableItem itemInHand)
    {
        itemInHand = GetComponent<InteractableItem>();
    }
}
