using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件SO基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;//描述

    public UnityAction<T> OnEventRaised;

    public string lastSender;//谁是最后进行广播的
    public void RaiseEvent(T value,object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender=sender.ToString();
    }
}
