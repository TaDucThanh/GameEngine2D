using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    #region Properties
    public bool IsPressed { get; private set; }
    #endregion

    #region Pointer Events
    public void OnPointerDown(PointerEventData eventData) => IsPressed = true;

    public void OnPointerUp(PointerEventData eventData) => IsPressed = false;

    public void OnPointerExit(PointerEventData eventData) => IsPressed = false;
    #endregion
}
