using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 此脚本不会挂在物体上
/// </summary>
/// <typeparam name="T"></typeparam>
[CustomEditor(typeof(BaseEventSO<>))]//因为是泛型，所以要加尖括号
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if (baseEventSO == null)
        {
            //target是Editor的属性，在此强制转化为我们需要的类型
            baseEventSO = target as BaseEventSO<T>;
        }
    }

    /// <summary>
    /// 根据CustomEditor特性中的类型，来重写该类型在Inspector窗口中显示的内容
    /// 点击SO文件即可查看该信息
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量：" + GetListeners().Count);

        foreach(var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());//显示监听者名称
        }
    }

    /// <summary>
    /// 因为监听者的脚本继承了mono，所以如果想获得所有监听者，可以返回一个mono类型的列表来实现
    /// </summary>
    /// <returns></returns>
    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour>listeners=new ();

        //避免报空
        if (baseEventSO == null||baseEventSO.OnEventRaised==null)
        {
            return listeners;
        }

        //因为监听者会将自己的方法添加到OnEventRaised委托中
        //所以可以通过GetInvocationList来返回一个Delegate类型的数组
        var subscribers =baseEventSO.OnEventRaised.GetInvocationList();

        //遍历数组，并将其每一项转化为mono类型
        foreach (var subscriber in subscribers)
        {            
            var obj=subscriber.Target as MonoBehaviour;

            //如果mono列表中没有该项，则添加进来
            if (!listeners.Contains(obj))
            {
                listeners.Add(obj);
            }
        }

        return listeners;
    }
}
