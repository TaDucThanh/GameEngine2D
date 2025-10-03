using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class CallEventTriggerButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData) { AudioManager.Instance.PlaySFX("Hover"); }
    public void OnPointerClick(PointerEventData eventData) { AudioManager.Instance.PlaySFX("Click"); }
}
