using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectEventSO))]
public class ObjectEventSOEditor : BaseEventSOEditor<object>
{
    //注意此处的typeof和继承的父类
}
