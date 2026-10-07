using UnityEngine;

public class FinishRoom : MonoBehaviour
{
    public ObjectEventSO LoadMapEvent;
    private void OnMouseDown()
    {
        LoadMapEvent.RaiseEvent(null,this);
    }
}
