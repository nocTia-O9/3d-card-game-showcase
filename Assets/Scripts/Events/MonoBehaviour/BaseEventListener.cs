using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 监听基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO;

    //在事件中添加方法，监听时触发事件
    public UnityEvent<T> response;
    private void OnEnable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }
    private void OnDisable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised -= OnEventRaised;
        }
    }

    private void OnEventRaised(T value)
    {
        response?.Invoke(value);
    }
}
