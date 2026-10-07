using UnityEngine;
using UnityEngine.UIElements;

public class StatePanel : MonoBehaviour
{
    public IntVariable playerHP;
    public IntVariable money;

    private VisualElement rootElement;
    private Label goldAmount;
    private Label HPValue;
    private void OnEnable()
    {
        rootElement=GetComponent<UIDocument>().rootVisualElement;
        goldAmount = rootElement.Q<Label>("GoldAmount");
        HPValue = rootElement.Q<Label>("HPValue");
    }

    /// <summary>
    /// 监听：增加金币
    /// </summary>
    /// <param name="amount"></param>
    public void OnMoneyChangedEvent(int amount)
    {
        money.currentValue += amount;
    }

    /// <summary>
    /// 更新状态面板
    /// </summary>
    private void Update()
    {
        goldAmount.text = money.currentValue.ToString();
        HPValue.text = playerHP.currentValue.ToString();
    }    
}
