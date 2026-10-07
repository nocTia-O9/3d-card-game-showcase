using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayPanelController : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO playerTurnEnd;

    private VisualElement rootElement;//根物体
    private Label energyAmount, drawAmount, discardAmount, turnLabel;
    private Button endTurnButton;
    private void OnEnable()
    {
        //通过获得挂载的UIDocument直接获取根物体
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        energyAmount = rootElement.Q<Label>("EnergyAmount");
        drawAmount = rootElement.Q<Label>("DrawAmount");
        discardAmount = rootElement.Q<Label>("DiscardAmount");
        turnLabel = rootElement.Q<Label>("TurnLabel");

        endTurnButton = rootElement.Q<Button>("EndTurn");
        //为点击结束回合按钮事件 添加方法
        endTurnButton.clicked += OnEndTurnButtonClicked;

        energyAmount.text = "0";
        drawAmount.text = "0";
        discardAmount.text = "0";
        turnLabel.text = "游戏开始";
    }

    //点击结束回合按钮事件
    private void OnEndTurnButtonClicked()
    {
        playerTurnEnd.RaiseEvent(null,this);
    }

    #region 监听事件
    public void DrawAmountChanged(int amount)
    {
        //更新抽牌堆显示数量
        drawAmount.text = amount.ToString();
    }
    public void DiscardAmountChanged(int amount)
    {
        //更新弃牌堆显示数量
        discardAmount.text = amount.ToString();
    }

    //更新能量显示
    public void UpdataEnergyAmount(int amount)
    {
        energyAmount.text = amount.ToString();
    }

    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "你的回合";
        turnLabel.style.color=new StyleColor(Color.white);
    }

    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "敌人回合";
        turnLabel.style.color = new StyleColor(Color.red);
    }
    #endregion
}
