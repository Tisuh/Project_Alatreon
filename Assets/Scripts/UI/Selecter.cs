//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;

//public class Selecter : MonoBehaviour, ISelectHandler, IDeselectHandler
//{
//    private bool isSelected;

//    [SerializeField] private UnityEvent onCancel;



//    private void Update()
//    {
//        if (Input.GetButtonDown("Cancel"))
//        {
//            onCancel.Invoke();
//        }
//    }

//    public void OnSelect(BaseEventData eventData)
//    {
//        isSelected = true;
//    }

//    public void OnDeselect(BaseEventData eventData)
//    {
//        isSelected = false;
//    }
//}
