using UnityEngine;
using UnityEngine.EventSystems;
public class InputPanel : Singleton<InputPanel>, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
{
    [System.NonSerialized] public float horizontal;
    Vector2 _lastPosition = Vector2.zero;
    public static float valX;

    public void OnPointerDown(PointerEventData eventData)
    {
        _lastPosition = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - _lastPosition;
        horizontal = direction.x * 2 / Screen.width;
        valX = Mathf.Lerp(valX, horizontal, 0.4f);
        _lastPosition = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        horizontal = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastPosition = eventData.position;
    }
}