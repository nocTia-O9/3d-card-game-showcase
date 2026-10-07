using UnityEngine;

/// <summary>
/// int类型数据SO框架
/// </summary>
[CreateAssetMenu(fileName = "IntVariable",menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int currentValue;

    public IntEventSO ValueChangeEvent;

    [TextArea]
    [SerializeField]private string description;

    /// <summary>
    /// 当前值变化时会启用事件
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(int value)
    {        
        currentValue = value;
        ValueChangeEvent?.RaiseEvent(currentValue,this);
    }
}
