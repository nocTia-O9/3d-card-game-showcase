using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChest : MonoBehaviour, IPointerDownHandler
{
    [Header("广播")]
    public ObjectEventSO gameWinEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        gameWinEvent.RaiseEvent(null, this);
    }
}
